using UnityEngine;

namespace Jeomseon.Unity.Singleton
{
    internal interface ISingletonLifecycle
    {
        MonoBehaviour Behaviour { get; }

        void InitializeSingleton();

        void DisposeSingleton();

        void NotifyApplicationQuit();
    }
}
