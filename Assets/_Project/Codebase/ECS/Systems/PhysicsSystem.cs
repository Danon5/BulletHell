using BulletHell.ECS.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace BulletHell.ECS.Systems
{
    public partial class PhysicsSystem : SystemBase
    {
        private EntityQuery _circleColliderQuery;

        protected override void OnCreate()
        {
            _circleColliderQuery = EntityManager.CreateEntityQuery(
                typeof(Translation),
                typeof(RigidbodyComponent),
                typeof(CircleColliderComponent));
        }

        protected override void OnUpdate()
        {
            int circleColliderCount = _circleColliderQuery.CalculateEntityCount();

            NativeArray<CircleCollider> circleColliders = new NativeArray<CircleCollider>(circleColliderCount, Allocator.TempJob);

            //Vector3 viewportMousePos = new Vector3(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
            //Vector3 worldMousePos = Camera.main.ViewportToWorldPoint(viewportMousePos);

            Entities.ForEach((
                int entityInQueryIndex,
                ref Translation translation,
                in RigidbodyComponent rigidbody,
                in CircleColliderComponent circleCollider) =>
            {
                //if (entityInQueryIndex == 0)
                //translation.Value = new float3(worldMousePos.x, worldMousePos.y, 0f);

                translation.Value += new float3(rigidbody.velocity.x, rigidbody.velocity.y, 0f) * GameConstants.TARGET_TIMESTEP;

                circleColliders[entityInQueryIndex] = new CircleCollider
                {
                    position = new float2(translation.Value.x, translation.Value.y),
                    radius = circleCollider.radius
                };
            }).Schedule();
            
            Dependency.Complete();
            
            Entities.ForEach((
                int entityInQueryIndex,
                ref Translation translation,
                in RigidbodyComponent rigidbody,
                in CircleColliderComponent circleCollider) =>
            {
                CircleCollider a = circleColliders[entityInQueryIndex];
                
                for (int i = 0; i < circleColliderCount; i++)
                {
                    if (i == entityInQueryIndex) continue;

                    CircleCollider b = circleColliders[i];
                    
                    float totalRadius = a.radius + b.radius;
                    float2 diff = b.position - a.position;

                    if (AreCirclesColliding(a, b, totalRadius, diff))
                        ResolveCircleCollision(ref a, ref b, totalRadius, diff);

                    circleColliders[i] = b;
                }
                
                circleColliders[entityInQueryIndex] = a;
            }).Schedule();
            
            Dependency.Complete();
            
            Entities.ForEach((
                int entityInQueryIndex,
                ref Translation translation,
                in RigidbodyComponent rigidbody,
                in CircleColliderComponent circleCollider) =>
            {
                CircleCollider resolvedCollider = circleColliders[entityInQueryIndex];
                translation.Value = new float3(resolvedCollider.position.x, resolvedCollider.position.y, 0f);
            }).Schedule();
            
            Dependency.Complete();

            circleColliders.Dispose();
        }

        private static bool AreCirclesColliding(in CircleCollider a, in CircleCollider b, in float totalRadius, in float2 diff)
        {
            
            return totalRadius * totalRadius > diff.x * diff.x + diff.y * diff.y;
        }

        private static void ResolveCircleCollision(ref CircleCollider a, ref CircleCollider b, in float totalRadius, in float2 diff)
        {
            float diffMag = math.length(diff);
            float2 normal = diff / diffMag;
            float overlapDist = totalRadius - diffMag;
            float depenetration = overlapDist / 2f;

            a.position -= normal * depenetration;
            b.position += normal * depenetration;
        }

        private struct CircleCollider
        {
            public float2 position;
            public float radius;
        }
    }
}