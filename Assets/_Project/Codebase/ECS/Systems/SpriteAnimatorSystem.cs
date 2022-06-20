using System.Collections.Generic;
using BulletHell.Animation;
using BulletHell.ECS.Components;
using BulletHell.ECS.SharedData;
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

                int frameRate = animationData.FramesPerSecond;
                float frameStep = 1f / ((float)GameConstants.TARGET_FRAMERATE / frameRate);
                int2 spriteSheetSize = animationData.SpriteSheetSize;
                int startIndex = animationData.StartIndex;
                int animationLength = animationData.Length;

                Entities.ForEach((
                    ref SpriteComponent sprite,
                    ref SpriteAnimatorComponent spriteAnimator) =>
                {
                    int frame = startIndex + (int)spriteAnimator.animationTime;
                    int xFrameIndex = frame % spriteSheetSize.x;
                    int yFrameIndex = frame / spriteSheetSize.x % spriteSheetSize.y;

                    sprite.spriteSheetIndex = new int2(xFrameIndex, yFrameIndex);
                    spriteAnimator.animationTime += frameStep * spriteAnimator.animationTimeScale;
                    spriteAnimator.animationTime %= animationLength;

                    if (spriteAnimator.animationTime < 0f)
                        spriteAnimator.animationTime += animationLength;
                }).ScheduleParallel();
            }

            _uniqueAnimationData.Clear();
        }
    }
}