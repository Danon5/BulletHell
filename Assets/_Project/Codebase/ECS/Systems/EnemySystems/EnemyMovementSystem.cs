using BulletHell.ECS.Components;
using BulletHell.ECS.SystemGroups;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace BulletHell.ECS.Systems.EnemySystems
{
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    public partial class EnemyMovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            EntityQuery aggroQuery = EntityManager.CreateEntityQuery(
                typeof(AggroAttractorComponent),
                typeof(Translation));

            NativeArray<Entity> aggroTargets = aggroQuery.ToEntityArray(Allocator.Temp);

            if (aggroTargets.Length == 0)
                return;

            Entity aggroTarget = aggroTargets[0];
            Translation aggroTargetTranslation = EntityManager.GetComponentData<Translation>(aggroTarget);

            Entities.ForEach((
                ref Translation translation,
                ref RigidbodyComponent rigidbody,
                in EnemyComponent enemy) =>
            {
                float2 dirToTarget = math.normalizesafe(aggroTargetTranslation.Value.xy - translation.Value.xy);
                rigidbody.velocity = dirToTarget * 3f;
            }).ScheduleParallel();

            aggroTargets.Dispose();
        }
    }
}