using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterShoot : MonoBehaviour {

    private AudioSource audioSource;

    [SerializeField] private AudioClip playerShoot;
    [SerializeField] private GameObject laserBeam;

    private float timeBetweenShots;

    private void Start() {
        audioSource = GetComponent<AudioSource>();

        timeBetweenShots = 0f;
    }

    // Update is called once per frame
    private void Update() {
        if (timeBetweenShots > 0f) timeBetweenShots -= Time.deltaTime;
        else if (timeBetweenShots < 0f) timeBetweenShots = 0f;

        HandleShoot();
    }

    private void HandleShoot() {
        if (Input.GetButtonDown("Fire1") && timeBetweenShots == 0f) {
            audioSource.PlayOneShot(playerShoot);

            Instantiate(laserBeam, transform.GetChild(1).position, transform.rotation);
            timeBetweenShots = 1f;
        }
    }
}
