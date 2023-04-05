using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;
using PlayFab;

public class PlayfabInventoryVirtualCurrencyView : MonoBehaviour
{
    GetUserInventoryResult inventory;

    void OnEnable()
    {
        // Hide
        this.Hide();

        // Inventory is setup?
        if (PlayfabInventory.Instance != null)
        {
            Debug.Log("Inventory not null");
            // Register to events
            //PlayfabInventory.Instance.OnInventoryUpdateSuccess.AddListener(this.OnInventoryUpdateSuccess);
            //PlayfabInventory.Instance.OnInventoryUpdateError.AddListener(this.OnInventoryUpdateError);

            // Ask inventory to update itself
            //PlayfabInventory.Instance.UpdateInventory();

            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnInventoryUpdateSuccess, OnInventoryUpdateError);
        }

        //// Update view to init
        this.UpdateView();
    }

    void OnDisable()
    {
        // Inventory is setup?
        if (PlayfabInventory.Instance != null)
        {
            // Unregister to events
            //PlayfabInventory.Instance.OnInventoryUpdateSuccess.RemoveListener(this.OnInventoryUpdateSuccess);
            //PlayfabInventory.Instance.OnInventoryUpdateError.RemoveListener(this.OnInventoryUpdateError);
        }
    }

    private void OnInventoryUpdateSuccess(GetUserInventoryResult result)
    {
        Debug.Log("Success");
        foreach (var item in result.Inventory)
        {
            Debug.Log(item.DisplayName + ":" + item.CatalogVersion);
        }
        inventory = result;
        this.Show();
        this.UpdateView();
    }

    private void OnInventoryUpdateError(PlayFabError error)
    {
        Debug.Log("Error");
        this.Show();
        this.UpdateView();
    }

    [Header("Inventory")]
    public string virtualCurrencyListing = "FC";

    [Header("UI")]
    // Content root
    public Transform contentRoot = null;

    // Content UI
    public Text usernameText = null;
    //
    public Text crystalsCountText = null;
    public Image crystalsIcon = null;

    public void UpdateView()
    {
        int crystalsCount = 0;

        // Get crystals from data
        if (inventory != null && inventory.VirtualCurrency != null)
        {
            crystalsCount = inventory.VirtualCurrency["FC"];
            Debug.Log(inventory.VirtualCurrency["FC"]);
        }

        // Update crystals count
        {
            if (this.crystalsCountText != null)
            {
                this.crystalsCountText.gameObject.SetActive(true);
                this.crystalsCountText.text = crystalsCount.ToString();
            }

            if (this.crystalsIcon != null)
                this.crystalsIcon.gameObject.SetActive(true);
        }

        // Show
        this.Show();
    }

    // Show / Hide
    void Show()
    {
        if (this.contentRoot != null)
            this.contentRoot.gameObject.SetActive(true);
    }

    void Hide()
    {
        if (this.contentRoot != null)
            this.contentRoot.gameObject.SetActive(false);
    }
}
