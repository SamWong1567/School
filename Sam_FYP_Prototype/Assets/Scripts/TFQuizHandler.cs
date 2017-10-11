using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //for try catch blocks
using System.IO; //for StreamReader

//This class handles the true and false questions
public class TFQuizHandler : MonoBehaviour
{
    //list of Question objects
    public List<Question> qnsList;

    // Use this for initialization
    void Start ()
    {
        qnsList = new List<Question>();
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

        //get relative path to file as opposed to abosolute so that the file can be read on any computer
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "TFQuestions.txt");

        try
        {
            //Read text file and extract it's contents line by line
            StreamReader sr = new StreamReader(filePath);
           
            while((line = sr.ReadLine())!= null)
            {
                
                //split contents in file with the delimiter ','
                tempArray = line.Split(',');
                q = tempArray[0];
                a = tempArray[1];
                wa = tempArray[2];
            //creating and adding instance of Question objects to qnsList  
            qnsList.Add(new Question(q, a,wa));
                /* for printing 
                int counter = 0;
                print(qnsList[counter].question);
                print(qnsList[counter].answer);
                print(qnsList[counter].wrongAns);
                counter++;
                */
                
            }
            sr.Close();
        }
        catch(Exception e)
        {
            Debug.LogException(e, this);
        }

    }

}
