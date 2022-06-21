using System;
using BulletHell.ECS.Components;
using BulletHell.ECS.SharedData;
using JetBrains.Annotations;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BulletHell
{
    public sealed class TestSpawner : MonoBehaviour
    {
        private EntityManager _entityManager;
        private int _entityCount;

        [UsedImplicitly]
        private void Start()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;

            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        [UsedImplicitly]
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
                AddEntities();
        }

        [UsedImplicitly]
        private void OnGUI()
        {
            GUIStyle style = GUI.skin.label;
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 20;
            
            GUI.Label(
                new Rect(Screen.width / 2f - 250f, 5f, 500f, 50f),
                $"Entity Count: {_entityCount}", style);
        }

        private void AddEntities()
        {
            for (int i = 0; i < 500; i++)
            {
                Entity entity = _entityManager.CreateEntity();

                _entityManager.AddComponent<Translation>(entity);
                _entityManager.AddComponent<Rotation>(entity);
                _entityManager.AddComponent<LocalToWorld>(entity);
                _entityManager.AddComponent<SpriteComponent>(entity);
                _entityManager.AddComponent<SpriteAnimatorComponent>(entity);
                _entityManager.AddComponent<SpriteSharedData>(entity);
                _entityManager.AddComponent<SpriteAnimationSharedData>(entity);

                _entityManager.SetComponentData(entity, new SpriteComponent
                {
                    spriteSheetIndex = new int2(0, 0)
                });

                _entityManager.SetComponentData(entity, new SpriteAnimatorComponent
                {
                    animationTime = Random.Range(0f, 10f),
                    animationTimeScale = 1f
                });

                _entityManager.SetSharedComponentData(entity, new SpriteSharedData
                {
                    textureId = TextureId.Character_Template_Run,
                    spriteSheetColumnsRows = new int2(8, 1) // remove this and get from scriptableobject in sprite render system
                });

                _entityManager.SetSharedComponentData(entity, new SpriteAnimationSharedData
                {
                    animationId = AnimationId.Character_Template_Run
                });

                _entityManager.SetComponentData(entity, new Translation
                {
                    Value = new float3
                    {
                        x = Random.Range(-15f, 15f),
                        y = Random.Range(-10f, 10f)
                    }
                });
            }
            
            _entityCount += 500;
        }
    }
}