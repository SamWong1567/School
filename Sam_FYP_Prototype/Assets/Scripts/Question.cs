using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Question{

    public string question;
    public string[] correctAnswer = new string[10];
    //for displaying mcq and fill in the blanks options
    public string[] wrongAns = new string[10];
    public int qnsType;

    //constructor
    public Question(string q, string[] a, string[] wa, int type)
    {
        question = q;
        //source, destination, size
        Array.Copy(a, correctAnswer,10);
        Array.Copy(wa,wrongAns,10);
        qnsType = type;
    }

}
