using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected CapsuleCollider2D cc;
    protected Vector2 ccSize;
    protected Vector2 ccOffset;

    protected float initScale;
    protected Vector3 startPos;

    protected Rigidbody2D rigid;
    protected float moveSpeed = 5;
    protected float moveDir = 1;//1=right,-1=left
    protected float jumpForce = 14;
    protected Animator anim;
    protected Transform shadow;
    protected float shadowOffset;
    protected bool isStart;
    protected bool canMove=true;
    protected bool isGround = true;
    protected bool isMove = false;
    protected bool isJump = false;
    protected bool beatTrigger = false;
    [HideInInspector]
    public bool isBeat = false;
    [HideInInspector]
    public bool isCatch = false;
    protected float catchForce = 6;

    protected AudioSource voice;
    protected AudioClip atk1;
    protected AudioClip atk2;
    protected AudioClip winClip;
    protected AudioClip loseClip;

    // Use this for initialization
    protected virtual void Start()
    {
        atk1 = Resources.Load<AudioClip>("Voice/atk1");//heavy beat
        atk2 = Resources.Load<AudioClip>("Voice/atk2");//beat
        winClip= Resources.Load<AudioClip>("Voice/win");
        loseClip = Resources.Load<AudioClip>("Voice/lose");

        cc = GetComponent<CapsuleCollider2D>();
        ccSize = cc.size;
        ccOffset = cc.offset;

        initScale = transform.localScale.x;
        startPos = transform.position;

        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        shadow = transform.parent.Find("shadow");
        shadowOffset = transform.position.x - shadow.position.x - 0.5f;

        voice = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (isStart)
        {
            if (canMove)
            {
                if (isMove)
                {
                    transform.Translate(moveDir * moveSpeed * Time.deltaTime, 0, 0);
                    if (isCatch)
                    {
                        cc.direction = CapsuleDirection2D.Horizontal;
                        cc.size = new Vector2(ccSize.x * 2, ccSize.y / 2);
                        cc.offset = new Vector2(ccOffset.x, ccOffset.y - 50);

                        rigid.velocity = new Vector2(moveDir * catchForce / 1.2f, catchForce);
                        transform.localScale = new Vector3(-moveDir, 1, 1);
                        isMove = false;
                        canMove = false;
                    }
                }
                else
                {
                    rigid.velocity = new Vector2(0, rigid.velocity.y);
                }
                if (isJump && isGround && !isCatch)
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
                    isGround = false;
                }
                if (isGround)
                {
                    isBeat = false;
                }

            }
            else if (!isCatch)
            {
                rigid.velocity = Vector3.zero;
            }
        }
        else
        {
            rigid.velocity = Vector3.zero;
        }
        SwitchAnim();
    }

    void SwitchAnim()
    {
        if (isGround)
        {
            anim.SetBool("jump", false);
            anim.SetBool("fall", false);
            anim.ResetTrigger("beat");
            anim.SetBool("catch", isCatch);
        }
        else
        {
            if (rigid.velocity.y > 0)
            {
                anim.SetBool("jump", true);
                anim.SetBool("catch", false);
            }
            else if (rigid.velocity.y < 0)
            {
                anim.SetBool("fall", true);
            }
            if (beatTrigger)
            {
                GlobalValues.PlayVoice(voice, atk2);
                anim.SetTrigger("beat");
                beatTrigger = false;
            }
        }
    }

    protected virtual void Update()
    {
        isStart = GameObject.FindObjectOfType<GameManager>().isStart;
        shadow.position = new Vector2(shadowOffset + transform.position.x, shadow.position.y);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
            isJump = false;
            if (isCatch)
            {
                anim.SetTrigger("catchFall");
                StartCoroutine(WaitCatchFall(0.5f));
            }
        }
        if (collision.gameObject.tag == "Ball")
        {
            if (collision.contacts[0].normal.y >= -0.5f)//heavy beat
            {
                if (isBeat && !isGround)
                {
                    GlobalValues.PlayVoice(voice, atk1);
                }
            }
            else
            {
                rigid.velocity = -collision.contacts[0].normal;
            }
        }
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
        }
    }

    IEnumerator WaitCatchFall(float t)
    {
        yield return new WaitForSeconds(t);
        isCatch = false;
        isMove = true;
        canMove = true;
        transform.localScale = new Vector3(initScale,1,1);

        cc.direction = CapsuleDirection2D.Vertical;
        cc.size = ccSize;
        cc.offset = ccOffset;
    }

    public void RoundEnd()
    {
        canMove = false;
        isGround = true;
        isMove = false;
        isJump = false;
        beatTrigger = false;
        isCatch = false;
        anim.Play("walk");
        transform.localScale = new Vector3(initScale, 1, 1);
        rigid.velocity = Vector3.zero;
        cc.direction = CapsuleDirection2D.Vertical;
        cc.size = ccSize;
        cc.offset = ccOffset;
    }
    public void RoundStart()
    {
        canMove = true;
        anim.Play("walk");
    }
    public void End(bool isWin,bool canTalk)
    {
        canMove = false;
        transform.position = startPos;
        if (isWin)
        {
            anim.SetBool("win", true);
            if (canTalk)
            {
                GlobalValues.PlayVoice(voice, winClip);
            }
        }
        else
        {
            anim.SetBool("lose", true);
            if (canTalk)
            {
                GlobalValues.PlayVoice(voice, loseClip);
            }
        }
    }
}
