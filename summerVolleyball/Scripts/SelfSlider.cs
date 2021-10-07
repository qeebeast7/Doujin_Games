using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelfSlider : MonoBehaviour,IPointerUpHandler {
    private AudioClip ac;
    private AudioSource se;
    public void OnPointerUp(PointerEventData eventData)
    {
        GlobalValues.PlaySE(se, ac);
    }

    // Use this for initialization
    void Start () {
        ac= Resources.Load<AudioClip>("SE/button");
        se = GameObject.Find("MainManager").GetComponents<AudioSource>()[1];
    }
}
