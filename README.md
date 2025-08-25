# For-Submission
프로젝트에서 제가 작성한 스크립트 제출용입니다

저작권이 있는 에셋을 포함시키지 않기 위해 스크립트만 따로 제출하는 레포지토리입니다

아래는 빌드 파일을 실행시키기 위한 링크입니다.
https://drive.google.com/file/d/1JG1akxKOh-EYhb07p84_bXnLRH5ysh7c/view?usp=sharing

# 미궁 다이스

## 📌 개요
- 장르: (예: 모바일 턴제 SRPG)
- 개발 기간: (2025.07 - 2025.08)
- 팀 규모: (예: 6인 프로젝트)
- 본인 역할: (예: 스테이지 매니저 및 주요 게임 시스템 개발)

---

## 🔧 담당 업무 및 기여
### 주요 담당
- StageManager: 전반적인 스테이지 로직 구현
- StageData / ChapterData: ScriptableObject 기반 정적 데이터 관리
- StageSaveData: Json 기반 저장/로드 처리
- BattleUIController: 스테이지 UI와 로직 연결

### 보조 담당
- Enemy Prefab: 애니메이션 및 애니메이터 설정
- UI 연동: 로비의 캐릭터 보유창, 인벤토리 UI 데이터 디스플레이

---

## 🖥️ 결과물 스크린샷
(이미지들은 `images/` 폴더에 넣고 참조하세요)

![캐릭터 리스트 화면](Images/CharacterList.png)
![인벤토리 화면](Images/Inventory.png)
![층 선택 화면](Images/FloorSelect.png)
![캐릭터 셀렉 화면](Images/CharacterSelect.png)
![구매 화면](Images/Buy.png)
![회복 화면](Images/Heal.png)

---
## 🚀 배운 점 & 성과

- 데이터 구조 개선: 아티팩트 세트 효과를 List에서 HashSet으로 리팩토링하여 중복 처리 문제를 방지.
- 아키텍처 개선: 프리팹이 직접 데이터를 다루지 않고, 중앙 매니저에서만 데이터 처리를 담당하도록 수정. 이를 통해 애니메이션 방식과 관계없이 안정적인 데이터 처리 가능.
- 데이터 직렬화 경험: 직렬화된 클래스를 활용하여 계층적 데이터 구조를 설계.
- 성능 최적화: FindAnyObjectByType 및 FindWithTag 사용으로 빌드 환경에서 발생한 성능 저하 문제를, 사전 할당 방식으로 개선.
- 자원 관리 개선: 캐릭터 셀렉트 기능에서 매번 객체를 생성·제거하던 방식을 오브젝트 풀링(Object Pooling)으로 전환하여 성능 향상 시도.


