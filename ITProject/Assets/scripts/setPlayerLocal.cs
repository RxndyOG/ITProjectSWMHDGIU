using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setPlayerLocal : MonoBehaviour
{
    private bool found = false;

    [SerializeField] public string preLevel;

    [SerializeField] public float playerX;
    [SerializeField] public float playerY;

    void Start()
    {
        // Suche nach einem GameObject mit dem Namen "MyGameObject"
        GameObject myObject = GameObject.Find("Player");

        // Überprüfe, ob das GameObject gefunden wurde
        if (myObject != null && found == false)
        {
            found = true;

            // Erhalte das Transform-Component des GameObjects
            Transform myObjectTransform = myObject.transform;

            if (myObject.GetComponent<PlayerController>().preScene == preLevel)
            {
                // Setze die neue Position
                myObjectTransform.position = new Vector3(playerX, playerY, myObjectTransform.position.z);
            }
            // Alternative: myObjectTransform.position = new Vector3(newX, newY, 0f);
            // Wenn du nur die x- und y-Position ändern möchtest und die z-Position beibehalten möchtest

            // Debug-Ausgabe zur Überprüfung
            Debug.Log("Neue Position gesetzt: " + myObjectTransform.position);
        }
    }

}
