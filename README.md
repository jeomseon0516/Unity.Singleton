# Jeomseon Unity Singleton

Unity 생명주기가 필요한 전역 MonoBehaviour 호스트를 제공합니다.

## 요구 사항

- Unity 6000.6.0f1 이상

## OpenUPM으로 설치

```json
{
  "dependencies": {
    "com.jeomseon.unity.singleton": "0.1.0"
  }
}
```

## Git URL로 설치

```text
https://github.com/jeomseon0516/Unity.Singleton.git#v0.1.1
```

## 포함 기능

- 기본적으로 씬 전환 후에도 유지되는 `Singleton<T>`
- `SingletonLifetime.Scene`을 선택할 수 있는 명시적 생명주기 정책
- 인스턴스의 공개 `Lifetime` 프로퍼티를 통한 현재 생명주기 정책 조회
- 공개 `IsInitialized` 프로퍼티를 통한 초기화 상태 조회
- Unity 메시지 대신 사용하는 `OnSingletonInitialize()`/`OnSingletonDispose()` 생명주기 훅
- Domain Reload 비활성화 Play Mode 재진입을 위한 정적 상태 초기화

`Singleton<T>`는 GameObject, Coroutine, Inspector 참조처럼 Unity 객체 수명이 필요한 호스트에만
사용합니다. 일반 게임 서비스는 일반 C# 인터페이스와 클래스로 작성하고 서드파티 DI 컨테이너에서
Singleton 또는 Scene scope로 등록하는 방식을 권장합니다. 이 패키지는 자체 DI 컨테이너나 서비스
로케이터를 제공하지 않습니다.

참조 가능 영역을 제한하는 Scope 정책도 제공하지 않습니다. `Instance`는 Unity 호스트의 전역 접근
계약을 유지하며, 더 좁은 접근 경계는 DI와 명시적 의존성 주입으로 표현합니다.

일반 C# 서비스는 managed object이므로 MonoBehaviour에서 분리하는 것만으로 Jobs/Burst 호환 객체가
되지는 않습니다. DOTS 경로는 `ISystem`과 ECS 컴포넌트, Burst 호환 unmanaged 데이터로 별도
구성합니다.

## 기본 사용

```csharp
public sealed class GameService
{
    public void Initialize() { }
}

public sealed class GameServiceHost : Singleton<GameServiceHost>
{
    public GameService Service { get; private set; }

    protected override void OnSingletonInitialize()
    {
        Service = new GameService();
        Service.Initialize();
    }

    protected override void OnSingletonDispose()
    {
        Service = null;
    }
}
```

파생 타입은 필요한 경우 자체 `Awake()` 등 Unity 메시지를 사용할 수 있습니다. Singleton 등록과
생명주기는 상속되지 않는 `SingletonLifecycleRelay`가 담당하므로 파생 메시지가 기반 초기화를
가리지 않습니다. Singleton 초기화와 해제 자체는 각각 `OnSingletonInitialize()`와
`OnSingletonDispose()`에서 수행합니다.

## 테스트

패키지를 `testables`에 등록한 후 Unity Test Runner의 PlayMode에서 실행합니다. Singleton 테스트는
실제 `Awake`/`OnDestroy`/`OnApplicationQuit` 메시지와 프레임 수명을 사용하며 EditMode 테스트를
제공하지 않습니다.

## 라이선스

[MIT License](./LICENSE.md)
