using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCData : FSMDataFunctions, IInteractable
{
    [Header("States")]
    public NPCTypes NPCType;
    public string CurrentState;
    public float CooldownTimer;

    [Header("Interactions")]
    public string RumorName;
    public int Affinity = 0;
    public int LiedTo = 0;
    public GameObject CurrentPartner;
    public GameObject LastPartner;

    [Header("Go To States")]
    public bool GoToWaitState;
    public bool GoToListenState;
    public bool GoToWanderState;
    public bool GoToQuestionState;
    public bool CanBeQuestioned;
    public bool CanBeTalkedTo;
    public bool EnableTimer = true;

    [Header("Set Values")]
    public float WanderMinDistance = 1;
    public float WanderMaxDistance = 4;
    public float InterestPointMinDistance = 1f;
    public float SearchRange = 18;
    public float MoveSpeed = 6;
    public float TurnStep = 200;
    public Vector3 YOffset = new(0, 0, 0);

    [Header("Interest Points")]
    public List<Transform> InterestPoints = new();

    [Header("Positions")]
    public Transform WorldCenter;
    public Vector3 StartingPosition;
    public Vector3 TargetPosition;

    [Header("Graphics")]
    public Image StatusImage;
    public TMP_Text Nametag;
    public Outline Outline;


    private void Start()
    {
        StartingPosition = transform.position;

        StatusImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        StatusImage.gameObject.SetActive(false);
        Nametag = transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        Nametag.text = gameObject.name;
        Outline = transform.GetComponent<Outline>();
        Outline.enabled = false;
    }

    public Vector3 GenerateRandomPosition()
    {
        float distance = Random.Range(WanderMinDistance, WanderMaxDistance);
        // Random.insideUnitCircle is not normalized, so we have to normalize it 
        Vector3 randPos = Random.insideUnitCircle.normalized * distance;
        // this position is inside the xy plane, we want to convert that to use the xz plane
        randPos.z = randPos.y; // we change the z value to the current y
        randPos.y = 0; // set the y to be 0
        // make sure randPos is relative to transform.position
        return transform.position + randPos;
    }
    public GameObject FindNearestNPC(NPCData thisNPC, float range)
    {
        GameObject nearest = null;
        float distanceToNearest = 0;
        foreach (GameObject npc in NPCManager.Instance.NPCSpawned)
        {
            if (npc == thisNPC.gameObject) continue;
            if (npc == thisNPC.LastPartner) continue;
            if (npc.transform.GetComponent<NPCData>().CanBeQuestioned) continue;

            float distance = Vector3.Distance(thisNPC.transform.position, npc.transform.position);
            if (distance > range)
            {
                continue;
            }

            if (nearest == null || distance < distanceToNearest)
            {
                nearest = npc;
                distanceToNearest = distance;
            }
        }
        return nearest;
    }

    public void Interact()
    {
        if (CanBeQuestioned)
        {
            GoToQuestionState = true;
        }
        else
        {
            print("Interacted!");
        }    
    }
}
