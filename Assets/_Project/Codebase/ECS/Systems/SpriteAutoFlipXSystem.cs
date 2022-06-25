using BulletHell.ECS.Components;
using BulletHell.ECS.SystemGroups;
using BulletHell.ECS.Tags;
using Unity.Entities;
using Unity.Mathematics;

namespace BulletHell.ECS.Systems
{
    [UpdateInGroup(typeof(RenderingSystemGroup))]
    [UpdateBefore(typeof(SpriteRenderSystem))]
    public partial class SpriteAutoFlipXSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((
                ref SpriteComponent sprite,
                in SpriteAutoFlipXTag flipX,
                in RigidbodyComponent rigidbody) =>
            {
                float xVel = rigidbody.velocity.x;

                if (math.abs(xVel) <= .01) return;

                sprite.flipX = xVel < 0f;
            }).ScheduleParallel();
        }
    }
}