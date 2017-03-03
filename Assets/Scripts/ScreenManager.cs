using UnityEngine;

public class ScreenManager : MonoBehaviour {

    void Awake() {
        this.SetResolution();
    }

    private void SetResolution() {
#if !UNITY_EDITOR
        Screen.SetResolution(
            (int) Mathf.Round(Screen.currentResolution.height * 9f / 16f),
            Screen.currentResolution.height,
            false
        );
#endif
    }

}
