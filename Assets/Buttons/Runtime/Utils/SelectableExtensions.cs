using System;
using System.Collections;
using Buttons.Runtime.Components;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Buttons.Runtime.Utils
{
    public static class SelectableExtensions
    {
        public static void Deselect(this Selectable selectable, BaseEventData eventData)
        {
            if (!EventSystem.current) return;

            EventSystem.current.SetSelectedGameObject(null);
            selectable.OnDeselect(eventData);
        }

        public static float GetFadeDuration(this Selectable selectable)
        {
            return Mathf.Max(selectable.colors.fadeDuration, 0.1f);
        }

        public static IEnumerator WaitFor(float time, Action callback = null)
        {
            yield return new WaitForSecondsRealtime(time);
            callback?.Invoke();
        }

        public static void AddListener(SelectableButton button)
        {
            void listener()
            {
                Debug.Log("Button Clicked!");
            }
            
            button.OnClick.AddListener(listener);
            button.OnClickEvent += listener;
        }
    }
}