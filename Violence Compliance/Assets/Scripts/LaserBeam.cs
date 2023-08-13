using UnityEngine;

public class LaserBeam : MonoBehaviour {

    [SerializeField] private bool playerShotThis;

    private Vector2 shootDir;
    private float lifeTime;

    public float damage;
    public bool isCharged;

    // Start is called before the first frame update
    private void Start() {
        lifeTime = 5f;

        if (!playerShotThis) {
            SetDirectionForEnemyBeam();
        }
    }

    private void Update() {
        HandleLaserBeamLifetime();

        // This sets the direction of the beam depending on who shot it
        // A beam shot by the player will move the same direction as where they are pointing
        // While an enemy beam will move towards the player
        if (playerShotThis) {
            transform.Translate(transform.up * 20 * Time.deltaTime, Space.World);
        }

        else if (!playerShotThis) {
            transform.Translate(shootDir * 2.5f * Time.deltaTime, Space.World);
        }
    }

    private void HandleLaserBeamLifetime() {
        if (lifeTime > 0f) {
            lifeTime -= Time.deltaTime;
        }

        else {
            DestroyObject();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Asteroid" && playerShotThis) {
            collision.gameObject.GetComponent<AsteroidBehaviour>().TakeDamage(damage);

            // This allows a charged beam to not be destroyed when it hits an asteroid
            if (!isCharged) {
                DestroyObject();
            }
        }

        else if (collision.gameObject.tag == "Enemy" && playerShotThis) {
            DestroyObject();
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }

        else if (collision.gameObject.tag == "Player" && !playerShotThis) {
            DestroyObject();
            collision.gameObject.GetComponent<CharacterStatus>().TakeDamage(damage);
        }
    }

    private void SetDirectionForEnemyBeam() {
        shootDir = GameObject.Find("Player").transform.position - transform.position;
    }

    private void DestroyObject() {
        Destroy(gameObject);
    }
}
