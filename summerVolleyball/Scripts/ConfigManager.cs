using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ConfigManager : MonoBehaviour
{
    private Toggle[] scoreTgs;
    private Slider bgmSlider;
    private Slider seSlider;
    private Slider voiceSlider;
    private Button closeBtn;

    private AudioSource bgm;
    private AudioSource se;
    private AudioSource voice;
    private AudioClip seClip;

    private MainManager mm;
    // Use this for initialization
    void Start()
    {
        seClip = Resources.Load<AudioClip>("SE/button2");

        scoreTgs = GetComponentsInChildren<Toggle>();
        bgmSlider = transform.Find("bgmSlider").GetComponent<Slider>();
        seSlider = transform.Find("seSlider").GetComponent<Slider>();
        voiceSlider = transform.Find("voiceSlider").GetComponent<Slider>();
        closeBtn= transform.Find("closeBtn").GetComponent<Button>();

        mm = GameObject.Find("MainManager").GetComponent<MainManager>();
        bgm = GameObject.Find("MainManager").GetComponent<AudioSource>();
        se = GameObject.Find("MainManager").GetComponents<AudioSource>()[1];
        voice = GameObject.Find("MainManager").GetComponents<AudioSource>()[2];

        foreach (Toggle tg in scoreTgs)
        {
            tg.onValueChanged.AddListener((bool isOn) => { OnToggleClick(tg, isOn); });
        }

        bgmSlider.onValueChanged.AddListener(delegate
        {
            GlobalValues.bgmVolume = bgmSlider.value;
            bgm.volume = bgmSlider.value;
        });

        seSlider.onValueChanged.AddListener(delegate
        {
            GlobalValues.seVolume = seSlider.value;
            se.volume = seSlider.value;
        });
        voiceSlider.onValueChanged.AddListener(delegate
        {
            GlobalValues.voiceVolume = voiceSlider.value;
            voice.volume = voiceSlider.value;
        });
        closeBtn.onClick.AddListener(OnCloseBtnDown);

        SetUIs();
    }

    void OnCloseBtnDown()
    {
        GlobalValues.PlaySE(se, seClip);
        gameObject.SetActive(false);
        mm.canSelect = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnCloseBtnDown();
        }
    }
    void OnToggleClick(Toggle tg, bool isOn)
    {
        GlobalValues.PlaySE(se, seClip);

        if (isOn)
        {
            GlobalValues.maxScore = int.Parse(tg.GetComponentInChildren<Text>().text);
        }
    }

    void SetUIs()
    {
        foreach (Toggle t in scoreTgs)
        {
            if (t.GetComponentInChildren<Text>().text==GlobalValues.maxScore.ToString())
            {
                t.isOn = true;
                EventSystem.current.SetSelectedGameObject(t.gameObject);
            }
        }
        bgmSlider.value = GlobalValues.bgmVolume;
        seSlider.value = GlobalValues.seVolume;
        voiceSlider.value = GlobalValues.voiceVolume;
    }
}
