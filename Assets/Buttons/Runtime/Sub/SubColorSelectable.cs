using UnityEngine;
using UnityEngine.UI;

namespace Buttons.Runtime.Sub
{
    public class SubColorSelectable : SubSelectable
    {
        [SerializeField] protected ColorBlock colorBlock;

        public override void DoNormal(bool instant) =>
            ChangeColorState(colorBlock.normalColor, instant);

        public override void DoHighlighted(bool instant) =>
            ChangeColorState(colorBlock.highlightedColor, instant);

        public override void DoPressed(bool instant) =>
            ChangeColorState(colorBlock.pressedColor, instant);

        public override void DoSelected(bool instant) =>
            ChangeColorState(colorBlock.selectedColor, instant);

        public override void DoDisabled(bool instant) =>
            ChangeColorState(colorBlock.disabledColor, instant);

        private void ChangeColorState(Color color, bool instant)
        {
            float duration = instant ? 0f : colorBlock.fadeDuration;
            targetGraphic.CrossFadeColor(color * colorBlock.colorMultiplier, duration, true, true);
        }
    }
}