using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    #endregion

    public string PlayerName;
    public bool EnableControls;
    private float timer = 0.5f;
    public int KnownCounter;
    public int TotalAffinity;

    public float NPCAskNameTimer = 2;
    public float NPCDialogueTimer = 5;
    public NPCData AskNameNPC;
    public List<NPCData> DialogueNPCs = new();

    private void Start()
    {
        if (PlayerName == string.Empty) 
            PlayerName = "Your Name";
        EnableControls = true;
        AskNameNPC = null;
        NPCAskNameTimer = 2;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (timer <= 0)
        {
            UIManager.Instance.UpdateKnownCounter(CountKnown());
            UIManager.Instance.UpdateTotalAffinity(CountAffinity());

            foreach(GameObject npc in NPCManager.Instance.NPCSpawned)
            {
                NPCData data = npc.GetComponent<NPCData>();

                if (data.CanBeQuestioned)
                {
                    UIManager.Instance.SetStatusImage(data, EStatus.Question);
                }
                else if (data.CurrentState == "Talk" || data.CurrentState == "Listen")
                {
                    UIManager.Instance.SetStatusImage(data, EStatus.Speech);
                }
                else
                {
                    UIManager.Instance.SetStatusImage(data, EStatus.None);
                }
            }
            timer = 0.5f;
        }

        if (NPCAskNameTimer > 0)
        {
            NPCAskNameTimer -= Time.deltaTime;
        }
        else if (NPCAskNameTimer <= 0)
        {
            if (AskNameNPC == null && KnownCounter < 3)
            {
                AskNameNPC = NPCManager.Instance.PickRandomNPC();
                AskNameNPC.CanBeQuestioned = true;
                UIManager.Instance.SetStatusImage(AskNameNPC, EStatus.Question);
            }
            NPCAskNameTimer = 15;
        }

        if (NPCDialogueTimer > 0)
        {
            NPCDialogueTimer -= Time.deltaTime;
        }
        else if (NPCDialogueTimer <= 0)
        {
            if (DialogueNPCs.Count < 3)
            {
                NPCData dialogueNPC = NPCManager.Instance.PickRandomNPC();
                DialogueNPCs.Add(dialogueNPC);
                dialogueNPC.CanBeQuestioned = true;
                UIManager.Instance.SetStatusImage(dialogueNPC, EStatus.Question);
            }
            NPCDialogueTimer = 7;
        }
    }

    public int CountAffinity()
    {
        int counter = 0;
        foreach (GameObject npc in NPCManager.Instance.NPCSpawned)
        {
            NPCData data = npc.GetComponent<NPCData>();
            counter += data.Affinity;
        }
        TotalAffinity = counter;
        return counter;
    }

    public int CountKnown()
    {
        int counter = 0;
        foreach (GameObject npc in NPCManager.Instance.NPCSpawned)
        {
            NPCData data = npc.GetComponent<NPCData>();
            if (data.RumorName == PlayerName)
            {
                counter++;
            }
        }
        KnownCounter = counter;
        return counter;
    }

    public void ReadStringInput(string s)
    {
        if(AskNameNPC.RumorName != null && AskNameNPC.RumorName != PlayerName)
        {
            if(AskNameNPC.LiedTo > 5)
            {
                AskNameNPC.Affinity -= 2;
                AskNameNPC.RumorName = string.Empty;
                UIManager.Instance.InstantiateDebugText(
                    $"{AskNameNPC.name} has had enough of {PlayerName}'s lies.");
            }
            else if (AskNameNPC.LiedTo > 2)
            {
                AskNameNPC.Affinity--;
                AskNameNPC.RumorName = string.Empty;
                UIManager.Instance.InstantiateDebugText(
                    $"{AskNameNPC.name} wonders why {PlayerName} changed their name.");
            }
            else
            {
                bool random = Random.value < 0.5f;
                if (random)
                {
                    AskNameNPC.RumorName = s;
                }
                else
                {
                    AskNameNPC.RumorName = string.Empty;
                }
                AskNameNPC.RumorName = string.Empty;
                UIManager.Instance.InstantiateDebugText($"{AskNameNPC.name} is confused.");
            }
        }
        else 
        {
            AskNameNPC.Affinity++;
            AskNameNPC.RumorName = s;
            PlayerName = s;
            UIManager.Instance.InstantiateDebugText($"{PlayerName} told {AskNameNPC.name} about themselves.");
        }

        PlayerName = s;
        UIManager.Instance.InputNameScreen.SetActive(false);
        AskNameNPC.CooldownTimer = 2f;
        AskNameNPC.EnableTimer = true;
        EnableControls = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RestartGame()
    {
        StartCoroutine(DelayRestartGame());
    }
    IEnumerator DelayRestartGame()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }

}