using UnityEngine;

public class MainMenu : MonoBehaviour {

    [SerializeField] private GameObject title;
    [SerializeField] private GameObject mainMenuPage;
    [SerializeField] private GameObject controlsPage;
    [SerializeField] private GameObject controlsSprites;

    
    private void Update() {
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.R)) {
            ResetGame();
        }
    }

    // This is being used for the Showcase
    // So as to quickly reset the game for new players
    private void ResetGame() {
        OutroCutscene.eligibleForHiddenEnding = false;
        EventManager.Instance.Event_SceneChange(0);
    }

    public void ToggleControlsMenu(bool openingControls) {
        if (openingControls) {
            controlsPage.SetActive(true);
            controlsSprites.SetActive(true);
            title.SetActive(false);
            mainMenuPage.SetActive(false);
        }

        else {
            controlsPage.SetActive(false);
            controlsSprites.SetActive(false);
            title.SetActive(true);
            mainMenuPage.SetActive(true);
        }
    }

    public void ExitFromGame() {
        OutroCutscene.eligibleForHiddenEnding = false;
        Application.Quit();
    }
}
