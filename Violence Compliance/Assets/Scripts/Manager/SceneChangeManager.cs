using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour {

    public static SceneChangeManager Instance;

    public int currentScene;

    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(this);
        }

        else {
            Instance = this;
        }
    }

    private void Start() {
        EventManager.Instance.onSceneChange += ChangeScene;
    }

    public void ChangeScene(int index) {
        currentScene = index;

        SceneManager.LoadScene(index);
    }

    private void OnDisable() {
        EventManager.Instance.onSceneChange -= ChangeScene;
    }
}