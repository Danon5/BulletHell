using System;
using Unity.Entities;

namespace BulletHell.ECS.SharedData
{
    [Serializable]
    public struct SpriteSharedData : ISharedComponentData
    {
        public TextureId textureId;
    }
}