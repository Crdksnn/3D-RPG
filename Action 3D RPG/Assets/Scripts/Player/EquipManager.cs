using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipManager : MonoBehaviour
{

    public Equip currentEquip;
    public Transform equipParent;
    private PlayerController controller;

    //Singleton

    public static EquipManager instance;

    void Awake()
    {
        instance = this;
        controller = GetComponent<PlayerController>();
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed && currentEquip != null && controller.canLook == true)
        {
            currentEquip.OnAltAttackInput();
        } 
    }

    public void OnAltAttackInput(InputAction.CallbackContext context)
    {
        //Only when we press down && current equip does exist && player controller can look around
        if (context.phase == InputActionPhase.Performed && currentEquip != null && controller.canLook == true)
        {
            currentEquip.OnAltAttackInput();
        }
    }
    
    public void EquipNewItem(ItemDatabase item)
    {
        //Unequip anything we currently have
        UnEquipItem();
        //Instantioate a new object as a child of camera and get it equip component
        currentEquip = Instantiate(item.equipPrefab, equipParent).GetComponent<Equip>();

    }

    public void UnEquipItem()
    {
        //Check if we have anything to unequip and if so destroying that object and set out current equip to null
        if(currentEquip != null)
        {
            Destroy(currentEquip.gameObject);
            currentEquip = null;
        }
    }

}
