using System;
using UnityEngine;

public class EventManager : MonoBehaviour {

    public static EventManager Instance;

    public event Action<string, string> onDialoguePrompt;
    public event Action<bool> onGameEnd;
    public event Action<int> onSceneChange;
    public event Action<int> onUpdateScore;
    public event Action<int> onAsteroidHitPlayer;
    public event Action onEnemySpawned;
    

    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(this);
        }

        else {
            Instance = this;
        }
    }

    public void Event_DisplayDialogue(string senderName, string dialogue) {
        onDialoguePrompt?.Invoke(senderName, dialogue);
    }

    public void Event_OnGameEnd(bool isPlayerDead) {
        onGameEnd?.Invoke(isPlayerDead);
    }

    public void Event_SceneChange(int index) {
        onSceneChange?.Invoke(index);
    }

    public void Event_UpdateScore(int score) {
        onUpdateScore?.Invoke(score);
    }

    public void Event_AsteroidHitPlayer(int amountOfTimesHit) {
        onAsteroidHitPlayer?.Invoke(amountOfTimesHit);
    }

    [ContextMenu("Simulate Enemy Spawn")]
    public void Event_EnemySpawned() {
        onEnemySpawned?.Invoke();
    }
}
