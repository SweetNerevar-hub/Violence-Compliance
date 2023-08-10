using UnityEngine;
using UnityEngine.UI;

public class OutroCutscene : MonoBehaviour {

    private Text text;

    [TextArea][SerializeField] private string lowScoreEnding;
    [TextArea][SerializeField] private string highScoreEnding;
    [TextArea][SerializeField] private string hiddenEnding;

    private static bool eligibleForHiddenEnding;

    private void Start() {
        text = GetComponent<Text>();

        DisplayEndingText();
    }

    private void DisplayEndingText() {
        if(UIManager.Instance.score >= 10) {
            text.text = highScoreEnding;

            eligibleForHiddenEnding = true;
        }

        else if(UIManager.Instance.score < 10 && !eligibleForHiddenEnding) {
            text.text = lowScoreEnding;
        }

        else {
            text.text = hiddenEnding;

            eligibleForHiddenEnding = false;
        }
    }
}
