using Unity.Entities;

namespace BulletHell.ECS.Components.Singletons
{
    [GenerateAuthoringComponent]
    public struct EntityPrefabsSingletonComponent : IComponentData
    {
        public Entity playerPrefab;
        public Entity enemyPrefab;
        public Entity testPrefab;
        public Entity testPrefab2;
    }
}