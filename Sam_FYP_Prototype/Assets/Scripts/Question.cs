using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question{

    public Question(string q, string a, string wa)
    {
        question = q;
        answer = a;
        wrongAns = wa;
    }

    public string question;
    public string answer;
    //for displaying mcq options
    public string wrongAns;
    
}
