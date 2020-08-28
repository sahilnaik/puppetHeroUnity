using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : Enemy
{
  

    
    
    [SerializeField] private LayerMask Everything;
    




    private Collider2D coll;
 
   

    protected override void Start(){
        base.Start();
        coll = GetComponent<Collider2D>();

        anim = GetComponent<Animator>();

        _trans = GetComponent<Transform>();
        _startingPos = _trans.position;
    }
    Vector3 _startingPos;
    Transform _trans;
    
    void Update() {
        _trans.position = new Vector3(_startingPos.x, _startingPos.y + Mathf.PingPong(Time.time, 8), _startingPos.z);
    }
 }

 
    
   
   


