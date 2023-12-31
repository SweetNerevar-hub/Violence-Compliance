using System.Collections;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource audioSource;

    [SerializeField] private Sprite[] decayAnimation;
    [SerializeField] private AudioClip asteroidHit;
    [SerializeField] private AudioClip asteroidDestroy;

    private int movementSpeed;
    private int rotationSpeed;
    private int lifeTime;
    private float health;
    private float startingScale;
    private Vector3 moveDir;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        InitializeAsteroidData();
        StartCoroutine(AsteroidLifetime());
    }

    private void InitializeAsteroidData() {
        health = 3;
        lifeTime = 20;

        movementSpeed = Random.Range(1, 4);
        rotationSpeed = Random.Range(30, 60);
        startingScale = Random.Range(0.5f, 1.5f);

        moveDir = new Vector3(Random.Range(-1f, 2f), Random.Range(-1f, 2f), 0);
        transform.localScale = new Vector3(startingScale, startingScale, 1);
    }

    private void Update() {
        transform.Translate(moveDir * movementSpeed * Time.deltaTime, Space.World);
        transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    public void TakeDamage(float damage) {
        health -= damage;
        PlayAudio(asteroidHit);

        CheckHealthStatus();
    }

    private void CheckHealthStatus() {
        switch (health) {
            case 2:
                spriteRenderer.sprite = decayAnimation[0];

                break;

            case 1:
                spriteRenderer.sprite = decayAnimation[1];

                break;

            case <= 0:
                SetupForDestroy();
                UIManager.Instance.UpdateScore();

                break;
        }
    }

    private void SetupForDestroy() {
        animator.enabled = true; // Starts the crumble animation
        PlayAudio(asteroidDestroy);
    }

    private void PlayAudio(AudioClip audioClip) {
        float newPitch = Random.Range(0.5f, 2f);
        audioSource.pitch = newPitch;

        audioSource.PlayOneShot(audioClip);
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        // If this asteroids hits another asteroid
        if(collision.gameObject.tag == "Asteroid") {
            SetupForDestroy();
        }

        else if(collision.gameObject.tag == "Player") {
            CharacterStatus playerStatus = collision.gameObject.GetComponent<CharacterStatus>();

            playerStatus.TakeDamage(0.5f);
            playerStatus.hitAsteroidAmount++;
            SetupForDestroy();

            UIManager.Instance.UpdateScore();
            EventManager.Instance.Event_AsteroidHitPlayer(playerStatus.hitAsteroidAmount); // This sets up General Floyd's dialogue
        }
    }

    private IEnumerator AsteroidLifetime() {
        while (lifeTime > 0) {
            yield return new WaitForSeconds(1);

            lifeTime--;
        }

        SetupForDestroy();
    }

    // This method is called at the end of the crumble animation
    private void DestroyAsteroidObject() => Destroy(gameObject);

    // This method is called at the beginning of the crumble animation
    private void DisableAsteroidCollider() => GetComponent<BoxCollider2D>().enabled = false;
}
