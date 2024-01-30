using UnityEngine;

namespace Buttons.Runtime.Sub
{
    public class SubSelectableMultiplier : SubSelectable
    {
        [SerializeField] protected SubSelectable[] subSelectableAtThisGameObject;

        public override void DoNormal(bool instant)
        {
            foreach (SubSelectable subSelectable in subSelectableAtThisGameObject)
                subSelectable.DoNormal(instant);
        }

        public override void DoHighlighted(bool instant)
        {
            foreach (SubSelectable subSelectable in subSelectableAtThisGameObject)
                subSelectable.DoHighlighted(instant);
        }

        public override void DoPressed(bool instant)
        {
            foreach (SubSelectable subSelectable in subSelectableAtThisGameObject)
                subSelectable.DoPressed(instant);
        }

        public override void DoSelected(bool instant)
        {
            foreach (SubSelectable subSelectable in subSelectableAtThisGameObject)
                subSelectable.DoSelected(instant);
        }

        public override void DoDisabled(bool instant)
        {
            foreach (SubSelectable subSelectable in subSelectableAtThisGameObject)
                subSelectable.DoDisabled(instant);
        }
    }
}