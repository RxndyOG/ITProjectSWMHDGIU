using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    private GameObject attackArea = default;
    Transform target;

    [SerializeField] public float moveSpeed = 11f;
    [SerializeField] public float jumpForce = 10f;
    private float timeToAttack = 0.25f;
    private float timer = 0f;
    private float Dash_max = 0.25f;
    private float Dash_time = 0f;
    private bool dashing = false;
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask enemyLayer;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [SerializeField] public float coyoteTime = 0.2f; // Zeit in Sekunden, die der Spieler nach Verlassen der Plattform noch springen kann
    [SerializeField] private Transform wallCheck;
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
   
    private bool isGroundedBool;
    private float coyoteTimeCounter; // Z�hler f�r Coyote Time

    public bool facingRight = true;
    private bool attacking = false;

    public float moveInput;

    public bool doubleJump = false;

    private static PlayerController instance;
    public string preScene;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        attackArea = GameObject.FindWithTag ("playerAttackArea");
        target = attackArea.transform;
        attackArea.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (isGrounded() && isMoving())
        {
            anim.SetBool("walking", true);
        }
        else
        {
            anim.SetBool("walking", false);
        }

        if (!isGrounded())
        {
            anim.SetBool("jumping", true);
        }
        else
        {
            anim.SetBool("jumping", false);
        }


        if (attacking)
        {
            anim.SetBool("attacking", true);
        }
        else if(!attacking)
        {
            anim.SetBool("attacking", false);
        }
        

    }

    void Update()
    {
         Debug.Log(isWallJumping);
        isGrounded();
        isJumping();
        isMoving();
        WallSlide();
        IsWalled();
        WallJump();

        attackArea.transform.position=rb.position;
        

        if (isGrounded())
        {
            coyoteTimeCounter = coyoteTime; // Reset Coyote Time
            doubleJump = true;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; // Coyote Time verringern
        }

        moveInput = Input.GetAxis("Horizontal");

        if(moveInput == 0 && !isWallJumping)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        
        if(Input.GetButtonDown("Fire"))
        {
            Attack();     
            
            if(facingRight)
            {
             attackArea.transform.position+= new Vector3(1.2f,0,0);
            }
            if(!facingRight)
            {
                attackArea.transform.position+= new Vector3(-1.2f,0,0);
                attackArea.transform.localScale*=-1;
             }
        }
        if(Input.GetButtonDown("Dash"))
        {
            Dash();
        }
    	if(attacking)
        {
            timer += Time.deltaTime;
            if(facingRight)
            {
             attackArea.transform.position+= new Vector3(1.2f,0,0);
            }
            if(!facingRight)
            {
                attackArea.transform.position+= new Vector3(-1.2f,0,0);

             }
            if(timer > timeToAttack)
            {
                timer=0;
                attacking=false;
                
                attackArea.SetActive(attacking);
                if(!facingRight)
                {
                attackArea.transform.localScale*=-1;
                }
                
            }
        }
        if(dashing)
        {
            Dash_time += Time.deltaTime;
            if(Dash_time >= Dash_max)
            {
                Dash_time=0;
                dashing=false;
                moveSpeed = 11f;
            }
        }
        if(isWallJumping)
        {
            wallJumpingTime += Time.deltaTime;
            Debug.Log(wallJumpingTime);
            if(wallJumpingTime >= wallJumpingDuration)
            {
                wallJumpingTime=0;
                isWallJumping=false;
            }
        }
    }
    private void Attack()
    {
        attacking = true;
        attackArea.SetActive(attacking);
    }
    void Dash()
    {   if(dashing==false)
        {
            moveSpeed = 40f;
        }
        dashing = true;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    bool isGrounded()
    {
        // Pr�ft, ob der Boden unter dem Spieler den Ground Layer hat
        RaycastHit2D hit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        isGroundedBool = hit.collider != null;
        return isGroundedBool;
    }

    bool isMoving()
    {
        
        if (moveInput != 0)
        {
            if (moveInput > 0 && !facingRight)
            {
                Flip();
            }
            else if (moveInput < 0 && facingRight)
            {
                Flip();
            }
            if(!isWallJumping)
            {
                rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
            }
                
            
            return true;
        }
        else
        {
            return false;
        }
    }

    bool isJumping()
    {
        if (coyoteTimeCounter > 0 && Input.GetButtonDown("Jump") && !isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            return true;
        }
        else
        {
            if (!(coyoteTimeCounter > 0) && Input.GetButtonDown("Jump") && doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                doubleJump = false;
                return true;
            }
            return false;
        }
    }

    private bool IsWalled()
    {
        Console.WriteLine("Wall");
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if(IsWalled() && !isGrounded() && moveInput != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if(isWallSliding&&Input.GetButtonDown("Jump"))
        {
            isWallJumping = true;
            wallJumpingDirection = -transform.localScale.x;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            if(transform.localScale.x != wallJumpingDirection)
            {
                facingRight = !facingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
            
        }
    }
    


}


