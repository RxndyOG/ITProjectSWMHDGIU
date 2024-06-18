using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEdgeDetection : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
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
        if (!isEdgeAhead())
        {
            enemyCon.facingRight = !enemyCon.facingRight;
            enemyCon.MoveDirection = -1 * enemyCon.MoveDirection;
        }
    }

    [SerializeField] bool isEdgeAhead()
    {
        // Get the bounds of the object
        Bounds bounds = coll.bounds;

        // Determine the lower corner based on the facing direction
        Vector3 lowerCorner;
        if (enemyCon.facingRight)
        {
            // Bottom-right corner
            lowerCorner = new Vector3(bounds.max.x, bounds.min.y, transform.position.z);
        }
        else
        {
            // Bottom-left corner
            lowerCorner = new Vector3(bounds.min.x, bounds.min.y, transform.position.z);
        }

        // Cast a ray downward from the lower corner
        float rayLength = 0.5f; // Adjust the length as needed
        RaycastHit2D hit = Physics2D.Raycast(lowerCorner, Vector2.down, rayLength, enemyCon.groundLayer);

        Debug.DrawRay(lowerCorner, Vector2.down * rayLength, Color.red);

        if (hit.collider != null)
        {
            return true; // Ground detected
        }

        return false; // No ground detected
    }
}
