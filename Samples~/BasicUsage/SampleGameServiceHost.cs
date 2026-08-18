using Jeomseon.Unity.Singleton;
using UnityEngine;

namespace Jeomseon.Samples.Singleton
{
    public sealed class SampleGameServiceHost : Singleton<SampleGameServiceHost>
    {
        public SampleGameService Service { get; private set; }

        private void Awake()
        {
            Debug.Log("SampleGameServiceHost 자체 Awake 호출");
        }

        protected override void OnSingletonInitialize()
        {
            Service = new SampleGameService();
            Service.Initialize();
            Debug.Log("SampleGameService 초기화");
        }

        protected override void OnSingletonDispose()
        {
            Service?.Dispose();
            Service = null;
        }

        [ContextMenu("Singleton 상태 출력")]
        private void PrintState()
        {
            Debug.Log($"Singleton 준비 상태: {Instance.Service.IsReady}");
        }
    }
}
