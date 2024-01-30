using UnityEngine;
using UnityEngine.UI;

namespace Buttons.Runtime.Sub
{
    public class SubSpriteSelectable : SubSelectable
    {
        [SerializeField] private SpriteState spriteBlock;

        private Image Image => targetGraphic as Image;

        public override void DoNormal(bool instant) =>
            SpriteSwap(null);

        public override void DoHighlighted(bool instant) =>
            SpriteSwap(spriteBlock.highlightedSprite);

        public override void DoPressed(bool instant) =>
            SpriteSwap(spriteBlock.pressedSprite);

        public override void DoSelected(bool instant) =>
            SpriteSwap(spriteBlock.selectedSprite);

        public override void DoDisabled(bool instant) =>
            SpriteSwap(spriteBlock.disabledSprite);

        private void SpriteSwap(Sprite sprite)
        {
            if (Image)
                Image.overrideSprite = sprite;
        }
    }
}