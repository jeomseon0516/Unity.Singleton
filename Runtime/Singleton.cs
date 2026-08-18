using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jeomseon.Unity.Singleton
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SingletonLifecycleRelay))]
    public abstract class Singleton<T> : MonoBehaviour, ISingletonLifecycle where T : Singleton<T>
    {
        private static T _instance;
        private static bool _quitting;
        private static readonly bool StaticStateRegistered = SingletonRuntime.Register(ResetStaticState);

        public static bool HasInstance => _instance;

        public virtual SingletonLifetime Lifetime => SingletonLifetime.Application;

        public bool IsInitialized { get; private set; }

        MonoBehaviour ISingletonLifecycle.Behaviour => this;

        public static T Instance
        {
            get
            {
                _ = StaticStateRegistered;

                if (_quitting) return null; // 종료 중엔 생성/접근 회피

                if (_instance == null)
                {
                    var found = FindObjectsByType<T>(FindObjectsInactive.Include);
                    if (found != null && found.Length > 0)
                    {
                        _instance = SelectPreferredInstance(found);
                        ((ISingletonLifecycle)_instance).InitializeSingleton();
                    }
                    else
                    {
                        var go = new GameObject(typeof(T).Name);
                        _instance = go.AddComponent<T>();
                    }

                    SingletonLifecycleRelay.EnsureAttached(_instance);
                    _instance.EnsureInitialized();
                }

                _instance.EnsureInitialized();
                return _instance;
            }
        }

        void ISingletonLifecycle.InitializeSingleton()
        {
            // 자신 포함 모든 중복 수집
            var instances = FindObjectsByType<T>(FindObjectsInactive.Include);

            if (_instance == null)
            {
                _instance = SelectPreferredInstance(instances);
            }

            if (_instance == this)
            {
                EnsureInitialized();
            }
            else
            {
                // 같은 GameObject에 T가 여러 개 붙은 경우 등 부작용 최소화를 위해 "컴포넌트만" 제거
                // 단, 이미 DontDestroyOnLoad 된 루트와 경합 시 현재 컴포넌트만 제거
                Debug.LogWarning(
                    $"Duplicate {typeof(T).Name} instances were found. " +
                    $"Keeping '{GetObjectPath(_instance)}' and removing the duplicate component " +
                    $"from '{GetObjectPath((T)this)}'.",
                    this);
                Destroy(this);
                return;
            }

            // 혹시 모를 중복 컴포넌트들 정리 (자기 자신 제외)
            if (instances != null && instances.Length > 1)
            {
                Debug.LogWarning(
                    $"Duplicate {typeof(T).Name} instances were found. " +
                    $"Keeping '{GetObjectPath(_instance)}' and removing {instances.Length - 1} " +
                    "duplicate component(s).",
                    _instance);

                foreach (var inst in instances)
                {
                    if (inst == _instance) continue;
                    // 같은 오브젝트에 여러 개 붙어 있어도 안전하게 컴포넌트만 제거
                    Destroy(inst);
                }
            }
        }

        private static T SelectPreferredInstance(T[] instances)
        {
            T preferred = instances[0];
            for (int i = 1; i < instances.Length; i++)
            {
                if (CompareSceneOrder(instances[i], preferred) < 0)
                {
                    preferred = instances[i];
                }
            }

            return preferred;
        }

        private static int CompareSceneOrder(T left, T right)
        {
            int sceneOrder = GetLoadedSceneOrder(left.gameObject.scene)
                .CompareTo(GetLoadedSceneOrder(right.gameObject.scene));
            if (sceneOrder != 0) return sceneOrder;

            int hierarchyOrder = CompareHierarchyOrder(left.transform, right.transform);
            if (hierarchyOrder != 0) return hierarchyOrder;

            var components = left.GetComponents<T>();
            return System.Array.IndexOf(components, left).CompareTo(System.Array.IndexOf(components, right));
        }

        private static int GetLoadedSceneOrder(Scene scene)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i) == scene) return i;
            }

            return int.MaxValue;
        }

        private static int CompareHierarchyOrder(Transform left, Transform right)
        {
            int leftDepth = GetDepth(left);
            int rightDepth = GetDepth(right);
            Transform leftCursor = left;
            Transform rightCursor = right;

            while (leftDepth > rightDepth)
            {
                leftCursor = leftCursor.parent;
                leftDepth--;
            }

            while (rightDepth > leftDepth)
            {
                rightCursor = rightCursor.parent;
                rightDepth--;
            }

            if (leftCursor == rightCursor)
            {
                return GetDepth(left).CompareTo(GetDepth(right));
            }

            while (leftCursor.parent != rightCursor.parent)
            {
                leftCursor = leftCursor.parent;
                rightCursor = rightCursor.parent;
            }

            return leftCursor.GetSiblingIndex().CompareTo(rightCursor.GetSiblingIndex());
        }

        private static int GetDepth(Transform transform)
        {
            int depth = 0;
            while (transform.parent != null)
            {
                depth++;
                transform = transform.parent;
            }

            return depth;
        }

        private static string GetObjectPath(T instance) =>
            $"{instance.gameObject.scene.name}/{instance.gameObject.name}";

        void ISingletonLifecycle.NotifyApplicationQuit()
        {
            _quitting = true;
        }

        void ISingletonLifecycle.DisposeSingleton()
        {
            if (_instance != this) return;

            if (IsInitialized)
            {
                OnSingletonDispose();
                IsInitialized = false;
            }

            _instance = null;
        }

        private void EnsureInitialized()
        {
            if (IsInitialized) return;

            OnSingletonInitialize();
            IsInitialized = true;

            if (Lifetime == SingletonLifetime.Application)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        private static void ResetStaticState()
        {
            _instance = null;
            _quitting = false;
        }

        /// <summary>
        /// Unity 메시지를 재정의하지 않고 싱글톤 호스트를 초기화하는 지점입니다.
        /// </summary>
        protected abstract void OnSingletonInitialize();

        /// <summary>
        /// 현재 싱글톤 호스트가 파괴될 때 보유한 리소스를 해제하는 지점입니다.
        /// </summary>
        protected virtual void OnSingletonDispose() { }

        // 외부에서 new() 불가하지만, 명시적 기본 생성자 유지
        protected Singleton() { }
    }
}
