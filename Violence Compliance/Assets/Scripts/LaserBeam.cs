using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour {

    [SerializeField] private bool playerShotThis;

    private Vector2 shootDir;
    private int speed;
    private float lifeTime;

    public float damage;
    public bool isCharged;

    // Start is called before the first frame update
    private void Start() {
        lifeTime = 5f;
        speed = 20;

        if (!playerShotThis) {
            SetDirectionForEnemyBeam();
        }
    }

    private void Update() {
        HandleLaserBeamLifetime();

        if(playerShotThis) {
            transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
        }
        
        else if(!playerShotThis) {
            transform.Translate(shootDir * 2f * Time.deltaTime, Space.World);
        }
    }

    private void HandleLaserBeamLifetime() {
        if (lifeTime > 0f)
            lifeTime -= Time.deltaTime;

        else
            DestroyObject();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Asteroid" && playerShotThis) {
            collision.gameObject.GetComponent<AsteroidBehaviour>().TakeDamage(damage);

            if (!isCharged) {
                DestroyObject();
            }
            
            else {
                return;
            }
        }

        else if(collision.gameObject.tag == "Enemy" && playerShotThis) {
            DestroyObject();
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }

        else if(collision.gameObject.tag == "Player" && !playerShotThis) {
            DestroyObject();
            collision.gameObject.GetComponent<CharacterStatus>().TakeDamage(damage);
        }
    }

    private void SetDirectionForEnemyBeam() => shootDir = GameObject.Find("Player").transform.position - transform.position;

    private void DestroyObject() => Destroy(gameObject);
}
