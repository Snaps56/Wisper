using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTrigger : MonoBehaviour
{
    public Collider Player;
    public string LoadName;
    public string UnloadName;

private void OnTriggerEnter(Collider col)
    {
        if (col == Player)
        {
            if (LoadName != "")
            {
                StartCoroutine("LoadScene");
            }
            if (UnloadName != "")
            {
                StartCoroutine("UnloadScene");
            }
        }
    }

    IEnumerator UnloadScene()
    {
        yield return new WaitForSeconds(0.10f);
        LevelManager.Instance.Unload(UnloadName);
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.10f);
        LevelManager.Instance.Load(LoadName);
    }
}
