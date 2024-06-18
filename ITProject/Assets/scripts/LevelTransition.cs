using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{

    [SerializeField] public string SceneNameGoTo;
    [SerializeField] public string SceneNameGoFrom;

    public bool isNextScene = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") {

            collision.gameObject.GetComponent<PlayerController>().preScene = SceneNameGoFrom;
            SceneManager.LoadScene(SceneNameGoTo);
        }
    }
}
