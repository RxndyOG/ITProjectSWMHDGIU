using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVariables : MonoBehaviour
{

    [SerializeField] public float attackPower = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "playerAttackArea")
        {
            Debug.Log("Enemy Getroffen");

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
