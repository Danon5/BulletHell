using BulletHell.ECS.SystemGroups;
using BulletHell.ECS.Tags.DevelopmentTags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace BulletHell.ECS.Systems.DevelopmentSystems
{
    [UpdateInGroup(typeof(GameplaySystemGroup))]
    public partial class PhysicsTesterSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 viewportMousePos = new Vector2(mousePos.x / Screen.width, mousePos.y / Screen.height);
            Vector2 worldMousePos = Camera.main.ViewportToWorldPoint(viewportMousePos);

            Entities.ForEach((
                int entityInQueryIndex,
                ref Translation translation,
                in PhysicsTesterTag physicsTester) =>
            {
                if (entityInQueryIndex == 0)
                    translation.Value = new float3(worldMousePos.x, worldMousePos.y, 0f);
            }).Run();
        }
    }
}