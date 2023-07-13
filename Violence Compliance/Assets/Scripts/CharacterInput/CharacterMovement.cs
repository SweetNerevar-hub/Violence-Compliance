using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    Rigidbody2D rb;

    [SerializeField] private int speed;
    [SerializeField] private Camera mainCamera;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        MoveCharacter();
        /*if (Input.GetKey(KeyCode.LeftShift))
            speed = 20;

        else
            speed = 5;*/
    }

    private void Update() {
        RotateCharacterWithMouse();

        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);

        if (Input.GetKeyDown(KeyCode.Space)) {
            UIManager.Instance.ShowGameInfo();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
            
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
