using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveLoadMenu : MonoBehaviour {

    public GameObject saveLoadES;       // Event System used for the save and load menu(s)
    public GameObject generalMenuES;    // Event System used for the main or pause menu

    public GameObject saveLoadMenu;     // UI game object for save or load menu
    public GameObject generalMenu;      // UI game object for main or pause menu

    private GameObject loadingScreen;   // Game object for displaying loading text and logo
    private CanvasGroup blackFade;      // Canvas group for black fade when loading

    private PersistantStateData PSD;

    private bool doLoad = false;        // Signals beginning of loading data from file to PSD
    private bool doneLoad = false;      // Signals completion of loading data from file to PSD
    private bool doFade = false;        // Signals beginning of fading screen
    private bool doneFade = false;      // Signals completion of fading screen
    private bool startedAsync = false;  // Signals beginning of loading scene

    private string savePath;            // Path where save data is stored
    private string targetFile = "";     // File to be loaded

    private AsyncOperation async;

    private float fadeDuration = 2f;
    private float fadeRate;

    private float delayInitial;
    private float delayDuration = 10.0f;


    // Use this for initialization
    void Start () {
        fadeRate = Time.fixedDeltaTime / fadeDuration;
        PSD = PersistantStateData.persistantStateData;
        savePath = PSD.savePath;
    }
	

    
	// Update is called once per frame
	void Update () {
        if (startedAsync)
        {
            if (Time.time > delayInitial + delayDuration)
            {
                doFade = false;
                doneFade = false;
                doLoad = false;
                doneLoad = false;
                async.allowSceneActivation = true;
            }
        }

        
        if (Input.GetButtonDown("XBOX_Button_B") || Input.GetButtonDown("Cancel"))
        {
            if(saveLoadES.activeSelf)
            {
                generalMenuES.SetActive(true);
                saveLoadES.SetActive(false);
                foreach(Transform child in saveLoadMenu.transform)
                {
                    child.gameObject.SetActive(false);
                }
                generalMenu.SetActive(true);
            }
            
        }
    }

    private void FixedUpdate()
    {
        FadeChecker();
    }

    // Returns the directory or file name at the end of a path
    public string ParseFinalPathPortion(string path)
    {
        Debug.Log("Path separator is: " + Path.DirectorySeparatorChar);
        string[] splitPath = path.Split(Path.DirectorySeparatorChar);
        for (int i = 0; i < splitPath.Length; i++)
        {
            Debug.Log("Path parser part " + i + ": " + splitPath[i]);
        }
        return splitPath[splitPath.Length - 1];
    }

    public void SaveGame(string filename = "ShamusFile")
    {
        //Debug.Log("Saving game");
        int fileNum = 1;
        bool complete = false;

        while (!complete)
        {
            string saveFile = Path.Combine(savePath, fileNum + "" + Path.DirectorySeparatorChar + "" + filename + ".txt");
            if (File.Exists(saveFile))
            {
                fileNum++;
                Debug.Log("Increment file num");
            }
            else
            {
                Debug.Log("File num is " + fileNum);
                complete = !complete;

                Debug.Log("Save file is " + saveFile);

                string fileContentString = "";
                foreach (string key in PSD.stateConditions.Keys)
                {
                    fileContentString += key + ": " + PSD.stateConditions[key] + "\r\n";
                }


                Directory.CreateDirectory(Path.Combine(savePath, "" + fileNum));
                using (FileStream fs = File.Create(saveFile))
                {
                    byte[] saveData = new System.Text.UTF8Encoding(true).GetBytes(fileContentString);
                    fs.Write(saveData, 0, saveData.Length);
                }

            }
        }
    }

    // Takes in the index of the UI button element
    // Reads the save data index displayed on the button and begins loading process for that data
    public void LoadFromMenuClick(int loadButtonIndex)
    {
        Debug.Log("Hey there, you're trying to load save data!");
        string loadButtonString = "LoadButton" + loadButtonIndex;
        GameObject loadButton = GameObject.Find(loadButtonString);
        targetFile = loadButton.transform.Find("SaveNumber").GetComponent<Text>().text;
        doFade = true;
    }


    public void LoadFile(string fileIndex)
    {
        Debug.Log("Called load file with index: " + fileIndex);

        List<string> fileLines = new List<string>();
        if (Directory.Exists(Path.Combine(savePath, fileIndex)))
        {
            string saveFile = Directory.GetFiles(Path.Combine(savePath, fileIndex))[0];
            using (FileStream fs = File.OpenRead(saveFile))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        fileLines.Add(sr.ReadLine());
                    }
                }
            }

            Hashtable psdEntries = new Hashtable();
            foreach (string entry in fileLines)
            {
                string entryName = entry.Split(':')[0].Trim();
                if (entry.Split(':')[1].Trim().ToLower().Equals("true") || entry.Split(':')[1].Trim().ToLower().Equals("false"))
                {
                    psdEntries.Add(entryName, System.Boolean.Parse(entry.Split(':')[1]));
                }
                else if (entry.Split(':')[1].Contains("."))
                {
                    psdEntries.Add(entryName, float.Parse(entry.Split(':')[1]));
                }
                else
                {
                    psdEntries.Add(entryName, int.Parse(entry.Split(':')[1]));
                }
            }
            PSD.ChangeStateConditions(psdEntries);
            Debug.Log("Loaded PSD data");
            doneLoad = true;
        }

        else
        {
            Debug.Log("File not found, could not load");
        }
    }

    void FadeChecker()
    {
        if (doFade)
        {
            Debug.Log("Performing load fade");
            if (loadingScreen == null)
            {
                Debug.Log("Retrieving loading screen");
                loadingScreen = GameObject.Find("Canvas").transform.Find("Loading Screen").gameObject;
            }
            if (blackFade == null)
            {
                Debug.Log("Retrieving black fade");
                blackFade = GameObject.Find("Canvas").transform.Find("Faded").gameObject.GetComponentInChildren<CanvasGroup>();
            }
            if (!blackFade.gameObject.activeSelf)
            {
                blackFade.gameObject.SetActive(true);
            }
            if (blackFade.alpha < 1 && !doneFade)
            {
                blackFade.alpha += fadeRate;
                Debug.Log("Fading");
            }
            else
            {
                Debug.Log("Done Fading");
                doneFade = true;
            }
            if (doneFade && !doLoad)
            {
                Debug.Log("Performing load");
                doLoad = true;
                LoadFile(targetFile);
            }
            else if (doneFade && !startedAsync && doneLoad)
            {
                Debug.Log("Starting scene load");
                loadingScreen.SetActive(true);
                delayInitial = Time.time;
                StartCoroutine(LoadAsynchronously((int)PSD.stateConditions["CurrentScene"]));
            }
        }
    }

    IEnumerator LoadAsynchronously(int sceneBuildNumber)
    {
        async = SceneManager.LoadSceneAsync(sceneBuildNumber);
        Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;
        async.allowSceneActivation = false;
        startedAsync = true;
        while (!async.isDone)
        {
            //Debug.Log(async.progress);
            yield return null;
        }
    }
}
