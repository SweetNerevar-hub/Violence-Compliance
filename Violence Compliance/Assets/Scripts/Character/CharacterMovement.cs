using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    private Rigidbody2D rb;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private float cameraSmoothing;

    private int speed;
    private Vector3 cameraVelocity = Vector3.zero;

    // Start is called before the first frame update
    private void Start() {
        EventManager.Instance.onGameEnd += StopCharacterInput;

        rb = GetComponent<Rigidbody2D>();

        speed = 5;
    }

    private void FixedUpdate() {
        MoveCharacter();
    }

    private void Update() {
        RotateCharacterWithMouse();

        /*if (Input.GetKeyDown(KeyCode.LeftShift))
            speed = 10;

        else if (Input.GetKeyUp(KeyCode.LeftShift))
            speed = 5;*/

        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);

        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref cameraVelocity, cameraSmoothing);

        if (Input.GetKeyDown(KeyCode.Space)) {
            ToggleGameFade.Instance.CallSceneFadeOut(3);
        }
    }

    private void MoveCharacter() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector2 moveDir = new Vector2(h, v);

        rb.velocity = moveDir * speed;
    }

    private void RotateCharacterWithMouse() {
        Quaternion rotation = Quaternion.LookRotation(mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position, transform.TransformDirection(-Vector3.forward));
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
    }

    public void StopCharacterInput(bool x) {
        rb.velocity = Vector2.zero;
        enabled = false;
    }

    private void OnDisable() {
        EventManager.Instance.onGameEnd -= StopCharacterInput;
    }
}
