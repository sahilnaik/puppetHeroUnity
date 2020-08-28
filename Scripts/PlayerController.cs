using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using  TMPro;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    //Start variables
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    

    //fsm
    private enum State {idle, Running, Jumping, falling, hurt}
    private State state = State.idle;
   

    //Inspector Variables
    [SerializeField] private LayerMask ground;
    [SerializeField]private float jumpforce = 10f;
    [SerializeField]private float speed = 5f;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private AudioSource cherry;
    [SerializeField] private AudioSource footstep;

    [SerializeField] private AudioSource jump;
    [SerializeField] private AudioSource hurt;

    public Vector3 respawnPoint;


   

    private void Start(){
        Time.timeScale = 1;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
        respawnPoint = transform.position;
       
    }

    private void Update()
    {
        Movement();
        AnimationState();
        anim.SetInteger("State", (int)state); //sets animation based on enum state
        if(state != State.hurt)
        {
            Movement();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collectible")
        {
            cherry.Play();
            Destroy(collision.gameObject);
            PermanentUI.perm.cherries += 1;
            PermanentUI.perm.cherryText.text = PermanentUI.perm.cherries.ToString();
            if(PermanentUI.perm.cherries == 15)
            {
                PermanentUI.perm.cherries = 0;
                PermanentUI.perm.cherryText.text = PermanentUI.perm.cherries.ToString();
                PermanentUI.perm.health += 1;
                PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
            }
            
        }
        if(collision.tag == "Powerup"){
            Destroy(collision.gameObject);
            jumpforce = 18f;
            GetComponent<SpriteRenderer>().color = Color.green;
            StartCoroutine(ResetPower());
        }
        if(collision.tag == "PowerupSprint"){
            Destroy(collision.gameObject);
            speed = 11f;
            GetComponent<SpriteRenderer>().color = Color.blue;
            StartCoroutine(ResetPower());
        }

        if(collision.tag == "FallDetector"){
            transform.position = respawnPoint;
            PermanentUI.perm.health -= 1;
            PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
            if (PermanentUI.perm.health <= 0)
            {
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                SceneManager.LoadScene("GameOver");

                Destroy(gameObject);

            }
        }
        if(collision.tag == "Checkpoint"){
            if (PermanentUI.perm.health <= 0)
            {
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                SceneManager.LoadScene("GameOver");
               
                Destroy(gameObject);

            }
            else{
                respawnPoint = collision.transform.position;
            }
            
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if(state== State.falling)
            {
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                hurt.Play();
                state = State.hurt;
                HandleHealth();  //Reset lvl if ded
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy to right. Therefore hurt in left
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                    GetComponent<SpriteRenderer>().color = Color.red;
                    StartCoroutine(ResetHurt());

                }

                else
                {
                    //Enemy to left. Therefore hurt to right
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                    GetComponent<SpriteRenderer>().color = Color.red;
                    StartCoroutine(ResetHurt());

                }
            }
        }

        
    }

    private void HandleHealth() 
    {
        PermanentUI.perm.health -= 1;
        PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
        if (PermanentUI.perm.health <= 0)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name)
            SceneManager.LoadScene("GameOver");
         
            Destroy(gameObject);
        }
    }

    private void Movement()
    {
        float hDirection = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        float hpc = Input.GetAxisRaw("Horizontal");

        //Moving left
        if ((hDirection < 0) || (hpc < 0))
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);

        }

        //moving right
        else if ((hDirection > 0) || (hpc > 0))
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);

        }
        

        //jump
        if ((CrossPlatformInputManager.GetButtonDown("Jump") || Input.GetButtonDown("Jump")) && coll.IsTouchingLayers(ground))
        {
            Jump();

        }
        
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpforce);
        state = State.Jumping;
        jump.Play();

    }
    private void AnimationState()
    {
        if(state== State.Jumping)
        {
            if(rb.velocity.y < 0.1f){
                state = State.falling;
            }


        }
        else if(state== State.falling){
            if(coll.IsTouchingLayers(ground)){
                state=State.idle;
            }
        }
        else if(state == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < 0.1f){
                state = State.idle;
            }
        }
        else if(Mathf.Abs(rb.velocity.x) > 2f)
        {
            //Moving
            state = State.Running;
        }
        else
        {
            state = State.idle;
        }
    }

    private void Footstep()
    {
        footstep.Play();
    }

    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(4);
        jumpforce = 12;
        speed = 7;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    private IEnumerator ResetHurt()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}













 
