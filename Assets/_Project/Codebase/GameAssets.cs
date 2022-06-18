using UnityEngine;

namespace BulletHell
{
    public sealed class GameAssets : MonoBehaviour
    {
        [SerializeField] private Material _testMaterial;
        [SerializeField] private GameObject _testRenderer;

        public static Material TestMaterial => _singleton._testMaterial;

        private static GameAssets _singleton;
        
        private void Awake()
        {
            _singleton = this;
        }
    }
}