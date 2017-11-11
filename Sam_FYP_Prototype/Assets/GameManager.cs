using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//for using Text
using System; //for try catch blocks
using System.IO; //for StreamReader

public class GameManager : MonoBehaviour {

    //list of Question objects
    private static List<Question> qnsList = new List<Question>();
    //temporary store for the string that is read in line by line from the text file
    string line = "";
    //temporary stre for elements of a string split
    string[] tempArray;
    // temporary store for question
    string q;
    // temporary store for answer
    string a;
    // temporary store for wrong answer
    string wa;
    //identifies current question NOT USED
    private Question currentQns;
    //to allow questions to be edited in the unity editor. Dragged text under panel into game manager QnsText field
    [SerializeField]
    private Text qnsText;
    
    //counter for transiting to next question
    static int counter = 0;

    //delay when changing question
    private float delayBetweenQuestions = 1f;

    //Yet to use this var. Prob use to calculate score
    Boolean clearStage = false;

    //called everytime you load/reload a scene
    void Start()
    {
        //get relative path to file as opposed to abosolute so that the file can be read on any computer
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "TFQuestions.txt");

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
                //creating and adding instance of Question objects to qnsList  
                qnsList.Add(new Question(q, a, wa));

            }
            sr.Close();
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }

        displayQuestion();
        
    }
    //display questions at the question panel
    void displayQuestion()
    {
        qnsText.text = qnsList[counter].question;
        
    }
    //method is called when true button is tapped
    public void trueButton()
    {
        String a = "True";
        checkAnswer(a);
    }

    //method is called when false button is tapped
    public void falseButton()
    {
        String a = "False";
        checkAnswer(a);
    }

    //check if answer is correct or wrong
    public void checkAnswer(String ans)
    {
        if(ans.Equals(qnsList[counter].answer))
        {
            clearStage = true;
            //increase question counter by 1
            counter++;
            StartCoroutine(TransitionToNextQuestion());
        }
        else
        {
            counter++;
            StartCoroutine(TransitionToNextQuestion());
        }
    }

    //delay before displaying next question
    IEnumerator TransitionToNextQuestion ()
    {
        yield return new WaitForSeconds(delayBetweenQuestions);
        displayQuestion();
    }

}
