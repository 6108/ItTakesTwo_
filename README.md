# ItTakesTwo

### 게임

제목: It Takes Two

빌드: Window

조작: 마우스, 키보드, 조이스틱 

내용: 키보드와 조이스틱을 이용하여 진행하는 2인 멀티 게임

꿀 무기와 불 무기를 적절히 사용하여 라운드를 통과하고

최종적으로 보스인 벌을 잡는 게임입니다.

---

### 개발

인원: 프로그래머 2인

기간: 1개월

사용 프로그램: Unity3D(C#)

---
### 코드 요약
`GameManager.cs` 플레이어 사망, 부활 / 게임 종료, 시작 관리

`MenuManager.cs` 시작 화면 메뉴(플레이어 준비 완료 확인) 관리

`RailManager.cs`  레일을 사용하고 있는 플레이어 체크

`Player.cs` 꿀을 무기로 사용하는 플레이어의 조작(이동, 공격)

`Player2.cs` 불을 무기로 사용하는 플레이어의 조작(이동, 공격)

`SoloPlayer.cs` 테스트용 플레이어, 1인 조작

`Rail.cs` 어떤 플레이어가 레일에 닿았는지 체크

`RailEnemy.cs` 레일 위를 달리는 수레. 플레이어와 충동할 시 데미지 입힘

`RailEnemyTrigger.cs` 해당 트리거에 닿는 순간부터 수레 생성됨

`RailObject.cs` 플레이가 반대편 레일로 이동할 수 있게 함

`Bridge.cs` 붙어있는 꿀의 개수에 따라 각도가 바뀜

`Counter.cs` 오브젝트에 붙어있는 꿀의 개수 카운팅

`DeathZone.cs` 플레이어가 닿으면 죽는 구간

`DoorPulley.cs` 불에 맞으면 문이 열리는 도르래

`NextStage.cs` 다음 라운드로 이동

`EnemyCheck.cs` 모든 적을 배열에 담아서 무기에 자동 추적 기능에 사용

`Explosion.cs` 꿀과 불이 만나면 터지는데, 이 때 힘을 받아 날아갈 수 있게 함

`FireBullet.cs` 꿀에 만나면 터짐

`Honey.cs` 같은 곳을 여러번 공격하면 꿀이 뭉쳐 크기가 커짐

`HoneyBullet.cs` 베지어를 사용하여 곡선으로 날아갈 수 있게 함, 에임과 가까운 적 자동 타격

`Impulse.cs` 폭발할 때 화면 흔들림
