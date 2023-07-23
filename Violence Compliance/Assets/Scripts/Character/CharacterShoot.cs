using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShoot : MonoBehaviour {

    private AudioSource audioSource;
    private CharacterStatus characterStatus;

    [SerializeField] private AudioClip laserBeamShot;
    [SerializeField] private GameObject laserBeam;

    private float timeBetweenShots;
    private float beamCharge;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        characterStatus = GetComponent<CharacterStatus>();

        timeBetweenShots = 0f;
    }

    // Update is called once per frame
    private void Update() {
        CheckTimeBetweenShots();
        HandleShootType();

        if (!Input.GetButton("Fire1")) {
            beamCharge = 0f;
        }

        characterStatus.UpdateBeamChargeBar(beamCharge, timeBetweenShots);
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

        if (beamCharge >= 1.5f) {
            beamCharge = 3f;
            Shoot(beamCharge);

            beamCharge = 0f;
        }
    }

    private void Shoot(float value) {
        PlayAudio(laserBeamShot);

        GameObject beam = Instantiate(laserBeam, transform.GetChild(1).position, transform.rotation);
        beam.name = laserBeam.name;
        beam.GetComponent<LaserBeam>().damage = value;

        if (value >= 1.5f) value = 1.5f;

        timeBetweenShots = value;
    }

    private void CheckTimeBetweenShots() {
        if (timeBetweenShots > 0f)
            timeBetweenShots -= Time.deltaTime;

        else if (timeBetweenShots < 0f)
            timeBetweenShots = 0f;
    }

    private void PlayAudio(AudioClip audioClip) {
        float newPitch = Random.Range(0.8f, 1.2f);
        audioSource.pitch = newPitch;

        audioSource.PlayOneShot(audioClip);
    }

    public void StopCharacterShoot() => enabled = false;
}
