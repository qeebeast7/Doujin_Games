using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog:MonoBehaviour{
    private RectTransform rt;
    private RectTransform pet;
    private void Start()
    {
        rt = GetComponent<RectTransform>();
        pet = FindObjectOfType<Pet>().GetComponent<RectTransform>();
    }
    private void Update()
    {
        //rt.anchoredPosition = pet.anchoredPosition + new Vector2(-5,135);
    }
}
