using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;

public class Leaderboard : MonoBehaviour
{
    public string leaderboardName = "";
    public GameObject leaderboardEntryPrefab = null;
    //
    private List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();
    //
    public bool isFloatExpected = false;
    public bool isReversed = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        // Deactivate prefab in case
        if (this.leaderboardEntryPrefab != null)
            this.leaderboardEntryPrefab.gameObject.SetActive(false);

        // Refresh prefab
        this.RefreshLeaderboard();
    }

    // Refresh
    public void RefreshLeaderboard()
    {
        // Check prefab
        if (this.leaderboardEntryPrefab == null)
            return;

        // Trigger leaderboard data retrieval
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest {
            StatisticName = leaderboardName,
            StartPosition = 0,
            MaxResultsCount = 10
        }, result=> OnGetLeaderboardSuccess(result), OnGetLeaderboardError);
    }

    private void OnGetLeaderboardSuccess(GetLeaderboardResult result)
    {
        // Clear existing entries
        this.ClearExistingEntries();

        //// TODO: Use data from input & loop on it?
        // Go through scores
        if (result != null)
        {
            for (int i=0; i < result.Leaderboard.Count; i++)
            {
                //// Use data from input & loop on it?
                if (result.Leaderboard[i] != null)
                {
                    // Get data
                    string username = result.Leaderboard[i].DisplayName;                         // TODO: Get user leaderboard name
                    int statValue = result.Leaderboard[i].StatValue;                                      // Get score we want to display

                    // Instantiate object copy
                    GameObject leaderboardEntryGameobjectCopy = GameObject.Instantiate(this.leaderboardEntryPrefab, this.leaderboardEntryPrefab.transform.parent);
                    if (leaderboardEntryGameobjectCopy != null)
                    {
                        // Activate at our prefab is deactivated
                        leaderboardEntryGameobjectCopy.gameObject.SetActive(true);

                        // Get leaderboard entry
                        LeaderboardEntry leaderboardEntry = leaderboardEntryGameobjectCopy.GetComponent<LeaderboardEntry>();
                        if (leaderboardEntry != null)
                        {
                            // Fix value
                            if (isReversed == true)
                                statValue *= -1;

                            // Set value
                            leaderboardEntry.SetValue(username, (isFloatExpected == true ? ((float)statValue / 100.0f).ToString("0.00") : statValue.ToString()));

                            // Add to list
                            if (this.leaderboardEntries == null)
                                this.leaderboardEntries = new List<LeaderboardEntry>();
                            this.leaderboardEntries.Add(leaderboardEntry);
                        }
                        // Else, destroy object we just spawned
                        else
                        {
                            GameObject.Destroy(leaderboardEntryGameobjectCopy);
                        }
                    }
                }
            }
        }
    }

    private void OnGetLeaderboardError(PlayFabError error)
    {
        // Log
        Debug.LogError("Leaderboard.OnGetLeaderboardError() - Error: TODO");
    }

    // Clear existing entries
    public void ClearExistingEntries()
    {
        if (this.leaderboardEntries != null)
        {
            while (this.leaderboardEntries.Count > 0)
            {
                if (this.leaderboardEntries[0] != null)
                {
                    GameObject.Destroy(this.leaderboardEntries[0].gameObject);
                }

                // Remove first entry
                this.leaderboardEntries.RemoveAt(0);
            }
        }
    }
}
