using System;
using System.Collections.Generic;
using BulletHell.Animation;
using JetBrains.Annotations;
using UnityEngine;

namespace BulletHell
{
    public sealed class GameAssets : MonoBehaviour
    {
        private static GameAssets _singleton;
        [SerializeField] private Material _testMaterial;

        [SerializeField] [NonReorderable] private List<TextureLookup> _textureLookups = new List<TextureLookup>();

        [SerializeField] [NonReorderable] private List<AnimationData> _animationData = new List<AnimationData>();

        private readonly Dictionary<AnimationId, AnimationData> _animationDataDict =
            new Dictionary<AnimationId, AnimationData>();

        private readonly Dictionary<TextureId, Texture2D> _textureDict = new Dictionary<TextureId, Texture2D>();

        public static Material TestMaterial => _singleton._testMaterial;
        
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

            foreach (TextureLookup textureLookup in _textureLookups)
                _textureDict.Add(textureLookup.TextureId, textureLookup.Texture);

            foreach (AnimationData animationData in _animationData)
                _animationDataDict.Add(animationData.AnimationId, animationData);
        }
        
        public static Texture2D GetTexture(in TextureId id)
        {
            return _singleton._textureDict[id];
        }

        public static AnimationData GetAnimationData(in AnimationId id)
        {
            return _singleton._animationDataDict[id];
        }

        [Serializable]
        private sealed class TextureLookup
        {
            [field: SerializeField] public TextureId TextureId { get; private set; }
            [field: SerializeField] public Texture2D Texture { get; private set; }
        }
    }
}