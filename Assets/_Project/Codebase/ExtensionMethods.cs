using Unity.Mathematics;

namespace BulletHell
{
    public static class ExtensionMethods
    {
        public static float3 GetScale(this float4x4 matrix)
        {
            return new float3(
                math.length(matrix.c0.xyz),
                math.length(matrix.c1.xyz),
                math.length(matrix.c2.xyz));
        }
    }
}