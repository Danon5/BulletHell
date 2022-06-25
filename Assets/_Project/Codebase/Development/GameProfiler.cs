using System.Text;
using JetBrains.Annotations;
using Unity.Profiling;
using UnityEngine;

namespace BulletHell.Development
{
    public sealed class GameProfiler : MonoBehaviour
    {
        [SerializeField] private Vector2 _boxSize;
        [SerializeField] private GUIStyle _boxStyle;
        private ProfilerRecorder _gcMemoryRecorder;
        private ProfilerRecorder _mainThreadTimeRecorder;
        private string _statsText;
        private ProfilerRecorder _systemMemoryRecorder;

        [UsedImplicitly]
        private void Update()
        {
            double frameTime = GetRecorderFrameAverage(_mainThreadTimeRecorder) * 1e-6f;

            StringBuilder sb = new StringBuilder(500);
            sb.AppendLine($"Frame Rate: {1000f / frameTime:F1} FPS");
            //sb.AppendLine($"Frame Time: {frameTime:F1} ms");
            sb.AppendLine($"GC Memory: {_gcMemoryRecorder.LastValue / (1024 * 1024)} MB");
            sb.AppendLine($"System Memory: {_systemMemoryRecorder.LastValue / (1024 * 1024)} MB");
            _statsText = sb.ToString();
        }

        [UsedImplicitly]
        private void OnEnable()
        {
            _systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
            _gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
            _mainThreadTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread", 15);
        }

        [UsedImplicitly]
        private void OnDisable()
        {
            _systemMemoryRecorder.Dispose();
            _gcMemoryRecorder.Dispose();
            _mainThreadTimeRecorder.Dispose();
        }

        [UsedImplicitly]
        private void OnGUI()
        {
            GUI.Box(new Rect(25, 25, _boxSize.x, _boxSize.y), _statsText, _boxStyle);
        }

        private static double GetRecorderFrameAverage(ProfilerRecorder recorder)
        {
            int samplesCount = recorder.Capacity;
            if (samplesCount == 0)
                return 0;

            double r = 0;
            unsafe
            {
                ProfilerRecorderSample* samples = stackalloc ProfilerRecorderSample[samplesCount];
                recorder.CopyTo(samples, samplesCount);
                for (int i = 0; i < samplesCount; ++i)
                    r += samples[i].Value;
                r /= samplesCount;
            }

            return r;
        }
    }
}