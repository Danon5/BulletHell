using Unity.Entities;

namespace BulletHell.ECS.Components.Singletons
{
    [GenerateAuthoringComponent]
    public struct GameDataSingletonComponent : IComponentData
    {
        public byte ppu;
        
        // PREFABS
        public Entity playerPrefab;
    }
}