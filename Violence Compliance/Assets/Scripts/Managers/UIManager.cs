using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager Instance;

    [SerializeField] GameObject UIParent, fadeScreen;
    [SerializeField] Transform player;
    [SerializeField] Transform beamChargeBar;
    [SerializeField] Text scoreText, gameTimerText;

    private int score, gameTimer;
    private bool isShowingInfo;
    private float barWidthDivideByThree;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }

        else {
            Instance = this;
        }
    }

    private void Start() {
        gameTimer = 265;
        isShowingInfo = false;
        
        barWidthDivideByThree = beamChargeBar.localScale.x / 3;
        StartCoroutine(UpdateGameTimer());
    }

    private void Update() {
        UIParent.transform.GetChild(0).position = new Vector2(player.position.x, player.position.y + 1);
    }

    public void UpdateScore() {
        score++;
        scoreText.text = "Score: " + score;
    }

    public void ShowGameInfo() {
        if (!isShowingInfo) {
            UIParent.SetActive(true);
            isShowingInfo = true;
        }

        else {
            UIParent.SetActive(false);
            isShowingInfo = false;
        }
    }

    public void UpdateBeamChargeBar(float chargeValue, float timeBetweenShots) {
        Vector2 barSize = beamChargeBar.localScale;
        beamChargeBar.position = new Vector2(player.position.x - 1.12f, player.position.y - 1f);

        if (timeBetweenShots == 0) {
            barSize = new Vector2(chargeValue * barWidthDivideByThree, barSize.y);
        }
        
        else {
            barSize = new Vector2(timeBetweenShots * barWidthDivideByThree, barSize.y);
        }

        beamChargeBar.localScale = barSize;
    }

    public void ChangeUIOnGameEnd() {
        int lifeformsDestroyed = Random.Range(score * 10, score * 100);

        gameTimerText.text = "Lifeforms Destroyed: " + lifeformsDestroyed;
        UIParent.SetActive(true);
        StartCoroutine(FadeGameOut());
    }

    IEnumerator UpdateGameTimer() {
        while(gameTimer > 0) {
            yield return new WaitForSeconds(1);

            gameTimer--;
            gameTimerText.text = "Time Remaining: " + gameTimer.ToString();
        }

        EventManager.Instance.onGameEnd.Invoke();
    }

    IEnumerator FadeGameOut() {
        float a = fadeScreen.GetComponent<Image>().color.a;

        yield return new WaitForSeconds(10);

        while (a < 1f) {
            a += 0.005f;
            fadeScreen.GetComponent<Image>().color = new Color(0, 0, 0, a);

            yield return new WaitForSeconds(0.01f);
        }

        Application.Quit();
    }
}