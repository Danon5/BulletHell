using System.Collections.Generic;
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
        private static readonly int _HashUvScaleAndOffset = Shader.PropertyToID("_UvScaleAndOffset");
        private readonly Dictionary<SpriteSharedData, Material> _batchMaterialCache = new Dictionary<SpriteSharedData, Material>();

        private readonly Matrix4x4[] _batchMatrixCache = new Matrix4x4[BATCH_SIZE];
        private readonly Dictionary<SpriteSharedData, Mesh> _batchMeshCache = new Dictionary<SpriteSharedData, Mesh>();
        private readonly Vector4[] _batchUvScaleAndOffsetCache = new Vector4[BATCH_SIZE];
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
                if (uniqueSpriteData.textureId == TextureId.None) continue;
                
                _uniqueSpriteQuery.SetSharedComponentFilter(uniqueSpriteData);

                int entitiesWithUniqueSpriteCount = _uniqueSpriteQuery.CalculateEntityCount();

                Texture2D texture = GameAssets.GetTexture(uniqueSpriteData.textureId);

                Mesh mesh = _batchMeshCache.ContainsKey(uniqueSpriteData)
                    ? _batchMeshCache[uniqueSpriteData]
                    : CreateAndAddMeshToCache(uniqueSpriteData, texture);

                Material material = _batchMaterialCache.ContainsKey(uniqueSpriteData)
                    ? _batchMaterialCache[uniqueSpriteData]
                    : CreateAndAddMaterialToCache(uniqueSpriteData, texture);

                NativeArray<Matrix4x4> matrices = new NativeArray<Matrix4x4>(entitiesWithUniqueSpriteCount, Allocator.Temp);
                NativeArray<Vector4> uvScaleAndOffsets = new NativeArray<Vector4>(entitiesWithUniqueSpriteCount, Allocator.Temp);

                // needed to avoid burst compilation error involving duplicate variable names
                SpriteSharedData sharedDataCopy = uniqueSpriteData;

                Entities.WithSharedComponentFilter(uniqueSpriteData).ForEach((
                    int entityInQueryIndex,
                    in SpriteComponent sprite,
                    in LocalToWorld localToWorld) =>
                {
                    matrices[entityInQueryIndex] = localToWorld.Value;

                    float2 uvScalePerColumnRow = new float2(
                        1f / sharedDataCopy.spriteSheetColumnsRows.x,
                        1f / sharedDataCopy.spriteSheetColumnsRows.y);

                    uvScaleAndOffsets[entityInQueryIndex] = new Vector4
                    {
                        x = uvScalePerColumnRow.x,
                        y = uvScalePerColumnRow.y,
                        z = uvScalePerColumnRow.x * sprite.spriteSheetIndex.x,
                        w = uvScalePerColumnRow.y * sprite.spriteSheetIndex.y
                    };
                }).Run();

                for (int j = 0; j < entitiesWithUniqueSpriteCount; j += BATCH_SIZE)
                {
                    int count = math.min(entitiesWithUniqueSpriteCount - j, BATCH_SIZE);

                    NativeArray<Matrix4x4>.Copy(matrices, j, _batchMatrixCache, 0, count);
                    NativeArray<Vector4>.Copy(uvScaleAndOffsets, j, _batchUvScaleAndOffsetCache, 0, count);

                    MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
                    materialPropertyBlock.SetVectorArray(_HashUvScaleAndOffset, _batchUvScaleAndOffsetCache);

                    Graphics.DrawMeshInstanced(mesh, 0, material, _batchMatrixCache, count, materialPropertyBlock);
                }

                matrices.Dispose();
                uvScaleAndOffsets.Dispose();
            }

            _uniqueSpriteData.Clear();
        }

        private Mesh CreateAndAddMeshToCache(in SpriteSharedData spriteData, in Texture2D texture)
        {
            Mesh mesh = CreateQuadFromTexture(texture, spriteData.spriteSheetColumnsRows);
            _batchMeshCache.Add(spriteData, mesh);
            return mesh;
        }

        private Material CreateAndAddMaterialToCache(in SpriteSharedData spriteData, in Texture2D texture)
        {
            Material material = CreateMaterialFromTexture(texture);
            _batchMaterialCache.Add(spriteData, material);
            return material;
        }

        private static Material CreateMaterialFromTexture(in Texture2D texture)
        {
            Material material = new Material(GameAssets.TestMaterial);
            material.SetTexture(_HashMainTex, texture);
            return material;
        }

        private static Mesh CreateQuadFromTexture(in Texture2D texture, in int2 columnsRows)
        {
            return CreateQuad(
                texture.width / (float)columnsRows.x / GameConstants.PPU,
                texture.height / (float)columnsRows.y / GameConstants.PPU);
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