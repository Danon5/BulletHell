using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHell
{
    public sealed class GameRendering : MonoBehaviour
    {
        [SerializeField] private Camera _gameRenderCamera;
        [SerializeField] private Camera _pixelationCamera;
        [SerializeField] private RawImage _gameRenderImage;

        public static Camera GameRenderCamera => _singleton._gameRenderCamera;
        public static Camera PixelationCamera => _singleton._pixelationCamera;
        public static RenderTexture PixelationRenderTexture => _singleton._pixelationRenderTexture;

        private static GameRendering _singleton;
        
        private RenderTexture _pixelationRenderTexture;
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
            
            UpdatePixelationCamera();
        }

        [UsedImplicitly]
        private void LateUpdate()
        {
            UpdatePixelationCamera();
        }

        private void UpdatePixelationCamera()
        {
            if (_gameRenderCamera == null || _pixelationCamera == null) return;

            _pixelationCamera.orthographicSize = _gameRenderCamera.orthographicSize;
            
            TryRecalculateRenderTexture();
        }

        private void TryRecalculateRenderTexture()
        {
            float orthographicSize = _pixelationCamera.orthographicSize;

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

            if (_pixelationRenderTexture != null)
                _pixelationRenderTexture.Release();
            
            _pixelationRenderTexture = new RenderTexture(width, height, 24)
            {
                filterMode = FilterMode.Point
            };
            _pixelationRenderTexture.Create();
            
            _pixelationCamera.targetTexture = _pixelationRenderTexture;
            _gameRenderImage.texture = _pixelationRenderTexture;
        }
    }
}