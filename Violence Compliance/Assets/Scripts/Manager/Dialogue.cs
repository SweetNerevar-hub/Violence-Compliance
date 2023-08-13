using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

    private Animator animator;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Text dialogueBox;
    [SerializeField] private Text dialogueName;
    [SerializeField] private AudioClip printTransmission;
    [Range(10, 20)][SerializeField] private int textSpeed;

    private float characterPrintDelay;
    private char[] charactersInDialogue;

    private string _name;
    private string _dialogue;

    private void Start() {
        EventManager.Instance.onDialoguePrompt += InitDialogueText;

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        characterPrintDelay = 1f;
    }

    public void InitDialogueText(string senderName, string dialogue) {

        // If there is dialogue already being printed when more dialogue is being initilised, stop the current dialogue and print the new one
        if (animator.GetBool("IsDialogueActive")) {
            _name = senderName;

            StopAllCoroutines();
            StartCoroutine(DisplayDialogueText());
        }

        charactersInDialogue = dialogue.ToCharArray();
        _name = senderName;
        _dialogue = dialogue;
        spriteRenderer.enabled = true;

        animator.SetBool("IsDialogueActive", true);
    }

    IEnumerator DisplayDialogueText() {
        dialogueName.text = _name;

        // This For loop prints each character of the dialogue
        for (int i = 0; i < charactersInDialogue.Length + 1; i++) {
            dialogueBox.text = _dialogue.Substring(0, i);
            audioSource.PlayOneShot(printTransmission);

            yield return new WaitForSeconds(characterPrintDelay / textSpeed);
        }

        yield return new WaitForSeconds(3f);

        CloseDialogue();
    }

    private void CloseDialogue() {
        dialogueBox.text = null;
        dialogueName.text = null;

        animator.SetBool("IsDialogueActive", false);
    }

    // This method is referenced in the Animator, and is called when the dialogue box's opening animation is finished
    public void OpenDialogueBox() {
        StartCoroutine(DisplayDialogueText());
    }

    // This method is referenced in the Animator, and is called when the dialogue box is closed
    public void HideDialogueBox() {
        spriteRenderer.enabled = false;
    }

    private void OnDisable() {
        EventManager.Instance.onDialoguePrompt -= InitDialogueText;
    }
}
