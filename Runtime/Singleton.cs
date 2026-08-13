using UnityEngine;

namespace Jeomseon.Unity.Singleton
{
    [DisallowMultipleComponent]
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;
        private static bool _quitting;
        private bool _initialized;

        /* TODO(P1-01, architecture): 전역 정적 인스턴스 검색과 자동 GameObject 생성을
         * ScriptableObject 설정 또는 명시적인 부트스트랩 방식으로 대체할지 검토해야 합니다.
         * Enter Play Mode Options에서 도메인 리로드를 끈 경우의 정적 상태 초기화도 지원해야 합니다.
         * Project Settings에서 자동 생성 허용, DontDestroyOnLoad, 중복 처리와 초기화 순서를
         * 타입별로 선택하게 하고 빌드 전 누락·중복 인스턴스를 검사하는 Editor 검증기를 제공합니다.
         */
        public static T Instance
        {
            get
            {
                if (_quitting) return null; // 종료 중엔 생성/접근 회피

                if (_instance == null)
                {
                    var found = FindObjectsByType<T>(
                        FindObjectsInactive.Include,
                        FindObjectsSortMode.None);
                    if (found != null && found.Length > 0)
                    {
                        _instance = found[0];
                        _instance.EnsureInitialized();
                    }
                    else
                    {
                        var go = new GameObject(typeof(T).Name);
                        _instance = go.AddComponent<T>();
                        _instance.EnsureInitialized();
                    }
                }
                return _instance;
            }
        }

        // Awake는 되도록 건드리지 않게 하고, 실수 방지를 위해 protected internal로 공개
        protected internal void Awake()
        {
            // 자신 포함 모든 중복 수집
            var instances = FindObjectsByType<T>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

            if (_instance == null)
            {
                _instance = (T)this;
                gameObject.name = typeof(T).Name; // 통일된 이름
                EnsureInitialized();
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                // 같은 GameObject에 T가 여러 개 붙은 경우 등 부작용 최소화를 위해 "컴포넌트만" 제거
                // 단, 이미 DontDestroyOnLoad 된 루트와 경합 시 현재 컴포넌트만 제거
                Destroy(this);
                return;
            }

            // 혹시 모를 중복 컴포넌트들 정리 (자기 자신 제외)
            if (instances != null && instances.Length > 1)
            {
                foreach (var inst in instances)
                {
                    if (inst == _instance) continue;
                    // 같은 오브젝트에 여러 개 붙어 있어도 안전하게 컴포넌트만 제거
                    Destroy(inst);
                }
            }
        }

        protected virtual void OnApplicationQuit()
        {
            _quitting = true;
        }

        private void EnsureInitialized()
        {
            if (_initialized) return;
            _initialized = true;
            Init();
            DontDestroyOnLoad(gameObject); // 보장
        }

        // 자식이 반드시 구현
        protected abstract void Init();

        // 외부에서 new() 불가하지만, 명시적 기본 생성자 유지
        protected Singleton() { }
    }
}
