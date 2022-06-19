using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell
{
    public sealed class GameAssets : MonoBehaviour
    {
        [SerializeField] private Material _testMaterial;
        [SerializeField] [NonReorderable] private List<TextureLookup> _textureLookups = new List<TextureLookup>();

        public static Material TestMaterial => _singleton._testMaterial;

        private static GameAssets _singleton;
        
        private readonly Dictionary<TextureId, Texture2D> _textureDict = new Dictionary<TextureId, Texture2D>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            _singleton = null;
        }
        
        private void Awake()
        {
            _singleton = this;
            
            foreach (TextureLookup textureLookup in _textureLookups)
                _textureDict.Add(textureLookup.TextureId, textureLookup.Texture);
        }

        public static Texture2D GetTexture(in TextureId id)
        {
            return _singleton._textureDict[id];
        }

        [Serializable]
        private sealed class TextureLookup
        {
            [field: SerializeField] public TextureId TextureId { get; private set; }
            [field: SerializeField] public Texture2D Texture { get; private set; }
        }
    }
}