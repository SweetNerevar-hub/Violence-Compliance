using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager Instance;

    [Header("Gameplay Variables")]
    [SerializeField] Transform gameplayHUD;
    [SerializeField] Transform infoHUD;
    [SerializeField] Transform player;
    [SerializeField] GameObject fadeScreen;

    // [0] Score Text, [1] Game Timer/Lifeforms Text, [2] Mission Report Text
    [SerializeField] Text[] UIText;
    
    [SerializeField] private int gameTimer;
    private bool isShowingInfo;

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

        Invoke("StartGameTimer", 0.01f);
    }

    private void Update() {
        if (gameplayHUD == null) {
            return;
        }

        else gameplayHUD.position = player.transform.position;
    }

    public void UpdateScore() {
        score++;
        UIText[0].text = "Score: " + score;
    }

    /*public void ShowGameInfo() {
        if (!isShowingInfo) {
            UIParent.SetActive(true);
            isShowingInfo = true;
        }

        else {
            UIParent.SetActive(false);
            isShowingInfo = false;
        }
    }*/

    void StartGameTimer() {
        if(SceneChangeManager.Instance.currentScene == 1) {
            StartCoroutine(UpdateGameTimer());
        }
    }

    IEnumerator UpdateGameTimer() {
        while (gameTimer > 0) {
            yield return new WaitForSeconds(1);

            gameTimer--;
            UIText[1].text = "Time Remaining: " + gameTimer.ToString();
        }

        EventManager.Instance.onGameEnd.Invoke();
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

        if(SceneChangeManager.Instance.currentScene == 1) {
            yield return new WaitForSeconds(5);
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
        UIText[2].gameObject.SetActive(true);
        infoHUD.gameObject.SetActive(true);
        gameplayHUD.gameObject.SetActive(false);

        UIText[1].text = "Lifeforms Destroyed: " + lifeformsDestroyed;
        StartCoroutine(FadeGameOut());
    }

    public void OnGameEnd() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject item in enemies) {
            item.GetComponent<Enemy>().StopEnemyShooting();
        }
    }

    #endregion

    public void CallSceneFadeOut() => StartCoroutine(FadeGameOut());

}