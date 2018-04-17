using System.Collections;
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

    //sprite images
    public Sprite thumbsUp;
    public Sprite thumbsDown;

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
        UpdateSliderBar();
        UpdateQnsNumber();
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

    //update the question number of the question that is being shown to the user
    public void UpdateQnsNumber()
    {
        Text qnsNum = GameObject.Find("Q Text").GetComponentInChildren<Text>();
        //Since the slider bar value starts at 0, we increment it by 1 as to not display Question 0
        int temp = gcss.sliderBarValue;
        temp += 1;
        qnsNum.text = "Q" + temp;
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
        questionTypePanelText.text = "True or False";
    }

    //display questions at the question panel
    void DisplayQuestion()
    {
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
        //set color to green when button is enabled
        Text nextButtonColor = GameObject.Find("Bottom panel with slider").GetComponentInChildren<Text>();
        nextButtonColor.color = new Color32(0, 188, 212, 255);
        //dialogue box to appear to notify that user answers the question
        resultOutcomePanel = Instantiate(resultOutcomePanelPrefab) as GameObject;
        resultOutcomePanel.transform.SetParent(canvas.transform, false);
        resultOutcomePanel.transform.localScale.Set(1, 1, 1);
        Image iconImage = GameObject.Find("Container").GetComponentInChildren<Image>();
        Text resultOutComePanelText = resultOutcomePanel.GetComponentInChildren<Text>();
        //if answer is correct
        if (ans.Equals(gcss.qnsList[gcss.randomNum].correctAnswer[0]))
        {
            //add to score
            gcss.score += 1;
            //set font to 16 and color to green when answer is correct
            resultOutComePanelText.text = "<size=16><color=#009688>Correct!</color></size>" + "\n";
            //display thumbs up icon
            iconImage.sprite = thumbsUp;
        }
        else
        {
            //set font size to 16 and color to red when answer is wrong
            resultOutComePanelText.text = "<size=16><color=#D50000FF>Wrong!</color></size>" + "\n";
            //display thumbs down icon
            iconImage.sprite = thumbsDown;
        }
        //change color to black to display the correct answer
        resultOutComePanelText.color = new Color32(33,33,33,215);
        //Display the correct answer and explanation for the question
        resultOutComePanelText.text += "The answer is " + gcss.qnsList[gcss.randomNum].correctAnswer[0] + "\n" + gcss.qnsList[gcss.randomNum].qnsExplanation;
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

