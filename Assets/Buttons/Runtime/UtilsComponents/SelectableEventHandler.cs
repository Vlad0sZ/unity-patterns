using Buttons.Runtime.Components;
using UnityEngine;
using UnityEngine.Events;

namespace Buttons.Runtime.UtilsComponents
{
    internal sealed class SelectableEventHandler : MonoBehaviour
    {
        [System.Serializable]
        public class ToggleEvent : UnityEvent<SelectableToggle, bool>
        {
        }

        [SerializeField] private SelectableToggle toggle;

        [SerializeField] private ToggleEvent onToggleClick = new ToggleEvent();

        public ToggleEvent OnToggleClick => onToggleClick;

        public event UnityAction<SelectableToggle, bool> OnToggleClickEvent
        {
            add => onToggleClick.AddListener(value);

            remove => onToggleClick.RemoveListener(value);
        }


        private void OnValidate()
        {
            if (toggle == null)
                toggle = gameObject.GetComponent<SelectableToggle>();
        }

        private void OnEnable()
        {
            if (toggle)
                toggle.OnValueChanged.AddListener(OnClick);
        }

        private void OnDisable()
        {
            if (toggle)
                toggle.OnValueChanged.RemoveListener(OnClick);
        }

        private void OnClick(bool value) =>
            onToggleClick?.Invoke(toggle, value);
    }
}