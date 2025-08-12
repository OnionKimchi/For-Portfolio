﻿using NUnit.Compatibility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    static public TutorialManager Instance { get; private set; }

    [Header("Tutorial Completion Flags")]
    public bool isLobbyTutorialCompleted = false;
    public bool isGameTutorialCompleted = false;
    [Header("Tutorial Popup")]
    [SerializeField] private GameObject tutorialPopup;

    [Header("UI References")]
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private GameObject tutorialBg;
    //[SerializeField] private TMP_Text nextText;
    [SerializeField] private GameObject lobbyTutorialImage;
    [SerializeField] private GameObject lobbyTutorialImageBg;

    [Header("Tutorial Settings")]
    [SerializeField, Range(0, 6)] private int lobbyTutorialSteps;
    [SerializeField, Range(0, 6)] private int gameTutorialSteps;

    [Header("Tutorial Data")]
    [SerializeField] private List<LobbyTutorial> lobbyTutorials;

    private Coroutine nextTextBlinkCoroutine;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 중복 생성 방지
            return;
        }
    }
    public void Start()
    {
        // 초기화
        tutorialBg.SetActive(false);
        for (int i = 0; i < lobbyTutorials.Count; i++)
        {
            lobbyTutorials[i].Hide();
        }
        tutorialPopup.SetActive(false);
        lobbyTutorialImageBg.SetActive(false);
        lobbyTutorialImage.SetActive(true); 
        //lobbyTutorialImage[1].SetActive(false);
    }
    //public void OnEnable()
    //{
    //    StartNextTextBlink();
    //}
    //public void OnDisable()
    //{
    //    StopNextTextBlink();
    //}
    //private void StartNextTextBlink()
    //{
    //    if (nextTextBlinkCoroutine != null)
    //        StopCoroutine(nextTextBlinkCoroutine);
    //    nextTextBlinkCoroutine = StartCoroutine(BlinkNextText());
    //}
    //private void StopNextTextBlink()
    //{
    //    if (nextTextBlinkCoroutine != null)
    //    {
    //        StopCoroutine(nextTextBlinkCoroutine);
    //        nextTextBlinkCoroutine = null;
    //    }
    //    if (nextText != null)
    //    {
    //        var color = nextText.color;
    //        color.a = 1f;
    //        nextText.color = color;
    //    }
    //}

    //private IEnumerator BlinkNextText()
    //{
    //    while (true)
    //    {
    //        float t = Mathf.PingPong(Time.time, 1f); // 0~1 반복
    //        float alpha = Mathf.Lerp(0.5f, 1f, t);   // 0.5~1 선형보간
    //        if (nextText != null)
    //        {
    //            var color = nextText.color;
    //            color.a = alpha;
    //            nextText.color = color;
    //        }
    //        yield return null;
    //    }
    //}
    public void StartLobbyTutorial()
    {
        if ("LobbyScene" != SceneManager.GetActiveScene().name||isLobbyTutorialCompleted)
        {
            return;
        }
        tutorialPopup.SetActive(true);
        if (isLobbyTutorialCompleted)
        {
            tutorialBg.SetActive(false);
            return;
        }
        tutorialBg.SetActive(true);
        lobbyTutorialSteps = 0; // Reset tutorial steps
        ShowLobbyTutorial(0);
    }
    public void ShowLobbyTutorial(int step)
    {
        if (step < 0 || step >= lobbyTutorials.Count)
        {
            Debug.LogError("Invalid tutorial step index: " + step);
            return;
        }
        LobbyTutorial currentTutorial = lobbyTutorials[step];
        tutorialText.text = currentTutorial.description;

        if (currentTutorial.sprites[0] != null)
        {
            lobbyTutorialImage.GetComponent<Image>().sprite = currentTutorial.sprites[0];
        lobbyTutorialImageBg.SetActive(true);
        }
        else
        {
        lobbyTutorialImageBg.SetActive(false);
        }

        for (int i = 0; i < lobbyTutorials.Count; i++)
        {
            if (i == step)
            {
                lobbyTutorials[i].Blinking();
            }
            else
            {
                lobbyTutorials[i].Hide();
            }
        }
    }
    public void OnClickNextButton()
    {
        if ("LobbyScene" == SceneManager.GetActiveScene().name)
        {
            if (lobbyTutorialSteps < lobbyTutorials.Count - 1)
            {
                lobbyTutorialSteps++;
                ShowLobbyTutorial(lobbyTutorialSteps);
            }
            else
            {
                EndLobbyTutorial();
            }
        }
    }
    public void OnClickSkipButton()
    {
        UIManager.Instance.messagePopup.Open("튜토리얼을 건너 뛰겠습니까?",
            () =>
            {
                EndLobbyTutorial();
            },
            () =>
            {
                UIManager.Instance.messagePopup.Close();
            });
    }
    public void EndLobbyTutorial()
    {
        isLobbyTutorialCompleted = true;
        tutorialBg.SetActive(false);
        if ("LobbyScene" == SceneManager.GetActiveScene().name)
        {
            for (int i = 0; i < lobbyTutorials.Count; i++)
            {
                lobbyTutorials[i].Show();
            }
        }
        tutorialPopup.SetActive(false); // Hide tutorial popup
    }

    [System.Serializable]
    class LobbyTutorial
    {
        [TextArea] public string description;
        public GameObject image;
        public Coroutine blinkCoroutine;
        // 첨부 이미지 리스트
        public Sprite[] sprites = new Sprite[2];
        public void Blinking()
        {
            if (image != null)
            {
                if (image.GetComponent<CanvasGroup>() == null)
                {
                    image.AddComponent<CanvasGroup>();
                }
                blinkCoroutine = TutorialManager.Instance.StartCoroutine(BlinkButton());
            }
        }
        private IEnumerator BlinkButton()
        {
            CanvasGroup canvasGroup = image.GetComponent<CanvasGroup>();
            while (true)
            {
                float t = Mathf.PingPong(Time.time, 1f); // 0~1 반복
                float alpha = Mathf.Lerp(0.2f, 1f, t);   // 0.2~1 선형보간
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = alpha;
                }
                yield return null;
            }
        }

        public void Hide()
        {
            if (image != null)
            {
                if (image.GetComponent<CanvasGroup>() == null)
                {
                    image.AddComponent<CanvasGroup>();
                }
                if (blinkCoroutine != null)
                {
                    TutorialManager.Instance.StopCoroutine(blinkCoroutine);
                    blinkCoroutine = null;
                }
            image.GetComponent<CanvasGroup>().alpha = 0.2f;
            }
        }
        public void Show()
        {
            if (blinkCoroutine != null)
            {
                TutorialManager.Instance.StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
            }
            if (image != null && image.GetComponent<CanvasGroup>() != null)
            {
                image.GetComponent<CanvasGroup>().alpha = 1f; // Reset alpha to fully visible
            }
        }
    }
}
