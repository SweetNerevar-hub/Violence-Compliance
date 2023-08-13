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

        // There will be a 1 in 4 chance that when an alien spawns it will communicate with the player
        // This is avoid dialogue spam if aliens spawn frequently
        if (WillEnemyCommunicate()) {
            StartCoroutine(CallAlienDialogue());
        }
    }

    private IEnumerator CallAlienDialogue() {
        yield return new WaitForSeconds(4f); // Allows for a cool effect where the aliens will cut off General Floyd's warnings of aliens, in order to communicate with the player

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