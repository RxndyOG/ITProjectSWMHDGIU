using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerFlying : MonoBehaviour
{

    [SerializeField] public float flyingTime = 0.5f;
    private float flyingCounter;
    private bool FlyingUp;

    private bool isGroundedBool;

    [SerializeField] public float attackPower = 5f;

    [SerializeField] public LayerMask groundLayer;

    public Rigidbody2D rb;
    public Collider2D coll;
    public Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        flyingCounter = flyingTime;
        
    }

    private void FixedUpdate()
    {

        flyingUp();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField]
    bool isGrounded()
    {
        // Prüft, ob der Boden unter dem Spieler den Ground Layer hat
        RaycastHit2D hit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        isGroundedBool = hit.collider != null;
        return isGroundedBool;
    }

    bool isCelling()
    {
        // Prüft, ob der Boden unter dem Spieler den Ground Layer hat
        RaycastHit2D hit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.up, 0.1f, groundLayer);
        isGroundedBool = hit.collider != null;
        return isGroundedBool;
    }

    [SerializeField] bool flyingUp()
    {
        if (flyingCounter > 0)
        {
            flyingCounter -= Time.deltaTime;
            
        }

        if (flyingCounter  <= 0)
        {
            flyingCounter = flyingTime;
            FlyingUp = !FlyingUp;
        }

        if (FlyingUp)
        {
            if (!isCelling())
            {
                rb.velocity = new Vector2(0, flyingCounter);
            }
            else if(isGrounded() || isCelling())
            {
                flyingCounter = 0;
            }

        }
        else
        {

            if (!isGrounded())
            {
                rb.velocity = new Vector2(0, flyingCounter * -1);

            }
            else if (isGrounded() || isCelling())
            {
                flyingCounter = 0;
            }
            
        }


        return true;
    }
}
