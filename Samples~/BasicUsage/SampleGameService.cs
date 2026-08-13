using Jeomseon.Unity.Singleton;
using UnityEngine;

namespace Jeomseon.Samples.Singleton
{
    public sealed class SampleGameService : Singleton<SampleGameService>
    {
        public bool IsReady { get; private set; }

        protected override void Init()
        {
            IsReady = true;
            Debug.Log("SampleGameService 초기화");
        }

        [ContextMenu("Singleton 상태 출력")]
        private void PrintState()
        {
            Debug.Log($"Singleton 준비 상태: {Instance.IsReady}");
        }
    }
}
