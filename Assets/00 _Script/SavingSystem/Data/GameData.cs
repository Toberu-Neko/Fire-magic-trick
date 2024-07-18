using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;

    public int money;
    public float maxHealth;
    public float timePlayed;
    public Vector2 playerPosition;
    public bool gotoSavePoint;
    public bool firstTimePlaying;

    public string currentTimeSkill;
    public string lastInteractedSavepointID;

    public bool finishStartTL;


    public SerializableDictionary<string, int> interactableMapItem;

    public SerializableDictionary<string, bool> defeatedBosses;
    public SerializableDictionary<string, bool> pickedTreasures;
    public SerializableDictionary<string, bool> activatedMapItem;

    public SerializableDictionary<string, SavepointDetails> savepoints;


    public SerializableDictionary<string, string> equipedItems;

    public GameData()
    {
        firstTimePlaying = true;
        activatedMapItem = new();
        defeatedBosses = new();
        pickedTreasures = new();

        maxHealth = 100f;
        timePlayed = 0f;
        money = 0;
        currentTimeSkill = "PlayerTimeSkill_None";
        lastInteractedSavepointID = "Default";
        finishStartTL = false;
        gotoSavePoint = true;
        playerPosition = new Vector2(0, 0);

        interactableMapItem = new();

        savepoints = new();
        pickedTreasures = new();
        equipedItems = new();
        defeatedBosses = new();
    }

    public int GetPercentageComplete()
    {
        //TODO: use save points to calculate percentage complete
        return 24;
    }
}

[System.Serializable]
public class SavepointDetails
{
    public bool isActivated = false;
    public Vector3 teleportPosition;

    public SavepointDetails(bool isSavePointActive, Vector3 teleportPos)
    {
        isActivated = isSavePointActive;
        teleportPosition = teleportPos;
    }
}

