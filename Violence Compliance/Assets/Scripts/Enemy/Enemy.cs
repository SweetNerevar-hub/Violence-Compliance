using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource audioSource;

    [SerializeField] private Sprite[] shipDamageSprites;
    [SerializeField] private GameObject enemyBeam;
    [SerializeField] private AudioClip laserBeamShot, shipExplosion;
    [SerializeField] private float health;

    private GameObject player;

    private int movementSpeed;
    private float healthInPercentage;
    private float maxHealth;
    private float distanceToPlayer;
    private float timeBetweenShots;
    private bool isAlive;

    private void Start() {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");

        movementSpeed = 3;
        maxHealth = health;
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
        healthInPercentage = (health / maxHealth) * 100;

        UpdateAnimationBasedOnHealth();
    }

    private void EnemyMovementBehaviour() {
        Vector2 playerDirection = player.transform.position - transform.position;

        if (distanceToPlayer > 15f) {
            movementSpeed = 10;
        }

        if (distanceToPlayer < 6f) {
            transform.position = Vector2.MoveTowards(transform.position, -playerDirection, movementSpeed * Time.deltaTime);
        }

        else {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
        }
    }

    private void Shoot() {
        GameObject beam = Instantiate(enemyBeam, transform.position, Quaternion.identity);
        beam.name = enemyBeam.name;

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
        switch (healthInPercentage) {
            case < 64 and > 34:
                animator.enabled = false;
                spriteRenderer.sprite = shipDamageSprites[0];

                break;

            case < 34 and > 0:
                spriteRenderer.sprite = shipDamageSprites[1];

                break;

            case <= 0:
                spriteRenderer.sprite = shipDamageSprites[2];
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
        GetComponent<EdgeCollider2D>().enabled = false;
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