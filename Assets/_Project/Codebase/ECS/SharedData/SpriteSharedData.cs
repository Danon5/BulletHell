using System;
using Unity.Entities;
using Unity.Mathematics;

namespace BulletHell.ECS.SharedData
{
    [Serializable]
    public struct SpriteSharedData : ISharedComponentData
    {
        public TextureId textureId;
        public int2 spriteSheetColumnsRows;
    }
}