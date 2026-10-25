# Jeomseon Unity Singleton

[한국어](Overview.md) · [Installation](../README.en.md)

## Purpose

Provides a single MonoBehaviour host and initialize/dispose hooks where Unity object lifetime is needed.

## Usage flow

Derive a Host from Singleton<T>, implement OnSingletonInitialize/OnSingletonDispose, choose Scene or persistent lifetime, and access Instance. A separate Relay connects Unity messages.

## Contracts and limitations

Prefer explicit injection/external DI for plain C# services. This is not a DI container or access-scope boundary. Verify PlayMode re-entry with Domain Reload disabled.

## Package requirements

- Package ID: `com.jeomseon.unity.singleton`
- Version: `0.3.1`
- Unity: `6000.6` / `0f1`

## Samples and verification

Import the following from Package Manager > Samples. Open the included Scene or Editor tool; follow any Setup step in its sample README.

- [Basic Usage](../Samples~/BasicUsage/README.md)
  - [SingletonBasicUsageSample](../Samples~/BasicUsage/SingletonBasicUsageSample.unity)

[Test sources](../Tests)

Add the package ID to manifest `testables`, then run the relevant Unity Test Runner assemblies. These links describe available tests, not a new successful test run.

## Source map

- [Singleton](../Runtime/Singleton.cs)
- [SingletonLifecycleRelay](../Runtime/SingletonLifecycleRelay.cs)
- [SingletonLifetime](../Runtime/SingletonLifetime.cs)
