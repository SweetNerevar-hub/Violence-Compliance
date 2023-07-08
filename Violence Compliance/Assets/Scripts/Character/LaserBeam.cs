using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour {

    private int speed;
    private float lifeTime;

    // Start is called before the first frame update
    private void Start() {
        lifeTime = 5f;
        speed = 20;
    }

    private void Update() {
        if (lifeTime > 0f) lifeTime -= Time.deltaTime;
        else DestroyObject();

        transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Asteroid") {
            DestroyObject();
            collision.gameObject.GetComponent<AsteroidBehaviour>().TakeDamage(1);
        }
    }

    private void DestroyObject() => Destroy(gameObject);
}
