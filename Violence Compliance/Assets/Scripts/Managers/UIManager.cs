using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager Instance;

    [SerializeField] private Text scoreText;
    [SerializeField] private int score;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }

        else {
            Instance = this;
        }
    }

    public void UpdateScore() {
        score++;

        scoreText.text = "Score: " + score;
    }
}