using BulletHell.ECS.Components;
using BulletHell.ECS.Tags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace BulletHell.ECS.Systems
{
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