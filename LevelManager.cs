using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;

    public Fader fader;

	// Use this for initialization
	void Start () {
        if (instance == null)
            instance = this;
	}

    public void LoadLevel(string name)
    {
        if (fader != null)
            fader.FadeIn();
        StartCoroutine(loadNow(name));
        //SceneManager.LoadSceneAsync(name);
    }

    IEnumerator loadNow(string name)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(name);
    }
   
    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. 
        //Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Level Loaded" + scene.name + " " + mode.ToString());
        if (fader != null)
            fader.FadeOut();
        StartCoroutine(setupLevel(scene));
    }

    IEnumerator setupLevel(Scene scene)
    {
        while (GameManager.instance == null)
            yield return null;

        if (scene.name != "MainMenu")
            GameManager.instance.setupMatch();
        else
            GameManager.instance.setupMainMenu();
        StartCoroutine(positionPlayers());
    }

    IEnumerator positionPlayers()
    {
        //wait for game manager to initialize
        while(GameManager.instance == null)
            yield return null;
        GameManager.instance.PositionPlayers();
    }
}
