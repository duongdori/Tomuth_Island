using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;
using UnityEngine.Events;

public class CloudSaveManager : MonoBehaviour
{
    public static async Task ForceSaveSingleData<T>(string key, T value)
    {
        try
        {
            Dictionary<string, object> oneElement = new Dictionary<string, object> {
                // It's a text input field, but let's see if you actually entered a number.
                { key, value } };

            // Saving the data without write lock validation by passing the data as an object instead of a SaveItem
            Dictionary<string, string> result = await CloudSaveService.Instance.Data.Player.SaveAsync(oneElement);

            Debug.Log($"Successfully saved {key}:{value} with updated write lock {result[key]}");
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }
    }
    
    public static async Task<T> RetrieveSpecificData<T>(string key, UnityAction<bool> callbackNoKey = null)
    {
        try
        {
            var results = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> {key});

            if (results.TryGetValue(key, out var item))
            {
                callbackNoKey?.Invoke(true);
                return item.Value.GetAs<T>();
            }
            else
            {
                callbackNoKey?.Invoke(false);
                Debug.Log($"There is no such key as {key}!");
            }
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }

        return default;
    }
    
    public static async Task ForceDeleteAllData()
    {
        try
        {
            await CloudSaveService.Instance.Data.Player.DeleteAllAsync();

            Debug.Log($"Successfully deleted all data");
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }
    }
}
