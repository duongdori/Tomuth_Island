
using DR.BuildingSystem.Features.Runtime.Buildings.Part.Conditions;
using UnityEngine;

using UnityEditor;

namespace DR.BuildingSystem.Features.Runtime.Buildings.Part.Conditions.Editor
{
    [CustomEditor(typeof(BuildingTerrainCondition))]
    public class BuildingTerrainConditionEditor : UnityEditor.Editor
    {
        #region Unity Methods

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_CheckTreesCollision"),
                new GUIContent("Building Terrain Trees Collision", "Check for collision with trees on the Unity Terrain."));

            if (serializedObject.FindProperty("m_CheckTreesCollision").boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_CheckTreesCollisionDistance"),
                    new GUIContent("Building Terrain Trees Collision Distance", "The maximum distance to check for tree collisions."));
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_ClearGrassDetails"),
                new GUIContent("Building Terrain Clear Grass", "Clear grass details on the terrain at placement."));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_ClearGrassBounds"),
                new GUIContent("Building Terrain Clear Grass Bounds", "The bounds within which to clear grass details on the terrain."));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_ShowDebugs"), new GUIContent("Show Debugs"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_ShowGizmos"), new GUIContent("Show Gizmos"));

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        #endregion
    }
}