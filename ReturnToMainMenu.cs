using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMainMenu : MonoBehaviour {

    public float returnIn = 5f;

	// Use this for initialization
	void Start () {
        
	}
	
	public void Go() {
        GameManager.instance.mode = GameManager.GameMode.Title;
        GameManager.instance.level = GameManager.GameLevel.MainMenu;
        GameManager.instance.chosenMode = GameManager.GameMode.Title;
        GameManager.instance.chosenLevel = GameManager.GameLevel.MainMenu;
        GameManager.instance.startMatch();
    }
}
