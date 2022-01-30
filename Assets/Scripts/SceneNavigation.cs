using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneNavigation : MonoBehaviour
{
    public void ToRTSScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("RTSView");
    }
    public void ToFPPScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("FirstPersonTesting");
    }
    public void QuitGame()
    {
        Application.Quit();
    }


    //add too game scene
}
