﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    [SerializeField]private float leftCap;
    [SerializeField]private float rightCap;

    [SerializeField] private float jumpLength = 10f;
    [SerializeField] private float jumpHeight = 15f;
    [SerializeField] private LayerMask ground;

    

    private Collider2D coll;
 
    private bool facingLeft = true;

    protected override void Start(){
        base.Start();
        coll = GetComponent<Collider2D>();

        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if(anim.GetBool("Jumping"));
        {
            if(rb.velocity.y < 0.1){
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);
            }
        }
        if(coll.IsTouchingLayers(ground) && anim.GetBool("Falling")){
            anim.SetBool("Falling", false);

        }
    }

 
    private void Move()
    {
        if (facingLeft)
        {
            //Test to see if we're beyond leftcap.
            if (transform.position.x > leftCap)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                //Test to see if I am on ground. If so jump
                if (coll.IsTouchingLayers(ground))
                {

                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            //If not, faceRight.
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightCap)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                //Test to see if I am on ground. If so jump
                if (coll.IsTouchingLayers(ground))
                {

                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            //If not, faceRight.
            else
            {
                facingLeft = true;
            }

        }
    }
   
   

}
