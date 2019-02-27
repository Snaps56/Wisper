using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseModule : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private EventSystem masterEventSystem;
    private bool mouseOverObject = false;
    private GameObject currentGameObject;

    void Start()
    {
        masterEventSystem = EventSystem.current;
        currentGameObject = masterEventSystem.currentSelectedGameObject;
    }
    void Update()
    {
        currentGameObject = masterEventSystem.currentSelectedGameObject;
        if (currentGameObject != gameObject && mouseOverObject)
        {
            masterEventSystem.SetSelectedGameObject(gameObject);
        }
        Debug.Log(currentGameObject);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOverObject = true;
        masterEventSystem.SetSelectedGameObject(gameObject);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOverObject = false;
    }

}