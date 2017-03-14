using UnityEngine;

public class ScreenManager : MonoBehaviour {

    void Awake() {
        this.SetResolution();
    }

    private void SetResolution() {
#if !UNITY_EDITOR

        int newWidth = (int) Mathf.Round(
            Screen.currentResolution.height * 9f / 16f
        );

        // If the window is too tall for the screen.
        if (Screen.currentResolution.width != newWidth) {
            int newHeight = Screen.currentResolution.height * 2 / 3;

            newWidth = (int) Mathf.Round(newHeight * 9f / 16f);

            Screen.SetResolution(newWidth, newHeight, false);
        }

#endif
    }

}
