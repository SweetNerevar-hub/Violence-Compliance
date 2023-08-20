using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    private Rigidbody2D rb;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private AudioSource thrustersAudioSource;
    [SerializeField] private float cameraSmoothing;

    private int speed;
    private Vector3 cameraVelocity = Vector3.zero;

    private void Start() {
        EventManager.Instance.onGameEnd += StopCharacterInput;

        rb = GetComponent<Rigidbody2D>();

        speed = 5;
    }

    private void FixedUpdate() {
        MoveCharacter();

        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref cameraVelocity, cameraSmoothing); //Adds a small delay to the camera's position while following the player
    }

    private void Update() {
        RotateCharacterWithMouse();

        if (Input.GetButtonDown("HyperSpeedThrusters")) {
            speed = 10;

            StopAllCoroutines();
            StartCoroutine(HyperSpeedAudio(true));
        }

        else if (Input.GetButtonUp("HyperSpeedThrusters")) {
            speed = 5;

            StopAllCoroutines();
            StartCoroutine(HyperSpeedAudio(false));
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Destroy(GameObject.FindObjectOfType<UIManager>().gameObject);
            Cursor.visible = true;
            ToggleGameFade.Instance.CallSceneFadeOut(1);
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

    // Gradually increases/decreases ship thrusters audio volume
    private IEnumerator HyperSpeedAudio(bool usingHyperSpeed) {
        if (usingHyperSpeed) {
            while (thrustersAudioSource.volume < 1f) {
                thrustersAudioSource.volume += Time.deltaTime;

                yield return null;
            }

            thrustersAudioSource.volume = 1f;
        }

        else {
            while (thrustersAudioSource.volume > 0.3f) {
                thrustersAudioSource.volume -= Time.deltaTime;

                yield return null;
            }

            thrustersAudioSource.volume = 0.3f;
        }
    }

    public void StopCharacterInput(bool x) {
        rb.velocity = Vector2.zero;
        enabled = false;
    }

    private void OnDisable() {
        EventManager.Instance.onGameEnd -= StopCharacterInput;
    }
}
