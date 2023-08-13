using UnityEngine;
using UnityEngine.UI;

public class ButtonColourChange : MonoBehaviour {

    Button button;
    ColorBlock buttonColorBlock;

    private void Start() {
        button = GetComponent<Button>();
        buttonColorBlock = button.colors;
    }

    private void Update() {
        Color randomColor = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        );

        buttonColorBlock.highlightedColor = randomColor;
        button.colors = buttonColorBlock;
    }
}
