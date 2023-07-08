using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private int health;
    private int movementSpeed, rotationSpeed, lifeTime;
    private Vector3 moveDir;

    [SerializeField] private Sprite[] decayAnimation;

    // Start is called before the first frame update
    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        InitializeAsteroidData();
        StartCoroutine(AsteroidLifetime());
    }

    private void InitializeAsteroidData() {
        health = 3;
        lifeTime = 20;

        movementSpeed = Random.Range(1, 4);
        rotationSpeed = Random.Range(30, 60);

        moveDir = new Vector3(Random.Range(-1f, 2f), Random.Range(-1f, 2f), 0);
    }

    private void Update() {
        transform.Translate(moveDir * movementSpeed * Time.deltaTime, Space.World);
        transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    public void TakeDamage(int damage) {
        health -= damage;

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

            case 0:
                animator.enabled = true;
                UIManager.Instance.UpdateScore();

                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Asteroid") {
            animator.enabled = true;
        }
    }

    private void DestroyAsteroidObject() => Destroy(gameObject);
    private void DisableAsteroidCollider() => GetComponent<BoxCollider2D>().enabled = false;

    private IEnumerator AsteroidLifetime() {
        while (lifeTime > 0) {
            yield return new WaitForSeconds(1);

            lifeTime--;
        }

        animator.enabled = true;

        yield break;
    }
}
