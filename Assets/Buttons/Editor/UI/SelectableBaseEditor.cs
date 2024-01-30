using System.Collections.Generic;
using System.Linq;
using Buttons.Editor.ScriptableData;
using Buttons.Runtime.Components;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.UI;

namespace Buttons.Editor.UI
{
    [CustomEditor(typeof(SelectableBase), true)]
    public class SelectableBaseEditor : UnityEditor.Editor
    {
        private const int MaxLabelWidth = 80;

        // Selectable Base Editor (Custom)
        private SerializedProperty transparentNormalProperty;
        private SerializedProperty hasSubSelectableProperty;
        private SerializedProperty subSelectableArrayProperty;


        // Selectable Editor (by Unity)
        private SerializedProperty m_Script;
        private SerializedProperty m_InteractableProperty;
        private SerializedProperty m_TargetGraphicProperty;
        private SerializedProperty m_TransitionProperty;
        private SerializedProperty m_ColorBlockProperty;
        private SerializedProperty m_SpriteStateProperty;
        private SerializedProperty m_AnimTriggerProperty;
        private SerializedProperty m_NavigationProperty;

        private readonly GUIContent m_VisualizeNavigation =
            EditorGUIUtility.TrTextContent("Visualize", "Show navigation flows between selectable UI elements.");

        private readonly GUIContent guiContent =
            EditorGUIUtility.TrTextContent("Color Presets");


        private readonly AnimBool m_ShowColorTint = new AnimBool();
        private readonly AnimBool m_ShowSpriteTrasition = new AnimBool();
        private readonly AnimBool m_ShowAnimTransition = new AnimBool();

        private readonly AnimBool showSubSelectable = new AnimBool();
        private readonly AnimBool showColorBlocks = new AnimBool();

        private static List<SelectableBaseEditor> s_Editors = new List<SelectableBaseEditor>();

        private static bool s_ShowNavigation = false;
        private static string s_ShowNavigationKey = "SelectableEditor.ShowNavigation";

        // Whenever adding new SerializedProperties to the Selectable and SelectableEditor
        // Also update this guy in OnEnable. This makes the inherited classes from Selectable not require a CustomEditor.
        private string[] m_PropertyPathToExcludeForChildClasses;

        private static bool isColorBlockOpened;
        private Vector2 _clickedMousePosition;


        protected virtual void OnEnable()
        {
            transparentNormalProperty = serializedObject.FindProperty("transparentNormal");
            hasSubSelectableProperty = serializedObject.FindProperty("hasSubSelectable");
            subSelectableArrayProperty = serializedObject.FindProperty("subSelectables");

            m_Script = serializedObject.FindProperty("m_Script");
            m_InteractableProperty = serializedObject.FindProperty("m_Interactable");
            m_TargetGraphicProperty = serializedObject.FindProperty("m_TargetGraphic");
            m_TransitionProperty = serializedObject.FindProperty("m_Transition");
            m_ColorBlockProperty = serializedObject.FindProperty("m_Colors");
            m_SpriteStateProperty = serializedObject.FindProperty("m_SpriteState");
            m_AnimTriggerProperty = serializedObject.FindProperty("m_AnimationTriggers");
            m_NavigationProperty = serializedObject.FindProperty("m_Navigation");

            m_PropertyPathToExcludeForChildClasses = new[]
            {
                m_Script.propertyPath,
                m_NavigationProperty.propertyPath,
                m_TransitionProperty.propertyPath,
                m_ColorBlockProperty.propertyPath,
                m_SpriteStateProperty.propertyPath,
                m_AnimTriggerProperty.propertyPath,
                m_InteractableProperty.propertyPath,
                m_TargetGraphicProperty.propertyPath,

                // added for selectable base
                transparentNormalProperty.propertyPath,
                hasSubSelectableProperty.propertyPath,
                subSelectableArrayProperty.propertyPath,
            };


            var trans = GetTransition(m_TransitionProperty);
            m_ShowColorTint.value = (trans == Selectable.Transition.ColorTint);
            m_ShowSpriteTrasition.value = (trans == Selectable.Transition.SpriteSwap);
            m_ShowAnimTransition.value = (trans == Selectable.Transition.Animation);
            showSubSelectable.value = (hasSubSelectableProperty.boolValue);
            showColorBlocks.value = isColorBlockOpened;


            showSubSelectable.valueChanged.AddListener(Repaint);
            showColorBlocks.valueChanged.AddListener(Repaint);
            m_ShowColorTint.valueChanged.AddListener(Repaint);
            m_ShowSpriteTrasition.valueChanged.AddListener(Repaint);


            s_Editors.Add(this);
            RegisterStaticOnSceneGUI();

            s_ShowNavigation = EditorPrefs.GetBool(s_ShowNavigationKey);
        }

        protected virtual void OnDisable()
        {
            showSubSelectable.valueChanged.RemoveListener(Repaint);
            showColorBlocks.valueChanged.RemoveListener(Repaint);
            m_ShowColorTint.valueChanged.RemoveListener(Repaint);
            m_ShowSpriteTrasition.valueChanged.RemoveListener(Repaint);

            s_Editors.Remove(this);
            RegisterStaticOnSceneGUI();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_InteractableProperty);

            var selectableTarget = target as Selectable;
            if (selectableTarget == null)
                return;


            var trans = GetTransition(m_TransitionProperty);

            var graphic = m_TargetGraphicProperty.objectReferenceValue as Graphic;
            if (graphic == null)
                graphic = selectableTarget.GetComponent<Graphic>();

            var animator = selectableTarget.GetComponent<Animator>();
            m_ShowColorTint.target = (!m_TransitionProperty.hasMultipleDifferentValues &&
                                      trans == Selectable.Transition.ColorTint);
            m_ShowSpriteTrasition.target = (!m_TransitionProperty.hasMultipleDifferentValues &&
                                            trans == Selectable.Transition.SpriteSwap);
            m_ShowAnimTransition.target = (!m_TransitionProperty.hasMultipleDifferentValues &&
                                           trans == Selectable.Transition.Animation);

            showSubSelectable.target = (!m_TransitionProperty.hasMultipleDifferentValues &&
                                        hasSubSelectableProperty.boolValue);

            EditorGUILayout.PropertyField(m_TransitionProperty);

            ++EditorGUI.indentLevel;
            {
                if (trans == Selectable.Transition.ColorTint || trans == Selectable.Transition.SpriteSwap)
                {
                    EditorGUILayout.PropertyField(m_TargetGraphicProperty);
                }

                switch(trans)
                {
                case Selectable.Transition.ColorTint:
                    if (graphic == null)
                        EditorGUILayout.HelpBox("You must have a Graphic target in order to use a color transition.",
                            MessageType.Warning);
                    break;

                case Selectable.Transition.SpriteSwap:
                    if (graphic as Image == null)
                        EditorGUILayout.HelpBox(
                            "You must have a Image target in order to use a sprite swap transition.",
                            MessageType.Warning);
                    break;
                }

                if (EditorGUILayout.BeginFadeGroup(m_ShowColorTint.faded))
                {
                    DrawColorsPresets();
                    EditorGUILayout.PropertyField(m_ColorBlockProperty);
                }

                EditorGUILayout.EndFadeGroup();

                if (EditorGUILayout.BeginFadeGroup(m_ShowSpriteTrasition.faded))
                {
                    EditorGUILayout.PropertyField(m_SpriteStateProperty);
                    EditorGUILayout.PropertyField(transparentNormalProperty);
                }

                EditorGUILayout.EndFadeGroup();

                if (EditorGUILayout.BeginFadeGroup(m_ShowAnimTransition.faded))
                {
                    EditorGUILayout.PropertyField(m_AnimTriggerProperty);

                    if (animator == null || animator.runtimeAnimatorController == null)
                    {
                        Rect buttonRect = EditorGUILayout.GetControlRect();
                        buttonRect.xMin += EditorGUIUtility.labelWidth;
                        if (GUI.Button(buttonRect, "Auto Generate Animation", EditorStyles.miniButton))
                        {
                            var controller = GenerateSelectableAnimatorController(selectableTarget.animationTriggers,
                                selectableTarget);
                            if (controller != null)
                            {
                                if (animator == null)
                                    animator = selectableTarget.gameObject.AddComponent<Animator>();
                                UnityEditor.Animations.AnimatorController.SetAnimatorController(animator, controller);
                            }
                        }
                    }
                }

                EditorGUILayout.EndFadeGroup();
            }
            --EditorGUI.indentLevel;

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(m_NavigationProperty);

            EditorGUI.BeginChangeCheck();
            Rect toggleRect = EditorGUILayout.GetControlRect();
            toggleRect.xMin += EditorGUIUtility.labelWidth;
            s_ShowNavigation = GUI.Toggle(toggleRect, s_ShowNavigation, m_VisualizeNavigation, EditorStyles.miniButton);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(s_ShowNavigationKey, s_ShowNavigation);
                SceneView.RepaintAll();
            }


            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(hasSubSelectableProperty);

            EditorGUI.indentLevel++;
            if (EditorGUILayout.BeginFadeGroup(showSubSelectable.faded))
            {
                EditorGUILayout.PropertyField(subSelectableArrayProperty, true);
            }

            EditorGUILayout.EndFadeGroup();
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            ChildClassPropertiesGUI();

            serializedObject.ApplyModifiedProperties();
        }


        private static Selectable.Transition GetTransition(SerializedProperty property) =>
            (Selectable.Transition)property.enumValueIndex;

        private void ChildClassPropertiesGUI()
        {
            if (IsDerivedSelectableEditor())
                return;

            DrawPropertiesExcluding(serializedObject, m_PropertyPathToExcludeForChildClasses);
        }

        private bool IsDerivedSelectableEditor() =>
            GetType() != typeof(SelectableBaseEditor);

        private void DrawColorsPresets()
        {
            isColorBlockOpened =
                EditorGUILayout.Foldout(isColorBlockOpened, guiContent);

            if (!isColorBlockOpened)
                return;

            var allColors = SelectableColors.AllColors;

            if (allColors.Count == 0) 
                SelectableColors.FindColorPresets();
            
            foreach (SelectableColors allColor in allColors)
                DrawColorPreset(allColor);
        }

        private void DrawColorPreset(SelectableColors color)
        {
            Event e = Event.current;

            EditorGUILayout.BeginHorizontal();
            var content = new GUIContent(color.name);
            var size = EditorStyles.miniLabel.CalcSize(content);
            var widthSize = Mathf.Max(size.x, MaxLabelWidth);

            var propertyRect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);
            propertyRect.x += EditorGUIUtility.labelWidth;
            propertyRect.width = propertyRect.height * 4 + widthSize;


            var rect = new RectOffset(-4, -4, -2, -2).Add(propertyRect);
            GUI.Box(propertyRect, "", EditorStyles.miniButton);

            GUI.Label(rect, content, EditorStyles.miniLabel);
            rect.x += widthSize;
            rect.width = rect.height;

            int id = GUIUtility.GetControlID(FocusType.Passive, rect);
            EditorGUI.DrawRect(rect, color.colorBlock.normalColor);

            rect.x += rect.width;
            EditorGUI.DrawRect(rect, color.colorBlock.highlightedColor);

            rect.x += rect.width;
            EditorGUI.DrawRect(rect, color.colorBlock.pressedColor);

            rect.x += rect.width;
            EditorGUI.DrawRect(rect, color.colorBlock.disabledColor);

            EditorGUILayout.EndHorizontal();

            if (DoControl(propertyRect, e, id))
                ChangeColors(color);
        }

        private void ChangeColors(SelectableColors color)
        {
            serializedObject.Update();

            var colorBlock = color.colorBlock;

            m_ColorBlockProperty.FindPropertyRelative("m_NormalColor").colorValue =
                colorBlock.normalColor;

            m_ColorBlockProperty.FindPropertyRelative("m_HighlightedColor").colorValue =
                colorBlock.highlightedColor;

            m_ColorBlockProperty.FindPropertyRelative("m_PressedColor").colorValue =
                colorBlock.pressedColor;

            m_ColorBlockProperty.FindPropertyRelative("m_SelectedColor").colorValue =
                colorBlock.disabledColor;

            m_ColorBlockProperty.FindPropertyRelative("m_DisabledColor").colorValue =
                colorBlock.selectedColor;


            serializedObject.ApplyModifiedProperties();
        }

        private bool DoControl(Rect propertyRect, Event e, int id)
        {
            if (!propertyRect.Contains(e.mousePosition))
                return false;


            switch(e.GetTypeForControl(id))
            {
            case EventType.MouseDown:
                _clickedMousePosition = e.mousePosition;
                GUIUtility.hotControl = id;
                e.Use();
                return false;

            case EventType.MouseUp:
                GUIUtility.hotControl = 0;
                e.Use();
                return (Vector2.Distance(e.mousePosition, _clickedMousePosition) == 0f);
            default:
                return false;
            }
        }

        private static void RegisterStaticOnSceneGUI()
        {
            SceneView.duringSceneGui -= StaticOnSceneGUI;
            if (s_Editors.Count > 0)
                SceneView.duringSceneGui += StaticOnSceneGUI;
        }

        private static void StaticOnSceneGUI(SceneView view)
        {
            if (!s_ShowNavigation)
                return;

            Selectable[] selectables = Selectable.allSelectablesArray;

            for (int i = 0; i < selectables.Length; i++)
            {
                Selectable s = selectables[i];

                if (UnityEditor.SceneManagement.StageUtility.IsGameObjectRenderedByCamera(s.gameObject, Camera.current))
                    DrawNavigationForSelectable(s);
            }
        }

        private static void DrawNavigationForSelectable(Selectable sel)
        {
            if (sel == null)
                return;

            Transform transform = sel.transform;
            bool active = Selection.transforms.Any(e => e == transform);

            Handles.color = new Color(1.0f, 0.6f, 0.2f, active ? 1.0f : 0.4f);
            DrawNavigationArrow(-Vector2.right, sel, sel.FindSelectableOnLeft());
            DrawNavigationArrow(Vector2.up, sel, sel.FindSelectableOnUp());

            Handles.color = new Color(1.0f, 0.9f, 0.1f, active ? 1.0f : 0.4f);
            DrawNavigationArrow(Vector2.right, sel, sel.FindSelectableOnRight());
            DrawNavigationArrow(-Vector2.up, sel, sel.FindSelectableOnDown());
        }

        const float kArrowThickness = 2.5f;
        const float kArrowHeadSize = 1.2f;

        private static void DrawNavigationArrow(Vector2 direction, Selectable fromObj, Selectable toObj)
        {
            if (fromObj == null || toObj == null)
                return;
            Transform fromTransform = fromObj.transform;
            Transform toTransform = toObj.transform;

            Vector2 sideDir = new Vector2(direction.y, -direction.x);
            Vector3 fromPoint =
                fromTransform.TransformPoint(GetPointOnRectEdge(fromTransform as RectTransform, direction));
            Vector3 toPoint = toTransform.TransformPoint(GetPointOnRectEdge(toTransform as RectTransform, -direction));
            float fromSize = HandleUtility.GetHandleSize(fromPoint) * 0.05f;
            float toSize = HandleUtility.GetHandleSize(toPoint) * 0.05f;
            fromPoint += fromTransform.TransformDirection(sideDir) * fromSize;
            toPoint += toTransform.TransformDirection(sideDir) * toSize;
            float length = Vector3.Distance(fromPoint, toPoint);
            Vector3 fromTangent = fromTransform.rotation * direction * length * 0.3f;
            Vector3 toTangent = toTransform.rotation * -direction * length * 0.3f;

            Handles.DrawBezier(fromPoint, toPoint, fromPoint + fromTangent, toPoint + toTangent, Handles.color, null,
                kArrowThickness);
            Handles.DrawAAPolyLine(kArrowThickness, toPoint,
                toPoint + toTransform.rotation * (-direction - sideDir) * toSize * kArrowHeadSize);
            Handles.DrawAAPolyLine(kArrowThickness, toPoint,
                toPoint + toTransform.rotation * (-direction + sideDir) * toSize * kArrowHeadSize);
        }

        private static Vector3 GetPointOnRectEdge(RectTransform rect, Vector2 dir)
        {
            if (rect == null)
                return Vector3.zero;
            if (dir != Vector2.zero)
                dir /= Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
            dir = rect.rect.center + Vector2.Scale(rect.rect.size, dir * 0.5f);
            return dir;
        }

        private static UnityEditor.Animations.AnimatorController GenerateSelectableAnimatorController(
            AnimationTriggers animationTriggers, Selectable target)
        {
            if (target == null)
                return null;

            // Where should we create the controller?
            var path = GetSaveControllerPath(target);
            if (string.IsNullOrEmpty(path))
                return null;

            // figure out clip names
            var normalName = string.IsNullOrEmpty(animationTriggers.normalTrigger)
                ? "Normal"
                : animationTriggers.normalTrigger;
            var highlightedName = string.IsNullOrEmpty(animationTriggers.highlightedTrigger)
                ? "Highlighted"
                : animationTriggers.highlightedTrigger;
            var pressedName = string.IsNullOrEmpty(animationTriggers.pressedTrigger)
                ? "Pressed"
                : animationTriggers.pressedTrigger;
            var selectedName = string.IsNullOrEmpty(animationTriggers.selectedTrigger)
                ? "Selected"
                : animationTriggers.selectedTrigger;
            var disabledName = string.IsNullOrEmpty(animationTriggers.disabledTrigger)
                ? "Disabled"
                : animationTriggers.disabledTrigger;

            // Create controller and hook up transitions.
            var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(path);
            GenerateTriggerableTransition(normalName, controller);
            GenerateTriggerableTransition(highlightedName, controller);
            GenerateTriggerableTransition(pressedName, controller);
            GenerateTriggerableTransition(selectedName, controller);
            GenerateTriggerableTransition(disabledName, controller);

            AssetDatabase.ImportAsset(path);

            return controller;
        }

        private static AnimationClip GenerateTriggerableTransition(string name,
            UnityEditor.Animations.AnimatorController controller)
        {
            // Create the clip
            var clip = UnityEditor.Animations.AnimatorController.AllocateAnimatorClip(name);
            AssetDatabase.AddObjectToAsset(clip, controller);

            // Create a state in the animatior controller for this clip
            var state = controller.AddMotion(clip);

            // Add a transition property
            controller.AddParameter(name, AnimatorControllerParameterType.Trigger);

            // Add an any state transition
            var stateMachine = controller.layers[0].stateMachine;
            var transition = stateMachine.AddAnyStateTransition(state);
            transition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, name);
            return clip;
        }

        private static string GetSaveControllerPath(Selectable target)
        {
            var defaultName = target.gameObject.name;
            var message = string.Format("Create a new animator for the game object '{0}':", defaultName);
            return EditorUtility.SaveFilePanelInProject("New Animation Contoller", defaultName, "controller", message);
        }
    }
}