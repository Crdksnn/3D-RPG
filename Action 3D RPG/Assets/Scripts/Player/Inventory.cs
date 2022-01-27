using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    
    public ItemSlotUI[] uiSlots;
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform dropPosition;

    [Header("Selected Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;

    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;

    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    private int currentEquipIndex;

    //Components
    private PlayerController controller;
    private PlayerNeedsControl needsControl;

    [Header("Events")]
    public UnityEvent onOpenInventory;
    public UnityEvent onCloseInventory;

    //Singelton
    public static Inventory instance;

    private void Awake()
    {
        instance = this;
        controller = GetComponent<PlayerController>();
        needsControl = GetComponent<PlayerNeedsControl>();
    }

    private void Start()
    {
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[uiSlots.Length];

        //Initialize the slots
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = new ItemSlot();
            uiSlots[i].index = i;
            //All the slot gonna be set clear so we are holding no item
            uiSlots[i].Clear();
        }

        ClearSelectedItemWindow();

    }

    public void Toggle()
    {

        if (inventoryWindow.activeInHierarchy)
        {
            //Close the inventory
            inventoryWindow.SetActive(false);
            onCloseInventory.Invoke();
            controller.ToggleCursor(false);
        }
        else
        {
            //Open the inventory
            inventoryWindow.SetActive(true);
            onOpenInventory.Invoke();
            ClearSelectedItemWindow();
            controller.ToggleCursor(true);
        }

    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    public void AddItem(ItemDatabase item)
    {
        //Check if we can stack the item if we can stack then look for stack too add it to it if it can't add it to empty slot
        if (item.canStackItem)
        {
            //Get item slot to stack it to and send over the item that we want to stack
            ItemSlot slotToStackTo = GetItemStack(item);

            //If it find any item to stack
            if(slotToStackTo != null)
            {
                slotToStackTo.quantity++;
                UpdateUI();
                return;
            }
        }
        //We dont have item to stack to and if empty slot
        ItemSlot emptySlot = GetEmptySlot();
        //If we find empty slot
        if(emptySlot != null)
        {
            //Set the item to be the item slot
            emptySlot.item = item;
            //Set the quantity to 1
            emptySlot.quantity = 1;
            UpdateUI();
            return;
        }
        //If we dont have any slot available item can't stack and then just throw the item away
        ThrowItem(item);
    }

    //Spawns the objects
    void ThrowItem(ItemDatabase item)
    {
        //Instantie an item in fornt of player with random rotation
        Instantiate(item.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360.0f));
    }

    void UpdateUI()
    {
        //Loop through each of UI slots
        for(int i = 0; i < slots.Length; i++)
        {
            //Does this slot conatin an item
            if(slots[i].item != null)
            {
                //Set the ui slot if its true
                uiSlots[i].Set(slots[i]);
            }
            //If its not true
            else
            {
                //Clear that ui slot
                uiSlots[i].Clear();
            }
        }
    }

    //Look for inventory and try to find an item slot wich we can stack this new item to it
    ItemSlot GetItemStack(ItemDatabase item)
    {
        //Check if the item contain the slot is the item that we want to add an its quantity is not above the max stack amount
        for(int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item == item && slots[i].quantity < item.maxStackAmount)
            {
                //Return the item slot that the requested item can be stacked on
                return slots[i];
            }
        }
        //If there is no stack available return null
        return null;
    }

    ItemSlot GetEmptySlot()
    {

        for(int i = 0; i <slots.Length; i++)
        {
            //Return an empty slot in the inventory
            if(slots[i].item == null)
            {
                return slots[i];
            }
        }
        //If ther are no empty slots retun null
        return null;
    }

    public void SelectItem(int index)
    {
        //If there is no item inside of slot just dont do anything
        if (slots[index].item == null)
            return;
        //Setting the selected item slot
        selectedItem = slots[index];

        //Seeting selected item index
        selectedItemIndex = index;

        //Set the name of item
        selectedItemName.text = selectedItem.item.displayName;

        //Set the description of item
        selectedItemDescription.text = selectedItem.item.description;

        //Set the stat values and stat names
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        for(int i = 0; i < selectedItem.item.consumable.Length; i++)
        {
            selectedItemStatName.text += selectedItem.item.consumable[i].type.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.item.consumable[i].value.ToString() + "\n";
        }

        //Set the use button
        useButton.SetActive(selectedItem.item.type == Itemtype.Consumable);

        //Set the equip button                                               //item is not currently equipped
        equipButton.SetActive(selectedItem.item.type == Itemtype.Equipable && !uiSlots[index].equipped);

        //Set the unequip button                                                //item is currently equipped
        unequipButton.SetActive(selectedItem.item.type == Itemtype.Equipable && uiSlots[index].equipped);

        //Set drop button
        dropButton.SetActive(true);

    }

    void ClearSelectedItemWindow()
    {
        //Clear the text elements
        selectedItem = null;
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        //Disable buttons
        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void OnUseBotton()
    {
     
        if(selectedItem.item.type == Itemtype.Consumable)
        {
            for(int i = 0; i < selectedItem.item.consumable.Length; i++)
            {
                switch (selectedItem.item.consumable[i].type)
                {
                    case ConsumableType.Health: needsControl.Heal(selectedItem.item.consumable[i].value); break;
                    case ConsumableType.Hunger: needsControl.Eat(selectedItem.item.consumable[i].value); break;
                    case ConsumableType.Thirst: needsControl.Drink(selectedItem.item.consumable[i].value); break;
                    case ConsumableType.Sleep: needsControl.Sleep(selectedItem.item.consumable[i].value); break;
                }
            }
        }

        RemoveSelectedItem();
    }

    public void OnEquipButton()
    {
        if (uiSlots[currentEquipIndex].equipped)
            UnEquip(currentEquipIndex);

        uiSlots[selectedItemIndex].equipped = true;
        currentEquipIndex = selectedItemIndex;
        //Call the function EquipNewItem from equi Manager
        EquipManager.instance.EquipNewItem(selectedItem.item);
        //Update UI Inventory
        UpdateUI();
        //Update the buttons on the selected item preview window from equip to unequip
        SelectItem(selectedItemIndex);
    }

    //Item slot that we want Unequip
    void UnEquip(int index)
    {
        uiSlots[index].equipped = false;
        EquipManager.instance.UnEquipItem();
        UpdateUI();
        //If is this the currently selected item, if so then selected
        if (selectedItemIndex == index)
            SelectItem(index);

    }

    public void OnUneqipButton()
    {
        UnEquip(selectedItemIndex);
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem.item);
        RemoveSelectedItem();
    }

    //Remove the current item that we selected from inventory
    void RemoveSelectedItem()
    {
        selectedItem.quantity--;

        if(selectedItem.quantity == 0)
        {
            if(uiSlots[selectedItemIndex].equipped == true)
            {
                UnEquip(selectedItemIndex);
            }

            selectedItem.item = null;
            ClearSelectedItemWindow();
        }

        UpdateUI();

    }

    //If we crafting or building something we need to remove set of items
    public void RemoveItem(ItemDatabase item)
    {

        //Loop through item slots
        for(int i = 0; i < slots.Length; i++)
        {   

            //Does this slot contain item
            if(slots[i].item == item)
            {   

                //Subtrack 1 from quantity
                slots[i].quantity--;

                //But if the quantity is 0 reset the slot
                if(slots[i].quantity == 0)
                {   

                    //If item equiped unequip it first
                    if (uiSlots[i].equipped == true)
                        UnEquip(i);

                    //Reset the slot
                    slots[i].item = null;
                    ClearSelectedItemWindow();
                }

                UpdateUI();
                return;
            }
        }

    }

    public bool HasItems(ItemDatabase item,int quantity)
    {
        int amount = 0;

        //Loop through item slots
        for(int i = 0; i < slots.Length; i++)
        {   

            //If this item is the item we request if so add 1 to it
            if (slots[i].item == item)
                amount += slots[i].quantity;
            
            //If this item amount is bigger than quantity return true
            if (amount >= quantity)
                return false;
        }
        
        //We dont have enough items
        return false;

    }


    public void OnInventoryButton(InputAction.CallbackContext context)
    {
        
        if(context.phase == InputActionPhase.Started)
        {
            Toggle();     
        }
        
    }

}

public class ItemSlot
{
    public ItemDatabase item;
    public int quantity;
}
