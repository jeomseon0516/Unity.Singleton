# Singleton 기본 예제

`SingletonBasicUsageSample` Scene을 열고 Play Mode에 진입합니다.

1. `SampleGameServiceHost`가 일반 C# `SampleGameService`를 생성하는 구조인지 확인합니다.
2. Console에 `SampleGameService 초기화`가 한 번만 출력되는지 확인합니다.
   `SampleGameServiceHost 자체 Awake 호출`도 함께 출력되어 파생 `Awake()`와 Singleton 초기화가
   독립적으로 실행되는지 확인합니다.
3. `Sample Game Service Host` 컴포넌트의 Context Menu에서 `Singleton 상태 출력`을 실행합니다.
4. Scene을 전환해도 기본 `Application` 수명의 호스트가 유지되는지 확인합니다.
5. Domain Reload를 끈 상태에서 Play Mode를 종료했다가 다시 진입해 초기화 로그가 다시 한 번 출력되는지 확인합니다.
