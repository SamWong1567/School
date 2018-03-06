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
            conceptNamePanelText.text = "Basic Arithmetic Expressions";
        }
        else if (gcss.fileNum == 2)
        {
            fileName = "Datatype Intro";
            //change the panel header to the corresponding concept name
            conceptNamePanelText.text = "Datatypes";
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
        //spawn 1 panel per page
        for(int i = 0; i<listOfContentsByPage.Count; i++)
        {
            //spawn a panel for each page
            horizontalScrollingPanel = Instantiate(horizontalScrollingPanelPrefab) as GameObject;
            //set parent to parent panel
            horizontalScrollingPanel.transform.SetParent(contentPanel.transform, false);
            Text contentText = horizontalScrollingPanel.GetComponentInChildren<Text>();
            //rename subheader panel to uniquely identify and display text
            GameObject subheaderName = GameObject.Find("Subheader panel");
            subheaderName.name += i;
            //split content into actual lines
            listOfContentByLine = listOfContentsByPage[noOfPages].Split('@').ToList();
            for (int j = 0; j < listOfContentByLine.Count; j++)
            {   
                //display first line as subheader for every page
                if(j == 0)
                {
                    GameObject subheaderPanel = GameObject.Find(subheaderName.name);
                    Text subheaderText = subheaderPanel.GetComponentInChildren<Text>();
                    //remove unnecessary new lines
                    listOfContentByLine[0] = listOfContentByLine[0].Replace("\n", String.Empty);
                    subheaderText.text = listOfContentByLine[0];
                    continue;
                }
                if(j == 1)
                {
                    //remove unnecessary new lines
                    listOfContentByLine[1] = listOfContentByLine[1].Replace("\n", String.Empty);
                }
                contentText.text += listOfContentByLine[j];
            }
            noOfPages++;
        }
        
        print(noOfPages + "asd");
        print(listOfContentsByPage.Count);
    }

    //when the quiz button is pressed
    public void LoadQuiz(String s)
    {
        gcss.LoadConceptQuestions(s);
    }

    //for back button
    public void ReturnToMainMenu()
    {
        Destroy(gameManagerForCSS);
        gcss.LoadScene("Concept_Selection_Scene");
    }
}
