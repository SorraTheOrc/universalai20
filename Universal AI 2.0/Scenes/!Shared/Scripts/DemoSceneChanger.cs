using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UniversalAI
{

public class DemoSceneChanger : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(SceneManager.GetActiveScene().buildIndex > 0)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            if(SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
    
}