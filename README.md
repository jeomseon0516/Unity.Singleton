# Jeomseon Unity Singleton

한국어 | [English](README.en.md)

Unity 객체 수명이 필요한 MonoBehaviour 호스트의 단일 인스턴스와 초기화/해제 훅을 제공합니다.

## 빠르게 시작하기

Singleton<T> 파생 Host 작성 → OnSingletonInitialize/OnSingletonDispose 구현 → Scene 또는 지속 수명 선택 → Instance를 통해 접근. 별도 Relay가 Unity 수명 메시지를 연결합니다.

전체 샘플 코드: [SampleGameServiceHost.cs](Samples~/BasicUsage/SampleGameServiceHost.cs).

```csharp
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
```

## 계약과 주의사항

일반 C# 서비스에는 명시적 주입이나 외부 DI를 우선 고려하세요. 자체 DI나 접근 Scope 제한을 제공하지 않습니다. Domain Reload를 꺼도 재진입 상태를 검사하는 PlayMode 테스트가 필요합니다.

## 요구 사항

- Unity `6000.6.0f1`
- 패키지: `com.jeomseon.unity.singleton` `0.3.1`

## 설치

프로젝트 `Packages/manifest.json`의 기존 dependencies와 registry를 유지하면서 다음 항목을 병합합니다.

```json
{
  "scopedRegistries": [
    {
      "name": "OpenUPM",
      "url": "https://package.openupm.com",
      "scopes": [
        "com.jeomseon"
      ]
    }
  ],
  "dependencies": {
    "com.jeomseon.unity.singleton": "0.3.1"
  }
}
```

또는 Package Manager의 **Install package from git URL**을 사용합니다:

```text
https://github.com/jeomseon0516/Unity.Singleton.git#v0.3.1
```

## API 사용과 상세 설명

### 포함 기능

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

### 기본 사용

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

## 샘플 실행

**Package Manager > Samples**에서 예제를 Import합니다. 포함된 Scene 또는 Editor 도구를 열고 필요한 Setup 메뉴를 먼저 실행합니다.

- [Basic Usage](Samples~/BasicUsage/README.md)
  - [SingletonBasicUsageSample](Samples~/BasicUsage/SingletonBasicUsageSample.unity)

## 테스트

[테스트 코드](Tests)

manifest의 `testables`에 패키지 ID를 추가하고 Unity Test Runner의 관련 EditMode/PlayMode 어셈블리를 실행합니다.

## 추가 문서

[전체 코드 지도와 패키지 구성](Documentation~/Overview.md)
- [Changelog](CHANGELOG.md)

## 라이선스

[License](LICENSE.md)
