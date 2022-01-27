using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingRecipeUI : MonoBehaviour
{

    public CraftingRecipe recipe;
    public Image backgroundImage;
    public Image icon;
    public TextMeshProUGUI itemName;
    public Image[] resourceCosts;

    public Color canCraftColor;
    public Color cannottCraftColor;
    private bool canCraft;

    //Everytime we open the craft window we are going to update it
    private void OnEnable()
    {
        UpdateCanCraft();
    }

    public void UpdateCanCraft()
    {
        //As a default we set it true
        canCraft = true;

        //Check if we have enough resources
        for(int i = 0; i < recipe.cost.Length; i++)
        {
            //Check if inventory have enough item
            if(!Inventory.instance.HasItems(recipe.cost[i].item, recipe.cost[i].quantity))
            {
                //We are not allowed to craft and stop the call
                canCraft = false;
                break;
            }
        }
        //Set the backgroundcolor to cancraft color otherwise to cannotcraft color
        backgroundImage.color = canCraft ? canCraftColor : cannottCraftColor;
    }

    private void Start()
    {
        //Set the icon of crafting item
        icon.sprite = recipe.itemToCraft.icon;
        //Set the name of crafting item
        itemName.text = recipe.itemToCraft.displayName;

        //Set resource cost, icon, quantity we need for craft
        for(int i = 0; i < resourceCosts.Length; i++)
        {
            if( i < recipe.cost.Length)
            {
                //Activate recipe
                resourceCosts[i].gameObject.SetActive(true);
                //Activate icons
                resourceCosts[i].sprite = recipe.cost[i].item.icon;
                //Activate quantity text
                resourceCosts[i].transform.GetComponentInChildren<TextMeshProUGUI>().text = recipe.cost[i].quantity.ToString();
            }

            else
            {
                //Disable the crafting recipe that we dont need
                resourceCosts[i].gameObject.SetActive(false);
            }

        }

    }


    public void OnClickButton()
    {
        if (canCraft)
        {
            CraftingWindow.instance.Craft(recipe);

        }
    }

}
