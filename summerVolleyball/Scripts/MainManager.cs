using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{

    private Transform startText;
    private Transform configText;
    private Transform exitText;
    private Transform onePText;
    private Transform twoPText;
    private Transform returnText;
    private Transform pointer;

    private GameObject mains;
    private GameObject starts;
    private Image black;

    private int counter = 0;

    private AudioClip se;
    private AudioClip selectClip;
    private AudioClip[] readyVoices = new AudioClip[2];
    private AudioSource bgm;
    private AudioSource seAudio;
    private AudioSource voiceAudio;

    private GameObject config;
    public bool canSelect = true;

    // Use this for initialization
    void Start()
    {
        se = Resources.Load<AudioClip>("SE/button");
        selectClip = Resources.Load<AudioClip>("SE/button1");
        readyVoices[0] = Resources.Load<AudioClip>("Voice/ready");
        readyVoices[1] = Resources.Load<AudioClip>("Voice/ready1");

        //GetPrefs();

        startText = GameObject.Find("StartText").transform;
        configText = GameObject.Find("ConfigText").transform;
        exitText = GameObject.Find("ExitText").transform;
        onePText = GameObject.Find("1PText").transform;
        twoPText = GameObject.Find("2PText").transform;
        returnText = GameObject.Find("returnText").transform;

        pointer = GameObject.Find("Pointer").transform;
        mains = GameObject.Find("mains");
        starts = GameObject.Find("starts");
        black = GameObject.Find("black").GetComponent<Image>();

        mains.SetActive(true);
        starts.SetActive(false);

        bgm= GetComponents<AudioSource>()[0];
        seAudio = GetComponents<AudioSource>()[1];
        voiceAudio = GetComponents<AudioSource>()[2];
        bgm.volume = GlobalValues.bgmVolume;

        config = GameObject.Find("config");
        config.SetActive(false);

        //Bind
        #region Btns Bind
        startText.GetComponent<Button>().onClick.AddListener(delegate
        {
            PointerMove(startText);
            OnStartBtn();
        });
        configText.GetComponent<Button>().onClick.AddListener(delegate
        {
            PointerMove(configText);
            OnConfigBtn();
        });
        exitText.GetComponent<Button>().onClick.AddListener(delegate
        {
            PointerMove(exitText);
            OnExitBtn();
        });
        onePText.GetComponent<Button>().onClick.AddListener(delegate
        {
            PointerMove(onePText);
            On1PBtn();
        });
        twoPText.GetComponent<Button>().onClick.AddListener(delegate
        {
            PointerMove(twoPText);
            On2PBtn();
        });
        returnText.GetComponent<Button>().onClick.AddListener(delegate
        {
            PointerMove(returnText);
            OnReturnBtn();
        });
        #endregion
        //set pointer start pos
        PointerMove(startText);
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSelect)
        {
            switch (counter)
            {
                //start
                case 0:
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        PointerMove(configText);
                        counter++;
                        GlobalValues.PlaySE(seAudio, se);
                    }
                    break;
                //config
                case 1:
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        PointerMove(startText);
                        counter--;
                        GlobalValues.PlaySE(seAudio, se);
                    }
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        PointerMove(exitText);
                        counter++;
                        GlobalValues.PlaySE(seAudio, se);
                    }
                    break;
                //exit
                case 2:
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        PointerMove(configText);
                        counter--;
                        GlobalValues.PlaySE(seAudio, se);
                    }
                    break;
                //1P
                case 3:
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        PointerMove(twoPText);
                        counter++;
                        GlobalValues.PlaySE(seAudio, se);
                    }
                    break;
                //2P
                case 4:
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        PointerMove(onePText);
                        counter--;
                        GlobalValues.PlaySE(seAudio, se);
                    }
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        PointerMove(returnText);
                        counter++;
                        GlobalValues.PlaySE(seAudio, se);
                    }
                    break;
                //return
                case 5:
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        PointerMove(twoPText);
                        counter--;
                        GlobalValues.PlaySE(seAudio, se);
                    }
                    break;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                switch (counter)
                {
                    //start
                    case 0:
                        OnStartBtn();
                        break;
                    //config
                    case 1:
                        OnConfigBtn();
                        break;
                    //exit
                    case 2:
                        OnExitBtn();
                        break;
                    //1P
                    case 3:
                        On1PBtn();
                        canSelect = false;
                        break;
                    //2P
                    case 4:
                        On2PBtn();
                        canSelect = false;
                        break;
                    //return
                    case 5:
                        OnReturnBtn();
                        break;
                }
            }
        }
    }

    void PointerMove(Transform menu)
    {
        pointer.position = new Vector3(pointer.position.x, menu.position.y, pointer.position.z);
    }

    IEnumerator ILoadGame(float t, bool is1P)
    {
        yield return new WaitForSeconds(t - t / 10);
        StartCoroutine(Fade(1));
        yield return new WaitForSeconds(1);
        GlobalValues.is1P = is1P;
        SceneManager.LoadScene("Game");
    }

    IEnumerator Fade(float t)
    {
        for (float timer = 0; timer < t; timer += Time.deltaTime)
        {
            black.color = new Color(0, 0, 0, timer / t);
            yield return 0;
        }
    }

    #region MenuBtns Funcs
    void OnStartBtn()
    {
        GlobalValues.PlaySE(seAudio, selectClip);
        starts.SetActive(true);
        mains.SetActive(false);
        counter = 3;
        PointerMove(onePText);
    }
    void OnConfigBtn()
    {
        GlobalValues.PlaySE(seAudio, selectClip);
        canSelect = false;
        config.SetActive(true);
    }
    void OnExitBtn()
    {
        GlobalValues.PlaySE(seAudio, selectClip);
        PlayerPrefs.SetInt("maxScore", GlobalValues.maxScore);
        PlayerPrefs.SetFloat("bgmVolume", GlobalValues.bgmVolume);
        PlayerPrefs.SetFloat("seVolume", GlobalValues.seVolume);
        PlayerPrefs.SetFloat("voiceVolume", GlobalValues.voiceVolume);
        Application.Quit();
    }
    void On1PBtn()
    {
        canSelect = false;
        GlobalValues.PlaySE(seAudio, selectClip);
        GlobalValues.PlayVoice(voiceAudio, readyVoices[0]);
        StartCoroutine(ILoadGame(readyVoices[0].length, true));
    }
    void On2PBtn()
    {
        canSelect = false;
        GlobalValues.PlaySE(seAudio, selectClip);
        GlobalValues.PlayVoice(voiceAudio, readyVoices[1]);
        StartCoroutine(ILoadGame(readyVoices[1].length, false));
    }
    void OnReturnBtn()
    {
        GlobalValues.PlaySE(seAudio, se);
        mains.SetActive(true);
        starts.SetActive(false);
        PointerMove(startText);
        counter = 0;
    }
    #endregion

    private void OnApplicationQuit()
    {
        OnExitBtn();
    }
}
