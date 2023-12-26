
using UnityEditor;

using UnityEngine;
using UnityEngine.Rendering;

#if ENABLE_INPUT_SYSTEM
using DR.BuildingSystem.Features.Editor.Window;
#endif

namespace DR.BuildingSystem.Features.Editor
{
    [InitializeOnLoad]
    public static class ProjectIntegrity
    {
        private const string k_ProjectOpened = "_initializedProject";

        static ProjectIntegrity()
        {
            if (!EditorPrefs.GetBool(PlayerSettings.productGUID.ToString() + k_ProjectOpened, false))
            {
                EditorPrefs.SetBool(PlayerSettings.productGUID.ToString() + k_ProjectOpened, true);

                CheckMissingLayers(new string[1] { "Socket" });

                EditorApplication.delayCall += () =>
                {
                    string manifestPath;

                    if (GraphicsSettings.currentRenderPipeline)
                    {
                        if (GraphicsSettings.currentRenderPipeline.GetType().ToString().Contains("HighDefinition"))
                        {
                            manifestPath = AssetDatabase.GetAssetPath(Resources.Load("Packages/Supports/manifest"));
                            AssetDatabase.ImportPackage(manifestPath.Replace("manifest.prefab", "HDRP Support.unitypackage"), true);
                        }
                        else
                        {
                            manifestPath = AssetDatabase.GetAssetPath(Resources.Load("Packages/Supports/manifest"));
                            AssetDatabase.ImportPackage(manifestPath.Replace("manifest.prefab", "URP Support.unitypackage"), true);
                        }
                    }
                };
            }
        }

        public static void CheckMissingLayers(string[] layers)
        {
            SerializedObject manager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty layersProp = manager.FindProperty("layers");

            foreach (string name in layers)
            {
                bool found = false;

                for (int i = 0; i <= 31; i++)
                {
                    SerializedProperty sp = layersProp.GetArrayElementAtIndex(i);
                    if (sp != null && name.Equals(sp.stringValue))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    SerializedProperty slot = null;
                    for (int i = 8; i <= 31; i++)
                    {
                        SerializedProperty sp = layersProp.GetArrayElementAtIndex(i);

                        if (sp != null && string.IsNullOrEmpty(sp.stringValue))
                        {
                            slot = sp;
                            break;
                        }
                    }

                    if (slot != null)
                    {
                        slot.stringValue = name;
                    }
                }
            }

            manager.ApplyModifiedProperties();
        }
    }
}