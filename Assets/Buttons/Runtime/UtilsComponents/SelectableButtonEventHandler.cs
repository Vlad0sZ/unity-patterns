using System;
using Buttons.Runtime.Components;
using UnityEngine;
using UnityEngine.Events;

namespace Buttons.Runtime.UtilsComponents
{
    internal sealed class SelectableButtonEventHandler : MonoBehaviour
    {
        [System.Serializable]
        public class ButtonEvent : UnityEvent<SelectableButton>
        {
        }

        [SerializeField] private SelectableButton button;

        [SerializeField] private ButtonEvent onButtonClick = new ButtonEvent();

        public ButtonEvent OnButtonClick => onButtonClick;

        public event UnityAction<SelectableButton> OnButtonClickEvent
        {
            add => onButtonClick.AddListener(value);

            remove => onButtonClick.RemoveListener(value);
        }

        private void OnValidate()
        {
            if (button == null)
                button = gameObject.GetComponent<SelectableButton>();
        }

        private void OnEnable()
        {
            if (button)
                button.OnClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            if (button)
                button.OnClick.RemoveListener(OnClick);
        }

        private void OnClick() =>
            onButtonClick?.Invoke(button);
    }
}