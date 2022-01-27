using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : MonoBehaviour, IInteractable
{

    public CraftingWindow craftingWindow;
    private PlayerController player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        craftingWindow = FindObjectOfType<CraftingWindow>(true);
    }

    public string GetInteractPromp()
    {
        return "Craft";
    }

    public void OnInteract()
    {
        craftingWindow.gameObject.SetActive(true);
        player.ToggleCursor(true);
    }
}
