using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerOnBoarding : MonoBehaviour
{
    // Use this for initialization
    void Start ()
    {

    }

    //called when GET STARTED button is pressed
	public void GoToMainMenu()
    {
       SceneManager.LoadScene("Concept_Selection_Scene");
    }

}
