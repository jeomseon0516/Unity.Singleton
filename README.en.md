# Jeomseon Unity Singleton

Provides a global MonoBehaviour host for objects that require Unity lifecycle integration.

## Requirements

- Unity 6000.6.0f1 or newer

## Installation

```json
{
  "dependencies": {
    "com.jeomseon.unity.singleton": "0.1.0"
  }
}
```

## Scope

Use `Singleton<T>` only for Unity hosts that require a GameObject, Inspector references, coroutines, or Unity
lifecycle callbacks. Implement ordinary services as plain C# interfaces and classes, and register their lifetime
with a third-party DI container. This package does not provide a DI container, service locator, or reference-scope
policy.

An inherited Unity message does not drive singleton registration. A sealed `SingletonLifecycleRelay` owns the
Unity callbacks, so a derived host may declare its own `Awake()` without replacing singleton initialization. Put
singleton setup and cleanup in `OnSingletonInitialize()` and `OnSingletonDispose()`. The active lifecycle policy
and initialization state are available through `Lifetime` and `IsInitialized`.

Plain C# classes are still managed objects. DOTS and Burst workloads require a separate implementation based on
ECS systems, components, and Burst-compatible unmanaged data.

## License

[MIT License](./LICENSE.md)
