﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using System.Linq;
using DG.Tweening;

public class CursePanel : MonoBehaviour
{
    [SerializeField, Range(0, 4)] private int selectedCharacterIndex;

    [Header("UI Elements")]
    [SerializeField] private Image[] characterButtons = new Image[5];
    [SerializeField] private Image[] characterIcons = new Image[5];
    [SerializeField] private Image[] characterElimentBgs = new Image[5];
    [SerializeField] private Image[] characterDices = new Image[5];
    [SerializeField] private TMP_Text conditionText; // 저주 조건 텍스트
    [SerializeField] private Image diceSelection; // 주사위 선택 이미지

    [Header("Dice Image Data")]
    [SerializeField] private Sprite[] fireDiceImage = new Sprite[6];
    [SerializeField] private Sprite[] waterDiceImage = new Sprite[6];
    [SerializeField] private Sprite[] rockDiceImage = new Sprite[6];
    [SerializeField] private Sprite[] grassDiceImage = new Sprite[6];
    [SerializeField] private Sprite[] electroDiceImage = new Sprite[6];

    private void OnEnable()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "BattleScene"
            && StageManager.Instance.stageSaveData.currentPhaseState != StageSaveData.CurrentPhaseState.CurseEvent)
        {
            gameObject.SetActive(false);
            return; // 전투씬의 저주이벤트 상태가 아니면 패널을 비활성화
        }
        if (StageManager.Instance.stageSaveData.UpOrDown == 0)
        {
            StageManager.Instance.stageSaveData.UpOrDown = Random.value > 0.5f ? 1 : -1;
            StageManager.Instance.stageSaveData.upAndDownNumber = Random.Range(1, 7);
            List<RandomEventData> events = StageManager.Instance.chapterData.chapterIndex[StageManager.Instance.stageSaveData.currentChapterIndex].stageData.stageIndex[StageManager.Instance.stageSaveData.currentStageIndex].RandomEvents;
            List<RandomEventData> curseEvents = events.FindAll(e => e.EventType == RandomEventType.Curse);
            StageManager.Instance.stageSaveData.randomEventData = curseEvents[Random.Range(0, curseEvents.Count)];
        }
        RefreshUI();
    }

    private void RefreshUI()
    {
        RefreshViewers();
        RefreshConditionText();
        UpdateCharacterSelection();
    }

    public void OnClickCharacterButton(int index)
    {
        if (index < 0 || index >= characterButtons.Length)
            return;
        selectedCharacterIndex = index;
        UpdateCharacterSelection();
    }

    private void RefreshViewers()
    {
        for (int i = 0; i < characterButtons.Length; i++)
        {
            characterIcons[i].sprite = StageManager.Instance.stageSaveData.battleCharacters[i].CharacterData.icon;
            characterElimentBgs[i].sprite = StageManager.Instance.stageSaveData.battleCharacters[i].CharacterData.BackGroundIcon;
            characterDices[i].sprite = StageManager.Instance.stageSaveData.battleCharacters[i].CharacterData.DiceNumIcon;
        }
    }

    private void UpdateCharacterSelection()
    {
        for (int i = 0; i < characterButtons.Length; i++)
        {
            characterButtons[i].enabled = (i == selectedCharacterIndex);
        }
    }

    private void RefreshConditionText()
    {
        if (StageManager.Instance.stageSaveData.UpOrDown == 0)
        {
            StageManager.Instance.stageSaveData.UpOrDown = Random.value > 0.5f ? 1 : -1;
            StageManager.Instance.stageSaveData.upAndDownNumber = Random.Range(1, 7);
            List<RandomEventData> events = StageManager.Instance.chapterData.chapterIndex[StageManager.Instance.stageSaveData.currentChapterIndex].stageData.stageIndex[StageManager.Instance.stageSaveData.currentStageIndex].RandomEvents;
            List<RandomEventData> curseEvents = events.FindAll(e => e.EventType == RandomEventType.Curse);
            StageManager.Instance.stageSaveData.randomEventData = curseEvents[Random.Range(0, curseEvents.Count)];
        }
        string defaultText = "[주사위 눈금이 {DiceNumber}보다 {UpAndDownText} 저주를 받습니다.]"; // {DiceNumber}는 주사위 눈금, {UpAndDownText}는 높다면/낮다면 텍스트
        string diceNumberText = StageManager.Instance.stageSaveData.upAndDownNumber.ToString();
        string upAndDownText = StageManager.Instance.stageSaveData.UpOrDown > 0 ? "높다면" : "낮다면";
        conditionText.text = defaultText
            .Replace("{DiceNumber}", diceNumberText)
            .Replace("{UpAndDownText}", upAndDownText);
    }

    public void OnClickDiceRollButton()
    {
        int determinedDiceNumber = Random.Range(1, 7);
        RandomEventData randomEventData = null;
        // 현재의 저주 상태에 따라 랜덤한 이벤트를 스테이지 데이터에서 하나 결정
        if (StageManager.Instance.stageSaveData.currentPhaseState == StageSaveData.CurrentPhaseState.CurseEvent)
        {
            var curses = StageManager.Instance.chapterData.chapterIndex[StageManager.Instance.stageSaveData.currentChapterIndex]
                .stageData.stageIndex[StageManager.Instance.stageSaveData.currentStageIndex].RandomEvents
                .Where(e => e.EventType == RandomEventType.Curse)
                .ToList();
            randomEventData = curses[Random.Range(0, curses.Count)];
        }
        else if (StageManager.Instance.stageSaveData.currentPhaseState == StageSaveData.CurrentPhaseState.BlessingEvent)
        {
            var blessings = StageManager.Instance.chapterData.chapterIndex[StageManager.Instance.stageSaveData.currentChapterIndex]
                .stageData.stageIndex[StageManager.Instance.stageSaveData.currentStageIndex].RandomEvents
                .Where(e => e.EventType == RandomEventType.Blessing)
                .ToList();
            randomEventData = blessings[Random.Range(0, blessings.Count)];
        }

        while (StageManager.Instance.stageSaveData.selectedRandomEvents.Count < 4)
        {
            StageManager.Instance.stageSaveData.selectedRandomEvents.Add(null);
        }
        while (StageManager.Instance.stageSaveData.selectedRandomEvents.Count > 4)
        {
            StageManager.Instance.stageSaveData.selectedRandomEvents.RemoveAt(StageManager.Instance.stageSaveData.selectedRandomEvents.Count - 1);
        }
        switch (StageManager.Instance.stageSaveData.UpOrDown)
        {
            case 1: // 높다면
                if (determinedDiceNumber >= StageManager.Instance.stageSaveData.upAndDownNumber)
                {
                    for (int i = 0; i < StageManager.Instance.stageSaveData.selectedRandomEvents.Count; i++)
                    {
                        if (StageManager.Instance.stageSaveData.selectedRandomEvents[i] == null)
                        {
                            StageManager.Instance.stageSaveData.selectedRandomEvents[i] = randomEventData;
                            break;
                        }
                    }
                }
                break;
            case -1: // 낮다면
                if (determinedDiceNumber <= StageManager.Instance.stageSaveData.upAndDownNumber)
                {
                    for (int i = 0; i < StageManager.Instance.stageSaveData.selectedRandomEvents.Count; i++)
                    {
                        if (StageManager.Instance.stageSaveData.selectedRandomEvents[i] == null)
                        {
                            StageManager.Instance.stageSaveData.selectedRandomEvents[i] = randomEventData;
                            break;
                        }
                    }
                }
                break;
        }
        StartCoroutine(RollDiceAnimation(determinedDiceNumber, randomEventData));
    }

    private IEnumerator RollDiceAnimation(int determinedDiceNumber, RandomEventData randomEventData)
    {
        diceSelection.gameObject.SetActive(true);
        Sprite[] selectedDiceImage = null;
        switch (StageManager.Instance.stageSaveData.battleCharacters[selectedCharacterIndex].CharacterData.elementType)
        {
            case DesignEnums.ElementTypes.Fire:
                selectedDiceImage = fireDiceImage;
                break;
            case DesignEnums.ElementTypes.Water:
                selectedDiceImage = waterDiceImage;
                break;
            case DesignEnums.ElementTypes.Rock:
                selectedDiceImage = rockDiceImage;
                break;
            case DesignEnums.ElementTypes.Grass:
                selectedDiceImage = grassDiceImage;
                break;
            case DesignEnums.ElementTypes.Electro:
                selectedDiceImage = electroDiceImage;
                break;
        }
        CharacterSO selectedCharacter = StageManager.Instance.stageSaveData.battleCharacters[selectedCharacterIndex].CharacterData;
        var charDiceImage = selectedCharacter.DiceNumIcon;
        int charDiceNumber = selectedCharacter.charDiceData.Key;
        for (int i = 0; i < selectedDiceImage.Length; i++)
        {
            if (charDiceNumber == i + 1)
            {
                selectedDiceImage[i] = charDiceImage;
                break;
            }
        }

        for (int i = 0; i < 10; i++)
        {
            int randomIndex = Random.Range(0, 6);
            diceSelection.sprite = selectedDiceImage[randomIndex];
            yield return new WaitForSeconds(0.1f);
        }
        diceSelection.sprite = selectedDiceImage[determinedDiceNumber - 1];
        yield return new WaitForSeconds(1f);

        UIManager.Instance.messagePopup.Open(
            $"주사위를 굴렸습니다! 눈금은 {determinedDiceNumber}입니다.\n{conditionText.text}",
            onYes: () => OnDiceRollComplete(randomEventData)
        );
    }

    private void OnDiceRollComplete(RandomEventData randomEvent)
    {
        // 1. 데이터 처리 즉시
        if (randomEvent != null)
        {
            for (int i = 0; i < StageManager.Instance.stageSaveData.selectedRandomEvents.Count; i++)
            {
                if (StageManager.Instance.stageSaveData.selectedRandomEvents[i] == null)
                {
                    StageManager.Instance.stageSaveData.selectedRandomEvents[i] = randomEvent;
                    break;
                }
            }
        }
        StageManager.Instance.stageSaveData.currentPhaseState = StageSaveData.CurrentPhaseState.Standby;
        StageManager.Instance.stageSaveData.UpOrDown = 0;
        StageManager.Instance.stageSaveData.randomEventData = null;
        StageManager.Instance.stageSaveData.currentPhaseIndex++;
        StageManager.Instance.battleUIController.OpenStagePanel(StageManager.Instance.stageSaveData.currentPhaseIndex);

        // 2. 메시지 팝업 예약 (UIManager 등 항상 활성화된 곳에서 실행)
        UIManager.Instance.StartCoroutine(ShowResultPopupAfterStagePanel(randomEvent));
    }

    private IEnumerator ShowResultPopupAfterStagePanel(RandomEventData randomEvent)
    {
        yield return null; // 한 프레임 대기(또는 yield return new WaitForSeconds(0.1f);)

        if (randomEvent == null)
        {
            UIManager.Instance.messagePopup.Open("이벤트 실패: 아무 일도 일어나지 않았습니다.");
        }
        else if (randomEvent.EventType == RandomEventType.Blessing)
        {
            UIManager.Instance.messagePopup.Open(
                $"축복 이벤트 발생: {randomEvent.EventName}\n{RandomEventData.GetEventDescription(randomEvent)}"
            );
        }
        else if (randomEvent.EventType == RandomEventType.Curse)
        {
            UIManager.Instance.messagePopup.Open(
                $"저주 이벤트 발생: {randomEvent.EventName}\n{RandomEventData.GetEventDescription(randomEvent)}"
            );
        }
    }
}