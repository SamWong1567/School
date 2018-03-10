﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//for using Text
using System; //for try catch blocks
using System.IO; //for StreamReader
using UnityEngine.SceneManagement;//for loading between scenes
//Script for True False Scene
public class GameManagerTF : MonoBehaviour {
    float time;

   //to allow questions to be edited in the unity editor. Dragged text under panel into game manager QnsText field
    [SerializeField]
    private Text qnsText;

    //delay when changing question
    private float delayBetweenQuestions = 1f;

    GameObject gameManagerForCSS;
    GameManagerConceptSelectionScreen gcss;

    //variable for dialogue box
    public GameObject resultOutcomePanelPrefab;
    GameObject resultOutcomePanel;
    public GameObject returnToMainMenuDialogueBoxPreFab;
    GameObject returnToMainMenuDialogueBox;

    //variable for NEXT button
    Button nextButton;

    //called everytime you load/reload a scene
    void Start()
    {
        //retrieve the game object called GameManager
        gameManagerForCSS = GameObject.Find("GameManager");
        //retrieve the script called GameManagerConceptSelectionScreen.cs that is attached under GameManager
        gcss = gameManagerForCSS.GetComponent<GameManagerConceptSelectionScreen>();
        //retrieve the NEXT button
        nextButton = GameObject.Find("Bottom panel with slider").GetComponentInChildren<Button>();
        //update the slider
        //UpdateSliderBar();
        DisableNextButton();
        DisplayContentInHeaders();
        DisplayQuestion();
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
        questionTypePanelText.text = "True/False Question";
    }

    //display questions at the question panel
    void DisplayQuestion()
    {
        print(gcss.randomNum);
        qnsText.text = gcss.qnsList[gcss.randomNum].question;
    }
    
    //method is called when true button is tapped
    public void TrueButton()
    {
        string a = "True";
        CheckAnswer(a);
    }

    //method is called when false button is tapped
    public void FalseButton()
    {
        string a = "False";
        CheckAnswer(a);
    }

    //return to main menu
    public void ReturnToMainMenu()
    {
        GameObject canvas = GameObject.Find("Canvas");
        returnToMainMenuDialogueBox = Instantiate(returnToMainMenuDialogueBoxPreFab) as GameObject;
        returnToMainMenuDialogueBox.transform.SetParent(canvas.transform, false);
        returnToMainMenuDialogueBox.transform.localScale.Set(1, 1, 1);
    }

    //check if answer chosen by user is correct or wrong
    public void CheckAnswer(string ans)
    {
        GameObject canvas = GameObject.Find("Canvas");
        DisableAnswerOptionsButtons();
        //enable the NEXT button to allow to user to proceed to next question
        nextButton.interactable = true;
        resultOutcomePanel = Instantiate(resultOutcomePanelPrefab) as GameObject;
        resultOutcomePanel.transform.SetParent(canvas.transform, false);
        resultOutcomePanel.transform.localScale.Set(1, 1, 1);
        //if answer is correct
        if (ans.Equals(gcss.qnsList[gcss.randomNum].correctAnswer[0]))
        {
            //add to score
            gcss.score += 1;
            //dialogue box to appear to notify that user answered correctly
            resultOutcomePanel.GetComponentInChildren<Text>().text = "Correct!";
        }
        else
        {   
            //dialogue box to appear to notify that user answered wrongly
            resultOutcomePanel.GetComponentInChildren<Text>().text = "Wrong!";
            print("wrong");
            
            //acknowledgementBox = Instantiate(tryAgainPrefab) as GameObject;
            //acknowledgementBox.transform.SetParent(canvas.transform,false);

            //time = Time.time;
            
      
            //no score will be added    
            //StartCoroutine(TransitionToNextQuestion());
        }
        gcss.sliderBarValue += 1;
    }

    //safeguard user from skipping to next question without answering the question
    void DisableNextButton()
    {
        nextButton.interactable = false;
    }

    void DisableAnswerOptionsButtons()
    {
        Button trueButton = GameObject.Find("Button - TRUE").GetComponentInChildren<Button>();
        trueButton.interactable = false;
        Button falseButton = GameObject.Find("Button - FALSE").GetComponentInChildren<Button>();
        falseButton.interactable = false;
    }

    //delay before displaying next question
    IEnumerator TransitionToNextQuestion ()
    {
        yield return new WaitForSeconds(delayBetweenQuestions);
        DisplayQuestion();
    }

}

