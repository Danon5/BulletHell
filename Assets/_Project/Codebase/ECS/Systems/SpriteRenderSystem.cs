using BulletHell.ECS.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace BulletHell.ECS.Systems
{
    public partial class SpriteRenderSystem : SystemBase
    {
        private const int BATCH_SIZE = 1023;
        
        private Mesh _quad;

        protected override void OnCreate()
        {
            _quad = CreateQuad(1f, 1f);
        }

        protected override void OnUpdate()
        {
            int spriteCount = EntityManager.CreateEntityQuery(
                typeof(SpriteComponent),
                typeof(LocalToWorld)).CalculateEntityCount();
            
            NativeArray<Matrix4x4> matrices = new NativeArray<Matrix4x4>(spriteCount, Allocator.Temp);

            float time = (float) Time.ElapsedTime;
            
            Entities.ForEach((
                ref Translation translation,
                in SpriteComponent sprite) =>
            {
                translation.Value += new float3(
                    math.sin(time) * GameConstants.TARGET_TIMESTEP,
                    math.sin(time) * GameConstants.TARGET_TIMESTEP,
                    0f);
            }).Run();
            
            Entities.ForEach((
                int entityInQueryIndex,
                in SpriteComponent sprite,
                in LocalToWorld localToWorld) =>
            {
                matrices[entityInQueryIndex] = localToWorld.Value;
            }).Run();

            Matrix4x4[] drawMatrices = new Matrix4x4[BATCH_SIZE];

            for (int i = 0; i < spriteCount; i += BATCH_SIZE)
            {
                int count = math.min(spriteCount - i, BATCH_SIZE);
                
                NativeArray<Matrix4x4>.Copy(matrices, i, drawMatrices, 0, count);
                
                Graphics.DrawMeshInstanced(_quad, 0, GameAssets.TestMaterial, 
                    drawMatrices, count);
            }

            matrices.Dispose();
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