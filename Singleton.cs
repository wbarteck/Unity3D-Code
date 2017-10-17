using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour {

    public static Singleton instance;

	// Use this for initialization
	void Start () {
         if (instance != null && instance != this) {
            /*
            foreach (Transform t in transform)
            {
                Destroy(t.gameObject);
            }
            */
            Destroy(gameObject);

        } else if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        
	}
}
