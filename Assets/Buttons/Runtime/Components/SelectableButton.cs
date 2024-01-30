using Buttons.Runtime.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Buttons.Runtime.Components
{
    [AddComponentMenu("UI/Selectable/Button", 30)]
    public class SelectableButton : SelectableBase, IPointerClickHandler, ISubmitHandler
    {
        [System.Serializable]
        public class ButtonEvent : UnityEvent
        {
        }


        /// <summary>
        /// Эвент на нажатие кнопки из Unity.
        /// </summary>
        [SerializeField] private ButtonEvent onClick;

        /// <summary>
        /// Эвент на нажатие кнопки из кода.
        ///
        /// <example>
        /// void listener()
        /// {
        ///    Debug.Log("Button Clicked!");
        /// }
        /// 
        /// button.OnClick.AddListener(listener);
        /// button.OnClick.RemoveListener(listener);
        /// </example>
        /// </summary>
        public ButtonEvent OnClick => onClick;

        /// <summary>
        /// Аналог onClick, но UnityAction.
        ///
        /// <example>
        /// void listener()
        /// {
        ///    Debug.Log("Button Clicked!");
        /// }
        /// 
        /// button.OnClickEvent += listener;
        /// button.OnClickEvent -= listener;
        /// </example>
        /// </summary>
        public event UnityAction OnClickEvent
        {
            add => onClick.AddListener(value);

            remove => onClick.RemoveListener(value);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                PressButton(eventData);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            PressButton(eventData);

            if (!IsActive() || !IsInteractable())
                return;

            PressButton(eventData);
            DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(SelectableExtensions.WaitFor(this.GetFadeDuration(), DeselectAfterSubmit));
        }

        protected void PressButton(BaseEventData eventData)
        {
            if (!IsActive() || !IsInteractable())
                return;


            this.Deselect(eventData);
            this.onClick?.Invoke();
        }

        private void DeselectAfterSubmit() =>
            DoStateTransition(currentSelectionState, false);
    }
}