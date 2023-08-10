using System.Collections;
using UnityEngine;

public class GeneralFloydDialogue : MonoBehaviour {

    [TextArea][SerializeField] private string intro;
    [TextArea][SerializeField] private string playerDied;
    [TextArea][SerializeField] private string playerSucceded;
    [TextArea][SerializeField] private string playerFailed;

    [TextArea][SerializeField] private string[] scoreMilestone;
    [TextArea][SerializeField] private string[] playerHitByAsteroid;
    [TextArea][SerializeField] private string[] enemySpawnedDialogue;
    [TextArea][SerializeField] private string[] playerFailing;

    private string _name;
    private int _score;
    private bool canFloydTalk;

    private void Start() {
        EventManager.Instance.onUpdateScore += CheckScore;
        EventManager.Instance.onAsteroidHitPlayer += PlayerHitByAsteroid;
        EventManager.Instance.onEnemySpawned += WhenEnemySpawned;
        EventManager.Instance.onGameEnd += CallDialogueOnGameEnd;

        _name = "General Floyd";
        _score = 0;
        canFloydTalk = true;

        Invoke("Intro", 0.01f);
        InvokeRepeating("PlayerFailingMission", 1f, 1f);
    }

    private void Intro() {
        EventManager.Instance.Event_DisplayDialogue(_name, intro);
    }

    private void PlayerFailingMission() {
        int timer = UIManager.Instance.gameTimer;

        if(_score < 5 && timer == 170) {
            EventManager.Instance.Event_DisplayDialogue(_name, playerFailing[0]);
        }

        else if(_score < 10 && timer == 110) {
            EventManager.Instance.Event_DisplayDialogue(_name, playerFailing[1]);
        }

        else if(_score < 10 && timer == 50) {
            EventManager.Instance.Event_DisplayDialogue(_name, playerFailing[2]);
        }
    }

    private void CheckScore(int score) {
        _score = score;

        switch (score) {
            case 10:
                EventManager.Instance.Event_DisplayDialogue(_name, scoreMilestone[0]);

                break;

            case 20:
                EventManager.Instance.Event_DisplayDialogue(_name, scoreMilestone[1]);

                break;

            case 30:
                EventManager.Instance.Event_DisplayDialogue(_name, scoreMilestone[2]);

                break;

            case 40:
                EventManager.Instance.Event_DisplayDialogue(_name, scoreMilestone[3]);

                break;

            case 50:
                EventManager.Instance.Event_DisplayDialogue(_name, scoreMilestone[4]);

                break;

            case 60:
                EventManager.Instance.Event_DisplayDialogue(_name, scoreMilestone[5]);

                break;
        }
    }

    private void PlayerHitByAsteroid(int amountOfTimesHit) {
        EventManager.Instance.Event_DisplayDialogue(_name, playerHitByAsteroid[amountOfTimesHit - 1]);
    }

    private void WhenEnemySpawned() {
        if (!canFloydTalk) {
            return;
        }

        int randomDialogue = Random.Range(0, enemySpawnedDialogue.Length);

        EventManager.Instance.Event_DisplayDialogue(_name, enemySpawnedDialogue[randomDialogue]);

        StartCoroutine(CanDialogueContinue());
    }

    private IEnumerator CanDialogueContinue() {
        int timerBetweenDialogue = 10;
        canFloydTalk = false;
        
        while (timerBetweenDialogue > 0) {
            yield return new WaitForSeconds(1);

            timerBetweenDialogue--;
        }

        canFloydTalk = true;
    }

    private void CallDialogueOnGameEnd(bool isPlayerDead) {
        if (!isPlayerDead && _score >= 10) {
            EventManager.Instance.Event_DisplayDialogue(_name, playerSucceded);
        }

        else if(!isPlayerDead && _score < 10) {
            EventManager.Instance.Event_DisplayDialogue(_name, playerFailed);
        }

        else {
            EventManager.Instance.Event_DisplayDialogue(_name, playerDied);
        }
    }

    private void OnDisable() {
        EventManager.Instance.onUpdateScore -= CheckScore;
        EventManager.Instance.onAsteroidHitPlayer -= PlayerHitByAsteroid;
        EventManager.Instance.onEnemySpawned -= WhenEnemySpawned;
        EventManager.Instance.onGameEnd += CallDialogueOnGameEnd;
    } 
}
