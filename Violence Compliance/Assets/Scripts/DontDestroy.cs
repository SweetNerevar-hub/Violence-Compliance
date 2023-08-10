using System.Runtime.CompilerServices;
using UnityEngine;

public class DontDestroy : MonoBehaviour {

    private void Awake() {
        DontDestroyOnLoad(this);
    }
}
