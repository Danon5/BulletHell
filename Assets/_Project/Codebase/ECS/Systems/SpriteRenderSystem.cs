using System.Collections.Generic;
using System.Linq;
using BulletHell.ECS.Components;
using BulletHell.ECS.SharedData;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace BulletHell.ECS.Systems
{
    [AlwaysUpdateSystem]
    public partial class SpriteRenderSystem : SystemBase 
    {
        private const int BATCH_SIZE = 1023;

        private static readonly int _HashMainTex = Shader.PropertyToID("_MainTex");
        private static readonly int _HashUvRangeAndOffset = Shader.PropertyToID("_UvRangeAndOffset");
        
        private readonly Matrix4x4[] _batchMatrixCache = new Matrix4x4[BATCH_SIZE];
        private readonly Vector4[] _batchUvRangeAndOffsetCache = new Vector4[BATCH_SIZE]; 
        private readonly Dictionary<SpriteSharedData, Mesh> _batchMeshCache = new Dictionary<SpriteSharedData, Mesh>();
        private readonly Dictionary<SpriteSharedData, Material> _batchMaterialCache = new Dictionary<SpriteSharedData, Material>();
        private readonly List<SpriteSharedData> _uniqueSpriteData = new List<SpriteSharedData>();

        private EntityQuery _uniqueSpriteQuery;

        protected override void OnCreate()
        {
            _uniqueSpriteQuery = GetEntityQuery(
                typeof(SpriteSharedData),
                typeof(SpriteComponent),
                typeof(LocalToWorld));
        }

        protected override void OnUpdate()
        {
            EntityManager.GetAllUniqueSharedComponentData(_uniqueSpriteData);

            foreach (SpriteSharedData uniqueSpriteData in _uniqueSpriteData)
            {
                _uniqueSpriteQuery.SetSharedComponentFilter(uniqueSpriteData);

                int entitiesWithUniqueSpriteCount = _uniqueSpriteQuery.CalculateEntityCount();

                Texture2D texture = GameAssets.GetTexture(uniqueSpriteData.textureId);

                Mesh mesh = !_batchMeshCache.ContainsKey(uniqueSpriteData)
                    ? CreateAndAddMeshToCache(uniqueSpriteData, texture)
                    : _batchMeshCache[uniqueSpriteData];

                Material material = !_batchMaterialCache.ContainsKey(uniqueSpriteData)
                    ? CreateAndAddMaterialToCache(uniqueSpriteData, texture)
                    : _batchMaterialCache[uniqueSpriteData];

                NativeArray<Matrix4x4> matrices = new NativeArray<Matrix4x4>(entitiesWithUniqueSpriteCount, Allocator.Temp);
                NativeArray<Vector4> uvRanges = new NativeArray<Vector4>(entitiesWithUniqueSpriteCount, Allocator.Temp);

                Entities.WithSharedComponentFilter(uniqueSpriteData).ForEach((
                    int entityInQueryIndex,
                    in SpriteComponent sprite,
                    in LocalToWorld localToWorld) =>
                {
                    matrices[entityInQueryIndex] = localToWorld.Value;
                    uvRanges[entityInQueryIndex] = sprite.uvRangeAndOffset;
                }).Run();

                NativeArray<Matrix4x4>.Copy(matrices, 0, _batchMatrixCache, 0, entitiesWithUniqueSpriteCount);
                NativeArray<Vector4>.Copy(uvRanges, 0, _batchUvRangeAndOffsetCache, 0, entitiesWithUniqueSpriteCount);

                MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
                materialPropertyBlock.SetVectorArray(_HashUvRangeAndOffset, _batchUvRangeAndOffsetCache);
                
                for (int j = 0; j < entitiesWithUniqueSpriteCount; j += BATCH_SIZE)
                {
                    int count = math.min(entitiesWithUniqueSpriteCount - j, BATCH_SIZE);
                    Graphics.DrawMeshInstanced(mesh, 0, material, _batchMatrixCache, count, materialPropertyBlock);
                }
                
                matrices.Dispose();
                uvRanges.Dispose();
            }
            
            _uniqueSpriteData.Clear();
        }

        private Mesh CreateAndAddMeshToCache(in SpriteSharedData uniqueSpriteData, in Texture2D texture)
        {
            Mesh mesh = CreateQuadFromTexture(texture);
            _batchMeshCache.Add(uniqueSpriteData, mesh);
            return mesh;
        }
        
        private Material CreateAndAddMaterialToCache(in SpriteSharedData uniqueSpriteData, in Texture2D texture)
        {
            Material material = CreateMaterialFromTexture(texture);
            _batchMaterialCache.Add(uniqueSpriteData, material);
            return material;
        }

        private static Material CreateMaterialFromTexture(in Texture2D texture)
        {
            Material material = new Material(GameAssets.TestMaterial);
            material.SetTexture(_HashMainTex, texture);
            return material;
        }

        private static Mesh CreateQuadFromTexture(in Texture2D texture)
        {
            return CreateQuad(
                (float) texture.width / GameConstants.PPU, 
                (float) texture.height / GameConstants.PPU);
        }

        private static Mesh CreateQuad(float width, float height)
        {
            Mesh mesh = new Mesh();

            float w = width * .5f;
            float h = height * .5f;
            
            Vector3[] vertices = 
            {
                new Vector3(-w, -h, 0),
                new Vector3(w, -h, 0),
                new Vector3(-w, h, 0),
                new Vector3(w, h, 0)
            };

            int[] tris = 
            {
                0, 2, 1,
                2, 3, 1
            };

            Vector3[] normals =
            {
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward
            };

            Vector2[] uv =
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };

            mesh.vertices = vertices;
            mesh.triangles = tris;
            mesh.normals = normals;
            mesh.uv = uv;

            return mesh;
        }
    }
}