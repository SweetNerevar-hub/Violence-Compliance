using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShoot : MonoBehaviour {

    private Animator animator;
    private AudioSource audioSource;
    private CharacterStatus characterStatus;

    [SerializeField] private AudioClip laserBeamShot;
    [SerializeField] private AudioClip chargedBeamShot;
    [SerializeField] private AudioClip chargeUp;
    [SerializeField] private GameObject laserBeam;

    private float timeBetweenShots;
    private float beamChargeTimer;
    private bool isChargingUp;

    private void Start() {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        characterStatus = GetComponent<CharacterStatus>();
    }

    // Update is called once per frame
    private void Update() {
        CheckTimeBetweenShots();
        HandleShootType();

        if (!Input.GetButton("Fire1")) {
            beamChargeTimer = 0f;
            isChargingUp = false;
            animator.SetBool("IsCharging", false);
            audioSource.clip = null;
        }

        characterStatus.UpdateBeamChargeBar(beamChargeTimer, timeBetweenShots);
    }

    private void HandleShootType() {
        if (Input.GetButtonDown("Fire1") && timeBetweenShots == 0f) {
            NormalShot(1f, 1f);
        }
            
        else if (Input.GetButton("Fire1") && timeBetweenShots == 0f) {
            ChargeBeam(1.5f, 3f);
        }
            

        else if (Input.GetButton("Fire2") && timeBetweenShots == 0f) {
            RapidFire(0.25f, 0.5f);
        }
    }

    private void NormalShot(float shotCooldown, float damage) {
        PlayAudio(laserBeamShot);

        GameObject beam = Instantiate(laserBeam, transform.GetChild(1).position, transform.rotation);
        beam.name = laserBeam.name;
        beam.GetComponent<LaserBeam>().damage = damage;

        timeBetweenShots = shotCooldown;
    }

    private void ChargeBeam(float shotCooldown, float damage) {
        beamChargeTimer += Time.deltaTime;
        animator.SetBool("IsCharging", true);

        if (!isChargingUp) {
            PlayAudio(chargeUp);
        }

        if (beamChargeTimer >= 1.5f) {
            PlayAudio(chargedBeamShot);
            isChargingUp = false;
            animator.SetBool("IsCharging", false);

            GameObject beam = Instantiate(laserBeam, transform.GetChild(1).position, transform.rotation);
            beam.name = laserBeam.name;
            beam.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0);
            beam.GetComponent<LaserBeam>().damage = damage;
            beam.GetComponent<LaserBeam>().isCharged = true;

            beamChargeTimer = 0f;

            if (timeBetweenShots >= 1.5f) timeBetweenShots = 1.5f;

            timeBetweenShots = shotCooldown;
        }
    }

    private void RapidFire(float shotCooldown, float damage) {
        PlayAudio(laserBeamShot);

        float randomShotRotation = Random.Range(-25, 25);
        Vector3 rot = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + randomShotRotation);

        GameObject beam = Instantiate(laserBeam, transform.GetChild(1).position, transform.rotation);
        beam.GetComponent<LaserBeam>().damage = damage;
        beam.transform.Rotate(rot);
        beam.name = laserBeam.name;

        timeBetweenShots = shotCooldown;
    }

    private void CheckTimeBetweenShots() {
        if (timeBetweenShots > 0f)
            timeBetweenShots -= Time.deltaTime;

        else if (timeBetweenShots < 0f)
            timeBetweenShots = 0f;
    }

    private void PlayAudio(AudioClip audioClip) {
        if(audioClip == chargeUp) {
            audioSource.pitch = 1;
            audioSource.clip = audioClip;
            audioSource.Play();
            isChargingUp = true;

            return;
        }

        float newPitch = Random.Range(0.8f, 1.2f);

        audioSource.pitch = newPitch;
        audioSource.PlayOneShot(audioClip);
    }

    public void StopCharacterShoot() => enabled = false;
}
