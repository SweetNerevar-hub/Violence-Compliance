using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour {

    private int speed;
    private float lifeTime;

    public float damage;

    // Start is called before the first frame update
    private void Start() {
        lifeTime = 5f;
        speed = 20;
    }

    private void Update() {
        HandleLaserBeamLifetime();

        transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
    }

    private void HandleLaserBeamLifetime() {
        if (lifeTime > 0f)
            lifeTime -= Time.deltaTime;

        else
            DestroyObject();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Asteroid") {
            DestroyObject();
            collision.gameObject.GetComponent<AsteroidBehaviour>().TakeDamage(damage);
        }
    }

    private void DestroyObject() => Destroy(gameObject);
}
