using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;
    public float attackDistance;
    private bool attacking;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;

    [Header("Resource Gathering")]
    public bool doesGatherResources;

    //Components
    private Animator anim;
    private Camera cam;


    private void Awake()
    {
        //Get components
        anim = GetComponent<Animator>();
        cam = Camera.main;
    }


    public override void OnAltAttackInput()
    {
        if (!attacking)
        {
            attacking = true;
            anim.SetTrigger("Attack");
            Invoke("OnCanAttack", attackRate);
        }
    }

    void OnCanAttack()
    {
        attacking = false;
    }

    public void OnHit()
    {
        //Set the ray to shoot from center of screen
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        //Store all the actual hit data in
        RaycastHit hit;

        //Shoot raycast
        if(Physics.Raycast(ray, out hit, attackDistance))
        {
            //If we hit resource
            if(doesGatherResources && hit.collider.GetComponent<Resource>())
            {
                hit.collider.GetComponent<Resource>().Gather(hit.point, hit.normal);
            }

            //If we hit damageable or enemy
            if(doesDealDamage && hit.collider.GetComponent<IDamagable>() != null)
            {
                hit.collider.GetComponent<IDamagable>().TakePhysicsDamage(damage);
            }

        }

    }

}
