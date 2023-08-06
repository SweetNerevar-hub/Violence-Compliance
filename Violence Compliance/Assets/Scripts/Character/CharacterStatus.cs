using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    [SerializeField] private Transform healthBar, beamChargeBar;
    [SerializeField] private AudioClip shipExplosion;

    private float health;
    private float healthBarUnit;
    private float beamChargeBarUnit;

    public int hitAsteroidAmount;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        health = 3f;
        healthBarUnit = healthBar.localScale.x / health;
        beamChargeBarUnit = beamChargeBar.localScale.x / 1.5f;
    }

    public void TakeDamage(float damage) {
        health -= damage;

        Vector2 barSize = new Vector2(health * healthBarUnit, healthBar.localScale.y);
        healthBar.localScale = barSize;

        if(health <= 0) {
            StartCoroutine(PlayerDeath());
        }
    }

    public void UpdateBeamChargeBar(float chargeValue, float timeBetweenShots) {
        Vector2 barSize = beamChargeBar.localScale;

        if (timeBetweenShots == 0) {
            barSize = new Vector2(chargeValue * beamChargeBarUnit, barSize.y);
        }

        else {
            barSize = new Vector2(timeBetweenShots * beamChargeBarUnit, barSize.y);
        }

        beamChargeBar.localScale = barSize;
    }

    private IEnumerator PlayerDeath() {
        EventManager.Instance.onGameEnd_PlayerDied.Invoke();

        GetComponent<EdgeCollider2D>().enabled = false;

        float timer = 1f;
        float decayAmount = 0.2f;

        audioSource.clip = null;
        audioSource.PlayOneShot(shipExplosion);

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

        UIManager.Instance.CallSceneFadeOut();
    }
}
