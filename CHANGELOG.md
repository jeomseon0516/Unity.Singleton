# 변경 기록

## [0.2.0] - 2026-08-13

- **(Breaking)** 네임스페이스를 `Jeomseon.Singleton` → `Jeomseon.Unity.Singleton`으로 변경했습니다.
  워크스페이스 전체 네임스페이스 규칙(패키지 rootNamespace는 `Jeomseon.Unity.<패키지 폴더명>`,
  `AGENTS.md` 참고)을 적용한 것으로, 폴더 구조 변경은 없습니다.

## [0.1.2] - 2026-07-29

- asmdef의 `rootNamespace`와 Singleton 파일 위치를 namespace에 맞게 정리했습니다.

## [0.1.1] - 2026-07-29

- MonoBehaviour Singleton 초기화를 확인하는 `Basic Usage` 샘플을 추가했습니다.

## [Unreleased]

### Changed

- `Singleton<T>`에 애플리케이션/씬 생명주기 정책과 공개 `Lifetime` 조회 API를 추가했습니다.
- `Singleton<T>`를 Unity 생명주기 호스트로 한정하고 공개 초기화 훅을 `Init()`에서
  `OnSingletonInitialize()`로 변경했습니다.
- 기반 `Awake()`/`OnDestroy()`/`OnApplicationQuit()`을 완전히 제거하고, sealed
  `SingletonLifecycleRelay`가 Unity 메시지를 전달하도록 변경했습니다. 파생 타입의 같은 이름 메시지가
  Singleton 초기화를 가리지 않습니다.
- 명시적인 `OnSingletonDispose()` 훅과 공개 `IsInitialized` 상태를 추가했습니다.
- Basic Usage 샘플을 일반 C# 서비스와 Singleton MonoBehaviour 호스트의 조합으로 변경했습니다.
- Singleton 테스트를 모두 PlayMode로 전환해 reflection 없이 실제 `Awake()`/파괴/application quit
  메시지를 검증합니다.
- Domain Reload 비활성화 상태에서도 Play Mode 세션마다 정적 인스턴스와 종료 상태를 초기화합니다.

### Removed

- `SingletonScene<T>`와 `ScriptableObjectSingleton<T>`를 제거했습니다.

### Added

- 즉시 실행 가능한 `SingletonBasicUsageSample` Scene과 마이그레이션 문서를 추가했습니다.

## [0.1.0] - 2026-07-29

### Added

- JeomseonScriptPack에서 Singleton 모듈을 최초 분리했습니다.
- 씬, 영구 MonoBehaviour 및 ScriptableObject 싱글턴 기반 타입을 추가했습니다.
- EditMode 단위 테스트를 추가했습니다.


## [0.1.3] - 2026-08-05

- Unity 6000.5.7f1을 최소 지원 버전으로 상향했습니다.
