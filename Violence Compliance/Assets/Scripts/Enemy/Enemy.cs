using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    Animator animator;
    AudioSource audioSource;

    [SerializeField] Sprite[] shipDamageSprites;
    [SerializeField] GameObject PH_beam;
    [SerializeField] AudioClip laserBeamShot, shipExplosion;

    GameObject player;

    int movementSpeed;

    float health;
    float distanceToPlayer;
    float timeBetweenShots;

    bool isAlive;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");

        health = 3;
        movementSpeed = 3;
        isAlive = true;
    }

    private void Update() {
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        CheckTimeBetweenShots();

        if (isAlive) {
            EnemyMovementBehaviour();

            if(distanceToPlayer < 8f && timeBetweenShots == 0f) {
                movementSpeed = 3;
                Shoot();
            }
        }
    }

    public void TakeDamage(float damage) {
        health -= damage;

        UpdateAnimationBasedOnHealth();
    }

    private void EnemyMovementBehaviour() {
        Vector2 playerDirection = player.transform.position - transform.position;

        if (distanceToPlayer > 20f) {
            movementSpeed = 10;
        }

        if (distanceToPlayer < 7f) {
            transform.position = Vector2.MoveTowards(transform.position, -playerDirection, movementSpeed * Time.deltaTime);
        }

        else {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
        }
    }

    private void Shoot() {
        GameObject beam = Instantiate(PH_beam, transform.position, Quaternion.identity);
        beam.name = PH_beam.name;

        PlayAudio(laserBeamShot);

        timeBetweenShots = Random.Range(1f, 4f);
    }

    private void CheckTimeBetweenShots() {
        if(timeBetweenShots > 0f) {
            timeBetweenShots -= Time.deltaTime;
        }

        else {
            timeBetweenShots = 0f;
        }
    }

    private void UpdateAnimationBasedOnHealth() {
        switch (health) {
            case 2:
                animator.enabled = false;
                spriteRenderer.sprite = shipDamageSprites[0];

                break;

            case 1:
                spriteRenderer.sprite = shipDamageSprites[1];

                break;

            case <= 0:
                animator.enabled = false;
                isAlive = false;

                UIManager.Instance.UpdateScore();
                StartCoroutine(ShipDestruction());

                break;
        }
    }

    private void PlayAudio(AudioClip audioClip) {
        float newPitch = Random.Range(0.8f, 1.2f);
        audioSource.pitch = newPitch;

        audioSource.PlayOneShot(audioClip);
    }

    private IEnumerator ShipDestruction() {
        GetComponent<BoxCollider2D>().enabled = false;
        PlayAudio(shipExplosion);

        float timer = 1f;
        float decayAmount = 0.2f;

        spriteRenderer.sprite = shipDamageSprites[2];

        while (timer > 0f) {
            transform.localScale = new Vector2(transform.localScale.x - decayAmount, transform.localScale.y - decayAmount);
            spriteRenderer.color = new Color(
                spriteRenderer.color.r - decayAmount,
                spriteRenderer.color.g - decayAmount,
                spriteRenderer.color.b - decayAmount,
                spriteRenderer.color.a - decayAmount
            );

            yield return new WaitForSeconds(0.5f);

            timer -= decayAmount;
        }

        Destroy(gameObject);
    }

    public void StopEnemyShooting() => enabled = false;
}
