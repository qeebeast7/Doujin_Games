using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    private Rigidbody2D rigid;
    private Transform smashImg;
    private AudioSource se;
    private AudioClip beatClip;
    private AudioClip heavyClip;

    private GameManager gm;

    private float bounceForce = 50;
    private Vector3 startPos;
    // Use this for initialization
    void Start () {
        beatClip = Resources.Load<AudioClip>("SE/bounce");
        heavyClip = Resources.Load<AudioClip>("SE/splatoon");

        se = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody2D>();
        smashImg = transform.Find("smash");
        gm = GameObject.FindObjectOfType<GameManager>();
        startPos = transform.position;

        smashImg.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            Entity e = collision.gameObject.GetComponent<Entity>();

            if (collision.contacts[0].normal.y > 0)//above
            {
                GlobalValues.PlaySE(se, beatClip);
                if (e.isCatch)
                {
                    rigid.AddForce(10*bounceForce * collision.contacts[0].normal);
                }
                else
                {
                    if (collision.contacts[0].normal.y >= 0.5)
                    {
                        rigid.AddForce(2f * bounceForce * collision.contacts[0].normal);
                    }
                    else
                    {
                        rigid.AddForce(5 * bounceForce * collision.contacts[0].normal);
                    }
                }
                //rigid.velocity = bounceForce * collision.contacts[0].normal;
            }
            else//heavy beat
            {
                if (e.isBeat)
                {
                    GlobalValues.PlaySE(se, heavyClip);
                    smashImg.gameObject.SetActive(true);
                    smashImg.position = collision.contacts[0].point;
                    rigid.AddForce(20 * bounceForce * collision.contacts[0].normal);
                    e.isBeat = false;
                    StartCoroutine(HideSmash(0.3f));
                }
            }
        }
        if (collision.gameObject.tag == "Ground")
        {
            GlobalValues.PlaySE(se, heavyClip);
            if (collision.contacts[0].point.x < 0)//left p1 win
            {
                gm.AddScore("p1");
                transform.position = new Vector3(-startPos.x,startPos.y,startPos.z);
            }
            else if (collision.contacts[0].point.x > 0)//right p2s win
            {
                gm.AddScore("p2");
                transform.position = startPos;
            }
            rigid.velocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }
    }

    IEnumerator HideSmash(float t)
    {
        yield return new WaitForSeconds(t);
        smashImg.gameObject.SetActive(false);
    }
}
