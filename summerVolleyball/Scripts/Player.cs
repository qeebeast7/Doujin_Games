using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {

    public bool is1P;
    // Use this for initialization
    protected override void Start()
    {
		base.Start();
        if (!is1P)
        {
            shadowOffset = transform.position.x - shadow.position.x ;
        }
	}

    // Update is called once per frame
    protected override void Update () {
		base.Update();
        if (isStart)
        {
            if (canMove)
            {
                if (is1P)
                {
                    Move(KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.Return);
                }
                else
                {
                    Move(KeyCode.A, KeyCode.D, KeyCode.W, KeyCode.Space);
                }
            }
        }
	}

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void Move(KeyCode leftKey, KeyCode rightKey, KeyCode jumpKey, KeyCode catchKey)
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) <= 0.1f)
        {
            isMove = false;
        }
        if (Input.GetKey(leftKey))
        {
            moveDir = -1;
            isMove = true;
        }
        if (Input.GetKey(rightKey))
        {
            moveDir = 1;
            isMove = true;
        }
        if (Input.GetKeyDown(jumpKey))
        {
            isJump = true;
        }
        if (Input.GetKeyDown(catchKey) && isJump)
        {
            beatTrigger = true;
            isBeat = true;
        }
        if (Input.GetKeyDown(catchKey) && isMove && isGround)
        {
            isCatch = true;
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
