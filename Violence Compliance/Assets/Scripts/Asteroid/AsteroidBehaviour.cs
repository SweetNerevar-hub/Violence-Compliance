using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource audioSource;

    private int movementSpeed, rotationSpeed, lifeTime;
    private float health;
    private float startingScale;
    private Vector3 moveDir;

    [SerializeField] private Sprite[] decayAnimation;
    [SerializeField] private AudioClip asteroidHit, asteroidDestroy;

    // Start is called before the first frame update
    void Start() {
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

    private void PlayAudio(AudioClip audioClip) {
        float newPitch = Random.Range(0.5f, 2f);
        audioSource.pitch = newPitch;

        audioSource.PlayOneShot(audioClip);
    }

    private void SetupForDestroy() {
        animator.enabled = true;
        PlayAudio(asteroidDestroy);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Asteroid") {
            SetupForDestroy();
        }

        else if(collision.gameObject.tag == "Player") {
            SetupForDestroy();
            collision.gameObject.GetComponent<CharacterStatus>().TakeDamage(0.5f);
            UIManager.Instance.UpdateScore();
        }
    }

    private IEnumerator AsteroidLifetime() {
        while (lifeTime > 0) {
            yield return new WaitForSeconds(1);

            lifeTime--;
        }

        SetupForDestroy();

        yield break;
    }

    private void DestroyAsteroidObject() => Destroy(gameObject);

    private void DisableAsteroidCollider() => GetComponent<BoxCollider2D>().enabled = false;
}
