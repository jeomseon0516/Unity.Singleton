# Jeomseon Unity Singleton

[English](Overview.en.md) · [설치](../README.md)

## 목적

Unity 객체 수명이 필요한 MonoBehaviour 호스트의 단일 인스턴스와 초기화/해제 훅을 제공합니다.

## 사용 흐름

Singleton<T> 파생 Host 작성 → OnSingletonInitialize/OnSingletonDispose 구현 → Scene 또는 지속 수명 선택 → Instance를 통해 접근. 별도 Relay가 Unity 수명 메시지를 연결합니다.

## 계약과 제한

일반 C# 서비스에는 명시적 주입이나 외부 DI를 우선 고려하세요. 자체 DI나 접근 Scope 제한을 제공하지 않습니다. Domain Reload를 꺼도 재진입 상태를 검사하는 PlayMode 테스트가 필요합니다.

## 패키지 요구 사항

- Package ID: `com.jeomseon.unity.singleton`
- Version: `0.3.1`
- Unity: `6000.6` / `0f1`

## 샘플과 검증

Package Manager > Samples에서 다음 예제를 Import하세요. 포함된 Scene 또는 Editor 도구를 열고 샘플 README의 Setup 절차를 먼저 수행합니다.

- [Basic Usage](../Samples~/BasicUsage/README.md)
  - [SingletonBasicUsageSample](../Samples~/BasicUsage/SingletonBasicUsageSample.unity)

[테스트 코드](../Tests)

소비 프로젝트 manifest의 `testables`에 패키지 ID를 추가하고 Unity Test Runner의 관련 어셈블리를 실행합니다. 위 링크는 테스트 위치이며 이번 문서 수정으로 새 테스트 실행을 주장하지 않습니다.

## 코드 지도

- [Singleton](../Runtime/Singleton.cs)
- [SingletonLifecycleRelay](../Runtime/SingletonLifecycleRelay.cs)
- [SingletonLifetime](../Runtime/SingletonLifetime.cs)
