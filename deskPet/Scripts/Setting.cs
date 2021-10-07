using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Setting :IUIDrag{
    private Pet pet;

    private Button closeBtn;
    private Transform panel;
    //size
    private Slider sizeSlider;
    //walkSpeed
    private Slider wsSlider;
    //walkTimeInput
    private InputField wkMinInput;
    private InputField wkMaxInput;
    private float wkMin=3;
    private float wkMax=20;
    //talkTimeInput
    private InputField tkMinInput;
    private InputField tkMaxInput;
    private float tkMin=3;
    private float tkMax=20;
    //toggles
    private Toggle rwToggle;
    private Toggle tsToggle;
    // Use this for initialization
    public override void Start () {
        //base
        base.Start();
        //find
        pet = FindObjectOfType<Pet>();
        closeBtn = transform.Find("closeBtn").GetComponent<Button>();
        panel = transform.Find("panel");
        sizeSlider = panel.Find("size/sizeSlider").GetComponent<Slider>();
        wsSlider = panel.Find("walkSpeed/wsSlider").GetComponent<Slider>();
        wkMinInput = panel.Find("walkTime/wkMinInput").GetComponent<InputField>();
        wkMaxInput = panel.Find("walkTime/wkMaxInput").GetComponent<InputField>();
        tkMinInput = panel.Find("talkTime/tkMinInput").GetComponent<InputField>();
        tkMaxInput = panel.Find("talkTime/tkMaxInput").GetComponent<InputField>();
        rwToggle = panel.Find("rwToggle").GetComponent<Toggle>();
        tsToggle = panel.Find("tsToggle").GetComponent<Toggle>();
        //Bind
        closeBtn.onClick.AddListener(OnCloseBtnClick);
        sizeSlider.onValueChanged.AddListener((float value)=> { OnSizeChanged(value,sizeSlider); });
        wsSlider.onValueChanged.AddListener((float value) => { OnWalkSpeedChanged(value, wsSlider); });
        wkMinInput.onEndEdit.AddListener((string str) => { OnwkMinInputChanged(str, wkMinInput); });
        wkMaxInput.onEndEdit.AddListener((string str) => { OnwkMaxInputChanged(str, wkMaxInput); });
        tkMinInput.onEndEdit.AddListener((string str) => { OntkMinInputChanged(str, tkMinInput); });
        tkMaxInput.onEndEdit.AddListener((string str) => { OntkMaxInputChanged(str, tkMaxInput); });
        rwToggle.onValueChanged.AddListener((bool value) => { OnrwToggleChanged(value, rwToggle); });
        tsToggle.onValueChanged.AddListener((bool value) => { OntsToggleChanged(value, tsToggle); });

        LoadInfo();
    }
    public override void Update()
    {
        base.Update();
        if (gameObject.activeInHierarchy)
        {
            pet.Pause(true);
        }
    }
    void LoadInfo()
    {
        if (UIManager.Instance.userdata != null)
        {
            UserData ud = UIManager.Instance.userdata;
            rt.position = ud.setPos;
            float scale = ud.scale;
            float walkSpeed = ud.walkSpeed;
            float minWalkTime = ud.wkMin;
            float maxWalkTime = ud.wkMax;
            float minTalkTime = ud.tkMin;
            float maxTalkTime = ud.tkMax;

            sizeSlider.value = scale;
            wsSlider.value = walkSpeed;
            wkMinInput.text = minWalkTime.ToString();
            wkMaxInput.text = maxWalkTime.ToString();
            tkMinInput.text = minTalkTime.ToString();
            tkMaxInput.text = maxTalkTime.ToString();
            rwToggle.isOn = ud.isRw;
            tsToggle.isOn = ud.isTs;
        }
    }
    void OnCloseBtnClick()
    {
        pet.SetSize();
        pet.Pause(false);
        gameObject.SetActive(false);
    }
    void ChangeLabel(float value, Slider slider)
    {
        Text label = slider.transform.Find("Label").GetComponent<Text>();
        label.text = value.ToString("f1");
    }
    void OnSizeChanged(float value,Slider slider)
    {
        ChangeLabel(value,slider);
        pet.ChangeSize(value);
    }
    void OnWalkSpeedChanged(float value, Slider slider)
    {
        ChangeLabel(value, slider);
        pet.walkSpeed = value;
    }

    void OnwkMinInputChanged(string str,InputField input)
    {
        float value;
        float.TryParse(str,out value);

        float max;
        float.TryParse(wkMaxInput.text,out max);
        if (value < 3 || value > max)
        {
            input.text = wkMin.ToString();
        }
        else
        {
            wkMin = value;
        }
        OnwkInputEnd();
    }
    void OnwkMaxInputChanged(string str, InputField input)
    {
        float value;
        float.TryParse(str, out value);

        float min;
        float.TryParse(wkMinInput.text, out min);
        if (value <= 3 || value < min)
        {
            input.text = wkMax.ToString();
        }
        else
        {
            wkMax = value;
        }
        OnwkInputEnd();
    }
    void OnwkInputEnd()
    {
        float min;
        float.TryParse(wkMinInput.text,out min);        
        float max;
        float.TryParse(wkMaxInput.text,out max);
        pet.SetWalkTime(min,max);
    }

    void OntkMinInputChanged(string str, InputField input)
    {
        float value;
        float.TryParse(str, out value);

        float max;
        float.TryParse(tkMaxInput.text, out max);
        if (value <5 || value > max)
        {
            input.text = tkMin.ToString();
        }
        else
        {
            tkMin = value;
        }
        OntkInputEnd();
    }
    void OntkMaxInputChanged(string str, InputField input)
    {
        float value;
        float.TryParse(str, out value);

        float min;
        float.TryParse(tkMinInput.text, out min);
        if (value <=5 || value<min)
        {
            input.text = tkMax.ToString();
        }
        else
        {
            tkMax = value;
        }
        OntkInputEnd();
    }
    void OntkInputEnd()
    {
        float min;
        float.TryParse(tkMinInput.text, out min);
        float max;
        float.TryParse(tkMaxInput.text, out max);
        pet.SetTalkTime(min, max);
    }

    void OnrwToggleChanged(bool value,Toggle toggle)
    {
        pet.canWalk = toggle.isOn;
        pet.isStop = !toggle.isOn;
    }
    void OntsToggleChanged(bool value, Toggle toggle)
    {
        pet.canTalk = toggle.isOn;
    }
}
