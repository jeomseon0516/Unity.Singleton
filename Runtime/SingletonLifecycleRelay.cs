using System.ComponentModel;
using UnityEngine;

namespace Jeomseon.Unity.Singleton
{
    [AddComponentMenu("")]
    [DefaultExecutionOrder(-32000)]
    [DisallowMultipleComponent]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class SingletonLifecycleRelay : MonoBehaviour
    {
        internal static void EnsureAttached(ISingletonLifecycle lifecycle)
        {
            GameObject gameObject = lifecycle.Behaviour.gameObject;
            if (!gameObject.TryGetComponent(out SingletonLifecycleRelay _))
            {
                gameObject.AddComponent<SingletonLifecycleRelay>();
            }
        }

        private void Awake()
        {
            hideFlags |= HideFlags.HideInInspector;
            ForEachLifecycle(static lifecycle => lifecycle.InitializeSingleton());
        }

        private void Reset()
        {
            hideFlags |= HideFlags.HideInInspector;
        }

        private void Start()
        {
            ForEachLifecycle(static lifecycle => lifecycle.InitializeSingleton());
        }

        private void OnDestroy()
        {
            ForEachLifecycle(static lifecycle => lifecycle.DisposeSingleton());
        }

        private void OnApplicationQuit()
        {
            ForEachLifecycle(static lifecycle => lifecycle.NotifyApplicationQuit());
        }

        private void ForEachLifecycle(System.Action<ISingletonLifecycle> callback)
        {
            var behaviours = GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour is ISingletonLifecycle lifecycle)
                {
                    callback(lifecycle);
                }
            }
        }
    }
}
