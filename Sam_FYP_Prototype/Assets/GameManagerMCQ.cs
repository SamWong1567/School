using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//for using Text
using System; //for try catch blocks
using System.IO; //for StreamReader
using System.Linq;

public class GameManagerMCQ : MonoBehaviour
{
    //list to temporarily store the wrong answers
    List<string> tempList = new List<string>();
    //var to hold random number
    int randomIndex;

    //to allow questions to be edited in the unity editor. 
    [SerializeField]
    private Text qnsText;

    //to allow answers to be edited in the unity editor for option 1 button. 
    [SerializeField]
    private Text optionField1;

    //to allow answers to be edited in the unity editor for option 2 button. 
    [SerializeField]
    private Text optionField2;

    //to allow answers to be edited in the unity editor for option 3 button. 
    [SerializeField]
    private Text optionField3;

    //to allow answers to be edited in the unity editor for option 4 button. 
    [SerializeField]
    private Text optionField4;

    //fixed string to be displayed at option 1 button
    string option1Text = "A: ";
    //fixed string to be displayed at option 2 button
    string option2Text = "B: ";
    //fixed string to be displayed at option 3 button
    string option3Text = "C: ";
    //fixed string to be displayed at option 4 button
    string option4Text = "D: ";

    //variable to temporarily store the answer for checking purpose
    string s1;
    //variable to temporarily store the answer for checking purpose
    string s2;
    //variable to temporarily store the answer for checking purpose
    string s3;
    //variable to temporarily store the answer for checking purpose
    string s4;

    GameObject gameManagerForCSS;
    GameManagerConceptSelectionScreen gcss;

    //variable for dialogue box
    public GameObject AcknowedgementBoxPrefab;
    GameObject acknowledgementBox;

    //delay when changing question
    private float delayBetweenQuestions = 1f;

    // Use this for initialization
    void Start ()
    {
        //retrieve the game object called GameManager
        gameManagerForCSS = GameObject.Find("GameManager");
        //retrieve the script called GameManagerConceptSelectionScreen.cs that is attached under GameManager
        gcss = gameManagerForCSS.GetComponent<GameManagerConceptSelectionScreen>();
        UpdateSliderBar();
        DisplayContentInHeaders();
        DisplayQuestion();
        DisplayAnswers();
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
        GameObject conceptNamePanel = GameObject.Find("Concept Name Panel on top of screen");
        Text conceptNamePanelText = conceptNamePanel.GetComponentInChildren<Text>();
        conceptNamePanelText.text = gcss.conceptName;

        //display the type of question being attempted by the user
        GameObject questionTypePanel = GameObject.Find("Question Type Panel");
        Text questionTypePanelText = questionTypePanel.GetComponentInChildren<Text>();
        questionTypePanelText.text = "Multiple Choice Question";
    }

    //display questions at the question panel
    void DisplayQuestion()
    {
        print("list count" + gcss.qnsList.Count);
        qnsText.text = gcss.qnsList[gcss.randomNum].question;
    }
    
    //display answers for the MCQ question randomly across the 4 option buttons
    void DisplayAnswers()
    {
        //Convert array of wrong anaswers to a list but only from elements 0 to 2 as there are only 3 wrong answers
        tempList = gcss.qnsList[gcss.randomNum].wrongAns.ToList().GetRange(0,3);
        //add the correct answer for the mcq question into the list
        tempList.Add(gcss.qnsList[gcss.randomNum].correctAnswer[0]);

        //Randomizes how the answers will be placed at each option button

        //Option 1 button 
        //randoms a number between 0 and the size of the list of wrong answers
        randomIndex = UnityEngine.Random.Range(0, tempList.Count);
        //store answer to variable before concatenating the fixed message to be displayed
        s1 = tempList[randomIndex];
        //assign first random answer to be displayed at option 1 button
        //as well as concatenating the fixed message to be displayed
        optionField1.text = option1Text + tempList[randomIndex];
        //remove this answer from templist
        tempList.RemoveAt(randomIndex);

        //Option 2 button
        //randoms a number between 0 and the size of the list of wrong answers
        randomIndex = UnityEngine.Random.Range(0, tempList.Count);
        //store answer to variable before concatenating the fixed message to be displayed
        s2 = tempList[randomIndex];
        //assign first random answer to be displayed at option 2 button
        //as well as concatenating the fixed message to be displayed
        optionField2.text = option2Text + tempList[randomIndex];
        //remove this answer from templist
        tempList.RemoveAt(randomIndex);

        //Option 3 button
        //randoms a number between 0 and the size of the list of wrong answers
        randomIndex = UnityEngine.Random.Range(0, tempList.Count);
        //store answer to variable before concatenating the fixed message to be displayed
        s3 = tempList[randomIndex];
        //assign first random answer to be displayed at option 3 button
        //as well as concatenating the fixed message to be displayed
        optionField3.text = option3Text + tempList[randomIndex];
        //remove this answer from templist
        tempList.RemoveAt(randomIndex);

        //Option 4 button
        //since only 1 element is left. There is no need for randomizing
        //store answer to variable before concatenating the fixed message to be displayed
        s4 = tempList[0];
        //concatenating the fixed message to be displayed
        optionField4.text = option4Text + tempList[0];

    }
    
    //method is called when option 1 button is tapped
    public void OptionButton1()
    {
        //checks the answer displayed at option 1 button
        CheckAns(s1);
    }

    //method is called when option 2 button is tapped
    public void OptionButton2()
    {
        //checks the answer displayed at option 2 button
        CheckAns(s2);
    }

    //method is called when option 3 button is tapped
    public void OptionButton3()
    {
        //checks the answer displayed at option 3 button
        CheckAns(s3);
    }

    //method is called when option 4 button is tapped
    public void OptionButton4()
    {
        //checks the answer displayed at option 4 button
        CheckAns(s4);
    }
    
    //check if the answer selected by the user is correct
    public void CheckAns(string ans)
    {
        GameObject canvas = GameObject.Find("Canvas");
        acknowledgementBox = Instantiate(AcknowedgementBoxPrefab) as GameObject;
        if (ans.Equals(gcss.qnsList[gcss.randomNum].correctAnswer[0]))
        {   //add score
            gcss.score += 1;
            //dialogue box to appear to notify that user answered correctly
            acknowledgementBox.GetComponentInChildren<Text>().text = "Good Job!" + " Current Score: " + gcss.score;
            acknowledgementBox.transform.SetParent(canvas.transform, false);
            acknowledgementBox.transform.localScale.Set(1, 1, 1);

            print("CORRECT");
            //StartCoroutine(TransitionToNextQuestion());
        }
        else
        {
            //dialogue box to appear to notify that user answered wrongly
            acknowledgementBox.GetComponentInChildren<Text>().text = "Better luck next time!" + " Current Score: " + gcss.score;
            acknowledgementBox.transform.SetParent(canvas.transform, false);
            acknowledgementBox.transform.localScale.Set(1, 1, 1);

            print("WRONG");
            //StartCoroutine(TransitionToNextQuestion());
        }
        gcss.sliderBarValue += 1;
    }
    
    //delay before displaying next question
    IEnumerator TransitionToNextQuestion()
    {
        yield return new WaitForSeconds(delayBetweenQuestions);
        //displayQuestion();
        //displayAnswers();
    }
    
}



