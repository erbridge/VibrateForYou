using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(ScrollRect))]
public class ScrollController : MonoBehaviour {
    private ScrollRect _rect;
    private float?     _targetPosition;

    void Start() {
        this._rect = GetComponent<ScrollRect>();
    }

    void Update() {
        if (this._targetPosition != null) {
            float newPosition = Mathf.Lerp(
                this._rect.verticalNormalizedPosition,
                this._targetPosition.Value,
                0.5f
            );

            this._rect.verticalNormalizedPosition = newPosition;
        }
    }

    public void ScrollToBottom() {
        this._targetPosition = 0;
    }

    public void StopScrolling(float position) {
        if (this._rect.verticalNormalizedPosition != position) {
            this._targetPosition = null;
        }
    }

}
