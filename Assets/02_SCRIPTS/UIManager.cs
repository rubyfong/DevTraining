using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance { get; set; }
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

    public TMP_Text KnowsNameCounter;
    public TMP_Text TotalAffinityText;

    [Header("Status Sprites")]
    public Sprite QuestionSprite;
    public Sprite SpeechSprite;
    public Sprite OpinionSprite;

    [Header("Question Name Panel")]
    public GameObject InputNameScreen;
    public TMP_Text InputNPCName;
    public TMP_Text InputNPCContent;
    public TMP_InputField InputField;

    [Header("Question Buttons Panel")]
    public GameObject QuestionButtonsScreen;
    public TMP_Text QuestionNPCName;
    public TMP_Text QuestionNPCContent;
    public Button QuestionButton1;
    public Button QuestionButton2;

    [Header("Dialogue Panel")]
    public GameObject DialogueScreen;
    public TMP_Text DialogueNPCName;
    public TMP_Text DialogueNPCContent;
    public Button DialogueButton;

    [Header("Debug Text")]
    public GameObject DebugPrefab;
    public Transform DebugLog;

    public void InstantiateDebugText(string s)
    {
        GameObject debugText = Instantiate(DebugPrefab, DebugLog.position, Quaternion.identity, DebugLog);
        TMP_Text text = debugText.GetComponent<TMP_Text>();
        text.text = s;
        debugText.AddComponent<DestroyLog>();
    }

    public void UpdateKnownCounter(int count)
    {
        KnowsNameCounter.text = $"Knows {GameManager.Instance.PlayerName}: {count}/{NPCManager.Instance.NPCPerStage}";
    }

    public void UpdateTotalAffinity(int count)
    {
        TotalAffinityText.text = $"Total Affinity: {count}";
    }

    public void DisplayAskNameScreen()
    {
        NPCData data = GameManager.Instance.AskNameNPC;
        InputNPCName.text = data.transform.name;
        NPCTypes type = data.NPCType;

        EAffinity aff = EAffinity.Neutral;
        if (data.Affinity > 2)
        {
            aff = EAffinity.Like;
        }
        else if (data.Affinity >= 0)
        {
            aff = EAffinity.Neutral;
        }
        else
        {
            aff = EAffinity.Dislike;
        }

        QuestionData qData = type.WhatsYourName[0];
        foreach (QuestionData q in type.WhatsYourName)
        {
            if (q.AffinityLevel == aff)
            {
                qData = q;
                break;
            }
            else continue;
        }

        InputNPCContent.text = qData.QuestionContent;
        InputNameScreen.SetActive(true);
        InputField.ActivateInputField();
        Cursor.lockState = CursorLockMode.None;
    }

    public void DisplayQuestionScreen(NPCData data, string name)
    {
        QuestionNPCName.text = data.transform.name;
        NPCTypes type = data.NPCType;

        EAffinity aff = EAffinity.Neutral;
        if (data.Affinity > 2)
        {
            aff = EAffinity.Like;
        }
        else if (data.Affinity >= 0)
        {
            aff = EAffinity.Neutral;
        }
        else
        {
            aff = EAffinity.Dislike;
        }

        QuestionData qData = type.AreYouHim[0];
        foreach (QuestionData q in type.AreYouHim)
        {
            if (q.AffinityLevel == aff)
            {
                qData = q;
                break;
            }
            else continue;
        }

        QuestionNPCContent.text = qData.QuestionContent + $" {GameManager.Instance.PlayerName}?";

        QuestionButton1.transform.GetChild(0).GetComponent<TMP_Text>().text = "Yes";
        QuestionButton2.transform.GetChild(0).GetComponent<TMP_Text>().text = "No";

        QuestionButton1.onClick.AddListener(() =>
        {
            // if lying...
            if (name != GameManager.Instance.PlayerName)
            {
                if (data.LiedTo > 5)
                {
                    data.Affinity -= 2;
                    InstantiateDebugText(
                        $"{data.gameObject.name} hates {GameManager.Instance.PlayerName}'s deception.");
                }
                else if (data.LiedTo > 2)
                {
                    data.Affinity--;
                    InstantiateDebugText(
                        $"{data.gameObject.name} grows suspicious of {GameManager.Instance.PlayerName}.");
                }
                else if (data.Affinity >= 2)
                {
                    data.Affinity++;
                    data.RumorName = string.Empty;
                    InstantiateDebugText(
                        $"{data.gameObject.name} believed {GameManager.Instance.PlayerName}'s lie.");
                }
                else
                {
                    bool random = Random.value < 0.5f;
                    if (random)
                    {
                        data.RumorName = name;
                    }
                    else
                    {
                        data.RumorName = string.Empty;
                    }
                    InstantiateDebugText($"{data.gameObject.name} feels confused.");
                }
                data.LiedTo++;
            }
            else
            {
                if (data.Affinity > 2)
                {
                    data.Affinity++;
                    data.RumorName = GameManager.Instance.PlayerName;
                    InstantiateDebugText(
                        $"{data.gameObject.name} finally gets {GameManager.Instance.PlayerName}'s name right!");
                }
                else if (data.Affinity >= 0)
                {
                    bool random = Random.value < 0.5f;
                    if (random)
                    {
                        data.RumorName = name;
                    }
                    else
                    {
                        data.RumorName = string.Empty;
                    }
                    InstantiateDebugText($"{data.gameObject.name} feels confused.");
                }
                else
                {
                    data.Affinity--;
                    data.RumorName = name;
                    InstantiateDebugText(
                        $"{data.gameObject.name} doesn't respect {GameManager.Instance.PlayerName}'s name.");
                }
            }

            QuestionButtonsScreen.SetActive(false);
            data.CooldownTimer = 1f;
            data.EnableTimer = true;
            GameManager.Instance.EnableControls = true;
            Cursor.lockState = CursorLockMode.Locked;
            SetStatusImage(data, EStatus.None);
            QuestionButton1.onClick.RemoveAllListeners();
        });

        QuestionButton2.onClick.AddListener(() =>
        {
            // if lying...
            if (name == GameManager.Instance.PlayerName)
            {
                if (data.LiedTo > 5)
                {
                    data.Affinity -= 2;
                    InstantiateDebugText(
                        $"{data.gameObject.name} hates {GameManager.Instance.PlayerName}'s deception.");
                }
                else if (data.LiedTo > 2)
                {
                    data.Affinity--;
                    InstantiateDebugText(
                        $"{data.gameObject.name} grows suspicious of {GameManager.Instance.PlayerName}.");
                }
                else if (data.Affinity <= 2)
                {
                    data.Affinity++;
                    data.RumorName = string.Empty;
                    InstantiateDebugText(
                        $"{data.gameObject.name} believed {GameManager.Instance.PlayerName}'s lie.");
                }
                else
                {
                    bool random = Random.value < 0.5f;
                    if (random)
                    {
                        data.RumorName = name;
                    }
                    else
                    {
                        data.RumorName = string.Empty;
                    }
                    InstantiateDebugText($"{data.gameObject.name} feels confused.");
                }
                data.LiedTo++;
            }
            else
            {
                if (data.Affinity > 2)
                {
                    data.Affinity++;
                    data.RumorName = GameManager.Instance.PlayerName;
                    InstantiateDebugText(
                        $"{data.gameObject.name} finally gets {GameManager.Instance.PlayerName}'s name right!");
                }
                else if (data.Affinity >= 0)
                {
                    bool random = Random.value < 0.5f;
                    if (random)
                    {
                        data.RumorName = name;
                    }
                    else
                    {
                        data.RumorName = string.Empty;
                    }
                    InstantiateDebugText($"{data.gameObject.name} feels confused.");
                }
                else
                {
                    data.Affinity--;
                    data.RumorName = name;
                    InstantiateDebugText(
                        $"{data.gameObject.name} doesn't respect {GameManager.Instance.PlayerName}'s name.");
                }

            }

            QuestionButtonsScreen.SetActive(false);
            data.CooldownTimer = 1f;
            data.EnableTimer = true;
            GameManager.Instance.EnableControls = true;
            Cursor.lockState = CursorLockMode.Locked;
            SetStatusImage(data, EStatus.None);
            QuestionButton2.onClick.RemoveAllListeners();
        });

        QuestionButtonsScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void SetStatusImage(NPCData data, EStatus status)
    {
        switch (status)
        {
            case EStatus.None:
                data.StatusImage.sprite = null;
                break;
            case EStatus.Speech:
                data.StatusImage.sprite = SpeechSprite;
                break;
            case EStatus.Question:
                data.StatusImage.sprite = QuestionSprite;
                break;
            case EStatus.Opinion:
                data.StatusImage.sprite = OpinionSprite;
                break;
        }

        if (status == EStatus.None)
        {
            data.StatusImage.gameObject.SetActive(false);
        }
        else
        {
            data.StatusImage.gameObject.SetActive(true);
        }
    }
}

public enum EStatus
{
    None,
    Speech,
    Question,
    Opinion
}