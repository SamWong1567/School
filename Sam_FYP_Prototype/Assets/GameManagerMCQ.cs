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

    //counter for transiting to next question
    int counter = 0;

    GameObject gameManagerForCSS;
    GameManagerConceptSelectionScreen gcss;

    //delay when changing question
    private float delayBetweenQuestions = 1f;

    // Use this for initialization
    void Start ()
    {
        //retrieve the game object called GameManager
        gameManagerForCSS = GameObject.Find("GameManager");
        //retrieve the script called GameManagerConceptSelectionScreen.cs that is attached under GameManager
        gcss = gameManagerForCSS.GetComponent<GameManagerConceptSelectionScreen>();
        DisplayQuestion();
        DisplayAnswers();
    }

    //display questions at the question panel
    void DisplayQuestion()
    {
        qnsText.text = gcss.qnsList[gcss.randomNum].question;
    }
    
    //display answers for the MCQ question randomly across the 4 option buttons
    void DisplayAnswers()
    {
        //Convert array of wrong anaswers to a list
        tempList = gcss.qnsList[gcss.randomNum].wrongAns.ToList();
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
        if(ans.Equals(gcss.qnsList[gcss.randomNum].correctAnswer[0]))
        {
            print(ans);
            print(gcss.qnsList[gcss.randomNum].correctAnswer[0]);
            //increase question counter by 1
            counter++;
            print("CORRECT");
            //StartCoroutine(TransitionToNextQuestion());
        }
        else
        {
            print(ans);
            print(gcss.qnsList[gcss.randomNum].correctAnswer[0]);
            counter++;
            print("WRONG");
            //StartCoroutine(TransitionToNextQuestion());
        }
        gcss.callNextQuestion();
    }
    
    //delay before displaying next question
    IEnumerator TransitionToNextQuestion()
    {
        yield return new WaitForSeconds(delayBetweenQuestions);
        //displayQuestion();
        //displayAnswers();
    }
    
}



