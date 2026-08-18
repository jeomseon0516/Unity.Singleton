# Migration from 0.2.0

## `SingletonScene<T>` 제거

씬 수명이 필요한 타입은 `Singleton<T>`를 상속하고 생명주기만 재정의합니다.

```csharp
public sealed class SceneService : Singleton<SceneService>
{
    public override SingletonLifetime Lifetime => SingletonLifetime.Scene;

    protected override void OnSingletonInitialize()
    {
    }
}
```

## `Singleton<T>.Init()` 이름 변경

Unity 메시지인 `Awake()`와 Singleton 초기화 지점을 구분하기 위해 `Init()`을
`OnSingletonInitialize()`로 변경했습니다. Singleton 초기화는 이 훅을 재정의합니다.

Unity 메시지는 상속되지 않는 `SingletonLifecycleRelay`가 담당합니다. 따라서 파생 타입이 자체
`Awake()`/`OnDestroy()`를 선언해도 Singleton 기반 생명주기를 가리지 않습니다. Singleton이 소유한
리소스 해제는 `OnSingletonDispose()`를 재정의합니다. 기존 Scene 인스턴스에 relay가 직렬화되어
있지 않아도 첫 Scene은 `BeforeSceneLoad`, Additive Scene은 `sceneLoaded` 보정 경로가 연결합니다.

## `ScriptableObjectSingleton<T>` 제거

설정 에셋은 사용 목적에 따라 다음 공식 경로 중 하나로 전환합니다.

- Inspector 직렬화 참조
- 명시적인 `Resources.Load<T>()`
- Addressables
- Preloaded Assets 또는 기능 패키지 전용 Settings Provider

이 패키지는 더 이상 Reflection `FILE_PATH` 규약, Resources 경로 강제 또는 프로퍼티 접근 중
Editor 에셋 자동 생성을 제공하지 않습니다.
