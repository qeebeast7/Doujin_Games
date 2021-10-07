using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : Entity {
    private Transform ball;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        moveSpeed *= 1.5f;
        ball = GameObject.Find("ball").transform;
        Debug.Log(ball);
        voice.mute = true;
        shadowOffset = transform.position.x - shadow.position.x;
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (isStart)
        {
            if (canMove)
            {
                Move();
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void Move()
    {
        Vector3 dir = ball.position - transform.position;
        if (ball.position.x <= 0)
        {
            if (dir.x < -1)//left
            {
                moveDir = -1;
                isMove = true;
            }
            else if (dir.x > 1)//right
            {
                moveDir = 1;
                isMove = true;
            }
            else
            {
                isMove = false;
                isCatch = false;
            }

            if (ball.position.y < 0)
            {
                if (dir.y < 2 && isGround)
                {
                    isCatch = true;
                }
                else
                {
                    isCatch = false;
                }
            }
            else if (ball.position.y >2)
            {
                if (dir.y < 5 && isGround)
                {
                    isJump = true;
                    isCatch = false;
                }
                if (dir.y < 2 && isJump)
                {
                    beatTrigger = true;
                    isBeat = true;
                }
            }
        }
        else
        {
            isMove = false;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D c)
    {
        base.OnCollisionEnter2D(c);
    }
    protected override void OnCollisionStay2D(Collision2D collision)
    {
        base.OnCollisionStay2D(collision);
    }
}
