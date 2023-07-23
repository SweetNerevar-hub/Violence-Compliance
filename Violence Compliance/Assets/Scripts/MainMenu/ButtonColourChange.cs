using UnityEngine;
using UnityEngine.UI;

public class ButtonColourChange : MonoBehaviour {

    ColorBlock buttonColorBlock;

    private void Start() {
        buttonColorBlock = GetComponent<Button>().colors;
    }

    private void Update() {
        Color randomColor = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        );

        buttonColorBlock.highlightedColor = randomColor;
        GetComponent<Button>().colors = buttonColorBlock;
    }
}
