using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManagerFillInTheBlanks : MonoBehaviour {

    GameObject gameManagerForCSS;
    GameObject imageParent;
    GameObject panel;
    public GameObject blanksToFillIn;
    GameManagerConceptSelectionScreen gcss;


    //to allow questions to be edited in the unity editor. 
    [SerializeField]
    private Text qnsText;

    string[] pseudocodes;

    //Array of arrays to store the pseudocodes to be displayed
    string[][] arrayOfArrays = new string[30][];

    // Use this for initialization
    void Start ()
    {
        //retrieve the game object called GameManager
        gameManagerForCSS = GameObject.Find("GameManager");
        //retrieve the script called GameManagerConceptSelectionScreen.cs that is attached under GameManager
        gcss = gameManagerForCSS.GetComponent<GameManagerConceptSelectionScreen>();
        DisplayQuestion();
    }

    //display questions at the question panel
    public void DisplayQuestion()
    {
        //split the pseudocodes based on the delimiter "new line"
        string[] templist = gcss.qnsList[gcss.randomNum].question.Split('\n');
        string[] templist2;
        //loop according to the number of pseudocode statements
        for (int i = 0; i < templist.Length; i++)
        {
            //preserve the underscore using regex
            //won't add any empty spaces generated from regex as Regex.split generates a empty space before and after the delimiter
            templist2 = Regex.Split(templist[i],"(_)").Where(s=>!string.IsNullOrEmpty(s)).ToArray();
            print(templist2.Length + " templist2 size");
            //declare number of columns per row
            arrayOfArrays[i] = new string[templist2.Length];
            //storing an array to the 
            arrayOfArrays[i] = templist2;
            for (int j = 0; j < templist2.Length; j++)
            {
                qnsText.text += arrayOfArrays[i][j] +  '\n';
                InstantiatePrefab();
            }
        }
        
    }

    public void InstantiatePrefab()
    {
        imageParent = GameObject.Find("Panel");
        GameObject blanks = Instantiate(blanksToFillIn) as GameObject;
        blanks.transform.SetParent(imageParent.transform);
        //scale the instantiated prefab to its original size. Became smaller after instantiating
        blanks.transform.localScale = new Vector3(1, 1, 1);
        //panel = GameObject.Find("Panel");
        //get height of component
        //float width =  (panel.GetComponent<panel>().rect.width)/2;
        //float height = (panel.GetComponent<RectTransform>().rect.height)/2 ;

        //positioning it
        blanks.transform.localPosition = Vector3.zero;
    }
	


}
