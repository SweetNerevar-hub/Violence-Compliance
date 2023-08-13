using System.Collections;
using UnityEngine;

public class CharacterStatus : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    [SerializeField] private CameraShake mainCamera;
    [SerializeField] private Transform healthBar;
    [SerializeField] private Transform beamChargeBar;
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

        Vector2 healthBarSize = new Vector2(health * healthBarUnit, healthBar.localScale.y);
        healthBar.localScale = healthBarSize;

        mainCamera.CallCameraShake();

        if (health <= 0) {
            StartCoroutine(PlayerDeath());
        }
    }

    public void UpdateBeamChargeBar(float chargeValue, float timeBetweenShots) {
        Vector2 chargeBarSize = beamChargeBar.localScale;

        if (timeBetweenShots == 0) {
            chargeBarSize = new Vector2(chargeValue * beamChargeBarUnit, chargeBarSize.y);
        }

        else {
            chargeBarSize = new Vector2(timeBetweenShots * beamChargeBarUnit, chargeBarSize.y);
        }

        beamChargeBar.localScale = chargeBarSize;
    }

    private IEnumerator PlayerDeath() {
        EventManager.Instance.Event_OnGameEnd(true);
        Destroy(UIManager.Instance.gameObject);

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

        ToggleGameFade.Instance.CallSceneFadeOut(1);
    }
}
