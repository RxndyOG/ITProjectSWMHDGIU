using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class OnSceneLoad : MonoBehaviour
{
    private static OnSceneLoad instance;

    public CinemachineConfiner confiner;
    public PolygonCollider2D test;

    public GameObject collisionTest;


    void Awake()
    {

        

        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        confiner = GetComponent<CinemachineConfiner>();

        if (confiner.m_BoundingShape2D == null)
        {
            collisionTest = GameObject.Find("CameraBoundary");

            test = collisionTest.GetComponent<PolygonCollider2D>();

            

            confiner.m_BoundingShape2D = test;
        }
        
    }
}
