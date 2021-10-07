using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    #region singleton
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null) instance = new UIManager();
            return instance;
        }
    }
    #endregion singleton
    public UserData userdata;
    private Pet pet;
    private GameObject dialog;
    private RectTransform diaRt;
    private RectTransform petRt;
    private Text txt;
    private GameObject setting;
    private void Awake()
    {
        instance = this;
        userdata = JsonHelper.Read<UserData>("userData");
    }
    // Use this for initialization
    void Start () {
        pet = FindObjectOfType<Pet>();
        petRt = pet.GetComponent<RectTransform>();
        dialog = transform.Find("dialog").gameObject;
        diaRt = dialog.GetComponent<RectTransform>();
        txt = dialog.transform.Find("Text").GetComponent<Text>();
        setting = transform.Find("Setting").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        if (!setting.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                setting.SetActive(true);
                pet.Pause(true);
                pet.ChangeSize(1.3f, false);
                pet.Turn();
            }
        }
    }
    private void OnApplicationQuit()
    {
        Vector3 petPos=pet.GetComponent<RectTransform>().position;
        Vector3 setPos=setting.GetComponent<RectTransform>().position;
        float scale=pet.scale;
        float walkSpeed=pet.walkSpeed;
        float wkMin=pet.minWalkTime;
        float wkMax=pet.maxWalkTime;
        float tkMin=pet.minTalkTime;
        float tkMax=pet.maxTalkTime;
        bool isRw=!pet.isStop;
        bool isTs=pet.canTalk;
        int turn = -1;

        userdata = new UserData(petPos,setPos,scale,walkSpeed,wkMin,wkMax,tkMin,tkMax,isRw,isTs,turn);
        JsonHelper.Write(userdata,"userData");
    }
    public void ShowDialog(string t)
    {
        if(!dialog) dialog = transform.Find("dialog").gameObject;
        if(!txt) txt = dialog.transform.Find("Text").GetComponent<Text>();
        dialog.SetActive(true);
        txt.text = t;
        StartCoroutine(IHideDialog());
    }
    IEnumerator IHideDialog()
    {
        yield return new WaitForSeconds(5);
        dialog.SetActive(false);
    }
    public void SetDialog()
    {
        diaRt.anchoredPosition = petRt.anchoredPosition + new Vector2(-5, 135);
    }
}
