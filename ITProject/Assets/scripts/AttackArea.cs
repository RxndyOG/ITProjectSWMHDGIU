using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackArea : MonoBehaviour
{
    //private int damage = 3;
    private void OntriggerEnter2D(Collider2D collider)
    {
        Console.WriteLine("hit");
       /* if(collider.GetComponent<Health>() != null)
        {
            Health health = collider.GetComponent<Health>();
            health.Damage(damage);
        }*/
    }
}
