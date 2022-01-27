using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemObject : MonoBehaviour,IInteractable
{

    public ItemDatabase item;

    public string GetInteractPromp()
    {
        return string.Format("Pickup {0}", item.displayName);
    }

    public void OnInteract()
    {
        //After interact add item to inventory
        Inventory.instance.AddItem(item);
        //Destory item in the world beacuse we picked them up
        Destroy(gameObject);
    }
}
