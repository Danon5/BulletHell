using Unity.Entities;

namespace BulletHell.ECS.Components
{
    [GenerateAuthoringComponent]
    public struct SortingLayerComponent : IComponentData
    {
        public bool sortByY;
        public int layer;
    }
}