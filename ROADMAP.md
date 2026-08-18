# Singleton 로드맵

우선순위: `P0` 결함·안전성 → `P1` 핵심 구조 → `P2` API·성능 → `P3` 장기 확장

## Scene 샘플 검증 (완료, 2026-08-18)

- 사용자가 `SingletonBasicUsageSample` Scene을 직접 확인했습니다.
- Scene에 부착된 `SampleGameServiceHost`와 숨김 lifecycle relay 구성이 정상적으로 로드됨을 확인했습니다.
- 패키지 전반 문서화와 이미지 보강은 모든 패키지 안정화 완료 후 최종 일괄 문서화 단계에서 수행합니다.

## 작업 순서

1. **P0-01 — Domain Reload와 종료 상태 테스트** (완료, 2026-08-18)
   - Play Mode 재진입, 앱 종료, 파괴된 Unity 객체의 정적 참조를 검증합니다.
   - `SubsystemRegistration`에서 닫힌 제네릭 타입별 정적 상태를 초기화합니다.
   - Singleton 생명주기 테스트는 reflection을 사용하지 않고 실제 Unity 메시지가 실행되는 PlayMode
     fixture에서만 검증합니다.
   - 사용자가 Domain Reload 비활성화 상태에서 Play Mode 재진입을 직접 검증했습니다.
2. **P1-01 — 공개 Singleton 모델 단일화** (완료, 2026-08-18)
   - `SingletonScene<T>`와 `ScriptableObjectSingleton<T>`를 제거합니다.
   - 씬 수명은 `Singleton<T>.Lifetime` 정책으로 흡수합니다.
   - `Singleton<T>`는 Unity 객체 수명이 필요한 MonoBehaviour 호스트로 한정합니다.
   - 상속 가능한 Unity 메시지 대신 sealed lifecycle relay가 등록·초기화·해제를 전달합니다.
   - 일반 서비스는 일반 C# 객체와 서드파티 DI로 구성하고, DOTS/Burst 구현은 별도 ECS 계층으로
     분리합니다.
3. **P1-02 — 전역 검색·자동 생성 및 중복 선택 규칙** (완료, 2026-08-18)
   - 비활성 객체를 포함해 먼저 로드된 Scene, hierarchy, component 순서로 인스턴스를 결정합니다.
   - 기존 인스턴스가 이미 초기화됐다면 Additive Scene의 후발 중복보다 기존 인스턴스를 유지합니다.
   - 제거 대상과 유지 대상을 경고하고 중복 GameObject가 아닌 Singleton component만 제거합니다.
   - Scene unload dispose·재생성과 Additive 중복 선택을 별도 Scene 자산이나 Build Settings 없이
     실행하는 PlayMode 테스트를 추가했습니다.
   - 사용자가 Unity Test Runner에서 두 PlayMode 테스트의 통과를 확인했습니다.
4. **P1-03 — 참조 가능 영역 정책 검토** (도입하지 않음)
   - 전역 접근을 제한하는 Scope 정책은 싱글톤 계약과 충돌하고 구체적 수요가 없어 추가하지 않습니다.
   - 좁은 접근 경계가 필요한 소비자는 서드파티 DI 라이브러리와 명시적 의존성 주입을 사용합니다.
   - DI로 해결되지 않는 Unity authoring 요구가 확인될 때만 별도 ScriptableObject 정책 에셋으로
     재검토하며, `Singleton<T>` 자체에는 참조 Scope 옵션을 추가하지 않습니다.
5. **P2-01 — 중복 인스턴스 진단** (완료, 2026-08-18)
   - 자동 삭제 대신 원인과 선택 결과를 개발 빌드·Editor에서 명확히 보고합니다.
6. **P3-01 — 서비스 수명 어댑터** (Core에는 도입하지 않음)
   - 자체 DI 컨테이너는 구현하지 않습니다.
   - 실제 채택한 서드파티 DI 라이브러리와의 연동 필요성이 확인되면 선택적 어댑터로 검토합니다.
