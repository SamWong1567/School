using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ConceptIntroduction : MonoBehaviour {

    GameObject gameManagerForCSS;
    GameManagerConceptSelectionScreen gcss;

    //variable for dialogue box
    public GameObject AcknowedgementBoxPrefab;
    GameObject acknowledgementBox;

    //keeps track of the number of pages of content
    int noOfPages = 0;

    List<string> listOfContentsByPage = new List<string>();
    List<string> listOfContentByLine = new List<string>();

    string tempLine;
    string tempStoreContent;

    //panel to display text
    public GameObject horizontalScrollingPanelPrefab;
    GameObject horizontalScrollingPanel;

    // Use this for initialization
    void Start ()
    {
        //retrieve the game object called GameManager
        gameManagerForCSS = GameObject.Find("GameManager");
        //retrieve the script called GameManagerConceptSelectionScreen.cs that is attached under GameManager
        gcss = gameManagerForCSS.GetComponent<GameManagerConceptSelectionScreen>();
        LoadContent();
    }

    //identify which concept the user chosen and retrieve the content in the file for that concept
    public void LoadContent()
    {

        string fileName = "";
        GameObject conceptNamePanel = GameObject.Find("Concept Name Panel on top of screen");
        Text conceptNamePanelText = conceptNamePanel.GetComponentInChildren<Text>();
        conceptNamePanelText.text = "test";
        //identify which concept the user chosen
        if (gcss.fileNum == 1)
        {
            fileName = "Basic Arithmetic Intro";
            //change the panel header to the corresponding concept name
            conceptNamePanelText.text = "Arithmetic Expressions";
        }
        else if (gcss.fileNum == 2)
        {
            fileName = "Datatype Intro";
            //change the panel header to the corresponding concept name
            conceptNamePanelText.text = "Datatype";
        }
        else if (gcss.fileNum == 3)
        {
            fileName = "Input Output Intro";
            //change the panel header to the corresponding concept name
            conceptNamePanelText.text = "Input Output";
        }
        else if (gcss.fileNum == 4)
        {
            fileName = "Conditional Statements Intro";
            //change the panel header to the corresponding concept name
            conceptNamePanelText.text = "Conditional Statements";
        }
        else if (gcss.fileNum == 5)
        {
            fileName = "Loops Intro";
            //change the panel header to the corresponding concept name
            conceptNamePanelText.text = "Loops";
        }
        else if (gcss.fileNum == 6)
        {
            fileName = "Assessment Intro";
            //change the panel header to the corresponding concept name
            conceptNamePanelText.text = "Assessment";
        }

        //retrieve the file in correspond to the concept chosen
        TextAsset file = Resources.Load(fileName) as TextAsset;
        tempStoreContent = file.ToString();

        //split the contents to specific pages
        listOfContentsByPage = tempStoreContent.Split('~').ToList();
        DisplayContent();
    }

    public void DisplayContent()
    {
        //Main area to spawn panels
        GameObject contentPanel = GameObject.Find("Container");
        //Text contentText = contentPanel.GetComponentInChildren<Text>();
        //if(noOfPages < listOfContentsByPage.Count)
        //spawn 1 panel per page
        for(int i = 0; i<listOfContentsByPage.Count; i++)
        {
            //spawn a panel for each page
            horizontalScrollingPanel = Instantiate(horizontalScrollingPanelPrefab) as GameObject;
            //set parent to parent panel
            horizontalScrollingPanel.transform.SetParent(contentPanel.transform, false);
            Text contentText = horizontalScrollingPanel.GetComponentInChildren<Text>();
            //split content into actual lines
            listOfContentByLine = listOfContentsByPage[noOfPages].Split('@').ToList();
            for (int j = 0; j < listOfContentByLine.Count; j++)
            {
                contentText.text += listOfContentByLine[j];
                contentText.text += "\n";
            }
            noOfPages++;
        }
        
        print(noOfPages + "asd");
        print(listOfContentsByPage.Count);
        /*
        if(noOfPages > listOfContentsByPage.Count)
        {
            //on exit, destroy the current gameManager to prevent having duplicated gameManagers
            Destroy(gameManagerForCSS);
            print("game manager destroyed after concept intro");
            gcss.LoadScene("Concept_Selection_Scene");
        }*/
    }

    //when the next button is pressed
    public void ClearText()
    {
        //display on the panel
        GameObject contentPanel = GameObject.Find("Concept Content Panel");
        Text contentText = contentPanel.GetComponentInChildren<Text>();
        //clear the text
        contentText.text = "";
        //load next content
        DisplayContent();
    }

    //for back button
    public void ReturnToMainMenu()
    {
        Destroy(gameManagerForCSS);
        gcss.LoadScene("Concept_Selection_Scene");
    }
}
