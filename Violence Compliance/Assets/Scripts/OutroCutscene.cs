using UnityEngine;
using UnityEngine.UI;

public class OutroCutscene : MonoBehaviour {

    private Text text;

    private string lowScoreEnding;
    private string highScoreEnding;
    private string hiddenEnding;

    public static bool eligibleForHiddenEnding;

    private void Start() {
        text = GetComponent<Text>();

        InitEndingTexts();
        DisplayEndingText();
    }

    private void InitEndingTexts() {

        // Hardcoding the text into the script, so in the case that I change any of the variable name and forgot to copy the text, I won't lose it all
        lowScoreEnding =
            "You've royally screwed the pooch on this one haven't you. The D.S.S.R really aren't happy with what you did, or rather, failed to do. " +
            "Think of the hubris: to escape your crumbling civilisation they would have you destroy countless potential others. " +
            "But look on the brightside; because of your inaction, there are things greater than your comprehension that will have a future.\r\n\r\n" +
            "Species that have survived might find a nice planet to settle down one, perhaps even entire civilisations will be built, collapse and built again due to your actions; all because you didn't do what you were told.\r\n\r\n" +
            "Only you truly know why you did what you did. You were given a direct order, and billions of dollars were put into your one job; but still you refused. " +
            "It is curious... when we meet I would like you to tell me why. But until then, you've done your universe a service, and I thank you.";

        highScoreEnding =
            "Congratulations, I hope you've enjoyed yourself; for there is nothing more important than the satisfaction and gratification of the simple pleasures: " +
            "looking at the stars of the night sky, for instance, and wondering upon them, not knowing for sure whether there is anythere greater than yourself out there.\r\n\r\n" +
            "That is why you are here after all, flying among those same stars you once pondered on. And what did you do with that experience? " +
            "You reversed a course which was laid down long ago, before even your progenitors which travelled on much the same space bodies as the ones you've destroyed once did.\r\n\r\n" +
            "Things that could've been, never will be, thanks to you. And why after all? Because you were told to. " +
            "Did you not once think that the galaxy never wanted your crumbling civilisation to spread further than itself? To spread the same disease that doomed your own planet.";

        hiddenEnding =
            "So here you are again, only not the same as you once were not long ago. " +
            "I admire your efforts to undo what you've done, but that stain shall forever live on you. " +
            "What made you choose your current path? Making ammends? Or just out of curiosity? It makes no difference either way, for you are here now.\r\n\r\n" +
            "Yet, I wonder if you've figuered it out by now. The point of it all... To escape a dying world. " +
            "Still I ask: Did you ever think about why your people had to escape in the first place? How it got so bad? Perhaps you have, perhaps not.\r\n\r\n" +
            "But much like the frog in the boiling water, the wheel of greed slowly grinds away at Earth and will continue to do so, " +
            "unless the wheel is broken or until the Earth is ground to dust, never knowing where it truly went wrong, all the time unaware that the wheel was ever there, until the very last moment.";
    }

    private void DisplayEndingText() {

        // The 'Hidden Ending' is only accessed by triggering the 'High Score Ending' then the 'Low Score Ending'.
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

        // This destroys the 'completed' UI Manager, which has the end game score and the game timer set to 0
        // Since without destroying it, it would persist into the Main Menu, then into the Game scene again, which caused duplication and conflict
        // I've learnt that I probably should've just had those things (score and game timer) in a GameManager instead, then just sent events to the UIManager
        if (FindObjectOfType<UIManager>()) {
            Destroy(FindObjectOfType<UIManager>().gameObject);
        }
    }
}
