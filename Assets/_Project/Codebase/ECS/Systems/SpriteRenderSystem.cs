using Unity.Entities;
using UnityEngine;

namespace BulletHell.ECS.Systems
{
    public partial class SpriteRenderSystem : SystemBase
    {
        private Mesh _quad;

        private Matrix4x4[] _matrices;
        
        protected override void OnCreate()
        {
            _quad = CreateQuad(1f, 1f);
            _matrices = new Matrix4x4[1000];
        }

        protected override void OnUpdate()
        {
            _matrices[0] = Matrix4x4.TRS(
                new Vector3(Mathf.Sin((float) Time.ElapsedTime), Mathf.Sin((float) Time.ElapsedTime), 0f),
                Quaternion.identity,
                Vector3.one);
            Graphics.DrawMeshInstanced(_quad, 0, GameAssets.TestMaterial, _matrices, 1);
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