using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TypingBar : MonoBehaviour {
    public List<Image> Segments = new List<Image>();

    private Animator    _animator;
    private List<Color> _segmentColors;

    private bool _isVisible;

    void Awake() {
        this._segmentColors = new List<Color>();

        foreach (Image segment in this.Segments) {
            this._segmentColors.Add(segment.color);
        }

        this._animator = GetComponent<Animator>();
        this._animator.enabled = false;

        this.SetVisible(false);
    }

    public void SetVisible(bool visibility) {
        for (int i = 0; i < this.Segments.Count; i++) {
            Image segment = this.Segments[i];
            Color initialColour = this._segmentColors[i];

            segment.color = new Color(
                segment.color.r,
                segment.color.g,
                segment.color.b,
                visibility ? initialColour.a : 0
            );
        }

        this._animator.enabled = visibility;

        this._isVisible = visibility;
    }

    public bool IsVisible() {
        return this._isVisible;
    }

}
