using UnityEngine;

public class MusicManager : MonoBehaviour {

    private AudioSource audioSource;

    [SerializeField] private AudioClip openingTrack;
    [SerializeField] private AudioClip gameplayTrack;
    [SerializeField] private AudioClip outroTrack;

    private void Start() {
        EventManager.Instance.onSceneChange += ChangeMusicOnSceneChange;

        audioSource = GetComponent<AudioSource>();
    }

    private void ChangeMusicOnSceneChange(int index) {
        if(index == 0 || index == 1 && audioSource.clip != openingTrack) {
            ChangeAudio(openingTrack);
        }

        else if(index == 2) {
            ChangeAudio(gameplayTrack);
        }

        else if(index == 3) {
            ChangeAudio(outroTrack);
        }
    }

    private void ChangeAudio(AudioClip audioClip) {
        audioSource.Stop();
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    private void OnDisable() {
        EventManager.Instance.onSceneChange -= ChangeMusicOnSceneChange;
    }
}
