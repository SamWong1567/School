﻿using System;
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

    //store the name of concept that users are attempting
    public string conceptName;

    //keeps track of the player's score
    public int score = 0;

    //keep track of the concept intro file to load
    public int fileNum;

    //the very first attempt by user
    int NoOfBasicArithmeticAttempts = 0;
    int NoOfDatatypeAttempts = 0;
    int NoOfInputOutputAttempts = 0;
    int NoOfConditionalStatementsAttempts = 0;
    int NoOfLoopsAttempts = 0;
    int NoOfAssessmentAttempts = 0;

    //List to store the attempts of each concept
    public List<int> listOfConceptAttempts = new List<int>();

    private void Start()
    {
        //to ensure that this game object persist through scene changes
        GameObject.DontDestroyOnLoad(this.gameObject);

        listOfConceptAttempts.Add(NoOfBasicArithmeticAttempts);
        listOfConceptAttempts.Add(NoOfDatatypeAttempts);
        listOfConceptAttempts.Add(NoOfInputOutputAttempts);
        listOfConceptAttempts.Add(NoOfConditionalStatementsAttempts);
        listOfConceptAttempts.Add(NoOfLoopsAttempts);
        listOfConceptAttempts.Add(NoOfAssessmentAttempts);

        //save the number of attempts for each concepts in the player prefs for the very first time
        //For Basic Arithmetic
        if(!(PlayerPrefs.HasKey("Basic Arithmetic Attempts")))
        {
            PlayerPrefs.SetInt("Basic Arithmetic Attempts", listOfConceptAttempts[0]);
        }
        //For Datatype
        if(!(PlayerPrefs.HasKey("Datatype Attempts")))
        {
            PlayerPrefs.SetInt("Datatype Attempts", listOfConceptAttempts[1]);
        }
        //For Input & Output
        if (!(PlayerPrefs.HasKey("Input Output Attempts")))
        {
            PlayerPrefs.SetInt("Input Output Attempts", listOfConceptAttempts[2]);
        }
        //For Conditional Statements
        if (!(PlayerPrefs.HasKey("Conditional Statements Attempts")))
        {
            PlayerPrefs.SetInt("Conditional Statements Attempts", listOfConceptAttempts[3]);
        }
        //For Loops
        if (!(PlayerPrefs.HasKey("Loops Attempts")))
        {
            PlayerPrefs.SetInt("Loops Attempts", listOfConceptAttempts[4]);
        }
        //For Assessment
        if (!(PlayerPrefs.HasKey("Assessment Attempts")))
        {
            PlayerPrefs.SetInt("Assessment Attempts", listOfConceptAttempts[5]);
        }
    }
    //read in all the content from the file stated in order to instantiate objects of Question type
    public void LoadConceptQuestions(string questionFileName)
    {
        //DeleteAllSave();
        //get the name of the concept
        conceptName = questionFileName;
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

        //read the text files stored in the Resources folder
        TextAsset file = Resources.Load(questionFileName) as TextAsset;
        //= Path.Combine(Directory.GetCurrentDirectory(),"Assets");
        //filePath = Path.Combine(filePath, "Questions_Answers_Codes");
        //filePath = Path.Combine(filePath, questionFileName);

        //load all the contents of the file into a string array
        string[] contentsInFile = file.text.Split('\n');
        
        //remove all the unnecessary spaces from each line of string
        for(int i =0; i<contentsInFile.Length; i++)
        {
            contentsInFile[i] = contentsInFile[i].Replace("\n", String.Empty);
            contentsInFile[i] = contentsInFile[i].Replace("\r", String.Empty);
            contentsInFile[i] = contentsInFile[i].Replace("\t", String.Empty);
        }

        //counter for the array
        int index = 0;

        //print(contentsInFile[1].Length);
        try
        {
            while(true)
            {             
                //clears the value assigned to question
                question = "";
                //reached the end of file
                if(index >= contentsInFile.Length)
                {
                    break;
                }
                qnsType = int.Parse(contentsInFile[index]);
                index++;
                numberOfQnsLines = int.Parse(contentsInFile[index]);
                index++;

                if (qnsType != 3)
                {
                    question = contentsInFile[index];
                    index++;
                }
                else
                {
                    for (int i = 0; i < numberOfQnsLines; i++)
                    {
                        //read each question line in the format that is displayed in the text
                        question += contentsInFile[index] + '\n';
                        index++;
                    }

                    question = question.Remove(question.Length - 1, 1);
                    
                }

                //if question type is not fill in the blanks
                if (qnsType != 3)
                {
                    correctAnswer[0] = contentsInFile[index];
                    index++;
                }
                else
                {
                    numberOfCorrectAnswers = int.Parse(contentsInFile[index]);
                    index++;
                    for (int i = 0; i < numberOfCorrectAnswers ; i++)
                    {
                        correctAnswer[i] = contentsInFile[index];
                        index++;
                        
                    }

                }
                //question type is MCQ, then read in the wrong answers
                if(qnsType == 2)
                {
                    for(int i = 0; i<3; i++)
                    {
                        wrongAnswer[i] = contentsInFile[index];
                        index++;
                        
                    }
                    
                }

                //if it is fill in the blanks question, read in the question's wrong options
                else if(qnsType == 3)
                {
                    numberOfWrongAns = int.Parse(contentsInFile[index]);
                    index++;
                    //number of options to read in
                    for (int i = 0; i<numberOfWrongAns; i++)
                    {
                        wrongAnswer[i] = contentsInFile[index];
                        index++;
                    }
                }

                else
                {
                    wrongAnswer = new string[10];
                }

                //creating and adding instance of Question objects to qnsList  
                qnsList.Add(new Question(question, correctAnswer, wrongAnswer, qnsType));
                //skip empty line between questions in the textfile
                //string skipEmptyLine = contentsInFile[index];
                index++;
                wrongAnswer = new string[10];
                correctAnswer = new string[10];
            }
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
        RecordScore();
        //destroy the persisting gameManager
        Destroy(this.gameObject);
    }

    public void RecordScore()
    {    

        if (conceptName.Equals("Basic Arithmetic.txt"))
        {
            //save the final score obtained by the user for the concept: Basic Arimetic
            PlayerPrefs.SetInt("Basic Arithmetic Save" + PlayerPrefs.GetInt("Basic Arithmetic Attempts"), score);
            //increment the number of attempts for the concept: Basic Arithemtic for the upcoming attempt
            PlayerPrefs.SetInt("Basic Arithmetic Attempts", PlayerPrefs.GetInt("Basic Arithmetic Attempts") + 1);
        }
        else if (conceptName.Equals("Datatype.txt"))
        {
            //save the final score obtained by the user for the concept: Datatype
            PlayerPrefs.SetInt("Datatype Save" + PlayerPrefs.GetInt("Datatype Attempts"), score);
            //increment the number of attempts for the concept: Datatype for the upcoming attempt
            PlayerPrefs.SetInt("Datatype Attempts", PlayerPrefs.GetInt("Datatype Attempts") + 1);

        }
        else if (conceptName.Equals("Input Output.txt"))
        {
            //save the final score obtained by the user for the concept: Input Output
            PlayerPrefs.SetInt("Input Output Save" + PlayerPrefs.GetInt("Input Output Attempts"), score);
            //increment the number of attempts for the concept: Input Output for the upcoming attempt
            PlayerPrefs.SetInt("Input Output Attempts", PlayerPrefs.GetInt("Input Output Attempts") + 1);

        }
        else if (conceptName.Equals("Conditional Statements.txt"))
        {
            //save the final score obtained by the user for the concept: Conditional Statements
            PlayerPrefs.SetInt("Conditional Statements Save" + PlayerPrefs.GetInt("Conditional Statements Attempts"), score);
            //increment the number of attempts for the concept: Conditional Statements for the upcoming attempt
            PlayerPrefs.SetInt("Conditional Statements Attempts", PlayerPrefs.GetInt("Conditional Statements Attempts") + 1);
        }
        else if (conceptName.Equals("Loops.txt"))
        {
            //save the final score obtained by the user for the concept: Loops
            PlayerPrefs.SetInt("Loops Save" + PlayerPrefs.GetInt("Loops Attempts"), score);
            //increment the number of attempts for the concept: Loops for the upcoming attempt
            PlayerPrefs.SetInt("Loops Attempts", PlayerPrefs.GetInt("Loops Attempts") + 1);
        }
        else if (conceptName.Equals("Assessment.txt"))
        {
            //save the final score obtained by the user for the concept: Assessment
            PlayerPrefs.SetInt("Assessment Save" + PlayerPrefs.GetInt("Assessment Attempts"), score);
            //increment the number of attempts for the concept: Assessment for the upcoming attempt
            PlayerPrefs.SetInt("Assessment Attempts", PlayerPrefs.GetInt("Assessment Attempts") + 1);
        }
    }
    //load a concept introduction scene
    public void LoadConceptIntro(int i)
    {
        fileNum = i;
        LoadScene("Concept Intro Scene");
    }

        //for debugging
        private void OnDestroy()
    {
        Debug.Log("GameStatus was destroyed");
    }

    public void DeleteAllSave()
    {
        PlayerPrefs.DeleteAll();
    }
}
