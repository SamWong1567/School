using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.EventSystems;

public class GameManagerFillInTheBlanks : MonoBehaviour {

    //list of game objects
    List<GameObject> BlanksGameObjList = new List<GameObject>();
    List<GameObject> AnswersGameObjList = new List<GameObject>();

    //variables for displaying pseudocode
    GameObject parentPanel;
    GameObject panel;
    public GameObject panelPrefab;
    public GameObject pseudocodeBlockPrefab;

    //variables for displaying answer options
    public GameObject optionPanelPrefab;
    public GameObject answerBlockPrefab;
    int randomIndex;

    //variable for retrieving data from other scripts
    GameObject gameManagerForCSS;
    GameManagerConceptSelectionScreen gcss;
    AnswerButtonIndex ansBtnIndex;
    StoreAnsButtonIndex storeAnsBtnIndex;

    //variable for dialogue box
    public GameObject AcknowedgementBoxPrefab;
    GameObject acknowledgementBox;

    //Array of arrays to store the pseudocodes to be displayed
    string[][] arrayOfArrays = new string[30][];

    int numOfRows;
    int noOfBlanks = 0;
    int namingIndexForAnsButton = 1;

    void Start()
    {
        //retrieve the game object called GameManager
        gameManagerForCSS = GameObject.Find("GameManager");
        //retrieve the script called GameManagerConceptSelectionScreen.cs that is attached under GameManager
        gcss = gameManagerForCSS.GetComponent<GameManagerConceptSelectionScreen>();
        SplitString();
        DisplayPseudocode();
        StartCoroutine(DisplayAnsOptions());
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
        for(int i = 0; i<numOfRows; i++)
        {
            //instantiate panels with horizontal layout within the parent panel
            //this represents 1 row respectively
            GameObject subsequentPanels = Instantiate(panelPrefab) as GameObject;
            //make this panel as a child
            subsequentPanels.transform.SetParent(parentPanel.transform,false);
            //loops according to the number of columns of this specific row
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
                if(arrayOfArrays[i][j] == "_")
                {
                    codeText.text = "___";
                    noOfBlanks++;
                    //get the button component of the pseudocodeBlock
                    Button temp = pseudocodeBlock.GetComponent<Button>();
                    //add a onClick listener to the blanks
                    //method will remove selected answer from this blank
                    temp.onClick.AddListener(delegate { RemoveAnswer(); });
                    //add to gameObject list
                    BlanksGameObjList.Add(pseudocodeBlock);
                }
                else
                {
                    codeText.text = arrayOfArrays[i][j];
                }  
            }
        }
    }

    //Display the answer options for the specific fill in the blanks question
    IEnumerator DisplayAnsOptions()
    {
        //list to temporarily store the wrong answers
        List<string> tempList = new List<string>();
        int count = 0;

        while (true)
        {
            if (gcss.qnsList[gcss.randomNum].wrongAns.Length == count || string.IsNullOrEmpty(gcss.qnsList[gcss.randomNum].wrongAns[count]))
            {
                break;
            }
            tempList.Add(gcss.qnsList[gcss.randomNum].wrongAns[count]);
            count++;
        }
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
        //reset counter for below use
        count = 0;
        int listSize = tempList.Count;


        //get access to the panel to display the options
        GameObject parentOptionsPanel = GameObject.Find("OptionsPanel");
        //instantiate row
        GameObject optionPanel = Instantiate(optionPanelPrefab) as GameObject;
        //set parent to optionsPanel
        optionPanel.transform.SetParent(parentOptionsPanel.transform, false);
        //instantiate first option button
        GameObject answerBlock = Instantiate(answerBlockPrefab) as GameObject;
        //uniquely name each instantiated answer block prefab
        answerBlock.name = "Answer Option" + 0;
        //set parent to row
        answerBlock.transform.SetParent(optionPanel.transform, false);
        //keeps track of the row that i'm at
        GameObject row = optionPanel;
        

        //randoms a answer option to be displayed
        randomIndex = UnityEngine.Random.Range(0, tempList.Count);
        //displays the text on the answer block
        Text answerBlockText = answerBlock.GetComponentInChildren<Text>();
        answerBlockText.text = tempList[randomIndex];
        //remove this answer from the list
        tempList.RemoveAt(randomIndex);

        //get the width of answerBlock in the next frame
        yield return new WaitForEndOfFrame();
        float answerBlockWidth = GetWidthOfGameObject(answerBlock);
        //add listener to first instantiated button
        Button buttonTemp = answerBlock.GetComponent<Button>();
        buttonTemp.onClick.AddListener(delegate { SelectAnswer(); });
        //add it to the answer game object list
        AnswersGameObjList.Add(answerBlock);
        //assign the index of this gameObject to another script
        ansBtnIndex = answerBlock.GetComponent<AnswerButtonIndex>();
        ansBtnIndex.buttonIndex = count;

        //get width of optionPanel
        float optionPanelWidth = GetWidthOfGameObject(row);
        //get leftover space
        float availablePanelSpace = optionPanelWidth - answerBlockWidth;


        while (true)
        {
            //instantiate subsequent blocks
            GameObject subsequentBlock = Instantiate(answerBlockPrefab) as GameObject;
            subsequentBlock.name = "Answer Option" + namingIndexForAnsButton;
            namingIndexForAnsButton++;
            count++;
            //add listener to every instantiated button
            buttonTemp = subsequentBlock.GetComponent<Button>();
            buttonTemp.onClick.AddListener(delegate { SelectAnswer(); });
            //add it to the answer game object list
            AnswersGameObjList.Add(subsequentBlock);
            //assign the index of this gameObject to another script
            ansBtnIndex = subsequentBlock.GetComponent<AnswerButtonIndex>();
            ansBtnIndex.buttonIndex = count;

            randomIndex = UnityEngine.Random.Range(0, tempList.Count);
            answerBlockText = subsequentBlock.GetComponentInChildren<Text>();
            answerBlockText.text = tempList[randomIndex];
            tempList.RemoveAt(randomIndex);

            //get width of subsequent block
            yield return new WaitForEndOfFrame();
            float subsequentBlockWidth = GetWidthOfGameObject(subsequentBlock);


            if(subsequentBlockWidth <= availablePanelSpace)
            {   
                //set parent to current row
                subsequentBlock.transform.SetParent(row.transform, false);
                //update leftover space
                availablePanelSpace -= subsequentBlockWidth;
            }
            
            else
            {
                //make a new row
                row = Instantiate(optionPanelPrefab) as GameObject;
                //set new row to parentPanel
                row.transform.SetParent(parentOptionsPanel.transform, false);
                //get width of new row
                yield return new WaitForEndOfFrame();
                availablePanelSpace = GetWidthOfGameObject(row);
                //set block to new row
                subsequentBlock.transform.SetParent(row.transform, false);
                //update leftover space
                availablePanelSpace -= subsequentBlockWidth;
            }
                
            if((listSize - 1) == count)
            {
                break;
            }     
        }

    }

    public float GetWidthOfGameObject(GameObject obj)
    {
        RectTransform objRT = obj.GetComponent<RectTransform>();
        float temp = objRT.rect.width;
        return temp;
    }

    //method is called when user selects an answer
    //the answer will then be set to the very first blank
    public void SelectAnswer()
    {
        int temp;
        //get the game object of the button that is tapped
        GameObject ansButton = EventSystem.current.currentSelectedGameObject;
        //find the very first blank to fill the answer 
        for(int i = 0; i<BlanksGameObjList.Count; i++)
        {
            if(BlanksGameObjList[i].GetComponentInChildren<Text>().text == "___")
            {
                //keep track of which answer button was tapped
                temp = ansButton.GetComponentInChildren<AnswerButtonIndex>().buttonIndex;
                //store this index
                BlanksGameObjList[i].GetComponentInChildren<StoreAnsButtonIndex>().indexStored = temp;
                BlanksGameObjList[i].GetComponentInChildren<Text>().text = ansButton.GetComponentInChildren<Text>().text;
                //to signify the selection of the answer
                ansButton.GetComponentInChildren<Text>().text = "";      
                //disable the onClick listern of the button
                ansButton.GetComponent<Button>().interactable = false;
                break;
            }
        }
    }
    
    //method is called when user decides to remove the answer from the blank in the pseudocodes
    public void RemoveAnswer()
    {
        int index;
        //retrieve the gameObj for the blank
        GameObject blankPanel = EventSystem.current.currentSelectedGameObject;
        //if there is an existing answer on the blank
        if(blankPanel.GetComponentInChildren<Text>().text != "___")
        {
            //get the index of the answer button to return the text to
            index = blankPanel.GetComponentInChildren<StoreAnsButtonIndex>().indexStored;
            //restore the text back to the answer option button
            AnswersGameObjList[index].GetComponentInChildren<Text>().text = blankPanel.GetComponentInChildren<Text>().text;
            //enable the onClick listern of the button
            AnswersGameObjList[index].GetComponent<Button>().interactable = true;
            //AnswersGameObjList[index].GetComponent<>().enabled = true;
        }
        //resets the blank panel to empty again
        blankPanel.GetComponentInChildren<Text>().text = "___";
    }
    
    //check answers when the RUN CODE button is tapped
    public void CheckAnswer()
    {
        Boolean correct = true;
        GameObject canvas = GameObject.Find("Canvas");
        acknowledgementBox = Instantiate(AcknowedgementBoxPrefab) as GameObject;
        for (int i =0;i<BlanksGameObjList.Count;i++)
        {
            //as long as this condition is not triggered, the question is deemed to be answered correctly
            if(!(BlanksGameObjList[i].GetComponentInChildren<Text>().text.Equals(gcss.qnsList[gcss.randomNum].correctAnswer[i])))
            {
                print("Wrong   Ans");
                //dialogue box to appear to notify that user answered wrongly
                acknowledgementBox.GetComponentInChildren<Text>().text = "Better luck next time!" + " Current Score: " + gcss.score;
                acknowledgementBox.transform.SetParent(canvas.transform, false);
                acknowledgementBox.transform.localScale.Set(1, 1, 1);
                correct = false;
                break;
            }
        }
        
        if(correct)
        {
            //add score
            gcss.score += 1;
            //dialogue box to appear to notify that user answered correctly
            acknowledgementBox.GetComponentInChildren<Text>().text = "Good Job!" + " Current Score: " + gcss.score;
            acknowledgementBox.transform.SetParent(canvas.transform, false);
            acknowledgementBox.transform.localScale.Set(1, 1, 1);
        }
    }
}
