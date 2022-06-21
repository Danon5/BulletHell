using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHell
{
    public sealed class GameRendering : MonoBehaviour
    {
        [SerializeField] private RawImage _gameRenderImage;
        
        public RenderTexture GameRenderTexture { get; private set; }

        private static GameRendering _singleton;
        
        private Camera _camera;
        private float _previousOrthographicSize;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void InitializeOnLoad()
        {
            _singleton = null;
        }

        [UsedImplicitly]
        private void Awake()
        {
            _singleton = this;
            TryRecalculateRenderTexture();
        }

        [UsedImplicitly]
        private void LateUpdate()
        {
            TryRecalculateRenderTexture();
        }

        private void TryRecalculateRenderTexture()
        {
            if (_camera == null)
                _camera = Camera.main;
            
            if (_camera == null) return;
            
            float orthographicSize = _camera.orthographicSize;

            if (Mathf.Abs(orthographicSize - _previousOrthographicSize) > .01f)
                AssignNewRenderTextureWithCorrectDimensions(orthographicSize);

            _previousOrthographicSize = orthographicSize;
        }

        private void AssignNewRenderTextureWithCorrectDimensions(in float orthographicSize)
        {
            const float aspect = 16f / 9f;
            float verticalSize = orthographicSize * 2f * GameConstants.PPU;
            int height = Mathf.RoundToInt(verticalSize);
            int width = Mathf.RoundToInt(verticalSize * aspect);

            if (GameRenderTexture != null)
                GameRenderTexture.Release();
            
            GameRenderTexture = new RenderTexture(width, height, 24)
            {
                filterMode = FilterMode.Point
            };
            GameRenderTexture.Create();
            
            _camera.targetTexture = GameRenderTexture;
            _gameRenderImage.texture = GameRenderTexture;
        }
    }
}