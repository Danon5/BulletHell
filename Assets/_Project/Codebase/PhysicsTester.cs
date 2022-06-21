﻿using BulletHell.ECS.Components;
using BulletHell.ECS.SharedData;
using JetBrains.Annotations;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BulletHell
{
    public sealed class PhysicsTester : MonoBehaviour
    {
        private EntityManager _entityManager;
        
        [UsedImplicitly]
        private void Start()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            for (int i = 0; i < 10; i++)
            {
                Entity entity = _entityManager.CreateEntity();
            
                EntitySpawner.AddTransformComponentsToEntity(entity);
                _entityManager.AddComponentData(entity, new RigidbodyComponent());
                _entityManager.AddComponentData(entity, new CircleColliderComponent
                {
                    radius = .5f
                });
                _entityManager.AddComponentData(entity, new SpriteComponent());
                _entityManager.AddSharedComponentData(entity, new SpriteSharedData
                {
                    textureId = TextureId.Circle16,
                    spriteSheetColumnsRows = new int2(1, 1)
                });
                
                _entityManager.SetComponentData(entity, new Translation
                {
                    Value = new float3(Random.Range(-8f, 8f), Random.Range(-5f, 5f), 0f)
                });
            }
        }
    }
}