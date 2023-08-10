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
        EventManager.Instance.onGameEnd += StopEnemyShooting;

        if(SceneChangeManager.Instance.currentScene != 2) {
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

        player.gameObject.GetComponent<EdgeCollider2D>().enabled = false;
        EventManager.Instance.Event_OnGameEnd(false);
    }

    #region Game End Functions

    public void StopEnemyShooting(bool isPlayerDead) {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject item in enemies) {
            item.GetComponent<Enemy>().StopEnemyShooting();
        }

        if (!isPlayerDead) {
            ChangeUIOnGameEnd();
        }
    }

    public void ChangeUIOnGameEnd() {
        int lifeformsDestroyed = Random.Range(score * 10, score * 100);

        UIText[0].GetComponent<RectTransform>().localPosition = new Vector2(0, 175);
        UIText[1].GetComponent<RectTransform>().localPosition = new Vector2(0, 100);
        UIText[1].text = "Lifeforms Destroyed: " + lifeformsDestroyed;
        UIText[2].gameObject.SetActive(true);
        HUD[1].gameObject.SetActive(true);
        HUD[0].gameObject.SetActive(false);

        ToggleGameFade.Instance.CallSceneFadeOut(3);
    }

    #endregion

    private void OnDisable() {
        EventManager.Instance.onGameEnd -= StopEnemyShooting;
    }
}