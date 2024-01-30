using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Buttons.Runtime.Components
{
    [AddComponentMenu("UI/Selectable/Toggle Group", 31)]
    [DisallowMultipleComponent]
    public class SelectableToggleGroup : UIBehaviour
    {
        [System.Serializable]
        public class ToggleGroupEvent : UnityEvent<SelectableToggleGroup, SelectableToggle>
        {
        }

        [System.Serializable]
        public class ToggleGroupIndexEvent : UnityEvent<int>
        {
        }

        [SerializeField] private bool allowSwitchOff = false;

        [SerializeField] private int activeIndex;

        [SerializeField] protected List<SelectableToggle> toggles = new List<SelectableToggle>();

        [SerializeField] private ToggleGroupEvent onToggleGroupChanged;

        [SerializeField] private ToggleGroupIndexEvent onSelectedIndexChanged;


        public bool AllowSwitchOff
        {
            get => allowSwitchOff;
            set => allowSwitchOff = value;
        }

        public int ActiveIndex
        {
            get => activeIndex;
            set => SetActiveIndex(value);
        }

        public ToggleGroupEvent OnToggleGroupChanged => onToggleGroupChanged;

        public ToggleGroupIndexEvent OnIndexChanged => onSelectedIndexChanged;


        public event UnityAction<SelectableToggleGroup, SelectableToggle> OnToggleGroupChangedEvent
        {
            add => onToggleGroupChanged.AddListener(value);

            remove => onToggleGroupChanged.RemoveListener(value);
        }

        public event UnityAction<int> OnSelectedIndexChangedEvent
        {
            add => onSelectedIndexChanged.AddListener(value);

            remove => onSelectedIndexChanged.RemoveListener(value);
        }


        public bool IsAnyToggleOn() => toggles.Any(t => t.IsOn);

        internal void ValidateState()
        {
            if (!allowSwitchOff && !IsAnyToggleOn() && toggles.Count != 0)
            {
                toggles[0].IsOn = true;
                NotifyToggleIsOn(toggles[0]);
            }

            var selectableToggles = toggles.Where(t => t.IsOn).ToArray();

            if (selectableToggles.Length > 1)
            {
                var firstActive = selectableToggles.FirstOrDefault();
                foreach (SelectableToggle activeToggle in selectableToggles)
                {
                    if (activeToggle == firstActive)
                        continue;

                    activeToggle.IsOn = false;
                }
            }
        }

        internal void NotifyToggleIsOn(SelectableToggle toggle, bool sendCallback = true)
        {
            if (toggle == null || !IsContains(toggle))
                return;

            foreach (SelectableToggle selectableToggle in toggles)
            {
                if (selectableToggle == toggle)
                    continue;

                if (sendCallback)
                    selectableToggle.IsOn = false;
                else
                    selectableToggle.SetValueWithoutNotify(false);
            }

            SetActiveIndex(toggle);
            onToggleGroupChanged?.Invoke(this, toggle);
        }

        internal void NotifyToggleIsOff(SelectableToggle toggle)
        {
            var activeToggle = toggles.FirstOrDefault(t => t.IsOn);
            if (activeToggle != null)
                return;

            if (!allowSwitchOff) toggle.IsOn = true;
            else
            {
                SetActiveIndex(null);
                onToggleGroupChanged?.Invoke(this, null);
            }
        }

        internal void RegisterToggle(SelectableToggle toggle)
        {
            if (!IsContains(toggle))
                toggles.Add(toggle);
        }

        internal void UnregisterToggle(SelectableToggle toggle)
        {
            if (IsContains(toggle))
                toggles.Remove(toggle);
        }


        private bool IsContains(SelectableToggle toggle) =>
            toggles.Contains(toggle);

        private void SetActiveIndex(int value)
        {
            if (activeIndex == value)
                return;

            if (value >= toggles.Count)
                return;

            if (value < 0)
            {
                if (!allowSwitchOff)
                    return;

                var activeToggles = toggles.Where(t => t.IsOn);
                foreach (SelectableToggle selectableToggle in activeToggles)
                    selectableToggle.IsOn = false;
            }
            else
            {
                var activeToggle = toggles.ElementAtOrDefault(value);
                if (activeToggle)
                {
                    activeToggle.IsOn = true;
                }
            }
        }

        private void SetActiveIndex(SelectableToggle toggle)
        {
            activeIndex = toggle == null ? -1 : toggles.IndexOf(toggle);
            onSelectedIndexChanged?.Invoke(activeIndex);
        }
    }
}