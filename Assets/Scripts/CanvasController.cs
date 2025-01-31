using UnityEngine;

public class CanvasController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
    }
}