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
- SoundManager(사운드 조절)
  - BGM/SFX 채널 분리 및 `AudioMixer` 그룹 구성
  - 슬라이더 UI ↔ `AudioMixer` Exposed Parameters 연동(개별 볼륨 조절)
  - 씬 전환 시 `DontDestroyOnLoad`로 지속 관리
  - 사용자 볼륨 설정 저장·로드(Json SaveData 혹은 PlayerPrefs)로 세션 간 유지

### 보조 담당
- Enemy Prefab: 애니메이션 및 애니메이터 설정
- UI 연동: 로비의 캐릭터 보유창, 인벤토리 UI 데이터 디스플레이

---

## 🖥️ 결과물 스크린샷

### 프로젝트에서 제가 작업한 영역의 결과물을 표시합니다.

---

![캐릭터 리스트 화면](Images/CharacterList.png)  
- 보유 캐릭터 리스트 데이터를 기반으로 캐릭터 슬롯 UI를 동적으로 생성  
- 각 슬롯에 캐릭터 스프라이트, 이름, 등급 등의 텍스트 정보를 매핑  
- 미보유 캐릭터도 동일한 방식으로 슬롯을 생성 

---

![기본 정보 화면](Images/BasicInfo.png)  
- 선택된 캐릭터의 기본 능력치(HP, 공격력, 방어력 등)를 UI에 출력  
- ScriptableObject 기반 캐릭터 데이터를 읽어 뷰어에 반영  

---

![레벨업 화면](Images/LevelUp.png)  
- 경험치 아이템 사용 시 캐릭터 능력치 증가를 계산하고 UI에 반영  
- 버튼 입력 → 데이터 반영 → UI 업데이트 순서로 동작  

---

![스킬 레벨업 화면](Images/SkillUp.png)  
- 스킬 강화 시 강화 전/후 수치를 비교하여 UI에 표시  
- 강화 비용(재화, 아이템)을 체크하고 조건 충족 시 강화 가능  
- 강화 성공 시 Json SaveData에 반영  

---

![인벤토리 화면](Images/Inventory.png)  
- 소지 아이템 목록을 리스트로 UI에 동적 생성  
- 아이템 상세 정보를 선택 영역에 표시  
- Json SaveData에 따라 보유 개수 동기화  

---

![층 선택 화면](Images/FloorSelect.png)  
- StageManager와 연동된 층 선택 UI  
- 버튼 입력에 따라 해당 층의 StageData를 로드하도록 구현  
- 보스 정보 및 속성 아이콘을 함께 표시  

---

![캐릭터 셀렉 화면](Images/CharacterSelect.png)  
- 전투 진입 시 파티 구성 UI  
- 보유 캐릭터 리스트를 불러와 선택 가능하게 구성  
- 선택된 캐릭터를 전투 Scene으로 전달  

---

![구매 화면](Images/Buy.png)  
- 상점 UI에서 아이템 구매 시 재화 차감 및 아이템 지급 로직 연결  
- 판매 아이템 리스트를 ScriptableObject 데이터 기반으로 표시  

---

![회복 화면](Images/Heal.png)  
- 힐러 NPC UI에서 선택된 캐릭터 HP 회복 처리  
- 단일/전체 회복, 부활 기능 등 다양한 회복 옵션 제공  
- 사용 시 보유 재화 차감 및 SaveData 업데이트

---
## 🎬 시연 영상

[![미궁 다이스 시연 영상](https://img.youtube.com/vi/G4RhUlvXA5w/0.jpg)](https://www.youtube.com/watch?v=G4RhUlvXA5w)


---
## 🚀 배운 점 & 성과

- 데이터 구조 개선: 아티팩트 세트 효과를 List에서 HashSet으로 리팩토링하여 중복 처리 문제를 방지.
- 아키텍처 개선: 프리팹이 직접 데이터를 다루지 않고, 중앙 매니저에서만 데이터 처리를 담당하도록 수정. 이를 통해 애니메이션 방식과 관계없이 안정적인 데이터 처리 가능.
- 데이터 직렬화 경험: 직렬화된 클래스를 활용하여 계층적 데이터 구조를 설계.
- 성능 최적화: FindAnyObjectByType 및 FindWithTag 사용으로 빌드 환경에서 발생한 성능 저하 문제를, 사전 할당 방식으로 개선.
- 자원 관리 개선: 캐릭터 셀렉트 기능에서 매번 객체를 생성·제거하던 방식을 오브젝트 풀링(Object Pooling)으로 전환하여 성능 향상 시도.


