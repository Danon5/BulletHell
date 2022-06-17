using BulletHell.ECS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace BulletHell.ECS.Systems
{
    [DisableAutoCreation]
    public partial class SortingLayerSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((
                ref Translation translation,
                in LocalToWorld localToWorld,
                in SortingLayerComponent sortingLayer) =>
            {
                float3 pos = translation.Value;

                pos.z = sortingLayer.layer;

                if (sortingLayer.sortByY)
                    pos.z += localToWorld.Value.c3.y;
                
                translation.Value = pos;
            }).Run();
        }
    }
}