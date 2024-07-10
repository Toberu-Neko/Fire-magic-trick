using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


/// <summary>
/// <para>��h���@�s��!�C�����}�۰�Ū�ɡA�C�������۰ʦs��</para>
/// <para>�n�x�s�s��ƪ��� control+f �j�M "Data Here" ��n�s���Ѽƥ��i�h </para>
/// </summary>

public class SaveDataCollecter : MonoBehaviour
{
    //"Data Here" ��l�Ƴ]�w
    public static bool isFirstTimePlaying = true;


    public static int CorruptCheck = 1;
    static bool FileCorrupt;



    [Header("�}�o���мg �A�粒����Inspector���{���k��� Override Save")]
    [Header("Override")]
    //"Data Here"�}�o�H�� �C�����~�����Ƽg�s��
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
            Debug.LogError("�s�ɷ��l!�ҥβĤG�s��");

            if (!new JasonSaveData().Load())
            {
                Debug.LogError("�L��l���!");
                FirstTimeSave();
                new JasonSaveData().Save();
            }
            FileCorrupt = true;
        }
        if (CorruptCheck == 0)
        {
            if (FileCorrupt)
            {
                Debug.LogError("�ĤG�s�ɷ��l!�^�_��l��");
            }
            FirstTimeSave();
            FileCorrupt = true;
        }

        Instantiate(Resources.Load("Save System Don't Destory"));


        static void FirstTimeSave()
        {
            //"Data Here" ��l�Ƴ]�w
            isFirstTimePlaying = true;
            CorruptCheck = 1;
            SaveSystem.Save();
        }
    }


    [ContextMenu("Override Save")]
    void OverrideSave()
    {
        //"Data Here"�}�o�H�� �C�����~�����Ƽg�s��
        isFirstTimePlaying = _isFirstTimePlaying;
        SaveSystem.Save();
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        //"Data Here"�}�o�H�� �C�����~�����Ƽg�s��
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
