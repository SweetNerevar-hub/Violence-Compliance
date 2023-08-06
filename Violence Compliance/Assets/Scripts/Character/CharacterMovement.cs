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
    void Start() {
        rb = GetComponent<Rigidbody2D>();

        speed = 5;
    }

    void FixedUpdate() => MoveCharacter();

    private void Update() {
        RotateCharacterWithMouse();

        /*if (Input.GetKeyDown(KeyCode.LeftShift))
            speed = 10;

        else if (Input.GetKeyUp(KeyCode.LeftShift))
            speed = 5;*/

        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);

        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref cameraVelocity, cameraSmoothing);

        /*if (Input.GetKeyDown(KeyCode.Escape)) {
            UIManager.Instance.CallSceneFadeOut();
        }*/
    }

    void MoveCharacter() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector2 moveDir = new Vector2(h, v);

        rb.velocity = moveDir * speed;
    }

    void RotateCharacterWithMouse() {
        Quaternion rotation = Quaternion.LookRotation(mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position, transform.TransformDirection(-Vector3.forward));
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
    }

    public void StopCharacterInput() {
        rb.velocity = Vector2.zero;
        enabled = false;
    }
}
