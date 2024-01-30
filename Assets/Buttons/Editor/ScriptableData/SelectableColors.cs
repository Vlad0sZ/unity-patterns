using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Buttons.Editor.ScriptableData
{
    [CreateAssetMenu(fileName = "Selectable Colors", menuName = "UI/Selectable/Colors", order = 101)]
    public class SelectableColors : ScriptableObject
    {
        internal static readonly HashSet<SelectableColors> AllColors = new HashSet<SelectableColors>();

        private void OnEnable() =>
            AllColors.Add(this);

        private void OnDisable() =>
            AllColors.Remove(this);


        public ColorBlock colorBlock;


#if UNITY_EDITOR
        internal static void FindColorPresets()
        {
            UnityEditor.AssetDatabase.FindAssets($"t:{typeof(SelectableColors)}");
        }
#endif
    }
}