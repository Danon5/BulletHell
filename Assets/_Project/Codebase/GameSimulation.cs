using System;
using System.Collections.Generic;
using BulletHell.ECS.Components.Singletons;
using BulletHell.ECS.Systems;
using Unity.Entities;
using UnityEngine;

namespace BulletHell
{
    public sealed class GameSimulation : MonoBehaviour
    {
        private List<SystemBase> _systems;
        private List<SystemBase> _lateSystems;

        private EntityManager _entityManager;

        private void Start()
        {
            _systems = new List<SystemBase>
            {
                
            };
            
            _lateSystems = new List<SystemBase>
            {
                World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<QuadRendererSystem>()
            };

            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            Entity gameDataEntity = _entityManager.CreateEntityQuery(
                typeof(GameDataSingletonComponent)).GetSingletonEntity();

            GameDataSingletonComponent gameData = _entityManager.GetComponentData<GameDataSingletonComponent>(gameDataEntity);  

            _entityManager.Instantiate(gameData.playerPrefab);
        }
        
        private void Update()
        {
            foreach (SystemBase system in _systems)
            {
                if (system.ShouldRunSystem())
                    system.Update();
            }
        }

        private void LateUpdate()
        {
            foreach (SystemBase system in _lateSystems)
            {
                if (system.ShouldRunSystem())
                    system.Update();
            }
        }
    }
}