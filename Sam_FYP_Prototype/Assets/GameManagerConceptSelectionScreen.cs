using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerConceptSelectionScreen : MonoBehaviour
{
    //list of Question objects
    public List<Question> qnsList = new List<Question>();

    //will be assigned a random number and be used to display questions such as true/false, MCQ or fill in the blanks
    public int randomNum;

    private void Start()
    {
        //to ensure that this game object persist through scene changes
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
    //read in all the content from the file stated in order to instantiate objects of Question type
    public void LoadConceptQuestions(string questionFileName)
    {
        //temporary store for the string that is read in line by line from the text file
        string line;
        //temporary store for question
        string question = "";
        //temporary store for correctanswer
        string[] correctAnswer = new string[10];
        //temporary store for wrongAnswer
        string[] wrongAnswer = new string[3];
        //number to identify the type of question
        // 1 = true/false question 
        // 2 = MCQ 
        // 3 = Fill in the blank questions
        int qnsType;
        //number of lines in the question for "for" loop purposes
        int numberOfQnsLines;
        //for fill in the blanks
        int numberOfCorrectAnswers;
        
        //temporary store for line read in by file
        string tempLine;

        //get relative path to file as opposed to abosolute so that the file can be read on any computer
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), questionFileName);

        try
        {
            int count = 0;
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
                else
                {
                    wrongAnswer = new string[3];
                }
                
                //creating and adding instance of Question objects to qnsList  
                qnsList.Add(new Question(question, correctAnswer, wrongAnswer, qnsType));
                //skip empty line between questions in the textfile
                sr.ReadLine();
                /*
                //print("size: " + qnsList.Count);
                //print("count: " +count);
                //print("question: "+ qnsList[count].question);
                //print("correctAnswer: "+ qnsList[count].correctAnswer[0]);
                //print(qnsList[count].correctAnswer[1]);
                //print(qnsList[count].correctAnswer[2]);
                //print("wrong answer: " + qnsList[count].wrongAns[0]);
                //print(qnsList[count].wrongAns[1]);
                //print(qnsList[count].wrongAns[2]);
                //print("first Question answer: " + qnsList[0].correctAnswer[0]);

                print(count + "question: " + qnsList[count].question);
                print(count + "wrong answer: " + qnsList[count].wrongAns[0]);
                print(count + "correct answer: " + qnsList[count].correctAnswer[0]);
                count++;
                */
                
            }
            /*
            for (count = 0; count < 5; count++)
            {
                print(count + "question: " + qnsList[count].question);
                print(count + "wrong answer: " + qnsList[count].wrongAns[0]);
                print(count + "correct answer: " + qnsList[count].correctAnswer[0]);
            
            }
            */
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
        print("hi");
        System.Random r = new System.Random();
        //randoms a number between 1 and the size of the list - 1
        randomNum = r.Next(0, qnsList.Count);
        print(randomNum);
        //if the object's question type = 1, display a true false question
        if (qnsList[randomNum].qnsType == 1)
        {
            print(qnsList[randomNum].correctAnswer[0]);
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

    public void callNextQuestion()
    {
        
        qnsList.RemoveAt(randomNum);
        if (qnsList.Count != 0)
        {
            RandomizeQuestion();
        }
    }

    //function to load to another scene
    public void LoadScene(string sceneName)
    {
        //load to the specific scene name that was passed into the parameter
        SceneManager.LoadScene(sceneName);
    }

    //for debugging
    private void OnDestroy()
    {
        Debug.Log("GameStatus was destroyed");
    }
}
