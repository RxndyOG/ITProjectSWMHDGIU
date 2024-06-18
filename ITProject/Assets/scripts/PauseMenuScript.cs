using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MainMenuOpeningScript : MonoBehaviour
{
    public GameObject PausePanel;

    // Update is called once per frame

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                Continue();
            }else if(Time.timeScale == 1)
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0;


    }


    public void Continue()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

}
