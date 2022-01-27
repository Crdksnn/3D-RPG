using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ItemDatabase itemToGive;
    public int capacity;
    public int quantityPerHit = 1;
    public GameObject hitParticle;

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        for(int i = 0; i < quantityPerHit; i++)
        {
            //If capacity become 0 jsut break the loop and no longer loop through this
            if (capacity <= 0)
                break;

            //Reduce 1 from capacity with every hit
            capacity -= 1;

            //Add resource to inventory
            Inventory.instance.AddItem(itemToGive);

        }

        //Instantiate a particle effect at the position which we hit the tree with correct orientation
        Destroy(Instantiate(hitParticle, hitPoint, Quaternion.LookRotation(hitNormal, Vector3.up)), 1.0f);

        if (capacity <= 0)
            Destroy(gameObject);

    }

}
