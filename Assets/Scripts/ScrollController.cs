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

            Canvas.ForceUpdateCanvases();

            this._rect.verticalNormalizedPosition = newPosition;

            float delta = newPosition - this._targetPosition.Value;

            if (Mathf.Abs(delta) < 1e-3f) {
                this._targetPosition = null;
            }
        }
    }

    public void ScrollToBottom() {
        this._targetPosition = 0;
    }

    public void StopScrolling(float newPosition) {
        float currentPosition = Mathf.Max(
            this._rect.verticalNormalizedPosition,
            0
        );

        float delta = newPosition - currentPosition;

        // delta is non-zero if the current position of the scrollbar doesn't
        // match the position of the ScrollRect.
        if (Mathf.Abs(delta) > 1e-3f) {
            this._targetPosition = null;
        }
    }

}
