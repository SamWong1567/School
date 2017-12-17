using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcknowledgeToProceed : MonoBehaviour {

    GameObject gameManagerForCSS;
    GameManagerConceptSelectionScreen gcss;

    // Use this for initialization
    void Start ()
    {
        //retrieve the game object called GameManager
        gameManagerForCSS = GameObject.Find("GameManager");
        //retrieve the script called GameManagerConceptSelectionScreen.cs that is attached under GameManager
        gcss = gameManagerForCSS.GetComponent<GameManagerConceptSelectionScreen>();
    }

    public void ProceedToNextQuestion()
    {
        gcss.CallNextQuestion();
    }
}
