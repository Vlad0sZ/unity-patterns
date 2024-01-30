using Buttons.Runtime.Sub;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Buttons.Runtime.Components
{
    [AddComponentMenu("UI/Selectable/Toggle", 30)]
    public class SelectableToggle : SelectableBase, IPointerClickHandler, ISubmitHandler
    {
        [System.Serializable]
        public class ToggleEvent : UnityEvent<bool>
        {
        }

        /// <summary>
        /// Список доступных состояний объекта, при нажатом состоянии
        /// </summary>
        public enum ToggleState
        {
            /// <summary>
            /// Когда Toggle имеет состояние IsOn, то он также имеет состояние Pressed
            /// </summary>
            IsOnPressedState,

            /// <summary>
            /// Когда Toggle имеет состояние IsOn, то он имеет состояние Normal
            /// </summary>
            IsOnNormalState
        }


        /// <summary>
        /// Стейт тогла при нажатом состоянии.
        /// <see cref="ToggleState"/>
        /// </summary>
        [SerializeField] private ToggleState toggleState;

        /// <summary>
        /// Дополнительный SubSelection, у которого будет состояние Pressed, если IsOn равен true.
        ///
        /// Может быть null.
        /// </summary>
        [CanBeNull] [SerializeField] private SubSelectable checkmark;

        [SerializeField] private SelectableToggleGroup toggleGroup;

        /// <summary>
        /// Эвент для смены Toggle из Unity
        /// </summary>
        [SerializeField] private ToggleEvent onValueChanged;

        public ToggleEvent OnValueChanged => onValueChanged;


        public event UnityAction<bool> OnToggleEvent
        {
            add => onValueChanged.AddListener(value);

            remove => onValueChanged.RemoveListener(value);
        }

        public bool IsOn
        {
            get => _isOn;
            set => SetValue(value);
        }

        public ToggleState ToggleIsOnState
        {
            get => toggleState;

            set
            {
                if (Equals(toggleState, value))
                    return;

                toggleState = value;
                DoStateTransition(currentSelectionState, true);
            }
        }

        public SubSelectable Checkmark
        {
            get => checkmark;

            set
            {
                if (checkmark == value)
                    return;

                checkmark = value;
                DoStateTransition(currentSelectionState, true);
            }
        }


        public SelectableToggleGroup ToggleGroup
        {
            get => toggleGroup;
            set { }
        }

        private bool _isOn;


        protected override void OnEnable()
        {
            base.OnEnable();
            SetToggleGroup(toggleGroup, false);
        }

        protected override void OnDisable()
        {
            SetToggleGroup(null, false);
            base.OnDisable();
        }

        protected override void OnDestroy()
        {
            if (toggleGroup != null)
                toggleGroup.ValidateState();

            base.OnDestroy();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            PressToggle();
        }

        public void OnSubmit(BaseEventData eventData) =>
            PressToggle();

        public void SetValueWithoutNotify(bool value) => SetValue(value, false);

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (state == SelectionState.Normal && toggleState == ToggleState.IsOnPressedState)
                state = IsOn ? SelectionState.Pressed : SelectionState.Normal;

            base.DoStateTransition(state, instant);

            SelectionState stateForCheckmark = IsOn ? SelectionState.Pressed : state;
            SubTransition(checkmark, stateForCheckmark, instant);
        }

        protected void SetValue(bool value, bool sendCallback = true)
        {
            if (_isOn == value)
                return;

            // if we are in a group and set to true, do group logic
            _isOn = value;
            if (toggleGroup != null && toggleGroup.isActiveAndEnabled && IsActive())
            {
                if (_isOn || (!toggleGroup.IsAnyToggleOn() && !toggleGroup.AllowSwitchOff))
                {
                    _isOn = true;
                    toggleGroup.NotifyToggleIsOn(this, sendCallback);
                }
            }

            if (sendCallback)
            {
                UISystemProfilerApi.AddMarker("Toggle.value", this);
                onValueChanged.Invoke(_isOn);
            }
            
            DoStateTransition(currentSelectionState, false);
        }

        private void SetToggleGroup(SelectableToggleGroup newToggleGroup, bool setMemberValue)
        {
            if (toggleGroup != null)
                toggleGroup.UnregisterToggle(this);

            if (setMemberValue)
                toggleGroup = newToggleGroup;

            if (newToggleGroup == null || !IsActive())
                return;

            newToggleGroup.RegisterToggle(this);

            if (_isOn)
                newToggleGroup.NotifyToggleIsOn(this);
        }

        private void PressToggle()
        {
            if (!IsActive() || !IsInteractable())
                return;

            IsOn = !IsOn;
        }
    }
}