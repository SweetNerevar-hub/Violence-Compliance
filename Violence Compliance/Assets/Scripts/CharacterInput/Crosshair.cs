using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {

    [SerializeField] Camera mainCamera;

    // Start is called before the first frame update
    void Start() {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update() {
        float crosshairX = mainCamera.ScreenToWorldPoint(Input.mousePosition).x;
        float crosshairY = mainCamera.ScreenToWorldPoint(Input.mousePosition).y;

        transform.position = new Vector2(crosshairX, crosshairY); 
    }
}
