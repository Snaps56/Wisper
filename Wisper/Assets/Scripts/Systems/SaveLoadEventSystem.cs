using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveLoadEventSystem : MonoBehaviour {
    private EventSystem es;
    private GameObject[] saveLoadButtons;


	// Use this for initialization
	void Start () {
        es = GetComponent<EventSystem>();

        // initialize saveLoadButtons
        GameObject[] tmpArr = new GameObject[4];
        int i = 0;
        foreach(GameObject child in this.transform)
        {
            if (child.transform.GetType() == typeof(Button));
            {
                tmpArr[i] = child;
                i++;
            }
        }
        i = 0;
        saveLoadButtons = new GameObject[tmpArr.Length];
        foreach(GameObject entry in tmpArr)
        {
            saveLoadButtons[i] = entry;
            i++;
        }
        
        saveLoadButtons = tmpArr;
        
        if(es.firstSelectedGameObject == null)
        {
            es.firstSelectedGameObject = saveLoadButtons[0];
        }
	}
    
	
	// Update is called once per frame
	void Update () {

    }

    

    
}
