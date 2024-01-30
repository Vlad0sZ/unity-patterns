using UnityEngine;
using UnityEngine.UI;

namespace Buttons.Runtime.Sub
{
    public abstract class SubSelectable : MonoBehaviour
    {
        [SerializeField] protected Graphic targetGraphic;

        public abstract void DoNormal(bool instant);

        public abstract void DoHighlighted(bool instant);

        public abstract void DoPressed(bool instant);

        public abstract void DoSelected(bool instant);

        public abstract void DoDisabled(bool instant);
    }
}