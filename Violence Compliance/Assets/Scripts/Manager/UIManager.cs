using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager Instance { get; private set; }

    [SerializeField] private Text[] UIText; // [0] Score Text, [1] Game Timer/Lifeforms Text, [2] Mission Report Text
    [SerializeField] private Transform[] HUD; // [0] GameplayHUD, [1] InfoHUD, [2] DialogueHUD
    [SerializeField] private Transform player;

    public int gameTimer;
    public int score;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }

        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {
        EventManager.Instance.onGameEnd += StopEnemyShooting;

        Cursor.visible = false;
        StartCoroutine(UpdateGameTimer());
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

        player.gameObject.GetComponent<EdgeCollider2D>().enabled = false; // This is to stop asteroids continuing to collide with the player after the game ends
        EventManager.Instance.Event_OnGameEnd(false);
    }

    #region Game End Functions

    // Stops the enemies curently in spawned in to stop shooting the player once the game ends
    public void StopEnemyShooting(bool isPlayerDead) {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject item in enemies) {
            item.GetComponent<Enemy>().StopEnemyShooting();
        }

        if (!isPlayerDead) {
            ChangeUIOnGameEnd();
        }
    }

    // If the player isn't dead when the game ends, then this shows the post-game screen that displays the score and the lifeforms destroyed
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