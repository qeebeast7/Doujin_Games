using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixCanvas : MonoBehaviour {

    private void Awake()
    {
        FixResolution();
    }
    public void FixResolution()
    {
        CanvasScaler scaler = GetComponent<CanvasScaler>();

        float sWToH = scaler.referenceResolution.x * 1.0f / scaler.referenceResolution.y;
        float vWToH = Screen.width * 1.0f / Screen.height;
        if (sWToH > vWToH)
        {
            //匹配宽
            scaler.matchWidthOrHeight = 0;
        }
        else
        {
            //匹配高
            scaler.matchWidthOrHeight = 1;
        }
    }
}
