using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Question{

    //question to be asked
    public string question;
    //corect answer/s to the question
    public string[] correctAnswer = new string[10];
    //for displaying mcq and fill in the blanks answer options
    public string[] wrongAns = new string[10];
    public int qnsType;
    //explanation for the question
    public string qnsExplanation;

    //constructor
    public Question(string q, string[] a, string[] wa, int type, string explanation)
    {
        question = q;
        //source, destination, size
        Array.Copy(a, correctAnswer,10);
        Array.Copy(wa,wrongAns,10);
        qnsType = type;
        qnsExplanation = explanation;
    }

}
