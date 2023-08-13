using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ToggleGameFade : MonoBehaviour {

    public static ToggleGameFade Instance { get; private set; }

    private Image fadeScreenImage;

    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
        }

        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {
        fadeScreenImage = GetComponent<Image>();

        StartCoroutine(FadeGameIn());
    }

    // This methods is called from other scripts to call a fade out before a scene change
    public void CallSceneFadeOut(int index) {
        StartCoroutine(FadeGameOut(index));
    }

    private IEnumerator FadeGameIn() {
        if (fadeScreenImage.color.a < 1f) {
            fadeScreenImage.color = new Color(0, 0, 0, 1);
        }

        float a = fadeScreenImage.color.a;

        while (a > 0f) {
            a -= 0.005f;
            fadeScreenImage.color = new Color(0, 0, 0, a);

            yield return new WaitForSeconds(0.01f);
        }

        fadeScreenImage.raycastTarget = false; // Making the image a raycast target until it's fully transparent is to stop players from clicking button to early,
                                               // which caused some graphical glitches as the screen would fade out as it was still fading in
    }

    private IEnumerator FadeGameOut(int index) {
        float a = fadeScreenImage.color.a;

        if(index == 3 && UIManager.Instance.gameTimer == 0) {
            yield return new WaitForSeconds(15f);
        }

        while (a < 1f) {
            a += 0.005f;
            fadeScreenImage.color = new Color(0, 0, 0, a);

            yield return new WaitForSeconds(0.01f);
        }

        EventManager.Instance.Event_SceneChange(index);
    }
}
