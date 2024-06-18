using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverlayScript : MonoBehaviour
{

    [SerializeField] public GameObject TextFieldObject;


    [SerializeField] public PlayerVariablesFight playerVar;

    public TextMeshProUGUI TextMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        TextMeshPro = TextFieldObject.GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        TextMeshPro.text = playerVar.currentHealth.ToString();
    }
}
