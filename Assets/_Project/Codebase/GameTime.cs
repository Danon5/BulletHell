using JetBrains.Annotations;
using UnityEngine;

namespace BulletHell
{
    [DefaultExecutionOrder(100)]
    public sealed class GameTime : MonoBehaviour
    {
        public static float Time => _singleton._time;

        private static GameTime _singleton;
        
        private float _time;

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
        }
        
        [UsedImplicitly]
        private void Update()
        {
            _time += GameConstants.TIMESTEP;
        }
    }
}