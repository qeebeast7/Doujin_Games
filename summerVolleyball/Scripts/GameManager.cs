using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	private AudioSource bgm;
	private AudioSource se;
	private Image black;
	private GameObject ready;
	private GameObject go;

    private Text p1Text;
    private Text p2Text;
    private int p1Score;
    private int p2Score;

    private GameObject exit;
	private GameObject yesPointer;
	private GameObject noPointer;
    private Button yes;
    private Button no;

    private GameObject ends;
    private GameObject youwin;
    private GameObject youlose;
    private GameObject p1win;
    private GameObject p2win;

    private AudioClip moveClip;
	private AudioClip selectClip;

	private GameObject ball;
    private Animator[] anims;
    private Entity p1;
    private Entity p2;
    private Vector3[] startPoses=new Vector3[3];

    public bool isStart = false;
    public bool isEnd = false;
    private bool canReturn = false;

	// Use this for initialization
	void Start () {
		moveClip = Resources.Load<AudioClip>("SE/button");
		selectClip = Resources.Load<AudioClip>("SE/button2");

		bgm = GetComponent<AudioSource>();
		se = GetComponents<AudioSource>()[1];
        bgm.volume = GlobalValues.bgmVolume;

        ready = GameObject.Find("ready");
		go = GameObject.Find("go");

        p1Text = GameObject.Find("p1Score").GetComponent<Text>();
        p2Text = GameObject.Find("p2Score").GetComponent<Text>();
        p1Text.text = "00";
        p2Text.text = "00";
        p1Score = 0;
        p2Score = 0;

        exit = GameObject.Find("exit");
		yesPointer= GameObject.Find("yesPointer");
		noPointer = GameObject.Find("noPointer");
        yes = GameObject.Find("yesText").GetComponent<Button>();
        no = GameObject.Find("noText").GetComponent<Button>();

        ball = GameObject.Find("ball");
		anims = GameObject.FindObjectsOfType<Animator>();
        p1 = GameObject.Find("p1").GetComponentInChildren<Player>();
        if (GlobalValues.is1P)
        {
            p2 = GameObject.Find("p2").GetComponentInChildren<Rigidbody2D>().gameObject.AddComponent<Robot>();
            Debug.Log("p2");
        }
        else
        {
            p2 = GameObject.Find("p2").GetComponentInChildren<Rigidbody2D>().gameObject.AddComponent<Player>();
        }
        black = GameObject.Find("black").GetComponent<Image>();

        ends = GameObject.Find("ends");

        startPoses[0] = ball.transform.position;
        startPoses[1] = p1.gameObject.transform.position;
        startPoses[2] = p2.gameObject.transform.position;

        yesPointer.SetActive(true);
        noPointer.SetActive(false);
        exit.SetActive(false);
        ends.SetActive(false);

        yes.onClick.AddListener(delegate
        {
            if (!yesPointer.activeInHierarchy)
            {
                yesPointer.SetActive(true);
                noPointer.SetActive(false);
            }
            GlobalValues.PlaySE(se, selectClip);
            Time.timeScale = 1;
            SceneManager.LoadScene("Main");
        });
        no.onClick.AddListener(delegate
        {
            if (!noPointer.activeInHierarchy)
            {
                yesPointer.SetActive(false);
                noPointer.SetActive(true);
            }
            GlobalValues.PlaySE(se, selectClip);
            exit.SetActive(false);
            Time.timeScale = 1;
        });

        ResetRoundStart();
	}

    void ResetRoundStart()
    {
        Time.timeScale = 1;
        ball.transform.position = startPoses[0];
        p1.gameObject.transform.position = startPoses[1];
        p2.gameObject.transform.position = startPoses[2];
        StartCoroutine(Fade(1.5f,true));
        ready.SetActive(false);
        go.SetActive(false);
        StartCoroutine(Appear(0.5f, ready));
        StartCoroutine(Appear(2.0f, go, ready));
        StartCoroutine(Appear(2.5f, null, go));

        ball.GetComponent<Rigidbody2D>().simulated = false;
        //foreach (var anim in anims)
        //{
        //    anim.enabled = false;
        //}

        StartCoroutine(WaitStart(2.5f));
    }

	// Update is called once per frame
	void Update () {
        if (!isEnd)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!exit.activeInHierarchy)
                {
                    GlobalValues.PlaySE(se, selectClip);
                    Time.timeScale = 0;
                    exit.SetActive(true);
                }
                //else
                //{
                //	exit.SetActive(false);
                //	Time.timeScale = 1;
                //}
            }

            if (exit.activeInHierarchy)
            {
                //yes
                if (yesPointer.activeInHierarchy)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        GlobalValues.PlaySE(se, moveClip);
                        noPointer.SetActive(true);
                        yesPointer.SetActive(false);
                    }
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        GlobalValues.PlaySE(se, selectClip);
                        Time.timeScale = 1;
                        SceneManager.LoadScene("Main");
                    }
                }
                //no
                else
                {
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        GlobalValues.PlaySE(se, moveClip);
                        noPointer.SetActive(false);
                        yesPointer.SetActive(true);
                    }
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        GlobalValues.PlaySE(se, selectClip);
                        exit.SetActive(false);
                        Time.timeScale = 1;
                    }
                }
            }
        }
        else
        {
            if (canReturn)
            {
                if (Input.anyKeyDown)
                {
                    SceneManager.LoadScene("Main");
                }
            }
        }
	}

	IEnumerator Fade(float t,bool isBlack)
	{
		for (float timer = 0; timer < t; timer += Time.deltaTime)
		{
            if (isBlack) black.color = new Color(0, 0, 0, (t - timer) / t);
            else black.color = new Color(0, 0, 0, timer / t);
            yield return 0;
		}
		yield return new WaitForSeconds(t+1f);
	}
    IEnumerator Appear(float t,GameObject obj=null,GameObject disObj=null) 
	{
		yield return new WaitForSeconds(t);
		if (obj!=null)
		{
			obj.SetActive(true);
		}
		if (disObj!=null)
		{
			disObj.SetActive(false);
		}
	}
	IEnumerator WaitStart(float t)
	{
		yield return new WaitForSeconds(t);
		isStart = true;
		ball.GetComponent<Rigidbody2D>().simulated = true;
		//foreach (var anim in anims)
		//{
		//	anim.enabled = true;
		//}
        p1.RoundStart();
        p2.RoundStart();
    }

    public void AddScore(string p)
    {
        if (p=="p1") { p1Score++;p1Text.text = p1Score.ToString("00"); }
        if (p == "p2") { p2Score++; p2Text.text = p2Score.ToString("00"); }

        if (p1Score >= GlobalValues.maxScore || p2Score >= GlobalValues.maxScore)//end
        {
            if (GlobalValues.is1P)
            {
                if (p1Score >= GlobalValues.maxScore && p1Score > p2Score)
                {
                    p1.End(true, true);
                    p2.End(false, false);
                }
                if (p2Score >= GlobalValues.maxScore && p2Score > p1Score)
                {
                    p1.End(false, true);
                    p2.End(true, false);
                }

            }
            else
            {
                if (p1Score >= GlobalValues.maxScore && p1Score > p2Score)
                {
                    p1.End(true, true);
                    p2.End(false, true);
                }
                if (p2Score >= GlobalValues.maxScore && p2Score > p1Score)
                {
                    p1.End(false, true);
                    p2.End(true, true);
                }
            }
            GameEnd();
        }
        else
        {
            ResetRound();
        }
    }
    public void ResetRound()
    {
        Time.timeScale = 0.5f;
        isStart = false;
        p1.RoundEnd();
        p2.RoundEnd();
        ball.transform.position = startPoses[0];
        p1.gameObject.transform.position = startPoses[1];
        p2.gameObject.transform.position = startPoses[2];
        StartCoroutine(Fade(1.5f, false));
        ResetRoundStart();
    }

    public void GameEnd()
    {
        bgm.Stop();
        ball.SetActive(false);
        isStart = false;
        isEnd = true;
        ends.SetActive(true);
        if (GlobalValues.is1P)
        {
            if (p1Score >= p2Score)
            {
                ends.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                ends.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        else
        {
            if (p1Score >= p2Score)
            {
                ends.transform.GetChild(2).gameObject.SetActive(true);
            }
            else
            {
                ends.transform.GetChild(3).gameObject.SetActive(true);
            }
        }
        StartCoroutine(WaitCanReturn(2f));
    }
    IEnumerator WaitCanReturn(float t)
    {
        yield return new WaitForSeconds(t);
        ends.transform.GetChild(4).gameObject.SetActive(true);
        canReturn = true;
    }

    private void OnApplicationQuit()
    {
        GlobalValues.PlaySE(se, selectClip);
        PlayerPrefs.SetInt("maxScore", GlobalValues.maxScore);
        PlayerPrefs.SetFloat("bgmVolume", GlobalValues.bgmVolume);
        PlayerPrefs.SetFloat("seVolume", GlobalValues.seVolume);
        PlayerPrefs.SetFloat("voiceVolume", GlobalValues.voiceVolume);
        Application.Quit();
    }
}
