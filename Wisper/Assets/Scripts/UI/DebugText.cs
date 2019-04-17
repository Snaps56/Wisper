using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    public GameObject tutorials;
    private Text debugText;
    // Start is called before the first frame update
    void Start()
    {
        debugText = GetComponent<Text>();
        if (PersistantStateData.persistantStateData.enableDebugMode)
        {
            debugText.enabled = true;
            Debug.Log("Enabled movement from debug start");
            GameObject.Find("Player").GetComponent<PlayerMovement>().EnableMovement();
            tutorials.SetActive(false);
        }
        else
        {
            debugText.enabled = false;
            tutorials.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Player").GetComponent<PlayerMovement>().GetToggleMovement())
        {
            if (PersistantStateData.persistantStateData.enableDebugMode)
            {
                Debug.Log("Enabled movement from debug");
                GameObject.Find("Player").GetComponent<PlayerMovement>().EnableMovement();
                debugText.enabled = true;
                tutorials.SetActive(false);
            }
            else
            {
                debugText.enabled = false;
                tutorials.SetActive(true);
            }
        }
    }
}
