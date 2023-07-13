using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShoot : MonoBehaviour {

    private AudioSource audioSource;

    [SerializeField] private AudioClip playerShoot;
    [SerializeField] private GameObject laserBeam;

    private float timeBetweenShots;
    private float beamCharge;

    private void Start() {
        audioSource = GetComponent<AudioSource>();

        timeBetweenShots = 0f;
    }

    // Update is called once per frame
    private void Update() {
        CheckTimeBetweenShots();
        HandleShootType();

        if (!Input.GetButton("Fire1")) {
            beamCharge = 0f;
        }

        UIManager.Instance.UpdateBeamChargeBar(beamCharge, timeBetweenShots);
    }

    private void HandleShootType() {
        if (Input.GetButtonDown("Fire1") && timeBetweenShots == 0f) {
            Shoot(1f);
        }
            
        else if (Input.GetButton("Fire1") && timeBetweenShots == 0f) {
            ChargeBeam();
        }
            

        else if (Input.GetButton("Fire2") && timeBetweenShots == 0f) {
            Shoot(0.5f);
        }
    }

    private void ChargeBeam() {
        beamCharge += Time.deltaTime;

        if (beamCharge >= 3f) {
            beamCharge = 3f;
            Shoot(beamCharge);

            beamCharge = 0f;
        }
    }

    private void Shoot(float value) {
        audioSource.PlayOneShot(playerShoot);

        GameObject beam = Instantiate(laserBeam, transform.GetChild(1).position, transform.rotation);
        beam.GetComponent<LaserBeam>().damage = value;
        timeBetweenShots = value;
    }

    private void CheckTimeBetweenShots() {
        if (timeBetweenShots > 0f)
            timeBetweenShots -= Time.deltaTime;

        else if (timeBetweenShots < 0f)
            timeBetweenShots = 0f;
    }

    public void StopCharacterShoot() => enabled = false;
}
