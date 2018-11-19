using System.Collections;
using UnityEngine;

public class credits : MonoBehaviour {

    public GameObject Image;
    public GameObject Menu;
    public GameObject Credits;

    public void reveal ()
    {
        Image.SetActive(true);
        Menu.SetActive(true);
        Credits.SetActive(false);

    }
}
