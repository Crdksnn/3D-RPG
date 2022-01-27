using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingWindow : MonoBehaviour
{
    public CraftingRecipeUI[] recipeUIs;
    //Singleton

    public static CraftingWindow instance;

    private void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        Inventory.instance.onOpenInventory.AddListener(OnEnable);
    }

    void OnDisable()
    {
        Inventory.instance.onOpenInventory.RemoveListener(OnEnable);
    }

    void onOpenInventory()
    {
        //Disable crafting window
        gameObject.SetActive(false);
    }


    public void Craft(CraftingRecipe recipe)
    {
        //Looping through all of the items we need for crafting
        for(int i = 0; i < recipe.cost.Length; i++)
        {
            //Loop through quantity for example if we need 3 stone loop 3 times and reduce 3 stone from inventory
            for(int j = 0; j < recipe.cost[j].quantity; j++)
            {
                Inventory.instance.RemoveItem(recipe.cost[i].item);
            }

        }

        //Add item to recipe
        Inventory.instance.AddItem(recipe.itemToCraft);

        //Add item to inventory
        for(int i = 0; i <recipeUIs.Length; i++)
        {
            //We are adding the crafted item to inventory
            recipeUIs[i].UpdateCanCraft();
        }
    }

}
