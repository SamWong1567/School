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

        //identify which concept the user chosen
        if (gcss.fileNum == 1)
        {
            fileName = "Basic Arithmetic Intro.txt";
        }
        else if (gcss.fileNum == 2)
        {
            fileName = "Datatype Intro.txt";
        }
        else if (gcss.fileNum == 3)
        {
            fileName = "Input Output Intro.txt";
        }
        else if (gcss.fileNum == 4)
        {
            fileName = "Conditional Statements Intro.txt";
        }
        else if (gcss.fileNum == 5)
        {
            fileName = "Loops Intro.txt";
        }
        else if (gcss.fileNum == 6)
        {
            fileName = "Assessment Intro.txt";
        }

        //retrieve the file in correspond to the concept chosen
        //get relative path to file as opposed to abosolute so that the file can be read on any computer
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets");
        filePath = Path.Combine(filePath, "Questions_Answers_Codes");
        filePath = Path.Combine(filePath, fileName);
        StreamReader sr = new StreamReader(filePath);
        try
        {
            while(true)
            {
                tempLine = sr.ReadLine();
                //if end of line, terminate
                if(tempLine == null)
                {
                    break;
                }
                else
                {
                    tempStoreContent += tempLine;
                }
            }
            sr.Close();
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
        //split the contents to specific pages
        listOfContentsByPage = tempStoreContent.Split('~').ToList();
        DisplayContent();
    }

    public void DisplayContent()
    {
        //display on the panel
        GameObject contentPanel = GameObject.Find("Concept Content Panel");
        Text contentText = contentPanel.GetComponentInChildren<Text>();
        if(noOfPages < listOfContentsByPage.Count)
        {
            //split content into actual lines
            listOfContentByLine = listOfContentsByPage[noOfPages].Split('@').ToList();
            for (int i = 0; i < listOfContentByLine.Count; i++)
            {
                contentText.text += listOfContentByLine[i];
                contentText.text += "\n";
            }
        }
        noOfPages++;
        print(noOfPages + "asd");
        print(listOfContentsByPage.Count);
        if(noOfPages > listOfContentsByPage.Count)
        {
            gcss.LoadScene("Concept_Selection_Scene");
        }
    }

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

    private void OnDestroy()
    {
        Debug.Log("Concept Intro was destroyed");
    }



}
