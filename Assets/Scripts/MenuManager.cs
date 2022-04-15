using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public bool[] levels;
    public float[] time;
    public Sprite locked;

    public Data background;

    private void Awake()
    {
        levels = new bool[6] {false, true,  true, true, true, true };
        time = new float[6] { 0f, 0f, 0f, 0f, 0f, 0f };
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadInfo();
        
    }
    public void SaveInfo()
    {
        SaveData data = new SaveData();
        for (int i = 0; i < levels.Length; i++)
        {
            data.levels[i] = levels[i];
            data.time[i] = time[i];
        }
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public void LoadInfo()
    {

        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            for (int i = 0; i < levels.Length; i++)
            {
                levels[i] = data.levels[i];
                time[i] = data.time[i];
            }
        }
    }

    [System.Serializable]
    class SaveData
    {
        public bool[] levels = new bool[6];
        public float[] time = new float[6];
    }
}
