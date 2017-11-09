using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//for using Text
using System; //for try catch blocks
using System.IO; //for StreamReader

public class GameManagerMCQ : MonoBehaviour {

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

    //counter for changing questions to display
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
    }

    //display questions at the question panel
    void displayQuestion()
    {
        qnsText.text = mcqQnsList[counter].question;

    }

    //display answers randomly across the 4 options
    void displayAnswers()
    {

    }

    //method is called when Option 1 button is tapped
    public void trueButton()
    {
        String a = "True";
        //checkAnswer(a);
    }

}
