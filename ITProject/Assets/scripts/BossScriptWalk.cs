using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class BossScriptWalk : MonoBehaviour
{

    [SerializeField] public GameObject player;

    private Animator anim;
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [SerializeField] public LayerMask playerLayer;
    [SerializeField] public LayerMask groundLayer;

    public bool facingRight = false;
    public float distanceToPlayer = 2f;
    public float moveDirect = -1;

    [SerializeField] public float walkSpeed = 1.0f;

    [SerializeField] public float TimerUntilNextAttack = 4f;
    public float TimeCounter;

    [SerializeField] public float TimerInAttack = 1f;
    public float TimeAttackCount;

    public bool inAttack = false;

    public bool inJump = false;
    [SerializeField] public float aoeRadius = 5f;
    [SerializeField] public float aoeDamage = 10f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");   

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
       
        TimeCounter = TimerUntilNextAttack;
        TimeAttackCount = TimerInAttack;
    }

    private void FixedUpdate()
    {

        if (!inAttack)
        {
            TimerUntilNextAttack -= Time.deltaTime;
        }

        // Wenn es Zeit für einen Angriff ist und der Spieler vor dem Boss ist
        if (TimerUntilNextAttack <= 0 && isPlayerAhead() && !inAttack)
        {
            StartAttack();
        }

        // Wenn der Boss sich im Angriff befindet, aktualisiere den Angriffstimer
        if (inAttack)
        {
            TimerInAttack -= Time.deltaTime;
            if (TimerInAttack <= 0)
            {
                EndAttack();
                dashDownAttack();
                //ApplyForceTowardsPlayer();
            }
        }

        // Bewege den Boss nur, wenn er nicht im Angriff ist
        if (!inAttack && isGround() && TimerUntilNextAttack < 5f)
        {
            isMoving();
        }

        if (isPlayerAhead())
        {
            Debug.Log("player in front");
        }

        if (isWallAhead())
        {
            Flip();
        }

        if (player.GetComponent<Transform>().position.x > transform.position.x && !facingRight)
        {
            Flip();
        }
        else if (player.GetComponent<Transform>().position.x < transform.position.x && facingRight)
        {
            Flip();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartAttack()
    {
        inAttack = true;
        isJumping();
        inJump = true;
    }

    void EndAttack()
    {
        inAttack = false;
        TimerInAttack = TimeAttackCount;
        TimerUntilNextAttack = TimeCounter;
        
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    bool isJumping()
    {

        rb.AddForce(Vector2.up*10f,ForceMode2D.Impulse);

        return true;
    }

    bool isMoving()
    {

        if (facingRight)
        {
            moveDirect = 1;
        }
        else
        {
            moveDirect = -1;
        }

        rb.velocity = new Vector2 (moveDirect * walkSpeed, rb.velocity.y);

        return true;
    }

    bool isGround()
    {
        Vector2 direction = Vector2.down;
        Vector2 origin = new Vector2(coll.bounds.center.x, coll.bounds.center.y);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, coll.bounds.extents.y + 0.1f, groundLayer);

        // Debug-Ray zeichnen
        Debug.DrawRay(origin, direction * (coll.bounds.extents.y + 0.1f), Color.red);

        return hit.collider != null;
    }

    bool dashDownAttack()
    {
        rb.AddForce(Vector2.down * 35f, ForceMode2D.Impulse);

        return true;
    }

    void ApplyForceTowardsPlayer()
    {
        float forceStrength = 20f;
        // Berechne die Richtung vom Boss zum Spieler
        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;

        // Wende die Kraft in Richtung des Spielers an
        rb.AddForce(directionToPlayer * forceStrength, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (inJump && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            inJump = false;
            TriggerAoEAttack();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aoeRadius);
    }

    void TriggerAoEAttack()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, aoeRadius, playerLayer);

        foreach (Collider2D player in hitPlayers)
        {
            // Hier können Sie den Spieler schädigen
            Debug.Log("Player hit with AoE attack!");
            // player.GetComponent<PlayerHealth>().TakeDamage(aoeDamage); // Beispiel für Schaden
        }

        // Sie können hier weitere AoE-Effekte hinzufügen (z.B. Animationen, Partikel)
    }

    [SerializeField] bool isWallAhead()
    {
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        Vector2 origin = new Vector2(coll.bounds.center.x, coll.bounds.min.y + 0.1f);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, coll.bounds.extents.x + 0.1f, groundLayer);

        // Debug-Ray zeichnen
        Debug.DrawRay(origin, direction * (coll.bounds.extents.x + distanceToPlayer), Color.red);

        return hit.collider != null;
    }

    [SerializeField] bool isPlayerAhead()
    {
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        Vector2 origin = new Vector2(coll.bounds.center.x, coll.bounds.min.y + 0.1f);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, coll.bounds.extents.x + distanceToPlayer, playerLayer);

        // Debug-Ray zeichnen
        Debug.DrawRay(origin, direction * (coll.bounds.extents.x + distanceToPlayer), Color.red);

        return hit.collider != null;
    }

}
