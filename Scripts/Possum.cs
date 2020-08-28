using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possum : Enemy
{

    [SerializeField] private bool leftCap;
  
    [SerializeField] private float speed;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float sizex;
    [SerializeField] private float sizey;



    private Collider2D coll;


    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();

        anim = GetComponent<Animator>();

    }
    private void Update()
    {
        if (leftCap)
        {
            transform.Translate(-2 * Time.deltaTime * speed, 0, 0);
            transform.localScale = new Vector2(sizex, sizey);
        }
        else
        {
            transform.Translate(2 * Time.deltaTime * speed, 0, 0);
            transform.localScale = new Vector2(-sizex, sizey);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Possumturn"))
        {
            if (leftCap)
            {
                leftCap = false;
            }
            else
            {
                leftCap = true;
            }
        }
    }

}