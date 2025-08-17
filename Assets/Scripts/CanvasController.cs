using System;
using UnityEngine;

[Obsolete("Can't remember what this was for.")]
public class CanvasController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
    }
}