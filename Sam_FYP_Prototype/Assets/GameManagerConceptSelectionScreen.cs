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

    //store the name of concept that users are attempting
    public string conceptName;

    //keeps track of the player's score
    public int score = 0;

    //keep track of the concept intro file to load
    public int fileNum;

    //keep track of the slider bar value
    public int sliderBarValue = 0;

    //stores the number of questions read in
    int qnsCount;
    private void Start()
    {
        //DeleteAllSave();
        //to ensure that this game object persist through scene changes
        GameObject.DontDestroyOnLoad(this.gameObject);
        //Show the scores of each concept when user loads into the mobile app
        DisplayScore();
    }
    //read in all the content from the file and represent each question as an object of Question type
    public void LoadConceptQuestions(string questionFileName)
    {
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
        //explanation text for the quiz questions
        string quizExplanation;

        //read the text files stored in the Resources folder
        TextAsset file = Resources.Load(questionFileName) as TextAsset;

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

                //loop for the number of lines in the question
                for (int i = 0; i < numberOfQnsLines; i++)
                {
                    //read each question line in the format that is displayed in the text
                    question += contentsInFile[index] + '\n';
                    index++;
                }
                //remove \n at the end of the last statement
                question = question.Remove(question.Length - 1, 1);

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

                quizExplanation = contentsInFile[index];
                index++;

                //creating and adding instance of Question objects to qnsList  
                qnsList.Add(new Question(question, correctAnswer, wrongAnswer, qnsType, quizExplanation));
                //skip empty line between questions in the textfile
                index++;
                wrongAnswer = new string[10];
                correctAnswer = new string[10];
            }
            //save the number questions read in to calculate the average score as the questions will be removed from the qnsList when attempted
            qnsCount = qnsList.Count;
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
        print(qnsList.Count);
        //transit to next question if less than 8 questions asked
        if (qnsList.Count != 7)
        {
            RandomizeQuestion();
        }
        //End of quiz. Return to the concept selection screen
        else
        {
            RecordScore();
            LoadScene("Concept_Selection_Scene");
            //destroy the persisting gameManager
            Destroy(this.gameObject);
        }
    }

    //function to load to another scene
    public void LoadScene(string sceneName)
    {
        //load to the specific scene name that was passed into the parameter
        SceneManager.LoadScene(sceneName);
    }

    //called after user finishes a quiz from a particular concept
    public void RecordScore()
    {
        float tempAvgScore = 0.0f;
        if (conceptName.Equals("Introduction & Basic Arithmetic"))
        {
            //increment the number of attempts for the concept: Introduction & Basic Arithemtic upon completing the quiz
            PlayerPrefs.SetInt("Basic Arithmetic Attempts", (PlayerPrefs.GetInt("Basic Arithmetic Attempts") + 1));
            //save the final score obtained by the user which will be used to display the latest score for the concept: Introduction & Basic Arithmetic 
            PlayerPrefs.SetInt("Basic Arithmetic Save", score);
            //compute the total scores that the user ever obtained for Introduction & Basic Arithmetic to calculate the average score
            PlayerPrefs.SetInt("Total Score For Basic Arithmetic", (PlayerPrefs.GetInt("Total Score For Basic Arithmetic") + score));
            //calculate the average score for the concept Introduction & Basic Arithmetic
            tempAvgScore = CalculateAverageScore(PlayerPrefs.GetInt("Total Score For Basic Arithmetic"), PlayerPrefs.GetInt("Basic Arithmetic Attempts"));
            //convert the average score to the following string format: 00.00%
            //save the average score for the concept Introduction & Basic Arithmetic
            PlayerPrefs.SetString("Average Score For Basic Arithmetic", (tempAvgScore*100).ToString("0.00"));
            
        }
        else if (conceptName.Equals("Variables & Datatypes"))
        {
            //increment the number of attempts for the concept: Datatype upon completing the quiz
            PlayerPrefs.SetInt("Datatype Attempts", (PlayerPrefs.GetInt("Datatype Attempts") + 1));
            //save the final score obtained by the user which will be used to display the latest score for the concept: Datatype
            PlayerPrefs.SetInt("Datatype Save", score);
            //compute the total scores that the user ever obtained for Datatype to calculate the average score
            PlayerPrefs.SetInt("Total Score For Datatype", (PlayerPrefs.GetInt("Total Score For Datatype") + score));
            //calculate the average score for the concept Datatype
            tempAvgScore = CalculateAverageScore(PlayerPrefs.GetInt("Total Score For Datatype"), PlayerPrefs.GetInt("Datatype Attempts"));
            //convert the average score to the following string format: 00.00%
            //save the average score for the concept Datatype
            PlayerPrefs.SetString("Average Score For Datatype", (tempAvgScore * 100).ToString("0.00"));
        }
        else if (conceptName.Equals("Input Output"))
        {
            //increment the number of attempts for the concept: Input Output upon completing the quiz
            PlayerPrefs.SetInt("Input Output Attempts", (PlayerPrefs.GetInt("Input Output Attempts") + 1));
            //save the final score obtained by the user which will be used to display the latest score for the concept: Input Output
            PlayerPrefs.SetInt("Input Output Save", score);
            //compute the total scores that the user ever obtained for Input Output to calculate the average score
            PlayerPrefs.SetInt("Total Score For Input Output", (PlayerPrefs.GetInt("Total Score For Input Output") + score));
            //calculate the average score for the concept Input Output
            tempAvgScore = CalculateAverageScore(PlayerPrefs.GetInt("Total Score For Input Output"), PlayerPrefs.GetInt("Input Output Attempts"));
            //convert the average score to the following string format: 00.00%
            //save the average score for the concept Input Output
            PlayerPrefs.SetString("Average Score For Input Output", (tempAvgScore * 100).ToString("0.00"));
        }
        else if (conceptName.Equals("Conditional Statements"))
        {
            //increment the number of attempts for the concept: Conditional Statements upon completing the quiz
            PlayerPrefs.SetInt("Conditional Statements Attempts", (PlayerPrefs.GetInt("Conditional Statements Attempts") + 1));
            //save the final score obtained by the user which will be used to display the latest score for the concept: Conditional Statements
            PlayerPrefs.SetInt("Conditional Statements Save", score);
            //compute the total scores that the user ever obtained for Conditional Statements to calculate the average score
            PlayerPrefs.SetInt("Total Score For Conditional Statements", (PlayerPrefs.GetInt("Total Score For Conditional Statements") + score));
            //calculate the average score for the concept Conditional Statements
            tempAvgScore = CalculateAverageScore(PlayerPrefs.GetInt("Total Score For Conditional Statements"), PlayerPrefs.GetInt("Conditional Statements Attempts"));
            //convert the average score to the following string format: 00.00%
            //save the average score for the concept Conditional Statements
            PlayerPrefs.SetString("Average Score For Conditional Statements", (tempAvgScore * 100).ToString("0.00"));
        }
        else if (conceptName.Equals("Loops"))
        {
            //increment the number of attempts for the concept: Loops upon completing the quiz
            PlayerPrefs.SetInt("Loops Attempts", (PlayerPrefs.GetInt("Loops Attempts") + 1));
            //save the final score obtained by the user which will be used to display the latest score for the concept: Loops
            PlayerPrefs.SetInt("Loops Save", score);
            //compute the total scores that the user ever obtained for Loops to calculate the average score
            PlayerPrefs.SetInt("Total Score For Loops", (PlayerPrefs.GetInt("Total Score For Loops") + score));
            //calculate the average score for the concept Loops
            tempAvgScore = CalculateAverageScore(PlayerPrefs.GetInt("Total Score For Loops"), PlayerPrefs.GetInt("Loops Attempts"));
            //convert the average score to the following string format: 00.00%
            //save the average score for the concept Loops
            PlayerPrefs.SetString("Average Score For Loops", (tempAvgScore * 100).ToString("0.00"));
        }
    }

    void DisplayScore()
    {
        //Scores for Basic Arithmetic 
        Text basicArithmeticNoOfAttemptsText = GameObject.Find("Basic Arithmetic Expressions - Attempts Number").GetComponent<Text>();
        //Display the number of attempts for Basic Arithmetic
        basicArithmeticNoOfAttemptsText.text = PlayerPrefs.GetInt("Basic Arithmetic Attempts").ToString();
        //Display the latest score for Basic Arithmetic
        Text basicArithmeticLatestScoreText = GameObject.Find("Basic Arithmetic Expressions - Latest Score").GetComponent<Text>();
        basicArithmeticLatestScoreText.text = PlayerPrefs.GetInt("Basic Arithmetic Save").ToString();
        //Display the average score for Basic Arithmetic
        if (PlayerPrefs.HasKey("Average Score For Basic Arithmetic"))
        {
            Text basicArithmeticAverageScoreText = GameObject.Find("Basic Arithmetic Expressions - Average Score").GetComponent<Text>();
            basicArithmeticAverageScoreText.text = PlayerPrefs.GetString("Average Score For Basic Arithmetic") + "%";
        }
        //Scores for DataType
        Text dataTypeNoOfAttemptsText = GameObject.Find("Variable and Datatype - Attempts Number").GetComponent<Text>();
        //Display the number of attempts for DataType
        dataTypeNoOfAttemptsText.text = PlayerPrefs.GetInt("Datatype Attempts").ToString();
        //Display the latest score for DataType
        Text dataTypeLatestScoreText = GameObject.Find("Variable and Datatype - Latest Score").GetComponent<Text>();
        dataTypeLatestScoreText.text = PlayerPrefs.GetInt("Datatype Save").ToString();
        //Display the average score for DataType
        if (PlayerPrefs.HasKey("Average Score For Datatype"))
        {
            Text datatypeAverageScoreText = GameObject.Find("Variable and Datatype - Average Score").GetComponent<Text>();
            datatypeAverageScoreText.text = PlayerPrefs.GetString("Average Score For Datatype") + "%";
        }
        
        //Scores for Input Output
        Text inputOutputNoOfAttemptsText = GameObject.Find("Input Output - Attempts Number").GetComponent<Text>();
        //Display the number of attempts for Input Output
        inputOutputNoOfAttemptsText.text = PlayerPrefs.GetInt("Input Output Attempts").ToString();
        //Display the latest score for Input Output
        Text inputOutputLatestScoreText = GameObject.Find("Input Output - Latest Score").GetComponent<Text>();
        inputOutputLatestScoreText.text = PlayerPrefs.GetInt("Input Output Save").ToString();
        //Display the average score for Input Output
        if (PlayerPrefs.HasKey("Average Score For Input Output"))
        {
            Text inputOutputAverageScoreText = GameObject.Find("Input Output - Average Score").GetComponent<Text>();
            inputOutputAverageScoreText.text = PlayerPrefs.GetString("Average Score For Input Output") + "%";
        }
        
        //Scores for Conditional Statements
        Text conditionalStatementsNoOfAttemptsText = GameObject.Find("Conditional Statements - Attempts Number").GetComponent<Text>();
        //Display the number of attempts for Conditional Statements
        conditionalStatementsNoOfAttemptsText.text = PlayerPrefs.GetInt("Conditional Statements Attempts").ToString();
        //Display the latest score for Conditional Statements
        Text conditionalStatementsLatestScoreText = GameObject.Find("Conditional Statements - Latest Score").GetComponent<Text>();
        conditionalStatementsLatestScoreText.text = PlayerPrefs.GetInt("Conditional Statements Save").ToString();
        //Display the average score for Conditional Statements
        if (PlayerPrefs.HasKey("Average Score For Conditional Statements"))
        {
            Text conditionalStatementsAverageScoreText = GameObject.Find("Conditional Statements - Average Score").GetComponent<Text>();
            conditionalStatementsAverageScoreText.text = PlayerPrefs.GetString("Average Score For Conditional Statements") + "%";
        }

        //Scores for Loops 
        Text loopsNoOfAttemptsText = GameObject.Find("Loops - Attempts Number").GetComponent<Text>();
        //Display the number of attempts for Loops
        loopsNoOfAttemptsText.text = PlayerPrefs.GetInt("Loops Attempts").ToString();
        //Display the latest score for Loops
        Text loopsLatestScoreText = GameObject.Find("Loops - Latest Score").GetComponent<Text>();
        loopsLatestScoreText.text = PlayerPrefs.GetInt("Loops Save").ToString();
        //Display the average score for Loops
        if (PlayerPrefs.HasKey("Average Score For Loops"))
        {
            Text loopsAverageScoreText = GameObject.Find("Loops - Average Score").GetComponent<Text>();
            loopsAverageScoreText.text = PlayerPrefs.GetString("Average Score For Loops") + "%";
        }
    }

    //method to calculate the average score obtained by the user
    float CalculateAverageScore(int totalScore, int totalNoOfAttempts)
    {
        return (float)totalScore / ((float)totalNoOfAttempts * 8);
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
        //print this message when the persisting GameManager is destroyed
        Debug.Log("GameManager was destroyed");
    }

    //To delete all the keys that are used to save the scores.
    public void DeleteAllSave()
    {
        PlayerPrefs.DeleteAll();
    }
}
