
using DR.BuildingSystem.Features.Runtime.Buildings.Placer.InputHandler;
using UnityEngine;

using UnityEditor;

#if EBS_INPUT_SYSTEM_SUPPORT
using EasyBuildSystem.Features.Editor.Window;
#endif

namespace DR.BuildingSystem.Features.Runtime.Buildings.Placer.InputHandler.Editor
{
    [CustomEditor(typeof(StandaloneInputHandler), true)]
    public class StandaloneInputHandlerEditor : UnityEditor.Editor
    {
        #region Unity Methods

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

#if EBS_INPUT_SYSTEM_SUPPORT && !ENABLE_INPUT_SYSTEM

            EditorGUILayout.HelpBox("The new Input System Package has been detected on this project.\n" +
                "Please, import the Unity Input System support via the Package Importer to able use this.", MessageType.Warning);

            if (GUILayout.Button("Open Package Importer..."))
            {
                PackageImporter.Init();
            }

#else

#if EBS_INPUT_SYSTEM_SUPPORT && ENABLE_INPUT_SYSTEM
            EditorGUILayout.HelpBox("New Input System detected. You can directly modify the inputs in the Input Action file.\n" +
                "For more information about New Input System support, please refer to the documentation.", MessageType.Info);

            if (GUILayout.Button("Edit Input Action Settings..."))
            {
                if (Resources.Load<UnityEngine.InputSystem.InputActionAsset>("Packages/Supports/Input System Support/Input Actions") != null)
                {
                    Selection.activeObject = Resources.Load<UnityEngine.InputSystem.InputActionAsset>("Packages/Supports/Input System Support/Input Actions");
                }
                else
                {
                    Debug.LogWarning("The default input action file <b>Input Actions</b> could be not found, the file not existing or has been renamed.");
                }
            }
#endif

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_InputSettings").FindPropertyRelative("m_BlockWhenCursorOverUI"),
                new GUIContent("Block When Pointer Over UI", "Prevents action keys from being used when the cursor is over a UI element."));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_InputSettings").FindPropertyRelative("m_CanRotateBuildingPart"),
                new GUIContent("Enable Building Rotation with Mouse Wheel", "Enables rotation of the preview using the mouse wheel."));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_InputSettings").FindPropertyRelative("m_CanSelectBuildingPart"),
                new GUIContent("Enable Building Selection with Mouse Wheel", "Enables selection of a Building Part using the mouse wheel."));

#if !ENABLE_INPUT_SYSTEM
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_InputSettings").FindPropertyRelative("m_ValidateActionKey"),
                new GUIContent("Validation Action Key", "The action key used to validate the current action."));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_InputSettings").FindPropertyRelative("m_CancelActionKey"),
                new GUIContent("Cancel Action Key", "The action key used to cancel the current action."));
#endif

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_InputSettings").FindPropertyRelative("m_UsePlacingModeShortcut"),
                new GUIContent("Use Placing Mode Shortcut", "Uses an action key to select the Placing build mode."));

            if (serializedObject.FindProperty("m_InputSettings").FindPropertyRelative("m_UsePlacingModeShortcut").boolValue)
            {
                EditorGUI.indentLevel++;
#if !ENABLE_INPUT_SYSTEM
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_InputSettings").FindPropertyRelative("m_PlacingModeKey"),
                    new GUIContent("Placing Mode Key Shortcut", "The action key for placing the preview."));
#endif
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_InputSettings").FindPropertyRelative("m_ResetModeAfterPlacing"),
                    new GUIContent("Reset Mode After Placing", "Resets the build mode to NONE after placing."));
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_InputSettings").FindPropertyRelative("m_UseEditingModeShortcut"),
                new GUIContent("Use Editing Mode Shortcut", "Uses an action key to select the Editing build mode."));

            if (serializedObject.FindProperty("m_InputSettings").FindPropertyRelative("m_UseEditingModeShortcut").boolValue)
            {
                EditorGUI.indentLevel++;
#if !ENABLE_INPUT_SYSTEM
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_InputSettings").FindPropertyRelative("m_EditingModeKey"),
                    new GUIContent("Editing Mode Key Shortcut", "The action key for editing the preview."));
#endif
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_InputSettings").FindPropertyRelative("m_ResetModeAfterEditing"),
                    new GUIContent("Reset Mode After Editing", "Resets the build mode to NONE after editing."));
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_InputSettings").FindPropertyRelative("m_UseDestroyingModeShortcut"),
                new GUIContent("Use Destroying Mode Shortcut", "Uses an action key to select the Destroy build mode."));

            if (serializedObject.FindProperty("m_InputSettings").FindPropertyRelative("m_UseDestroyingModeShortcut").boolValue)
            {
                EditorGUI.indentLevel++;
#if !ENABLE_INPUT_SYSTEM
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_InputSettings").FindPropertyRelative("m_DestroyingModeKey"),
                    new GUIContent("Destroying Mode Key Shortcut", "The action key for destroying the preview."));
#endif
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_InputSettings").FindPropertyRelative("m_ResetModeAfterDestroying"),
                    new GUIContent("Reset Mode After Destroying", "Resets the build mode to NONE after destroying."));
                EditorGUI.indentLevel--;
            }
#endif

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        #endregion
    }
}