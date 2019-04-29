using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterfallScroll : MonoBehaviour
{
    // Scroll main texture based on time

    //float scrollSpeed = 0.5f;
    Renderer rend;
    public Vector2 Scroll = new Vector2(0.05f, 0.05f);
    Vector2 Offset = new Vector2(0f, 0f);

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        Offset += Time.deltaTime * Scroll;
        rend.material.SetTextureOffset("_MainTex", Offset);
    }
}
