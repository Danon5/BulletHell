using BulletHell.ECS.BufferElements;
using BulletHell.ECS.Components;
using BulletHell.ECS.SharedComponents;
using BulletHell.ECS.SystemGroups;
using Unity.Entities;

namespace BulletHell.ECS.Systems.EnemySystems
{
    [UpdateInGroup(typeof(PostPhysicsSystemGroup))]
    public partial class EnemyDamageSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((
                in EnemyComponent enemy,
                in TeamSharedComponent team,
                in DynamicBuffer<EntityCollisionBufferElement> collisionBuffer) =>
            {
                foreach (EntityCollisionBufferElement collision in collisionBuffer)
                {
                    Entity collidedEntity = collision.value.other;

                    if (EntityManager.HasComponent<TeamSharedComponent>(collidedEntity))
                    {
                        if (EntityManager.GetSharedComponentData<TeamSharedComponent>(collidedEntity).teamId == team.teamId)
                            continue;
                    }

                    if (EntityManager.HasComponent<HealthComponent>(collidedEntity))
                    {
                        HealthComponent healthComponent = EntityManager.GetComponentData<HealthComponent>(collidedEntity);

                        if (healthComponent.invincible) continue;
                        
                        healthComponent.health -= 1;
                        EntityManager.SetComponentData(collidedEntity, healthComponent);
                    }
                }
            }).WithoutBurst().Run();
        }
    }
}