using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager Instance;

    // [0] Score Text, [1] Game Timer/Lifeforms Text, [2] Mission Report Text
    [SerializeField] private Text[] UIText;

    // [0] GameplayHUD, [1] InfoHUD, [2] DialogueHUD
    [SerializeField] private Transform[] HUD;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject fadeScreen;

    public int gameTimer;
    public int score;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }

        else {
            Instance = this;
        }
    }

    private void Start() {
        StartCoroutine(FadeGameIn());

        if(SceneChangeManager.Instance.currentScene == 0) {
            Cursor.visible = true;
        }

        else {
            StartCoroutine(UpdateGameTimer());
        }
    }

    public void UpdateScore() {
        score++;
        UIText[0].text = "Score: " + score;

        EventManager.Instance.Event_UpdateScore(score);
    }

    IEnumerator UpdateGameTimer() {
        while (gameTimer > 0) {
            yield return new WaitForSeconds(1);

            gameTimer--;
            UIText[1].text = "Time Remaining: " + gameTimer.ToString();
        }

        EventManager.Instance.onGameEnd_TimerEnded.Invoke();
    }

    IEnumerator FadeGameIn() {
        if(fadeScreen.GetComponent<Image>().color.a < 1f) {
            fadeScreen.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        }

        float a = fadeScreen.GetComponent<Image>().color.a;

        while (a > 0f) {
            a -= 0.005f;
            fadeScreen.GetComponent<Image>().color = new Color(0, 0, 0, a);

            yield return new WaitForSeconds(0.01f);
        }

        fadeScreen.GetComponent<Image>().raycastTarget = false;
    }

    IEnumerator FadeGameOut() {
        float a = fadeScreen.GetComponent<Image>().color.a;

        if(SceneChangeManager.Instance.currentScene == 1 && gameTimer == 0) {
            yield return new WaitForSeconds(10);
        }

        while (a < 1f) {
            a += 0.005f;
            fadeScreen.GetComponent<Image>().color = new Color(0, 0, 0, a);

            yield return new WaitForSeconds(0.01f);
        }

        SceneChangeManager.Instance.ChangeScene();
    }

    #region Game End Functions

    public void ChangeUIOnGameEnd() {
        int lifeformsDestroyed = Random.Range(score * 10, score * 100);

        UIText[0].GetComponent<RectTransform>().localPosition = new Vector2(0, 175);
        UIText[1].GetComponent<RectTransform>().localPosition = new Vector2(0, 100);
        UIText[1].text = "Lifeforms Destroyed: " + lifeformsDestroyed;
        UIText[2].gameObject.SetActive(true);
        HUD[1].gameObject.SetActive(true);
        HUD[0].gameObject.SetActive(false);

        StopEnemyShootingOnGameEnd();
        StartCoroutine(FadeGameOut());
    }

    public void StopEnemyShootingOnGameEnd() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject item in enemies) {
            item.GetComponent<Enemy>().StopEnemyShooting();
        }
    }

    #endregion

    public void CallSceneFadeOut() => StartCoroutine(FadeGameOut());

}