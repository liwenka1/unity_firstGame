using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    public Transform toppoint, bottompoint;
    public float Speed, topy, bottomy;

    private bool Facetop = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        topy = toppoint.position.y;
        bottomy = bottompoint.position.y;
        Destroy(toppoint.gameObject);
        Destroy(bottompoint.gameObject);
    }


    void Update()
    {
        MoveMent();
    }

    void MoveMent()
    {
        if (Facetop)
        {
            rb.velocity = new Vector2(rb.velocity.x, Speed);
            if (transform.position.y >= topy)
            {
                Facetop = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -Speed);
            if (transform.position.y <= bottomy)
            {
                Facetop = true;
            }
        }
    }
}

