using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour {

    public Question(string q, string a, string wa)
    {
        question = q;
        answer = a;
        wrongAns = wa;
    }

    public string question;
    public string answer;
    public string wrongAns;
   
}
