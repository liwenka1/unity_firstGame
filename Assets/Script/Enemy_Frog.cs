using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    public bool isGround;
    public LayerMask ground;
    public Transform groundCheck, leftpoint, rightpoint;
    public float Speed, JumpForce, leftx, rightx;

    private bool Faceleft = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
    }


    void Update()
    {
        SwitchAnim();
    }

    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
    }

    void Movement()
    {
        if (Faceleft)
        {
            if (isGround)
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(-Speed, JumpForce);
            }
            if (transform.position.x <= leftx)
            {
                rb.velocity = new Vector2(Speed, rb.velocity.y);
                transform.localScale = new Vector3(-1, 1, 1);
                Faceleft = false;
            }
        }
        else
        {
            if (isGround)
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(Speed, JumpForce);
            }
            if (transform.position.x >= rightx)
            {
                rb.velocity = new Vector2(-Speed, rb.velocity.y);
                transform.localScale = new Vector3(1, 1, 1);
                Faceleft = true;
            }
        }
    }

    void SwitchAnim()
    {
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0.1)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        if (isGround && anim.GetBool("falling"))
        {
            anim.SetBool("falling", false);
        }
    }

}
