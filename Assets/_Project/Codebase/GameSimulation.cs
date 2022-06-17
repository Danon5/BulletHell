using BulletHell.ECS.Components.Singletons;
using Unity.Entities;
using UnityEngine;

namespace BulletHell
{
    public sealed class GameSimulation : MonoBehaviour
    {
        private EntityManager _entityManager;
        
        private void Start()
        {
            Application.targetFrameRate = GameConstants.TARGET_FRAMERATE;
            QualitySettings.vSyncCount = 0;
            
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            Entity gameDataEntity = _entityManager.CreateEntityQuery(
                typeof(GameDataSingletonComponent)).GetSingletonEntity();

            GameDataSingletonComponent gameData = _entityManager.GetComponentData<GameDataSingletonComponent>(gameDataEntity);  

            _entityManager.Instantiate(gameData.playerPrefab);
            _entityManager.Instantiate(gameData.enemyPrefab);
        }
    }
}