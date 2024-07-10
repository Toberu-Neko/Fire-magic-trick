using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


/// <summary>
/// <para>兩層防護存檔!遊戲打開自動讀檔，遊戲關掉自動存檔</para>
/// <para>要儲存新資料的話 control+f 搜尋 "Data Here" 把要存的參數打進去 </para>
/// </summary>

public class SaveDataCollecter : MonoBehaviour
{
    //"Data Here" 初始化設定
    public static bool isFirstTimePlaying = true;


    public static int CorruptCheck = 1;
    static bool FileCorrupt;



    [Header("開發者覆寫 ，改完之後Inspector中程式右鍵選 Override Save")]
    [Header("Override")]
    //"Data Here"開發人員 遊戲中途直接複寫存檔
    [SerializeField] bool _isFirstTimePlaying;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void Laod()
    {
        if (SaveSystem.LoadFile() == null)
        {
            Debug.Log("New saving!");
        }

        if (CorruptCheck == 0)
        {
            Debug.LogError("存檔毀損!啟用第二存檔");

            if (!new JasonSaveData().Load())
            {
                Debug.LogError("無初始資料!");
                FirstTimeSave();
                new JasonSaveData().Save();
            }
            FileCorrupt = true;
        }
        if (CorruptCheck == 0)
        {
            if (FileCorrupt)
            {
                Debug.LogError("第二存檔毀損!回復初始值");
            }
            FirstTimeSave();
            FileCorrupt = true;
        }

        Instantiate(Resources.Load("Save System Don't Destory"));


        static void FirstTimeSave()
        {
            //"Data Here" 初始化設定
            isFirstTimePlaying = true;
            CorruptCheck = 1;
            SaveSystem.Save();
        }
    }


    [ContextMenu("Override Save")]
    void OverrideSave()
    {
        //"Data Here"開發人員 遊戲中途直接複寫存檔
        isFirstTimePlaying = _isFirstTimePlaying;
        SaveSystem.Save();
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        //"Data Here"開發人員 遊戲中途直接複寫存檔
        _isFirstTimePlaying = isFirstTimePlaying;
    }
    private void OnApplicationQuit()
    {
        SaveSystem.Save();
        if (FileCorrupt)
        {
            new JasonSaveData().Save();
        }
    }



}


public class JasonSaveData
{
    //"Data Here"
    public bool _isFirstTimePlaying;
    public int _CorruptCheck;


    readonly static string Path = Application.persistentDataPath + "/JsonSaveData.json";

    public void Save()
    {
        JasonSaveData saveData = new();

        //"Data Here"
        saveData._isFirstTimePlaying = SaveDataCollecter.isFirstTimePlaying;
        saveData._CorruptCheck = SaveDataCollecter.CorruptCheck;


        string SaveString = JsonUtility.ToJson(saveData);

        StreamWriter File = new(Path);
        File.Write(SaveString);
        File.Close();

    }

    public bool Load()
    {
        if (File.Exists(Path))
        {
            StreamReader file = new(Path);
            string loadJson = file.ReadToEnd();

            JasonSaveData loadData;

            loadData = JsonUtility.FromJson<JasonSaveData>(loadJson);


            //"Data Here"
            SaveDataCollecter.isFirstTimePlaying = loadData._isFirstTimePlaying;
            SaveDataCollecter.CorruptCheck = loadData._CorruptCheck;
            file.Close();
            return true;
        }
        return false;
    }
}



[System.Serializable]
public class SaveDatabase
{
    //"Data Here"
    readonly bool FirsttimePlaying;
    readonly int CorruptCheck;
    public SaveDatabase()
    {
        //"Data Here"
        FirsttimePlaying = SaveDataCollecter.isFirstTimePlaying;
        CorruptCheck = SaveDataCollecter.CorruptCheck;
    }

    public void LoadBackTocollector()
    {
        //"Data Here"
        SaveDataCollecter.isFirstTimePlaying = FirsttimePlaying;
        SaveDataCollecter.CorruptCheck = CorruptCheck;
    }

}
