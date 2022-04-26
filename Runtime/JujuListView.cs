using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Juju8e.UIToolkit.Collections {
  public sealed class JujuListView : VisualElement, IEventHandler {
    public class JujuListViewUxmlFactory : UxmlFactory<JujuListView, JujuListViewUxmlTraits> { }

    public class JujuListViewUxmlTraits : UxmlTraits {
      private UxmlEnumAttributeDescription<ScrollViewMode> _scrollViewModeAttributeDescription =
        new UxmlEnumAttributeDescription<ScrollViewMode> {
          name = "scroll-view-mode",
          defaultValue = ScrollViewMode.Vertical
        };
      private UxmlEnumAttributeDescription<ScrollerVisibility> _verticalScrollerVisibilityAttributeDescription =
        new UxmlEnumAttributeDescription<ScrollerVisibility> {
          name = "vertical-scroller-visibility",
          defaultValue = ScrollerVisibility.Auto
        };
      private UxmlEnumAttributeDescription<ScrollerVisibility> _horizontalScrollerVisibilityAttributeDescription =
        new UxmlEnumAttributeDescription<ScrollerVisibility> {
          name = "horizontal-scroller-visibility",
          defaultValue = ScrollerVisibility.Auto
        };
      private UxmlBoolAttributeDescription _touchControlEnabledAttributeDescription =
        new UxmlBoolAttributeDescription {
          name = "touch-control-enabled",
          defaultValue = false
        };
      private UxmlFloatAttributeDescription _touchControlVelocityAttributeDescription =
        new UxmlFloatAttributeDescription {
          name = "touch-control-velocity",
          defaultValue = 25
        };
      private UxmlFloatAttributeDescription _touchControlDampingAttributeDescription =
        new UxmlFloatAttributeDescription {
          name = "touch-control-damping",
          defaultValue = 1
        };

      public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription {
        get { yield break; }
      }

      public override void Init(VisualElement visualElement, IUxmlAttributes bag, CreationContext context) {
        base.Init(visualElement, bag, context);
        var jujuListView = visualElement as JujuListView;
        jujuListView.ScrollViewMode = _scrollViewModeAttributeDescription.GetValueFromBag(bag, context);
        jujuListView.VerticalScrollerVisibility = _verticalScrollerVisibilityAttributeDescription.GetValueFromBag(bag, context);
        jujuListView.HorizontalScrollerVisibility = _horizontalScrollerVisibilityAttributeDescription.GetValueFromBag(bag, context);
        jujuListView.TouchControlEnabled = _touchControlEnabledAttributeDescription.GetValueFromBag(bag, context);
        jujuListView.TouchControlVelocity = _touchControlVelocityAttributeDescription.GetValueFromBag(bag, context);
        jujuListView.TouchControlDamping = _touchControlDampingAttributeDescription.GetValueFromBag(bag, context);
      }
    }

    public ScrollViewMode ScrollViewMode {
      get => _scrollView.mode;
      set => _scrollView.mode = value;
    }
    public ScrollerVisibility VerticalScrollerVisibility {
      get => _scrollView.verticalScrollerVisibility;
      set => _scrollView.verticalScrollerVisibility = value;
    }
    public ScrollerVisibility HorizontalScrollerVisibility {
      get => _scrollView.horizontalScrollerVisibility;
      set => _scrollView.horizontalScrollerVisibility = value;
    }
    public bool TouchControlEnabled {
      get => _scrollViewTouchControl.IsEnabled();
      set {
        if (value) {
          _scrollViewTouchControl.Enable();
        } else {
          _scrollViewTouchControl.Disable();
        }
      }
    }
    public float TouchControlVelocity {
      get => _scrollViewTouchControl.Velocity;
      set => _scrollViewTouchControl.Velocity = value;
    }
    public float TouchControlDamping {
      get => _scrollViewTouchControl.Damping;
      set => _scrollViewTouchControl.Damping = value;
    }
    public Func<VisualElement> MakeItem { private get; set; }
    public List<object> ItemSource {
      set {
        value.ForEach(item => {
          var itemVisualElement = MakeItem.Invoke();
          _scrollView.Add(itemVisualElement);
        });
      }
    }

    private readonly ScrollView _scrollView;
    private readonly ScrollViewTouchControl _scrollViewTouchControl;

    public JujuListView() {
      _scrollView = new ScrollView();
      _scrollView.style.flexGrow = 1;
      Add(_scrollView);
      _scrollViewTouchControl = new ScrollViewTouchControl(_scrollView);
    }
  }
}
