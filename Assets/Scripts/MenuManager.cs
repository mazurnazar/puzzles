using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Sprite locked;
    [SerializeField] private Data background;
    [SerializeField] private GameObject volume;
    [SerializeField] private GameObject Title;

    public static MenuManager Instance;
    private const int totalLevels = 7;
    private bool[] levels;
    private float[] time;
    private bool isSoundOn = true;

    public bool[] Levels { get => levels; set { levels = value; } }
    
    public float[] Time { get => time; set { time = value; } }
   
    public Sprite Locked { get => locked; set { locked = value; } }
    
    public int TotalLevels { get => totalLevels; private set { } }
   
    public Data Background { get => background; private set { } }

    public bool IsSoundOn { get => isSoundOn;  set { isSoundOn = value; } }
   

    private void Awake()
    {
        levels = new bool[totalLevels] {false, true,  true, true, true, true, true };
        time = new float[totalLevels] { 0f, 0f, 0f, 0f, 0f, 0f, 0f};
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadInfo();
        //Debug.Log(Application.persistentDataPath);
    }
    private void Start()
    {
        StartCoroutine(ShowTitle());

    }
    IEnumerator ShowTitle()
    {
        Title.SetActive(true);
        yield return new WaitForSeconds(1);
        Title.SetActive(false);

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
        public bool[] levels = new bool[totalLevels];
        public float[] time = new float[totalLevels];
    }
}
