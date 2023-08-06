using UnityEngine;

public class GeneralFloydDialogue : MonoBehaviour {

    [TextArea][SerializeField] private string intro;
    [TextArea][SerializeField] private string playerDied;
    [TextArea][SerializeField] private string playerSucceded;

    [TextArea][SerializeField] private string[] scoreMilestone;
    [TextArea][SerializeField] private string[] playerHitByAsteroid;
    [TextArea][SerializeField] private string[] enemySpawnedDialogue;

    private string _name;

    private void Start() {
        EventManager.Instance.onUpdateScore += CheckScore;
        EventManager.Instance.onAsteroidHitPlayer += PlayerHitByAsteroid;
        EventManager.Instance.onEnemySpawned += WhenEnemySpawned;

        _name = "General Floyd";

        Invoke("Intro", 0.01f);
    }

    private void Intro() {
        EventManager.Instance.Event_DisplayDialogue(_name, intro);
    }

    private void CheckScore(int score) {
        if (score == 10) {
            EventManager.Instance.Event_DisplayDialogue(_name, scoreMilestone[0]);
        }

        else if (score == 20) {
            EventManager.Instance.Event_DisplayDialogue(_name, scoreMilestone[1]);
        }
    }

    private void PlayerHitByAsteroid(int amountOfTimesHit) {
        EventManager.Instance.Event_DisplayDialogue(_name, playerHitByAsteroid[amountOfTimesHit - 1]);
    }

    private void WhenEnemySpawned() {
        int randomDialogue = Random.Range(0, enemySpawnedDialogue.Length);

        EventManager.Instance.Event_DisplayDialogue(_name, enemySpawnedDialogue[randomDialogue]);
    }

    public void CallDialoguePlayerDied() {
        EventManager.Instance.Event_DisplayDialogue(_name, playerDied);
    }

    public void CallDialogueTimerEnded() {
        EventManager.Instance.Event_DisplayDialogue(_name, playerSucceded);
    }

    private void OnDisable() {
        EventManager.Instance.onUpdateScore -= CheckScore;
        EventManager.Instance.onAsteroidHitPlayer -= PlayerHitByAsteroid;
        EventManager.Instance.onEnemySpawned -= WhenEnemySpawned;
    } 
}
