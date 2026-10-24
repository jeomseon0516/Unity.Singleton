# Jeomseon Unity Singleton

[한국어](README.md) | English

Provides a single MonoBehaviour host and initialize/dispose hooks where Unity object lifetime is needed.

## Quick start

Derive a Host from Singleton<T>, implement OnSingletonInitialize/OnSingletonDispose, choose Scene or persistent lifetime, and access Instance. A separate Relay connects Unity messages.

Complete sample: [SampleGameServiceHost.cs](Samples~/BasicUsage/SampleGameServiceHost.cs).

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

## Contracts and limitations

Prefer explicit injection/external DI for plain C# services. This is not a DI container or access-scope boundary. Verify PlayMode re-entry with Domain Reload disabled.

## Requirements

- Unity `6000.6.0f1`
- Package: `com.jeomseon.unity.singleton` `0.3.1`

## Installation

Merge this into your project's `Packages/manifest.json`; preserve existing dependencies and registries.

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

Or use **Install package from git URL** in Package Manager:

```text
https://github.com/jeomseon0516/Unity.Singleton.git#v0.3.1
```

## API usage and details

### Scope

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

## Samples

Import the desired entry from **Package Manager > Samples**. Use the included Scene/Editor tool and complete its Setup step first, if required.

- [Basic Usage](Samples~/BasicUsage/README.md)
  - [SingletonBasicUsageSample](Samples~/BasicUsage/SingletonBasicUsageSample.unity)

## Tests

[Test sources](Tests)

Add the package ID to manifest `testables`, then execute its relevant EditMode/PlayMode assemblies in Unity Test Runner.

## Further reading

[Complete source map and package metadata](Documentation~/Overview.en.md)
- [Changelog](CHANGELOG.md)

## License

[License](LICENSE.md)
