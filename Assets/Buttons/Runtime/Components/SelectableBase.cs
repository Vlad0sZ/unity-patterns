using System;
using Buttons.Runtime.Sub;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Buttons.Runtime.Components
{
    /// <summary>
    /// Объект, имеющий состояния.
    /// </summary>
    public abstract class SelectableBase : Selectable
    {
        /// <summary>
        /// Если true, тогда sprite в Normal будет иметь альфу 0.
        /// </summary>
        [SerializeField] private bool transparentNormal;

        /// <summary>
        /// Имеет ли дополнительные объекты, меняющие состояние.
        /// </summary>
        [SerializeField] private bool hasSubSelectable;

        /// <summary>
        /// Список дополнительных объектов.
        /// </summary>
        [SerializeField] private SubSelectable[] subSelectables;

        private RectTransform _rectTransform;

        /// <summary>
        /// RectTransform объекта.
        /// </summary>
        public RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                    _rectTransform = this.GetComponent<RectTransform>();

                return _rectTransform;
            }
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            // base Selectable
            base.DoStateTransition(state, instant);

            // Transparent Normal
            DoTransparentNormal(state, instant);

            // SubSelectable states
            DoSubSelectableTransition(state, instant);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if (currentSelectionState == SelectionState.Selected)
                OnDeselect(eventData);
        }

        private void DoTransparentNormal(SelectionState state, bool instant)
        {
            if (transition != Transition.SpriteSwap || !transparentNormal)
                return;

            if (!image)
                return;

            float alpha = state == SelectionState.Normal ? 0f : 1f;
            float duration = instant ? 0f : colors.fadeDuration;
            image.CrossFadeAlpha(alpha, duration, true);
        }


        private void DoSubSelectableTransition(SelectionState currentState, bool instant)
        {
            if (!hasSubSelectable || subSelectables == null)
                return;

            foreach (var selectable in subSelectables)
                SubTransition(selectable, currentState, instant);
        }

        protected static void SubTransition(SubSelectable sub, SelectionState state, bool instant)
        {
            if (sub == null)
                return;

            switch(state)
            {
            case SelectionState.Normal:
                sub.DoNormal(instant);
                break;
            case SelectionState.Highlighted:
                sub.DoHighlighted(instant);
                break;
            case SelectionState.Pressed:
                sub.DoPressed(instant);
                break;
            case SelectionState.Selected:
                sub.DoSelected(instant);
                break;
            case SelectionState.Disabled:
                sub.DoDisabled(instant);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}