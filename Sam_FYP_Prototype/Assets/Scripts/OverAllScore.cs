using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverAllScore : MonoBehaviour {
    GameObject gameManagerForCSS;
    GameManagerConceptSelectionScreen gcss;

    // Use this for initialization
    void Start ()
    {
        //retrieve the game object called GameManager
        gameManagerForCSS = GameObject.Find("GameManager");
        //retrieve the script called GameManagerConceptSelectionScreen.cs that is attached under GameManager
        gcss = gameManagerForCSS.GetComponent<GameManagerConceptSelectionScreen>();
        DisplayAllScores();
    }

    public void DisplayAllScores()
    {
        int temp;
        GameObject panel = GameObject.Find("ScoreBoard");
        Text panelText = panel.GetComponentInChildren<Text>();
        panelText.text = "<b>Basic Arithmetic</b>";
        for (int i = 0; i<PlayerPrefs.GetInt("Basic Arithmetic Attempts"); i++)
        {
            //to display the very first Attempt Number to be 1 instead of 0
            temp = i;
            temp = ++temp;
            panelText.text += "\nAttempt Number: " + temp + "\tScore: " + PlayerPrefs.GetInt("Basic Arithmetic Save"+i);
        }
        panelText.text += "\n";

        panelText.text += "\n<b>Datatype</b>";
        for (int i = 0; i <PlayerPrefs.GetInt("Datatype Attempts"); i++)
        {
            temp = i;
            temp = ++temp;
            panelText.text += "\nAttempt Number: " + temp + "\tScore: " + PlayerPrefs.GetInt("Datatype Save"+i);
        }
        panelText.text += "\n";

        panelText.text += "\n<b>Input & Output</b>";
        for (int i = 0; i <PlayerPrefs.GetInt("Input Output Attempts"); i++)
        {
            temp = i;
            temp = ++temp;
            panelText.text += "\nAttempt Number: " + temp + "\tScore: " + PlayerPrefs.GetInt("Input Output Save"+i);
        }
        panelText.text += "\n";

        panelText.text += "\n<b>Conditional Statements</b>";
        for (int i = 0; i <PlayerPrefs.GetInt("Conditional Statements Attempts"); i++)
        {
            temp = i;
            temp = ++temp;
            panelText.text += "\nAttempt Number: " + temp + "\tScore: " + PlayerPrefs.GetInt("Conditional Statements Save"+i);
        }
        panelText.text += "\n";

        panelText.text += "\n<b>Loops</b>";
        for (int i = 0; i <PlayerPrefs.GetInt("Loops Attempts"); i++)
        {
            temp = i;
            temp = ++temp;
            panelText.text += "\nAttempt Number: " + temp + "\tScore: " + PlayerPrefs.GetInt("Loops Save"+i);
        }
        panelText.text += "\n";

        panelText.text += "\n<b>Assessment</b>";
        for (int i = 0; i <PlayerPrefs.GetInt("Assessment Attempts"); i++)
        {
            panelText.text += "\nAttempt Number: " + i + "\tScore: " + PlayerPrefs.GetInt("Assessment Save"+i);
        }
    }

    public void ReturnToMainMenu()
    {
        gcss.LoadScene("Concept_Selection_Scene");
    }
	
    
}
