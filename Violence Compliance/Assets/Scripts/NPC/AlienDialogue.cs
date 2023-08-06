using System.Collections;
using UnityEngine;

public class AlienDialogue : MonoBehaviour {

    [TextArea][SerializeField] private string[] dialogueOnSpawn;

    private string _name;

    private void Start() {
        EventManager.Instance.onEnemySpawned += WhenEnemySpawned;

        _name = "???";
    }

    private void WhenEnemySpawned() {
        if (WillEnemyCommunicate()) {
            StartCoroutine(CallAlienDialogue());
        }

        return;
    }

    private IEnumerator CallAlienDialogue() {
        yield return new WaitForSeconds(4f);

        int i = Random.Range(0, dialogueOnSpawn.Length);
        EventManager.Instance.Event_DisplayDialogue(_name, dialogueOnSpawn[i]);
    }

    private bool WillEnemyCommunicate() {
        int x = Random.Range(0, 4);

        if (x == 0) {
            return true;
        }

        return false;
    }

    private void OnDisable() {
        EventManager.Instance.onEnemySpawned -= WhenEnemySpawned;
    }
}