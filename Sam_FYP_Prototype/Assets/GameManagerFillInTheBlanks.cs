using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class GameManagerFillInTheBlanks : MonoBehaviour {

    GameObject gameManagerForCSS;
    GameObject parentPanel;
    GameObject panel;
    public GameObject panelPrefab;
    public GameObject pseudocodeBlockPrefab;

    public GameObject optionPanelPrefab;
    public GameObject answerBlockPrefab;

    GameManagerConceptSelectionScreen gcss;

    string[] pseudocodes;

    //Array of arrays to store the pseudocodes to be displayed
    string[][] arrayOfArrays = new string[30][];

    int numOfRows;

    // Use this for initialization
    void Start ()
    {
        //retrieve the game object called GameManager
        gameManagerForCSS = GameObject.Find("GameManager");
        //retrieve the script called GameManagerConceptSelectionScreen.cs that is attached under GameManager
        gcss = gameManagerForCSS.GetComponent<GameManagerConceptSelectionScreen>();
        SplitString();
        DisplayPseudocode();
        DisplayAnsOptions();

    }

    //split the string that is read in from the file
    public void SplitString()
    {
        //split the pseudocodes based on the delimiter "new line"
        string[] templist = gcss.qnsList[gcss.randomNum].question.Split('\n');

        numOfRows = templist.Length;

        string[] templist2;
        //loop according to the number of pseudocode statements
        for (int i = 0; i < templist.Length; i++)
        {
            //preserve the underscore using regex
            //won't add any empty spaces generated from regex as Regex.split generates a empty space before and after the delimiter
            templist2 = Regex.Split(templist[i],"(_)").Where(s=>!string.IsNullOrEmpty(s)).ToArray();
            print(templist2.Length + " templist2 size");
            //declare number of columns per row
            arrayOfArrays[i] = new string[templist2.Length];
            //storing the array across the whole row
            arrayOfArrays[i] = templist2;
        }
    }

    ////display pesudocode onto the panel
    public void DisplayPseudocode()
    {
        //parent panel
        parentPanel = GameObject.Find("Panel");
        //loop according to the total number of rows in the array of arrays
        print(numOfRows);
        for(int i = 0; i<numOfRows; i++)
        {
            //instantiate panels with horizontal layout within the parent panel
            //this represents 1 row respectively
            GameObject subsequentPanels = Instantiate(panelPrefab) as GameObject;
            //make this panel as a child
            subsequentPanels.transform.SetParent(parentPanel.transform,false);
            //loops according to the number of columns of this specific row
            print("length of row" +i + arrayOfArrays[i].Length);
            for(int j = 0; j<arrayOfArrays[i].Length; j++)
            {
                //print button for each element
                GameObject pseudocodeBlock = Instantiate(pseudocodeBlockPrefab) as GameObject;
                pseudocodeBlock.transform.SetParent(subsequentPanels.transform,false);
                //get access to change the text on the button
                Text codeText = pseudocodeBlock.GetComponentInChildren<Text>();
                //if string is a pseudocode
                if (arrayOfArrays[i][j] != "_")
                {
                    Image image = pseudocodeBlock.GetComponent<Image>();
                    Color c = image.color;
                    //changing the alpha to 0 for complete transparency
                    c.a = 0;
                    image.color = c;
                }
                //if string is a blank to be filled in extend the blank for visibility 
                //else print the psuedocode statement
                codeText.text = arrayOfArrays[i][j] == "_"? "___": arrayOfArrays[i][j];
                
            }
        }
        
        
        //blanks.transform.SetParent(imageParent.transform);
        //scale the instantiated prefab to its original size. Became smaller after instantiating
        //blanks.transform.localScale = new Vector3(1, 1, 1);
        
        //get height of component
        //float width =  (panel.GetComponent<panel>().rect.width)/2;
        //float height = (panel.GetComponent<RectTransform>().rect.height)/2 ;
        //positioning it
        //blanks.transform.localPosition = Vector3.zero;
    }

    //Display the answer options for the specific fill in the blanks question
    public void DisplayAnsOptions()
    {
        //list to temporarily store the wrong answers
        List<string> tempList = new List<string>();
        int count = 0;

        while (true)
        {
            if (gcss.qnsList[gcss.randomNum].wrongAns.Length == count || string.IsNullOrEmpty(gcss.qnsList[gcss.randomNum].wrongAns[count]))
            {
                print("reached");
                break;
            }
            tempList.Add(gcss.qnsList[gcss.randomNum].wrongAns[count]);
            count++;
        }
        print(tempList.Count + " size of list");
        print(tempList[0]);
        //reset counter
        count = 0;

        //add the correct answers into the list as well in order to be displayed
        while (true)
        {
            if(gcss.qnsList[gcss.randomNum].correctAnswer.Length == count || string.IsNullOrEmpty(gcss.qnsList[gcss.randomNum].correctAnswer[count]))
            {
                break;
            }
            tempList.Add(gcss.qnsList[gcss.randomNum].correctAnswer[count]);
            count++;
        }

        print(tempList.Count + " ackasndkansdkasn kasndkl nasdkasn ls");
        //get access to the panel to display the options
        GameObject parentOptionsPanel = GameObject.Find("OptionsPanel");
        
        //instantiate stuff
        for(int i =0; i<tempList.Count; i++)
        {
            //instantiate row
            GameObject optionPanel = Instantiate(optionPanelPrefab) as GameObject;
            //set parent to optionsPanel
            optionPanel.transform.SetParent(parentOptionsPanel.transform, false);
            //instantiate option button
            GameObject answerBlock = Instantiate(answerBlockPrefab) as GameObject;
            //set parent to row
            answerBlock.transform.SetParent(optionPanel.transform, false);

            /*
             *Instantiate button
                then current width - button.width
                if(nextButton.width < leftOverWidth)
                {continue to instantiate}
                else{
                make a new panel (row)}
                //repeat for loop
             */
        }

    }



}
