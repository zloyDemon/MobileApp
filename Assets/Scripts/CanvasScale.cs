using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScale : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<CanvasScaler>().scaleFactor = GetCanvasScale();
    }

    private float GetCanvasScale()
    {
#if UNITY_EDITOR
        return 1.5f;
#endif
#if UNITY_ANDROID
        return 2.5f;
#endif
    }
}
