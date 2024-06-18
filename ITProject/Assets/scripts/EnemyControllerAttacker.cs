using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyControllerAttacker : MonoBehaviour
{

    

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private EnemyController enemyCon;

    [SerializeField] public float speedBoost;
    [SerializeField] public float originalSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        enemyCon = GetComponent<EnemyController>();

        originalSpeed = enemyCon.speed;
    }

    private void FixedUpdate()
    {

        if (isPlayerAhead())
        {
            enemyCon.speed = originalSpeed * 2;
        }
        else
        {
            enemyCon.speed = originalSpeed;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool isPlayerAhead()
    {
        Vector2 direction = enemyCon.facingRight ? Vector2.right : Vector2.left;
        Vector2 origin = new Vector2(coll.bounds.center.x, coll.bounds.min.y + 0.1f);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, coll.bounds.extents.x + 1f, enemyCon.playerLayer);

        // Debug-Ray zeichnen
        Debug.DrawRay(origin, direction * (coll.bounds.extents.x + 0.1f), Color.red);

        return hit.collider != null;
    }

}
