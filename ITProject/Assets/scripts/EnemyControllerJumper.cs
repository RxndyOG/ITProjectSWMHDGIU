using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerJumper : MonoBehaviour
{

    [SerializeField] public float jumpForce = 11f;

    [SerializeField] public LayerMask groundLayer;
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    public Animator anim;
    private EnemyController enemyCon;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        enemyCon = GetComponent<EnemyController>();
    }

    private void FixedUpdate()
    {
        if (isPlayerAhead() && isGrounded())
        {
            
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            
        }

        if (isGrounded())
        {
            //anim.SetBool("jumping",false);
        }
        else
        {
            //anim.SetBool("jumping", true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool isGrounded()
    {
        // Pr�ft, ob der Boden unter dem Spieler den Ground Layer hat
        RaycastHit2D hit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
     
        return hit.collider != null;
    }

    bool isPlayerAhead()
    {
        Vector2 direction = enemyCon.facingRight ? Vector2.right : Vector2.left;
        Vector2 origin = new Vector2(coll.bounds.center.x, coll.bounds.min.y + 0.1f);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, coll.bounds.extents.x + 0.5f, enemyCon.playerLayer);

        // Debug-Ray zeichnen
        Debug.DrawRay(origin, direction * (coll.bounds.extents.x + 0.1f), Color.red);

        return hit.collider != null;
    }

}
