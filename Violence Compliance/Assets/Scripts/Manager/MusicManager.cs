using UnityEngine;

public class MusicManager : MonoBehaviour {

    private AudioSource audioSource;

    [SerializeField] private AudioClip openingTrack;
    [SerializeField] private AudioClip gameplayTrack;
    [SerializeField] private AudioClip outroTrack;

    private void Awake() {

        // If there is an object with the MusicManager component on it that isn't this one, then destroy this object
        // This is stop duplications during scene changes, while still allowing this object to be persistent and without making it a singleton
        if (FindObjectOfType<MusicManager>() != this) {
            Destroy(gameObject);
        }
    }

    private void Start() {
        EventManager.Instance.onSceneChange += ChangeMusicOnSceneChange;

        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    private void ChangeMusicOnSceneChange(int index) {
        if (index == 0 || index == 1 && audioSource.clip != openingTrack) {
            ChangeAudio(openingTrack);
        }

        else if (index == 2) {
            ChangeAudio(gameplayTrack);
        }

        else if (index == 3) {
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
