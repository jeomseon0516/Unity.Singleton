using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jeomseon.Unity.Singleton
{
    internal static class SingletonRuntime
    {
        private static readonly List<Action> ResetStaticStateActions = new();

        internal static bool Register(Action resetStaticState)
        {
            ResetStaticStateActions.Add(resetStaticState);
            return true;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticState()
        {
            foreach (Action resetStaticState in ResetStaticStateActions)
            {
                resetStaticState();
            }

            SceneManager.sceneLoaded -= PrepareLoadedSceneSingletons;
            SceneManager.sceneLoaded += PrepareLoadedSceneSingletons;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void PrepareInitialSceneSingletons()
        {
            var behaviours = UnityEngine.Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include);

            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour is not ISingletonLifecycle lifecycle) continue;

                SingletonLifecycleRelay.EnsureAttached(lifecycle);
                lifecycle.InitializeSingleton();
            }
        }

        private static void PrepareLoadedSceneSingletons(Scene scene, LoadSceneMode loadSceneMode)
        {
            foreach (GameObject rootGameObject in scene.GetRootGameObjects())
            {
                var behaviours = rootGameObject.GetComponentsInChildren<MonoBehaviour>(includeInactive: true);
                foreach (MonoBehaviour behaviour in behaviours)
                {
                    if (behaviour is not ISingletonLifecycle lifecycle) continue;

                    SingletonLifecycleRelay.EnsureAttached(lifecycle);
                    lifecycle.InitializeSingleton();
                }
            }
        }
    }
}
