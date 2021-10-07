using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pet : IUIDrag, IPointerClickHandler
{
    public float minWalkTime;
    public float maxWalkTime;
    public float minIdleTime;
    public float maxIdleTime;
    public float minTalkTime;
    public float maxTalkTime;
    public float walkSpeed = 1;
    public float scale = 1;
    public bool canWalk = true;
    public bool canTalk = true;
    public bool isStop = false;
    public int turn = -1;
    
    private float m_width;
    private float m_height;
    private Animator anim;

    private bool canClick = true;
    private bool canDrag = true;
    private float walkTime;
    private float idleTime;
    private float talkTime;
    private float walkTimer = 0;
    private float idleTimer = 0;
    private float talkTimer = 0;
    private Vector3 target;
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        if (UIManager.Instance.userdata != null)
        {
            UserData ud = UIManager.Instance.userdata;
            rt.position = ud.petPos;
            scale = ud.scale;
            walkSpeed = ud.walkSpeed;
            minWalkTime = ud.wkMin;
            maxWalkTime = ud.wkMax;
            minTalkTime = ud.tkMin;
            maxTalkTime = ud.tkMax;
            isStop = !ud.isRw;
            canTalk = ud.isTs;
            turn = ud.turn;
        }
        anim = GetComponent<Animator>();

        SetScale();

        idleTime = Random.Range(minIdleTime, maxIdleTime);
        talkTime = Random.Range(minTalkTime, maxTalkTime);
        walkTime = Random.Range(minWalkTime, maxWalkTime);
        //show dialog
        if (canTalk)
        {
            if (System.DateTime.Now.Hour >= 7 && System.DateTime.Now.Hour <= 9)
            {
                UIManager.Instance.ShowDialog(TextResources.xing_morning);
            }
            else if (System.DateTime.Now.Hour >= 21 && System.DateTime.Now.Hour <= 23)
            {
                UIManager.Instance.ShowDialog(TextResources.xing_night);
            }
            else
            {
                UIManager.Instance.ShowDialog(TextResources.xing_start);
            }
        }
    }

    void SetScale()
    {
        m_width = rt.rect.width;
        m_height = rt.rect.height;
        rt.localScale = new Vector3(turn, 1, 1) * scale;
        target = new Vector2(Random.Range(minWidth, maxWidth), Random.Range(minHeight, maxHeight));
    }
    public override void Update()
    {
        base.Update();

        //idle
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleTime)
        {
            anim.SetTrigger("idle");
            idleTime = Random.Range(minIdleTime, maxIdleTime);
            idleTimer = 0;
        }
        //随机行走
        if (!isStop)
        {
            if (canWalk)
            {
                if (walkTimer < walkTime)
                {
                    walkTimer += Time.deltaTime;
                }
                else
                {
                    //move
                    canClick = false;
                    anim.SetBool("walk", true);
                    ChangeSize(1.3f, true);
                    turn = (target.x > rt.position.x)?1:-1;
                    transform.localScale = new Vector3(-turn, 1, 1) * scale;
                    float t = 1 / (target - rt.position).magnitude;
                    rt.position = Vector2.Lerp(rt.position, target, t * walkSpeed);
                    if ((rt.position - target).sqrMagnitude <= 10)
                    {
                        Turn();
                        anim.SetBool("walk", false);
                        ChangeSize(1.3f, false);
                        rt.position = target;
                        target = new Vector2(Random.Range(minWidth, maxWidth), Random.Range(minHeight, maxHeight));
                        walkTimer = 0;
                        walkTime = Random.Range(minWalkTime, maxWalkTime);
                        canClick = true;
                    }
                }
            }
        }
        if (canTalk)
        {
            //随机显示话语
            talkTimer += Time.deltaTime;
            if (talkTimer >= talkTime)
            {
                //night
                if (System.DateTime.Now.Hour >= 21 && System.DateTime.Now.Hour <= 23)
                {
                    UIManager.Instance.ShowDialog(TextResources.xing_nightRandom
                        [Random.Range(0, TextResources.xing_nightRandom.Length)]);
                }
                //day
                else if (System.DateTime.Now.Hour >= 7 && System.DateTime.Now.Hour <= 9)
                {
                    UIManager.Instance.ShowDialog(TextResources.xing_dayRandom
                        [Random.Range(0, TextResources.xing_dayRandom.Length)]);
                }
                else
                {
                    UIManager.Instance.ShowDialog(TextResources.xing_dayRandom
                        [Random.Range(0, TextResources.xing_random.Length)]);
                }
                talkTimer = 0;
                talkTime = Random.Range(minTalkTime, maxTalkTime);
            }
        }
        UIManager.Instance.SetDialog();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canClick)
        {
            walkTimer = 0;
            anim.SetTrigger("click");
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            canClick = false;
            canWalk = false;
            anim.SetBool("walk", false);
            anim.SetBool("drag", true);
            ChangeSize(1.4f, true);
            idleTimer = 0;
            walkTimer = 0;
            base.OnBeginDrag(eventData);
        }
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            walkTimer = 0;
            base.OnDrag(eventData);
        }
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            canClick = true;
            canWalk = true;
            anim.SetBool("drag", false);
            ChangeSize(1.4f, false);
            target = new Vector2(Random.Range(minWidth, maxWidth), Random.Range(minHeight, maxHeight));
            base.OnEndDrag(eventData);
        }
    }
    public void Turn()
    {
        transform.localScale = new Vector3(turn, 1, 1) * scale;
    }

    public void ChangeSize(float value, bool isAdd = true)
    {
        transform.localScale = new Vector3(turn, 1, 1) * scale;
        if (isAdd) rt.sizeDelta = new Vector2(m_width, m_height) * value;
        else rt.sizeDelta = new Vector2(m_width, m_height);
    }
    public void ChangeSize(float value)
    {
        scale = value;
        transform.localScale = new Vector3(turn, 1, 1) * scale;
    }
    public void SetSize()
    {
        m_width = rt.rect.width;
        m_height = rt.rect.height;
    }
    public void SetWalkTime(float min, float max)
    {
        minWalkTime = min;
        maxWalkTime = max;
        walkTime = Random.Range(minWalkTime, maxWalkTime);
        walkTimer = 0;
    }
    public void SetTalkTime(float min, float max)
    {
        minTalkTime = min;
        maxTalkTime = max;
        talkTime = Random.Range(minTalkTime, maxTalkTime);
        talkTimer = 0;
    }
    public void Pause(bool isPause)
    {
        anim.SetBool("walk", false);
        canWalk = !isPause;
        walkTimer = 0;
        canDrag = !isPause;
        canClick = !isPause;
    }
}
