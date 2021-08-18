using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovesPlayer : MonoBehaviour
{

    public int runSpeed;
    float horizontal;
    float vertical;
    public Animator anim;
    bool facingRight;
    bool isCrouching;
    float countSlide;
    public Rigidbody2D rb;
    public float y;
    public bool isJumping;
    public float jumpForce;
    bool isAtk;

    private void Awake()
    {
        //takes animator script 
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.Sleep();
    }

    void Update()
    {
        //checks input for running
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        //changes speed based on inputs to start running animation
        anim.SetFloat("Speed", Mathf.Abs(horizontal != 0 ? horizontal : vertical));

        //if crouch button pressed and player is not moving
        if(Input.GetButton("Crouch") && (vertical == 0 && horizontal == 0))
        {
            //play crouch animation, is not sliding
            isCrouching = true;
            anim.SetBool("IsSliding", false);
            anim.SetBool("IsCrouching", isCrouching);
        }
        //else if player is moving while they go to crouch
        else if (Input.GetButtonDown("Crouch") && horizontal != 0 && !isCrouching)
        {
            //play slide animation for .5frames, is sliding to a crouch
            countSlide = 0.5f;
            anim.SetFloat("Speed", 0.0f);
            anim.SetBool("IsSliding", true);
        }
        //else if crouch button is released
        else if (Input.GetButtonUp("Crouch"))
        {
            //end crouch animation
            isCrouching = false;
            anim.SetBool("IsCrouching", isCrouching);
        }

        if(countSlide > 0)
        {
            //count down to 0
            anim.SetFloat("Speed", 0.0f);
            countSlide -= (1f * Time.deltaTime);
            if(countSlide <= 0)
            {
                //no longer sliding
                anim.SetBool("IsSliding", false);
            }
        }

    }

    private void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            isAtk = true;
            if(vertical != 0 || horizontal != 0)
            {
                vertical = 0;
                horizontal = 0;
                anim.SetFloat("Speed", 0f);
            }

            anim.SetTrigger("SwordSlash");
        }

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            y = transform.position.y;
            isJumping = true;
            rb.gravityScale = 1.5f;
            rb.WakeUp();
            rb.AddForce(new Vector2(0, jumpForce));
            anim.SetBool("IsJumping", isJumping);
        }

        if (transform.position.y <= y && isJumping)
        {
            OnLanding();
        }

        //if moving and not crouching
        if ((vertical != 0 || horizontal != 0) && !isCrouching && !isAtk)
        {
            //changes location of player based on input, mimicking run
            Vector3 movement = new Vector3(horizontal * runSpeed, vertical * runSpeed, 0.0f);
            transform.position = transform.position + movement * Time.deltaTime;
        }



        //flips sprite if moving to the left
        Flip(horizontal);

    }

    public void AlertObservers(string message)
    {
        if(message == "AttackEnded")
        {
            isAtk = false;
        }
    }

    private void Flip(float horizontal)
    {
        //if moving to the left and bool is false or if moving to the right and bool is true
        if(horizontal < 0 && !facingRight || horizontal > 0 && facingRight)
        {
            //bool is opposite of what it was, scale is inverted
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
    
  

    public void OnLanding()
    {
        isJumping = false;
        rb.gravityScale = 0f;
        rb.Sleep();
        y = transform.position.y;
        anim.SetBool("IsJumping", false);
   }
}

