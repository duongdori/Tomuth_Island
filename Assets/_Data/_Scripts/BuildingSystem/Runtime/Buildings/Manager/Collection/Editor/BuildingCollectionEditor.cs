
using UnityEngine;
using UnityEditor;

using DR.BuildingSystem.Features.Editor.Extensions;
using DR.BuildingSystem.Features.Runtime.Buildings.Manager.Collection;

namespace DR.BuildingSystem.Features.Runtime.Buildings.Manager.Collection.Editor
{
    [CustomEditor(typeof(BuildingCollection))]
    public class BuildingCollectionEditor : UnityEditor.Editor
    {
        #region Unity Methods

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUIUtilityExtension.DrawHeader("Building System - Building Collection",
                        "This component stores a collection of parts that can be easily loaded into the Building Manager.\n" +
                        "You can find more information on the Building Collection component in the documentation.");

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_BuildingParts"),
                new GUIContent("Building Parts", "The references to the Building Parts associated with this Building Collection."));
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        #endregion
    }
}