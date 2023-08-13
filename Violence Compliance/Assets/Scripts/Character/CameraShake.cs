using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    [SerializeField] private AnimationCurve curve;

    public void CallCameraShake() {
        StartCoroutine(CameraShakeOnHit());
    }

    private IEnumerator CameraShakeOnHit() {
        Vector3 startPos = transform.position;
        float duration = 1f;
        float timeElapsed = 0f;

        while(timeElapsed < duration) {
            float shakeStrength = curve.Evaluate(timeElapsed / duration); // Actively reads where in the animation curve it is

            timeElapsed += Time.deltaTime;
            transform.position = startPos + Random.insideUnitSphere * shakeStrength; // Changes the position of the camera to a random point in a sphere, and multiplies it by the shake strength so the insensity of the shake follows the animation curve
            yield return null;
        }

        transform.position = startPos;
    }
}
