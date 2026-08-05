# Jeomseon Unity Singleton

JeomseonScriptPack의 Singleton 기능을 독립된 Unity Package Manager 패키지로 제공합니다.

## 요구 사항

- Unity 6000.5.7f1 이상

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

- 씬 수명을 따르는 `SingletonScene<T>`
- 씬 전환 후에도 유지되는 `Singleton<T>`
- Resources 에셋을 사용하는 `ScriptableObjectSingleton<T>`

## 테스트

패키지를 `testables`에 등록한 후 Unity Test Runner의 EditMode에서 실행합니다.

## 라이선스

[MIT License](./LICENSE.md)
