using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManagerFillInTheBlanks : MonoBehaviour {

    GameObject gameManagerForCSS;
    GameObject parentPanel;
    GameObject panel;
    public GameObject panelPrefab;
    public GameObject pseudocodeBlockPrefab;
    GameManagerConceptSelectionScreen gcss;


    //to allow questions to be edited in the unity editor. 
    [SerializeField]
    private Text qnsText;

    string[] pseudocodes;

    //Array of arrays to store the pseudocodes to be displayed
    string[][] arrayOfArrays = new string[30][];

    int numOfRows;

    // Use this for initialization
    void Start ()
    {
        //retrieve the game object called GameManager
        gameManagerForCSS = GameObject.Find("GameManager");
        //retrieve the script called GameManagerConceptSelectionScreen.cs that is attached under GameManager
        gcss = gameManagerForCSS.GetComponent<GameManagerConceptSelectionScreen>();
        SplitString();
        InstantiatePrefab();
       // DisplayPseudocode();
    }

    //split the string that is read in from the file
    public void SplitString()
    {
        //split the pseudocodes based on the delimiter "new line"
        string[] templist = gcss.qnsList[gcss.randomNum].question.Split('\n');

        numOfRows = templist.Length;

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
            //storing the array across the whole row
            arrayOfArrays[i] = templist2;
        }
    }

    
    public void DisplayPseudocode()
    {


    }

    ////display pesudocode onto the panel
    public void InstantiatePrefab()
    {
        //parent panel
        parentPanel = GameObject.Find("Panel");
        //loop according to the total number of rows in the array of arrays
        print(numOfRows);
        for(int i = 0; i<numOfRows; i++)
        {
            //instantiate panels with horizontal layout within the parent panel
            //this represents 1 row respectively
            GameObject subsequentPanels = Instantiate(panelPrefab) as GameObject;
            //make this panel as a child
            subsequentPanels.transform.SetParent(parentPanel.transform,false);
            //loops according to the number of columns of this specific row
            print("length of row" +i + arrayOfArrays[i].Length);
            for(int j = 0; j<arrayOfArrays[i].Length; j++)
            {
                //print button for each element
                GameObject pseudocodeBlock = Instantiate(pseudocodeBlockPrefab) as GameObject;
                pseudocodeBlock.transform.SetParent(subsequentPanels.transform,false);

                Text codeText = pseudocodeBlock.GetComponentInChildren<Text>();

                if (arrayOfArrays[i][j] != "_")
                {
                    Image image = pseudocodeBlock.GetComponent<Image>();
                    Color c = image.color;
                    c.a = 0;
                    image.color = c;
                }
                codeText.text = arrayOfArrays[i][j] == "_"? "___": arrayOfArrays[i][j];
                
            }
        }
        
        
        //blanks.transform.SetParent(imageParent.transform);
        //scale the instantiated prefab to its original size. Became smaller after instantiating
        //blanks.transform.localScale = new Vector3(1, 1, 1);
        
        //get height of component
        //float width =  (panel.GetComponent<panel>().rect.width)/2;
        //float height = (panel.GetComponent<RectTransform>().rect.height)/2 ;
        //positioning it
        //blanks.transform.localPosition = Vector3.zero;
    }
	


}
