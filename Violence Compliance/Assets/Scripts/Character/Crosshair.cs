using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {

    [SerializeField] Camera mainCamera;

    void Start() {
        Cursor.visible = false;
    }

    void Update() {
        float crosshairX = mainCamera.ScreenToWorldPoint(Input.mousePosition).x;
        float crosshairY = mainCamera.ScreenToWorldPoint(Input.mousePosition).y;

        transform.position = new Vector2(crosshairX, crosshairY); 
    }
}
