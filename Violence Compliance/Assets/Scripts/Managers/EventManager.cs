using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour {

    public static EventManager Instance;

    public UnityEvent onGameEnd;

    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(this);
        }

        else {
            Instance = this;
        }
    }
}
