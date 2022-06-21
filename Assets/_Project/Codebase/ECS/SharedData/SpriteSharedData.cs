using System;
using Unity.Entities;
using Unity.Mathematics;

namespace BulletHell.ECS.SharedData
{
    public struct SpriteSharedData : ISharedComponentData
    {
        public TextureId textureId;
        public int2 spriteSheetColumnsRows;
        public int2 spriteOriginOffset;
    }
}