using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToHomeDialogueBox : MonoBehaviour {

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
    //Leave and quit the quiz
    public void YesButton()
    {
        //destroy persisting gameObj upon exit
        Destroy(gameManagerForCSS);
        gcss.LoadScene("Concept_Selection_Scene");
    }
    //Stay and destroy the dialogue box
    public void NoButton()
    {
        GameObject dialogueBox = GameObject.Find("Return to home dialogue box(Clone)");
        Destroy(dialogueBox);
    }
}
