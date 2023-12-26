using UnityEngine;
using UnityEngine.AI;

public class BaseFunction
{
    public bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        //random point in a sphere
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        // Check if the destination position is valid
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documenttation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    // Check if the destination position is valid
    public bool IsValidDestination(Vector3 destinationPosition)
    {
        // Check if the destination position is within the NavMesh bounds
        NavMeshHit hit;
        return NavMesh.SamplePosition(destinationPosition, out hit, 0.3f, NavMesh.AllAreas);
    }

    public string GetPhoenicia(int index)
    {
        return index switch
        {
            1 => "Alpha",
            2 => "Beta",
            3 => "Gamma",
            4 => "Delta",
            5 => "Epsilon",
            6 => "Zeta",
            7 => "Eta",
            8 => "Theta",
            9 => "Iota",
            10 => "Kappa",
            11 => "Lambda",
            12 => "Mu",
            _ => "Phoenicia",
        };
    }

    public string GenerateRandomKey()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        const int keyLength = 8;
        char[] key = new char[keyLength];
        for (int i = 0; i < keyLength; i++)
        {
            key[i] = chars[Random.Range(0, chars.Length)];
        }

        return new string(key);
    }

    public void AddEnemyTag(string tag)
    {
        // SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        // SerializedProperty tags = tagManager.FindProperty("tags");
        //
        // bool found = false;
        // for (int i = 0; i < tags.arraySize; i++)
        // {
        //     SerializedProperty t = tags.GetArrayElementAtIndex(i);
        //     if (t.stringValue.Equals(tag))
        //     {
        //         found = true;
        //         break;
        //     }
        // }
        //
        // if (!found)
        // {
        //     tags.InsertArrayElementAtIndex(tags.arraySize);
        //     SerializedProperty newTag = tags.GetArrayElementAtIndex(tags.arraySize - 1);
        //     newTag.stringValue = tag;
        //     tagManager.ApplyModifiedProperties();
        //     // Debug.Log(tag + " tag added!");
        // }
    }

    public void AddEnemyLayer(string layerName)
    {
        // int newLayer = LayerMask.NameToLayer(layerName);
        // if (newLayer == -1)
        // {
        //     SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        //     SerializedProperty layersProp = tagManager.FindProperty("layers");
        //     for (int i = 8; i < layersProp.arraySize; i++) // Start from index 8 (custom layers)
        //     {
        //         SerializedProperty layer = layersProp.GetArrayElementAtIndex(i);
        //         if (layer.stringValue == "")
        //         {
        //             layer.stringValue = layerName;
        //             tagManager.ApplyModifiedProperties();
        //         }
        //     }
        // }
    }
}
