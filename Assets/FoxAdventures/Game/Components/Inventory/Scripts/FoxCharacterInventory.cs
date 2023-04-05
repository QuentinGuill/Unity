using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;

public class FoxCharacterInventory : MonoBehaviour
{
    public int keyCount = 0;
    public int jewelsCount = 0;

    public void Win() {
        AddVC();
    }

    private void AddVC()
    {
        AddUserVirtualCurrencyRequest request = new AddUserVirtualCurrencyRequest
        {
            Amount = jewelsCount,
            VirtualCurrency = "FC"
        };

        PlayFabClientAPI.AddUserVirtualCurrency( request , result=> OnVCUpdated(result), OnVCFail);
    }

    private void OnVCUpdated(ModifyUserVirtualCurrencyResult updateResult) {
        SubmitScore(updateResult.Balance);
        Debug.Log("Successfully submitted high score");
    }

    private void OnVCFail(PlayFabError error){
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    public void SubmitScore(int playerScore) {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate {
                    StatisticName = "level1_crystals",
                    Value = playerScore
                }
            }
        }, result=> OnStatisticsUpdated(result), FailureCallback);
    }

    private void OnStatisticsUpdated(UpdatePlayerStatisticsResult updateResult) {
        Debug.Log("Successfully submitted high score");
    }

    private void FailureCallback(PlayFabError error){
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
}
