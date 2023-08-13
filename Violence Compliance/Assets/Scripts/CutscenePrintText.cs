using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CutscenePrintText : MonoBehaviour {

    private Text textBody;

    [Range(10, 20)][SerializeField] private float textSpeed;

    private float characterPrintDelay;
    private char[] charactersInText;

    private void Start() {
        textBody = GetComponent<Text>();
        characterPrintDelay = 1f;

        StartCoroutine(DisplayText());
    }

    private IEnumerator DisplayText() {
        yield return new WaitForEndOfFrame();

        string textToPrint = textBody.text;

        charactersInText = textToPrint.ToCharArray();
        textBody.text = null;

        for (int i = 0; i < charactersInText.Length + 1; i++) {
            textBody.text = textToPrint.Substring(0, i);

            yield return new WaitForSeconds(characterPrintDelay / textSpeed);
        }
    }
}