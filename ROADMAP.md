# Singleton 로드맵

우선순위: `P0` 결함·안전성 → `P1` 핵심 구조 → `P2` API·성능 → `P3` 장기 확장

## 작업 순서

1. **P0-01 — Domain Reload와 종료 상태 테스트**
   - Play Mode 재진입, 앱 종료, 파괴된 Unity 객체의 정적 참조를 검증합니다.
2. **P1-01 — 전역 검색·자동 생성 정책 교체**
   - 명시적 Bootstrap 또는 등록 방식으로 전환하고 자동 생성은 선택 정책으로 둡니다.
3. **P1-02 — Additive Scene 선택 규칙**
   - 비활성 객체와 여러 씬의 중복 인스턴스 중 어느 것을 사용할지 명시합니다.
4. **P1-03 — ScriptableObjectSingleton 로딩 규약 개선**
   - Reflection `FILE_PATH` 규약 대신 명시적인 설정, Resources 또는 Addressables 전략을 선택합니다.
5. **P2-01 — 중복 인스턴스 진단**
   - 자동 삭제 대신 원인과 선택 결과를 개발 빌드·Editor에서 명확히 보고합니다.
6. **P3-01 — 서비스 수명 어댑터**
   - 모든 서비스에 Singleton 상속을 강제하지 않는 인터페이스·DI 연동을 검토합니다.
