using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerConceptSelectionScreen : MonoBehaviour
{
    //list of Question objects
    public List<Question> qnsList = new List<Question>();

    //will be assigned a random number and be used to display questions such as true/false, MCQ or fill in the blanks
    public int randomNum;

    //variable for dialogue box
    public GameObject AcknowedgementBoxPrefab;
    GameObject acknowledgementBox;

    float delayTime = 2f;

    private void Start()
    {
        //to ensure that this game object persist through scene changes
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
    //read in all the content from the file stated in order to instantiate objects of Question type
    public void LoadConceptQuestions(string questionFileName)
    {
        //temporary store for question
        string question = "";
        //temporary store for correctanswer
        string[] correctAnswer = new string[10];
        //temporary store for wrongAnswer for MCQ and Fill in the blanks question
        string[] wrongAnswer = new string[10];
        //number to identify the type of question
        // 1 = true/false question 
        // 2 = MCQ 
        // 3 = Fill in the blank questions
        int qnsType;
        //number of lines in the question for "for" loop purposes
        int numberOfQnsLines;
        //number of wrong answers in the question for "for" loop purposes
        int numberOfWrongAns;
        //for fill in the blanks
        int numberOfCorrectAnswers;

        //temporary store for line read in by file
        string tempLine;

        //get relative path to file as opposed to abosolute so that the file can be read on any computer
        string filePath = Path.Combine(Directory.GetCurrentDirectory(),"Assets");
        filePath = Path.Combine(filePath, "Questions_Answers_Codes");
        filePath = Path.Combine(filePath, questionFileName);

        try
        {
            //Read text file and extract it's contents line by line
            StreamReader sr = new StreamReader(filePath);
            while (true)
            {
                //clears the value assigned to question
                question = "";
                //if end of line, terminate
                if((tempLine = sr.ReadLine()) == null)
                {
                    break;
                }
                qnsType = int.Parse(tempLine);
                numberOfQnsLines = int.Parse(sr.ReadLine());

                if (qnsType != 3)
                {
                    question = sr.ReadLine();
                }
                else
                {
                    for (int i = 0; i < numberOfQnsLines; i++)
                    {
                        //read each question line in the format that is displayed in the text
                        question += sr.ReadLine() + '\n';
                    }

                    question = question.Remove(question.Length - 1, 1);

                }

                //if question type is not fill in the blanks
                if(qnsType != 3)
                {
                    correctAnswer[0] = sr.ReadLine();
                }
                else
                {
                    numberOfCorrectAnswers = int.Parse(sr.ReadLine());
                    for (int i = 0; i < numberOfCorrectAnswers ; i++)
                    {
                        correctAnswer[i] = sr.ReadLine();
                    }

                }
                //question type is MCQ, then read in the wrong answers
                if(qnsType == 2)
                {
                    for(int i = 0; i<3; i++)
                    {
                        wrongAnswer[i] = sr.ReadLine();
                    }
                    
                }

                //if it is fill in the blanks question, read in the question's wrong options
                else if(qnsType == 3)
                {
                    numberOfWrongAns = int.Parse(sr.ReadLine());
                    //number of options to read in
                    for (int i = 0; i<numberOfWrongAns; i++)
                    {
                        wrongAnswer[i] = sr.ReadLine();
                    }
                }

                else
                {
                    wrongAnswer = new string[10];
                }

                //creating and adding instance of Question objects to qnsList  
                qnsList.Add(new Question(question, correctAnswer, wrongAnswer, qnsType));
                //skip empty line between questions in the textfile
                sr.ReadLine();
                wrongAnswer = new string[10];
                correctAnswer = new string[10];
            }
            sr.Close();
            RandomizeQuestion();
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }

    //randomizes a question to be asked
    public void RandomizeQuestion()
    {
        System.Random r = new System.Random();
        //randoms a number between 1 and the size of the list - 1
        randomNum = r.Next(0, qnsList.Count);
        //if the object's question type = 1, display a true false question
        if (qnsList[randomNum].qnsType == 1)
        {
            LoadScene("TrueFalseScene");
        }
        //if the object's question type = 2, display a multiple choice question
        else if (qnsList[randomNum].qnsType == 2)
        {
            LoadScene("McqScene");
        }
        //if the object's question type = 3, display a fill in the blanks question
        else if (qnsList[randomNum].qnsType == 3)
        {
            LoadScene("Fill in the blanks scene");
        } 
    }

    public void CallNextQuestion()
    {
        //remove the question that was asked
        qnsList.RemoveAt(randomNum);
        //transit to next question
        if (qnsList.Count != 0)
        {
            RandomizeQuestion();
        }
        //End of quiz. Return to the concept selection screen
        else
        {
            StartCoroutine(DelayDialoguePopUp());
        }
    }

    //function to load to another scene
    public void LoadScene(string sceneName)
    {
        //load to the specific scene name that was passed into the parameter
        SceneManager.LoadScene(sceneName);
    }

    //delay before displaying "end of quiz" message
    IEnumerator DelayDialoguePopUp()
    {
        yield return new WaitForSeconds(delayTime);
        GameObject canvas = GameObject.Find("Canvas");
        acknowledgementBox = Instantiate(AcknowedgementBoxPrefab) as GameObject;
        acknowledgementBox.transform.SetParent(canvas.transform, false);
        acknowledgementBox.transform.localScale.Set(1, 1, 1);
        //destroy the persisting gameManager
        Destroy(this.gameObject);
    }

        //for debugging
        private void OnDestroy()
    {
        Debug.Log("GameStatus was destroyed");
    }
}
