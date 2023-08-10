using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGameFade : MonoBehaviour {

    public static ToggleGameFade Instance;

    private Image fadeScreenImage;

    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(this);
        }

        else {
            Instance = this;
        }
    }

    private void Start() {
        fadeScreenImage = GetComponent<Image>();

        StartCoroutine(FadeGameIn());
    }

    public void CallSceneFadeOut(int index) {
        StartCoroutine(FadeGameOut(index));
    }

    IEnumerator FadeGameIn() {
        if (fadeScreenImage.color.a < 1f) {
            fadeScreenImage.color = new Color(0, 0, 0, 1);
        }

        float a = fadeScreenImage.color.a;

        while (a > 0f) {
            a -= 0.005f;
            fadeScreenImage.color = new Color(0, 0, 0, a);

            yield return new WaitForSeconds(0.01f);
        }

        fadeScreenImage.raycastTarget = false;
    }

    IEnumerator FadeGameOut(int index) {
        float a = fadeScreenImage.color.a;

        if(index == 3 && UIManager.Instance.gameTimer == 0) {
            yield return new WaitForSeconds(10f);
        }

        while (a < 1f) {
            a += 0.005f;
            fadeScreenImage.color = new Color(0, 0, 0, a);

            yield return new WaitForSeconds(0.01f);
        }

        EventManager.Instance.Event_SceneChange(index);
    }
}
