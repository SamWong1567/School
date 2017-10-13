using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour {

    //leads to another scene when a button is pressed
	public void changeScreen(string s)
    {
        Application.LoadLevel(s);

    }
}
