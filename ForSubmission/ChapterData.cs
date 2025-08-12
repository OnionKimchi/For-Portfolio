﻿using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "ChapterData", menuName = "ScriptableObjects/Stages/ChapterData", order = 1)]
public class ChapterData : ScriptableObject
{ 
    public List<ChapterInfo> chapterIndex; // 짝수 인덱스는 Normal, 홀수 인덱스는 Hard 챕터로 설정, 예 : 0번은 1챕터 Normal, 1번은 1챕터 Hard, 2번은 2챕터 Normal, 3번은 2챕터 Hard 등
    public string GetNameAndDifficulty(int idx)
    {
        string chapterName = chapterIndex[idx].ChapterName;
        string difficulty = (idx % 2 == 0) ? "Normal" : "Hard";
        return $"{chapterName} ({difficulty})";
    }
}
[System.Serializable]
public class ChapterInfo
{
    [SerializeField] private string chapterName;
    [SerializeField, TextArea] private string description;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int chapterCost;
    [SerializeField] private int firstClearJewelReward; // 챕터를 처음 클리어했을 때 주는 보석 보상
    [SerializeField] private bool defaultIsUnLocked; // 챕터가 기본적으로 잠금 해제되어 있는지 여부
    [SerializeField] private bool defaultIsCompleted; // 챕터가 기본적으로 완료되어 있는지 여부

    public StageData stageData;

    public string ChapterName => chapterName;
    public string Description => description;
    public Sprite Sprite => sprite;
    public int ChapterCost => chapterCost;
    public int FirstClearJewelReward => firstClearJewelReward;
    public bool DefaultIsUnLocked => defaultIsUnLocked; // 챕터가 기본적으로 잠금 해제되어 있는지 여부
    public bool DefaultIsCompleted => defaultIsCompleted; // 챕터가 기본적으로 완료되어 있는지 여부
}
