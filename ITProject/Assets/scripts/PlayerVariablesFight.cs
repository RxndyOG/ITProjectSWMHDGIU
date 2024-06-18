using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVariablesFight : MonoBehaviour
{

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private PlayerController playerCon;


    [SerializeField] public float Health = 100;
    [SerializeField] public float currentHealth;

    [SerializeField] public float attackPower = 10;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        playerCon = GetComponent<PlayerController>();

        currentHealth = Health;
    }

    private void FixedUpdate()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Player Dead");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy"&& gameObject.tag!="playerAttackArea")
        {
            if (collision.gameObject.GetComponent<EnemyController>() != null)
            {
                collision.gameObject.GetComponent<EnemyController>().MoveDirection = -1 * collision.gameObject.GetComponent<EnemyController>().MoveDirection;
                collision.gameObject.GetComponent<EnemyController>().facingRight = !collision.gameObject.GetComponent<EnemyController>().facingRight;

            }

            currentHealth = currentHealth - collision.gameObject.GetComponent<EnemyVariables>().attackPower;

            Debug.Log("Player got damaged!");
            Debug.Log(currentHealth);

        }
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    bool isEnemyAhead()
    {
        Vector2 direction = playerCon.facingRight ? Vector2.right : Vector2.left;
        Vector2 origin = new Vector2(coll.bounds.center.x, coll.bounds.min.y + 0.2f);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, coll.bounds.extents.x + 0.1f, playerCon.enemyLayer);

        // Debug-Ray zeichnen
        Debug.DrawRay(origin, direction * (coll.bounds.extents.x + 0.1f), Color.red);

        return hit.collider != null;
    }
}
