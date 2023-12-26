
using UnityEngine;
using UnityEditor;

using DR.BuildingSystem.Features.Runtime.Buildings.Manager;
using DR.BuildingSystem.Features.Runtime.Buildings.Part.Conditions;

namespace DR.BuildingSystem.Features.Runtime.Buildings.Part.Conditions.Editor
{
    [CustomEditor(typeof(BuildingCollisionCondition))]
    public class BuildingCollisionConditionEditor : UnityEditor.Editor
    {
        #region Fields

        BuildingCollisionCondition Target
        {
            get
            {
                return ((BuildingCollisionCondition)target);
            }
        }

        #endregion

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_LayerMask"), new GUIContent("Building Collision Layers",
                "Layers that will be taken into account during collision detection."));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_IgnoreBuildingSurface"), new GUIContent("Ignore Building Surface",
                "Toggle to ignore collision with building surfaces during placement."));

            if (!serializedObject.FindProperty("m_IgnoreBuildingSurface").boolValue)
            {
                if (BuildingManager.Instance == null || BuildingManager.Instance.AllSurfaces.Count == 0)
                {
                    if (BuildingManager.Instance.AllSurfaces.Count == 0)
                    {
                        EditorGUILayout.HelpBox("No building collision surfaces detected. Create a new one to define here.", MessageType.Warning);
                        GUI.enabled = false;
                        Target.CollisionBuildingSurfaceFlags = EditorGUILayout.MaskField(new GUIContent("Building Collision Require Surfaces",
                            "Required collision surface(s) for placement."), Target.CollisionBuildingSurfaceFlags, new string[1] { "Empty" });
                        GUI.enabled = true;
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Requires a Building Manager in the scene.", MessageType.Warning);
                        GUI.enabled = false;
                        Target.CollisionBuildingSurfaceFlags = EditorGUILayout.MaskField(new GUIContent("Building Collision Require Surfaces",
                            "Required collision surface(s) for placement."), Target.CollisionBuildingSurfaceFlags, new string[1] { "Empty" });
                        GUI.enabled = true;
                    }
                }
                else
                {
                    serializedObject.FindProperty("m_BuildingSurfaceFlags").intValue =
                        EditorGUILayout.MaskField(new GUIContent("Building Collision Require Surfaces",
                        "Required collision surface(s) for placement."), Target.CollisionBuildingSurfaceFlags, BuildingManager.Instance.AllSurfaces.ToArray());
                }
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Tolerance"), new GUIContent("Building Collision Tolerance",
                "Collision tolerance for allowing the placement."));

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