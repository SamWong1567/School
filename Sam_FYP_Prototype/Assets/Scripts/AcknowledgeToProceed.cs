using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        //update that user attempted the quesiton

        //checks if whether user is at the last question
        if(gcss.qnsList.Count == 8)
        {
            GameObject proceedButtonObj = GameObject.Find("Next button");
            Text proceedButtonText = proceedButtonObj.GetComponentInChildren<Text>();
            proceedButtonText.text = "FINISH";
        }
    }

    public void ProceedToNextQuestion()
    {
        gcss.CallNextQuestion();
    }

}
