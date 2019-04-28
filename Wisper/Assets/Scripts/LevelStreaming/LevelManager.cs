using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance { set; get; }
    public string sceneLoadOnAwake = "LS Playground";
    private PersistantStateData PSD;
    public string PlaygroundScene = "LS Playground";
    public string CityScene = "LS City";
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        
        PSD = PersistantStateData.persistantStateData;
        if(PSD != null)
        {
            if((bool)PSD.stateConditions["InPlayground"])
            {
                sceneLoadOnAwake = PlaygroundScene;
            }
            else
            {
                sceneLoadOnAwake = CityScene;
            }
        }

        Load(sceneLoadOnAwake);
    }

    public void Load(string sceneName)
    {
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }

    public void Unload(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}