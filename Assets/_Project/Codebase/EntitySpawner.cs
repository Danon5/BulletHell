using BulletHell.ECS.Components;
using BulletHell.ECS.SharedComponents;
using BulletHell.ECS.Tags;
using JetBrains.Annotations;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace BulletHell
{
    public sealed class EntitySpawner : MonoBehaviour
    {
        private static EntitySpawner _singleton;
        private static EntityManager _entityManager;
        
        [UsedImplicitly]
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void InitializeOnLoad()
        {
            _singleton = null;
        }
        
        [UsedImplicitly]
        private void Awake()
        {
            _singleton = this;
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        public static Entity SpawnPlayerEntity()
        {
            Entity playerEntity = _entityManager.CreateEntity();
            
            AddTransformComponentsToEntity(playerEntity);

            _entityManager.AddComponentData(playerEntity, new PlayerComponent());
            _entityManager.AddComponentData(playerEntity, new CameraFollowTargetComponent());
            _entityManager.AddComponentData(playerEntity, new RigidbodyComponent());
            _entityManager.AddComponentData(playerEntity, new SpriteAutoFlipXTag());
            _entityManager.AddComponentData(playerEntity, new SpriteComponent());
            _entityManager.AddComponentData(playerEntity, new AggroAttractorComponent());
            _entityManager.AddComponentData(playerEntity, new ColliderComponent
            {
                colliderType = ColliderType.Circle,
                isTrigger = true,
                tracksCollisions = true,
                circleColliderRadius = .5f
            });
            _entityManager.AddComponentData(playerEntity, new HealthComponent
            {
                invincible = false,
                maxHealth = 5,
                health = 5
            });
            _entityManager.AddSharedComponentData(playerEntity, new TeamSharedComponent
            {
                teamId = TeamId.Friendly
            });
            _entityManager.AddSharedComponentData(playerEntity, new SpriteSharedComponent
            {
                textureId = TextureId.Character_Havoc_Default,
                spriteSheetColumnsRows = new int2(1, 1),
                spriteOriginOffset = new int2(0, -12)
            });

            return playerEntity;
        }
        
        public static Entity SpawnEnemyEntity()
        {
            Entity enemyEntity = _entityManager.CreateEntity();
            
            AddTransformComponentsToEntity(enemyEntity);

            _entityManager.AddComponentData(enemyEntity, new EnemyComponent());
            _entityManager.AddComponentData(enemyEntity, new RigidbodyComponent());
            _entityManager.AddComponentData(enemyEntity, new SpriteAutoFlipXTag());
            _entityManager.AddComponentData(enemyEntity, new SpriteComponent());
            _entityManager.AddComponentData(enemyEntity, new ColliderComponent
            {
                colliderType = ColliderType.Circle,
                isTrigger = false,
                tracksCollisions = true,
                circleColliderRadius = 1f
            });
            _entityManager.AddComponentData(enemyEntity, new HealthComponent
            {
                invincible = false,
                maxHealth = 100,
                health = 100
            });
            _entityManager.AddSharedComponentData(enemyEntity, new TeamSharedComponent
            {
                teamId = TeamId.Enemy
            });
            _entityManager.AddSharedComponentData(enemyEntity, new SpriteSharedComponent
            {
                textureId = TextureId.Cheeseburger,
                spriteSheetColumnsRows = new int2(1, 1),
                spriteOriginOffset = new int2(0, 0)
            });

            return enemyEntity;
        }

        public static void AddTransformComponentsToEntity(in Entity entity)
        {
            _entityManager.AddComponentData(entity, new Translation());
            _entityManager.AddComponentData(entity, new Rotation());
            _entityManager.AddComponentData(entity, new NonUniformScale
            {
                Value = new float3(1f, 1f, 1f)
            });
            _entityManager.AddComponentData(entity, new LocalToWorld());
        }
    }
}