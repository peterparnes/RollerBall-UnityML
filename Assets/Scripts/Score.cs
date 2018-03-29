using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public Text scoreText;

    int score = 0;
    int best = 0;

	// Use this for initialization
	void Start () {
        updateText();
	}

    public void incScore() {
        score++;
        if(score > best) {
            best = score;
        }
        updateText();

    }

    public void resetScore() {
        score = 0;
        updateText();
    }

    void updateText() {
        scoreText.text = "Score: " + score + "   Best: " + best; 
    }
}
