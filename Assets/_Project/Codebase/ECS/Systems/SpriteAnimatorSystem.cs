using System.Collections.Generic;
using BulletHell.Animation;
using BulletHell.ECS.Components;
using BulletHell.ECS.SharedData;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace BulletHell.ECS.Systems
{
    public partial class SpriteAnimatorSystem : SystemBase
    {
        private readonly Dictionary<SpriteAnimationSharedData, AnimationData> _batchAnimationDataCache =
            new Dictionary<SpriteAnimationSharedData, AnimationData>();

        private readonly List<SpriteAnimationSharedData> _uniqueAnimationData = new List<SpriteAnimationSharedData>();

        protected override void OnUpdate()
        {
            EntityManager.GetAllUniqueSharedComponentData(_uniqueAnimationData);

            foreach (SpriteAnimationSharedData uniqueAnimationData in _uniqueAnimationData)
            {
                if (uniqueAnimationData.animationId == AnimationId.None) continue;
                
                AnimationData animationData = _batchAnimationDataCache.ContainsKey(uniqueAnimationData)
                    ? _batchAnimationDataCache[uniqueAnimationData]
                    : GameAssets.GetAnimationData(uniqueAnimationData.animationId);

                // port all this setup to a cached data structure
                
                int frameRate = animationData.FramesPerSecond;
                float frameStep = 1f / ((float)GameConstants.TARGET_FRAMERATE / frameRate);
                int2 spriteSheetSize = animationData.SpriteSheetSize;
                int startIndex = animationData.StartIndex;
                int animationLength = animationData.Length;
                int animationLengthWithFrameDataIncluded = animationData.LengthWithFrameDurationIncluded;

                NativeArray<int> frameMap = new NativeArray<int>(animationLengthWithFrameDataIncluded, Allocator.TempJob);

                int frameMapIndex = 0;
                
                for (int i = 0; i < animationLength; i++)
                {
                    for (int j = 0; j < animationData.FrameData[i].frameDuration; j++)
                    {
                        frameMap[frameMapIndex] = i;
                        frameMapIndex++;
                    }
                }

                Entities.ForEach((
                    ref SpriteComponent sprite,
                    ref SpriteAnimatorComponent spriteAnimator) =>
                {
                    int frameIndex = (startIndex + (int)spriteAnimator.animationTime) % animationLengthWithFrameDataIncluded;
                    int mappedIndex = frameMap[frameIndex];
                    int xFrameIndex = mappedIndex % spriteSheetSize.x;
                    int yFrameIndex = mappedIndex / spriteSheetSize.x % spriteSheetSize.y;

                    sprite.spriteSheetIndex = new int2(xFrameIndex, yFrameIndex);
                    spriteAnimator.animationTime += frameStep * spriteAnimator.animationTimeScale;
                    spriteAnimator.animationTime %= animationLengthWithFrameDataIncluded;

                    if (spriteAnimator.animationTime < 0f)
                        spriteAnimator.animationTime += animationLengthWithFrameDataIncluded;
                }).Schedule();
                
                Dependency.Complete();

                frameMap.Dispose();
            }

            _uniqueAnimationData.Clear();
        }
    }
}