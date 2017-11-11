using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//for using Text
using System; //for try catch blocks
using System.IO; //for StreamReader
using System.Linq;

public class GameManagerMCQ : MonoBehaviour
{

    //list of Question objects
    private static List<Question> mcqQnsList = new List<Question>();
    //temporary store for the string that is read in line by line from the text file
    string line = "";
    //temporary stre for elements of a string split
    string[] tempArray;
    // temporary store for question
    string q;
    // temporary store for answer
    string a;
    // temporary store the wrong answer string
    string wa;
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
    static int counter = 0;

    //delay when changing question
    private float delayBetweenQuestions = 1f;

    // Use this for initialization
    void Start ()
    {

        //get relative path to file as opposed to abosolute so that the file can be read on any computer
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "McqQuestions.txt");

        try
        {
            //Read text file and extract it's contents line by line
            StreamReader sr = new StreamReader(filePath);

            while ((line = sr.ReadLine()) != null)
            {

                //split contents in file with the delimiter ','
                tempArray = line.Split(',');
                q = tempArray[0];
                a = tempArray[1];
                wa = tempArray[2];
                //creating and adding instance of Question objects to mcqQnsList  
                mcqQnsList.Add(new Question(q, a, wa));
            }
            sr.Close();
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }

        displayQuestion();
        displayAnswers();
    }

    //display questions at the question panel
    void displayQuestion()
    {
        qnsText.text = mcqQnsList[counter].question;

    }

    //display answers for the MCQ question randomly across the 4 option buttons
    void displayAnswers()
    {
        //split the set of wrong answers in every Question object
        //also converts array to list
        tempList = mcqQnsList[counter].wrongAns.Split('+').ToList();
        //add the correct answer for the mcq question into the list
        tempList.Add(mcqQnsList[counter].answer);

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
    public void optionButton1()
    {
        //checks the answer displayed at option 1 button
        checkAns(s1);
    }

    //method is called when option 2 button is tapped
    public void optionButton2()
    {
        //checks the answer displayed at option 2 button
        checkAns(s2);
    }

    //method is called when option 3 button is tapped
    public void optionButton3()
    {
        //checks the answer displayed at option 3 button
        checkAns(s3);
    }

    //method is called when option 4 button is tapped
    public void optionButton4()
    {
        //checks the answer displayed at option 4 button
        checkAns(s4);
    }

    //check if the answer selected by the user is correct
    public void checkAns(String ans)
    {
        if(ans.Equals(mcqQnsList[counter].answer))
        {
            print(ans);
            print(mcqQnsList[counter].answer);
            //increase question counter by 1
            counter++;
            print("CORRECT");
            StartCoroutine(TransitionToNextQuestion());
        }
        else
        {
            print(ans);
            print(mcqQnsList[counter].answer);
            counter++;
            print("WRONG");
            StartCoroutine(TransitionToNextQuestion());
        }
    }

    //delay before displaying next question
    IEnumerator TransitionToNextQuestion()
    {
        yield return new WaitForSeconds(delayBetweenQuestions);
        displayQuestion();
        displayAnswers();
    }
}


