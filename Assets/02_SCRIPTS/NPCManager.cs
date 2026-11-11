using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    #region Singleton
    public static NPCManager Instance { get; set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SetupNPCCatalog();
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
    #endregion

    #region Variables

    [Header("Stage Variables")]
    public int NPCPerStage = 6;
    public GameObject NPCPrefab;
    public List<Material> MatList = new();
    [SerializeField] private float _SpawnOffset = 5;

    [Header("Data")]
    public List<GameObject> NPCSpawned = new();
    private List<string> _CheckNameList = new();

    [Header("Catalog")]
    [SerializeField] private List<NPCTypes> NPCCatalog = new();
    const string NPCTYPESPATH = "NPCTypes";
    #endregion

    #region NPC List
    public void AddToNPCList(GameObject npc)
    {
        NPCSpawned.Add(npc);
    }
    public void RemoveFromNPCList(GameObject npc)
    {
        NPCSpawned.Remove(npc);
    }
    #endregion

    private void Start()
    {
        for (int i = 0; i < NPCPerStage; i++)
        {
            // Randomize type of NPC
            NPCTypes npcType = NPCCatalog[Random.Range(0, NPCCatalog.Count)];

            // Randomize variation of NPC?

            // Randomize spawn position & rotation
            float xPos = transform.position.x + Random.Range(-_SpawnOffset, _SpawnOffset);
            float zPos = transform.position.z + Random.Range(-_SpawnOffset, _SpawnOffset);
            Vector3 randomPos = new(xPos, 0, zPos);
            Vector3 randomRot = new(0, Random.Range(0, 360), 0);

            // Spawn NPC
            GameObject NPCtoSpawn = Instantiate(NPCPrefab, randomPos, Quaternion.Euler(randomRot));

            // Edit Name
            NPCtoSpawn.name = npcType.NameOptions[Random.Range(0, npcType.NameOptions.Count)];

            // Make sure spawned NPC name is unique
            #region TODO get back to this, need more names first
            if (i > 0 && i < 4)
            {
                // add all spawned npcs names into a list
                foreach (GameObject npc in NPCSpawned)
                {
                    string name = npc.name;
                    _CheckNameList.Add(name);                 
                }

                while (_CheckNameList.Contains(NPCtoSpawn.name))
                {
                    NPCtoSpawn.name = npcType.NameOptions[Random.Range(0, npcType.NameOptions.Count)];
                }

                _CheckNameList.Clear();
            }
            else if (i >= 4)
            {
                if (_CheckNameList.Contains((NPCtoSpawn.name)))
                {
                    NPCtoSpawn.name = "Impostor" + i;
                }
            }          
            #endregion

            // Add colour variation
            NPCtoSpawn.GetComponent<Renderer>().material = MatList[Random.Range(0, MatList.Count)];

            // Add collider
            Collider collider = NPCtoSpawn.AddComponent<CapsuleCollider>();

            // Add NPCdata script
            NPCData data = NPCtoSpawn.AddComponent<NPCData>();
            data.NPCType = npcType;

            // Add NPCStateMachine script
            NPCtoSpawn.AddComponent<NPCStateMachine>();

            // Add NPC layer
            NPCtoSpawn.layer = LayerMask.NameToLayer("NPC");

            // Add monster to spawned list
            NPCSpawned.Add(NPCtoSpawn);
        }
    }

    private void SetupNPCCatalog()
    {
        // adds all NPCTypes from Resources/NPCTypes to NPCCatalog list

        NPCCatalog.Clear();
        ScriptableObject[] npcTypes = Resources.LoadAll<ScriptableObject>(NPCTYPESPATH);

        foreach (ScriptableObject npcType in npcTypes)
        {
            NPCCatalog.Add((NPCTypes)npcType);
        }
    }

    public NPCData PickRandomNPC()
    {
        GameObject randomNPC = NPCSpawned[Random.Range(0, NPCSpawned.Count)];
        NPCData npcData = randomNPC.transform.GetComponent<NPCData>();

        while(npcData.CanBeQuestioned)
        {
            randomNPC = NPCSpawned[Random.Range(0, NPCSpawned.Count)];
            npcData = randomNPC.transform.GetComponent<NPCData>();
        }
        return npcData;
    }
}
