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
    List<GameObject> blanksGameObjList = new List<GameObject>();
    List<GameObject> answersGameObjList = new List<GameObject>();

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
    public GameObject resultOutcomePanelPrefab;
    GameObject resultOutcomePanel;
    public GameObject returnToMainMenuDialogueBoxPreFab;
    GameObject returnToMainMenuDialogueBox;

    //variable for NEXT button
    Button nextButton;

    //Array of arrays to store the pseudocodes to be displayed
    string[][] arrayOfArrays = new string[30][];

    int numOfRows;
    int noOfBlanks = 0;
    int namingIndexForAnsButton = 1;

    //sprite images
    public Sprite thumbsUp;
    public Sprite thumbsDown;

    void Start()
    {
        //retrieve the game object called GameManager
        gameManagerForCSS = GameObject.Find("GameManager");
        //retrieve the script called GameManagerConceptSelectionScreen.cs that is attached under GameManager
        gcss = gameManagerForCSS.GetComponent<GameManagerConceptSelectionScreen>();
        //retrieve the NEXT button
        nextButton = GameObject.Find("Bottom panel with slider").GetComponentInChildren<Button>();
        UpdateSliderBar();
        DisableNextButton();
        DisplayContentInHeaders();
        SplitString();
        DisplayPseudocode();
        StartCoroutine(DisplayAnsOptions());
    }

    //update the slider as users attempt each question
    public void UpdateSliderBar()
    {
        Slider s = GameObject.Find("Bottom panel with slider").GetComponentInChildren<Slider>();
        //15 questions
        s.maxValue = 15;
        //update the progress
        s.value = gcss.sliderBarValue;
    }

    //To let users know which concept they are currently at as well as what type of question they are attempting
    void DisplayContentInHeaders()
    {
        //display the concept name on the top most subheader
        GameObject conceptNamePanel = GameObject.Find("Concept Name Panel on top of screen for quizzes");
        Text conceptNamePanelText = conceptNamePanel.GetComponentInChildren<Text>();
        conceptNamePanelText.text = gcss.conceptName;

        //display the type of question being attempted by the user
        GameObject questionTypePanel = GameObject.Find("Question Type Panel");
        Text questionTypePanelText = questionTypePanel.GetComponentInChildren<Text>();
        questionTypePanelText.text = "Fill-in-the-blanks ";
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
                    blanksGameObjList.Add(pseudocodeBlock);
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
        answersGameObjList.Add(answerBlock);
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
            answersGameObjList.Add(subsequentBlock);
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
        for(int i = 0; i<blanksGameObjList.Count; i++)
        {
            if(blanksGameObjList[i].GetComponentInChildren<Text>().text == "___")
            {
                //keep track of which answer button was tapped
                temp = ansButton.GetComponentInChildren<AnswerButtonIndex>().buttonIndex;
                //store this index
                blanksGameObjList[i].GetComponentInChildren<StoreAnsButtonIndex>().indexStored = temp;
                blanksGameObjList[i].GetComponentInChildren<Text>().text = ansButton.GetComponentInChildren<Text>().text;
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
            answersGameObjList[index].GetComponentInChildren<Text>().text = blankPanel.GetComponentInChildren<Text>().text;
            //enable the onClick listern of the button
            answersGameObjList[index].GetComponent<Button>().interactable = true;
            //AnswersGameObjList[index].GetComponent<>().enabled = true;
        }
        //resets the blank panel to empty again
        blankPanel.GetComponentInChildren<Text>().text = "___";
    }

    //safeguard user from skipping to next question without answering the question
    void DisableNextButton()
    {
        nextButton.interactable = false;
    }

    //check answers when the RUN CODE button is tapped
    public void CheckAnswer()
    {
        Boolean correct = true;
        //Disable the run button to avoid user from attempting the question again
        Button runButton = GameObject.Find("Run Button").GetComponentInChildren<Button>();
        runButton.interactable = false;
        //enable the NEXT button to allow to user to proceed to next question
        nextButton.interactable = true;
        //set color to green when button is enabled
        Text nextButtonColor = GameObject.Find("Bottom panel with slider").GetComponentInChildren<Text>();
        nextButtonColor.color = new Color32(0, 188, 212, 255);
        //spawn the result outcome panel to display results
        GameObject canvas = GameObject.Find("Canvas");
        //dialogue box to appear to notify that user answers the question
        resultOutcomePanel = Instantiate(resultOutcomePanelPrefab) as GameObject;
        resultOutcomePanel.transform.SetParent(canvas.transform, false);
        resultOutcomePanel.transform.localScale.Set(1, 1, 1);
        Image iconImage = GameObject.Find("Container").GetComponentInChildren<Image>();
        Text resultOutComePanelText = resultOutcomePanel.GetComponentInChildren<Text>();
        for (int i =0;i<blanksGameObjList.Count;i++)
        {
            //as long as this condition is not triggered, the question is deemed to be answered correctly
            if(!(blanksGameObjList[i].GetComponentInChildren<Text>().text.Equals(gcss.qnsList[gcss.randomNum].correctAnswer[i])))
            {
                //set font size to 16 and color to red when answer is wrong
                resultOutComePanelText.text = "<size=16><color=#D50000FF>Wrong!</color></size>" + "\n";
                //display thumbs down icon
                iconImage.sprite = thumbsDown;
                correct = false;
                break;
            }
        }
        
        if(correct)
        {
            //add score
            gcss.score += 1;
            //set font to 16 and color to green when answer is correct
            resultOutComePanelText.text = "<size=16><color=#009688>Correct!</color></size>" + "\n";
            //display thumbs up icon
            iconImage.sprite = thumbsUp;
        }
        //change color to black to display the correct answer
        resultOutComePanelText.color = new Color32(33, 33, 33, 215);
        //Display the correct answer and explanation for the question
        string temp = "";
        for(int i = 0; i<gcss.qnsList[gcss.randomNum].correctAnswer.Length;i++)
        {
            temp += gcss.qnsList[gcss.randomNum].correctAnswer[i] + " ";
        }
        resultOutComePanelText.text += "The answer is " + temp + "\n" + gcss.qnsList[gcss.randomNum].qnsExplanation;
        gcss.sliderBarValue += 1;
    }

    //return to main menu
    public void ReturnToMainMenu()
    {
        GameObject canvas = GameObject.Find("Canvas");
        returnToMainMenuDialogueBox = Instantiate(returnToMainMenuDialogueBoxPreFab) as GameObject;
        returnToMainMenuDialogueBox.transform.SetParent(canvas.transform, false);
        returnToMainMenuDialogueBox.transform.localScale.Set(1, 1, 1);
    }
}
