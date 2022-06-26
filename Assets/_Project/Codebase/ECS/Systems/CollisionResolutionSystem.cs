using BulletHell.ECS.BufferElements;
using BulletHell.ECS.Components;
using BulletHell.ECS.SystemGroups;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace BulletHell.ECS.Systems
{
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    public partial class CollisionResolutionSystem : SystemBase
    {
        private EntityQuery _circleColliderQuery;
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;

        protected override void OnCreate()
        {
            _circleColliderQuery = EntityManager.CreateEntityQuery(
                typeof(Translation),
                typeof(RigidbodyComponent),
                typeof(ColliderComponent));

            _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            int colliderCount = _circleColliderQuery.CalculateEntityCount();
            NativeArray<EntityCollider> colliders = new NativeArray<EntityCollider>(colliderCount, Allocator.TempJob);
            NativeQueue<EntityCollision> collisions = new NativeQueue<EntityCollision>(Allocator.TempJob);

            EntityCommandBuffer commandBuffer = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer();

            Entities.WithAll<RigidbodyComponent>().ForEach((
                Entity entity,
                int entityInQueryIndex,
                in Translation translation,
                in ColliderComponent collider) =>
            {
                colliders[entityInQueryIndex] = new EntityCollider
                {
                    entity = entity,
                    colliderType = collider.colliderType,
                    isTrigger = collider.isTrigger,
                    tracksCollisions = collider.tracksCollisions,
                    position = new float2(translation.Value.x, translation.Value.y),
                    circleColliderRadius = collider.circleColliderRadius
                };
            }).Schedule();

            Dependency.Complete();

            Entities.WithAll<Translation, RigidbodyComponent>().ForEach((
                int entityInQueryIndex,
                in ColliderComponent collider) =>
            {
                EntityCollider colA = colliders[entityInQueryIndex];

                for (int i = 0; i < colliderCount; i++)
                {
                    if (i == entityInQueryIndex) continue;

                    EntityCollider colB = colliders[i];

                    if (colA.colliderType == ColliderType.Circle && colB.colliderType == ColliderType.Circle)
                    {
                        float totalRadius = colA.circleColliderRadius + colB.circleColliderRadius;
                        float2 positionDiff = colB.position - colA.position;

                        if (AreCirclesColliding(colA, colB, totalRadius, positionDiff))
                        {
                            if (collider.tracksCollisions)
                            {
                                collisions.Enqueue(new EntityCollision
                                {
                                    collider = colA,
                                    otherCollider = colB
                                });
                            }

                            if (!colA.isTrigger && !colB.isTrigger)
                                ResolveCircleCollision(ref colA, ref colB, totalRadius, positionDiff);
                        }
                    }

                    colliders[i] = colB;
                }

                colliders[entityInQueryIndex] = colA;
            }).Schedule();

            Dependency.Complete();

            Entities.WithAll<RigidbodyComponent, ColliderComponent>().ForEach((
                int entityInQueryIndex,
                ref Translation translation) =>
            {
                EntityCollider resolvedCollider = colliders[entityInQueryIndex];
                translation.Value = new float3(resolvedCollider.position.x, resolvedCollider.position.y, 0f);
            }).Schedule();

            Dependency.Complete();

            Entities.WithStructuralChanges().ForEach((
                Entity entity,
                in ColliderComponent circleCollider) =>
            {
                bool collisionTrackStateHasChanged = circleCollider.tracksCollisions !=
                                                     EntityManager.HasComponent<EntityCollisionBufferElement>(entity);

                if (!collisionTrackStateHasChanged) return;
                
                if (circleCollider.tracksCollisions)
                    EntityManager.AddBuffer<EntityCollisionBufferElement>(entity);
                else
                    EntityManager.RemoveComponent<EntityCollisionBufferElement>(entity);
            }).WithoutBurst().Run();

            Entities.WithAll<EntityCollisionBufferElement>().ForEach((
                Entity entity) =>
            {
                commandBuffer.SetBuffer<EntityCollisionBufferElement>(entity);
            }).Schedule();
            
            Dependency.Complete();

            while (collisions.TryDequeue(out EntityCollision collision))
            {
                Entity entity = collision.collider.entity;

                EntityManager.GetBuffer<EntityCollisionBufferElement>(entity).Add(new EntityCollisionBufferElement
                {
                    value = new EntityCollisionData
                    {
                        other = collision.otherCollider.entity,
                        point = default,
                        normal = default
                    }
                });
            }

            Dependency.Complete();
            
            colliders.Dispose();
            collisions.Dispose();
        }

        private static bool AreCirclesColliding(in EntityCollider a, in EntityCollider b, in float totalRadius, in float2 diff)
        {
            return totalRadius * totalRadius > diff.x * diff.x + diff.y * diff.y;
        }

        private static void ResolveCircleCollision(ref EntityCollider a, ref EntityCollider b, in float totalRadius, in float2 diff)
        {
            float diffMag = math.length(diff);

            if (diffMag == 0f)
            {
                b.position += new float2(0f, 1f) * .001f;
                return;
            }
            
            float2 normal = diff / diffMag;
            float overlapDist = totalRadius - diffMag;
            float depenetration = overlapDist / 2f;

            a.position -= normal * depenetration;
            b.position += normal * depenetration;
        }

        private struct EntityCollider
        {
            public Entity entity;
            public ColliderType colliderType;
            public bool isTrigger;
            public bool tracksCollisions;
            public float2 position;
            public float circleColliderRadius;
        }

        private struct EntityCollision
        {
            public EntityCollider collider;
            public EntityCollider otherCollider;
        }
    }
}