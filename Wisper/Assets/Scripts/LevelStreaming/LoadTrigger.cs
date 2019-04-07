using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTrigger : MonoBehaviour
{

    public string LoadName;
    public string UnloadName;

private void OnTriggerEnter(Collider col)
    {
        if (LoadName != "")
        {
            LevelManager.Instance.Load(LoadName);
        }
        if (UnloadName != "")
        {
            StartCoroutine("UnloadScene");
        }
    }

    IEnumerator UnloadScene()
    {
        yield return new WaitForSeconds(0.10f);
        LevelManager.Instance.Unload(UnloadName);
    }
}
