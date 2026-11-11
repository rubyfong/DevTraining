using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCType", menuName ="Scriptable Objects/NPC Type")]
public class NPCTypes : ScriptableObject
{
    [Header("Trope")]
    public string TropeName;
    public ETropes TropeType;

    [Header("Multipliers")]
    public float Trust;
    public float Gossip;

    [Header("Contents")]
    public List<string> NameOptions = new();
    public List<QuestionData> WhatsYourName = new();
    public List<QuestionData> AreYouHim = new();
    public List<QuestionData> DoIKnowYou = new();
    public List<QuestionData> IKnowYou = new();
 
    private void OnValidate()
    {
        TropeName = TropeType.ToString();
    }
}

public enum ETropes
{
    Default,
    Frat,
    Babe,
    Weird,
    Cute
}

public enum EAffinity
{
    Default,
    Like,
    Neutral,
    Dislike
}

public enum EQuestionType
{
    Default,
    Name,
    CheckName,
    Opinion,
    Statement
}

[Serializable]
public class QuestionData
{
    public string QuestionName;
    public EQuestionType QuestionType;
    public EAffinity AffinityLevel;
    [TextArea(5, 10)] public string QuestionContent;
}