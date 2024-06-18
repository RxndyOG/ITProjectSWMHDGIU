using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.Image;

public class EnemyController : MonoBehaviour
{

    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public LayerMask enemyLayer;
    [SerializeField] public LayerMask playerLayer;
    [SerializeField] public LayerMask transitionLayer;

    [SerializeField] public float attackPower = 5f;

    [SerializeField] public float speed;
    [SerializeField] public bool facingRight = true;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;

    public int MoveDirection = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        

    }

    private void FixedUpdate()
    {
        if (isWallAhead() || isEnemyAhead() || isTransitionAhead())
        {
            MoveDirection = -1 * MoveDirection;
            facingRight = !facingRight;
        }

        if (facingRight)
        {
            Vector3 scaler = transform.localScale;
            scaler.x = 1;
            transform.localScale = scaler;
        }
        else if (!facingRight)
        {
            Vector3 scaler = transform.localScale;
            scaler.x = -1;
            transform.localScale = scaler;
        }

        isMoving();
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool isMoving()
    {

        rb.velocity = new Vector2(speed * MoveDirection, rb.velocity.y);
        

        return true;
    }

    [SerializeField] bool isEnemyAhead()
    {
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        Vector2 origin = new Vector2(coll.bounds.center.x, coll.bounds.min.y + 0.1f);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, coll.bounds.extents.x + 0.1f, enemyLayer);

        // Debug-Ray zeichnen
        Debug.DrawRay(origin, direction * (coll.bounds.extents.x + 0.1f), Color.red);

        return hit.collider != null;
    }

    [SerializeField] bool isTransitionAhead()
    {
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        Vector2 origin = new Vector2(coll.bounds.center.x, coll.bounds.min.y + 0.1f);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, coll.bounds.extents.x + 0.05f, transitionLayer);

        // Debug-Ray zeichnen
        Debug.DrawRay(origin, direction * (coll.bounds.extents.x + 0.05f), Color.red);

        return hit.collider != null;
    }

    [SerializeField] bool isPlayerAhead()
    {
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        Vector2 origin = new Vector2(coll.bounds.center.x, coll.bounds.min.y+0.1f);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, coll.bounds.extents.x + 0.05f, playerLayer);

        // Debug-Ray zeichnen
        Debug.DrawRay(origin, direction * (coll.bounds.extents.x + 0.05f), Color.red);

        return hit.collider != null;
    }

    [SerializeField] bool isWallAhead()
    {
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        Vector2 origin = new Vector2(coll.bounds.center.x, coll.bounds.min.y+0.1f);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, coll.bounds.extents.x + 0.1f, groundLayer);

        // Debug-Ray zeichnen
        Debug.DrawRay(origin, direction * (coll.bounds.extents.x + 0.1f), Color.red);

        return hit.collider != null;
    }
}
