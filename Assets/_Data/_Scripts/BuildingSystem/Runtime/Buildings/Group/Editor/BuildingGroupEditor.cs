
using EasyBuildSystem.Features.Runtime.Buildings.Group;
using UnityEngine;

using UnityEditor;

namespace DR.BuildingSystem.Features.Runtime.Buildings.Group.Editor
{
    [CustomEditor(typeof(BuildingGroup))]
    public class BuildingGroupEditor : UnityEditor.Editor
    {
        #region Fields

        BuildingGroup Target
        {
            get
            {
                return ((BuildingGroup)target);
            }
        }

        #endregion

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUI.enabled = false;
            EditorGUILayout.TextField(new GUIContent("Group Identifier", "A unique identifier for the Building Group."), Target.Identifier);
            GUI.enabled = true;

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        #endregion
    }
}