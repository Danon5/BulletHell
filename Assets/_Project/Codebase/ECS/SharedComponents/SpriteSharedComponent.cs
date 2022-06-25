using Unity.Entities;
using Unity.Mathematics;

namespace BulletHell.ECS.SharedComponents
{
    public struct SpriteSharedComponent : ISharedComponentData
    {
        public TextureId textureId;
        public int2 spriteSheetColumnsRows;
        public int2 spriteOriginOffset;
    }
}