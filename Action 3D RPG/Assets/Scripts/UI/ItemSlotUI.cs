using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{

    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    private ItemSlot currentSlot;
    private Outline outline;

    public int index;
    public bool equipped;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    public void Set(ItemSlot slot)
    {
        //Set the slot
        currentSlot = slot;

        //Enable the icon
        icon.gameObject.SetActive(true);

        //Set the sprite of item
        icon.sprite = slot.item.icon;

        //If slot.quantity greater than 1 then run slot.quantity.ToString() but if its not greater than 1 then string is empty
        quantityText.text = slot.quantity > 1 ? slot.quantity.ToString() : string.Empty;

        if(outline != null)
        {
            outline.enabled = equipped;
        }
    }

    //Funtion when we remove item
    public void Clear()
    {
        currentSlot = null;

        //Disable to icon
        icon.gameObject.SetActive(false);

        //Set quantity text empty we dont have item inside slot
        quantityText.text = string.Empty;
    }

    public void OnClickButton()
    {
        Inventory.instance.SelectItem(index);
    }


}

