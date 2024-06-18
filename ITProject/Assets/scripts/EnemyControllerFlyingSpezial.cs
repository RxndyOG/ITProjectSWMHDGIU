using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerFlyingSpezial : MonoBehaviour
{

    public float speed = 1.0f;

   
    public float amplitude = 1.0f;

    [SerializeField] public float attackPower = 5f;


    private float timeOffset;

    void Start()
    {       
        timeOffset = Random.Range(0, Mathf.PI * 2);
    }

    void Update()
    {

        float t = Time.time * speed + timeOffset;

 
        float x = Mathf.Sin(t) * amplitude;
        float y = Mathf.Sin(t * 2) * amplitude / 2;

        transform.position = new Vector3(x, y, transform.position.z);
    }
}
