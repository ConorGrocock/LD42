using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOver : MonoBehaviour {

	
	// Update is called once per frame
	void Update() {
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("__Menu");
        }
	}
}
