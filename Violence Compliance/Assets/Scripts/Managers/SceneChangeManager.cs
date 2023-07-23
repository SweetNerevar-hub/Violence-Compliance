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
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void ChangeScene() {
        if(SceneManager.GetActiveScene().buildIndex == 0) {
            SceneManager.LoadScene(1);
        }

        else {
            SceneManager.LoadScene(0);
        }
    }
}