using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace Juju8e.UIToolkit.Collections {
  public sealed class ScrollViewTouchControl {
    public float Velocity { get; set; } = 25f;
    public float Damping { get; set; } = 1f;

    private readonly ScrollView _scrollView;
    private bool _enabled;
    private bool _pointerDown;
    private Vector2 _pointerDeltaPosition;
    private ValueAnimation<Vector2> _scrollOffsetAnimation;

    public ScrollViewTouchControl(ScrollView scrollView) {
      _scrollView = scrollView;
    }

    public void Enable() {
      if (_enabled) {
        return;
      }
      _scrollView.contentViewport.RegisterCallback<PointerDownEvent>(OnPointerDown);
      _scrollView.contentViewport.RegisterCallback<PointerMoveEvent>(OnPointerMove);
      _scrollView.contentViewport.RegisterCallback<PointerUpEvent>(OnPointerUp);
      _scrollView.contentViewport.RegisterCallback<PointerLeaveEvent>(OnPointerLeave);
      _enabled = true;
    }

    public void Disable() {
      if (!_enabled) {
        return;
      }
      _scrollView.contentViewport.UnregisterCallback<PointerDownEvent>(OnPointerDown);
      _scrollView.contentViewport.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
      _scrollView.contentViewport.UnregisterCallback<PointerUpEvent>(OnPointerUp);
      _scrollView.contentViewport.UnregisterCallback<PointerLeaveEvent>(OnPointerLeave);
      _enabled = false;
    }

    public bool IsEnabled() {
      return _enabled;
    }

    private void OnPointerDown(PointerDownEvent pointerDownEvent) {
      _pointerDeltaPosition = Vector2.zero;
      _pointerDown = true;
    }

    private void OnPointerMove(PointerMoveEvent pointerMoveEvent) {
      if (!_pointerDown) {
        return;
      }
      if (_scrollOffsetAnimation != null) {
        _scrollOffsetAnimation.Stop();
      }
      _pointerDeltaPosition = pointerMoveEvent.deltaPosition;
      _scrollView.scrollOffset -= _pointerDeltaPosition;
    }

    private void OnPointerUp(PointerUpEvent pointerUpEvent) {
      if (!_pointerDown) {
        return;
      }
      Scroll(_scrollView.scrollOffset - Velocity * _pointerDeltaPosition, Mathf.RoundToInt(1000 * Damping));
      _pointerDown = false;
    }

    private void OnPointerLeave(PointerLeaveEvent pointerLeaveEvent) {
      if (!_pointerDown) {
        return;
      }
      Scroll(_scrollView.scrollOffset - Velocity * _pointerDeltaPosition, Mathf.RoundToInt(1000 * Damping));
      _pointerDown = false;
    }

    private void Scroll(Vector2 to, int durationMs) {
      if (_scrollOffsetAnimation != null) {
        _scrollOffsetAnimation.Stop();
      }
      _scrollOffsetAnimation = _scrollView.experimental.animation
        .Start(_scrollView.scrollOffset, to, durationMs, (_, value) => _scrollView.scrollOffset = value)
        .KeepAlive();
    }
  }
}
