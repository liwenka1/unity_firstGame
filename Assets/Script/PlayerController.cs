using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    public AudioSource jumpAudio, hurtAudio, cherryAuido;
    public float speed, jumpForce;
    private float horizontalMove;
    public Transform groundCheck, cellCheck;
    public LayerMask ground;
    public int Cherry;

    public Text CherryNum;

    public bool isGround, isCell, isJump, isCrouch, isHurt;

    bool jumpPressed;
    int jumpcount;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpcount > 0)
        {
            jumpPressed = true;
        }

        if (Input.GetKey(KeyCode.S))
        {
            isCrouch = true;
        }

        if (!Input.GetKey(KeyCode.S) && !isCell)
        {
            isCrouch = false;
        }
    }

    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        isCell = Physics2D.OverlapCircle(cellCheck.position, 0.1f, ground);

        if (!isHurt)
        {
            GroundMovement();
        }

        Jump();

        Crouch();

        SwitchAnim();
    }

    void GroundMovement()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");//只返回-1，0，1
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }

    }

    void Jump()
    {
        if (isGround)
        {
            jumpcount = 2;
            isJump = false;
        }
        if (jumpPressed && isGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpcount--;
            jumpPressed = false;
        }
        else if (jumpPressed && jumpcount > 0 && isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpcount--;
            jumpPressed = false;
        }
    }

    void Crouch()
    {
        if (isGround && isCrouch)
        {
            coll.enabled = false;
        }
        else
        {
            coll.enabled = true;
        }
    }

    void SwitchAnim()
    {
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));

        if (isGround)
        {
            anim.SetBool("falling", false);
        }
        else if (!isGround && rb.velocity.y > 0)
        {
            anim.SetBool("jumping", true);
            jumpAudio.Play();
        }
        else if (rb.velocity.y < 0)
        {
            anim.SetFloat("running", 0);
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }

        if (isCrouch)
        {
            anim.SetFloat("running", 0);
            anim.SetBool("crouching", true);
        }
        else
        {
            anim.SetBool("crouching", false);
        }

        if (isHurt)
        {
            anim.SetBool("hurting", true);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("hurting", false);
                isHurt = false;
            }
        }
    }

    //收集物品
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection" && collision.isActiveAndEnabled)
        {
            cherryAuido.Play();
            Destroy(collision.gameObject);
            Cherry += 1;
            CherryNum.text = Cherry.ToString();
        }
    }

    //消灭敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("falling"))
            {
                enemy.JumpOn();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-10, rb.velocity.y);
                hurtAudio.Play();
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(10, rb.velocity.y);
                hurtAudio.Play();
                isHurt = true;
            }
        }

    }
}
