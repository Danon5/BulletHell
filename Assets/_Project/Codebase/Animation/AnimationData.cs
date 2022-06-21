using Unity.Mathematics;
using UnityEngine;

namespace BulletHell.Animation
{
    [CreateAssetMenu(
        fileName = nameof(AnimationData),
        menuName = nameof(AnimationData),
        order = 0)]
    public sealed class AnimationData : ScriptableObject
    {
        [field: SerializeField] public AnimationId AnimationId { get; private set; }
        [field: SerializeField] public Texture2D SpriteSheet { get; private set; }
        [field: SerializeField] public int FramesPerSecond { get; private set; }
        [field: SerializeField] public int2 SpriteSheetSize { get; private set; }
        [field: SerializeField] public int StartIndex { get; private set; }
        [field: SerializeField] [field: NonReorderable] public AnimationFrameData[] FrameData { get; private set; }
        public int Length => FrameData.Length;
        public int LengthWithFrameDurationIncluded
        {
            get
            {
                int length = 0;
                for (int i = 0; i < FrameData.Length; i++)
                    length += FrameData[i].frameDuration;
                return length;
            }
        }
    }
}