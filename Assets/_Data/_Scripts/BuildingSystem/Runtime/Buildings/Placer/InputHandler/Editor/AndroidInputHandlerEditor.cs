
using DR.BuildingSystem.Features.Runtime.Buildings.Placer.InputHandler;
using UnityEngine;
using UnityEditor;

namespace DR.BuildingSystem.Features.Runtime.Buildings.Placer.InputHandler.Editor
{
    [CustomEditor(typeof(AndroidInputHandler), true)]
    public class AndroidInputHandlerEditor : UnityEditor.Editor
    {
        #region Unity Methods

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        #endregion
    }
}