using UnityEngine;
using UnityEngine.Events;

public class PlayerNeedsControl : MonoBehaviour, IDamagable
{

    public Needs health;
    public Needs hunger;
    public Needs thirst;
    public Needs sleep;

    public float hungerHealthdecay;
    public float thirstHealthdecay;

    public UnityEvent onTakeDamage;

    private void Start()
    {
        //Set the start values

        health.currentValue = health.startValue;
        hunger.currentValue = hunger.startValue;
        thirst.currentValue = thirst.startValue;
        sleep.currentValue = sleep.startValue;

    }

    private void Update()
    {
        //Reduce needs over time
        hunger.Subtrack(hunger.decayRate * Time.deltaTime);
        thirst.Subtrack(thirst.decayRate * Time.deltaTime);
        sleep.Add(sleep.regenrate * Time.deltaTime);

        //Reduce health if we dont have hunger
        if(hunger.currentValue == 0)
        {
            health.Subtrack(hungerHealthdecay * Time.deltaTime);
        }

        //Reduce health if we dont have thirst
        if (thirst.currentValue == 0)
        {
            health.Subtrack(thirstHealthdecay * Time.deltaTime);
        }

        //Check if the player is dead
        if(health.currentValue == 0)
        {
            Die();
        }

        //Update UI bars
        health.Uibar.fillAmount = health.GetPercentage();
        hunger.Uibar.fillAmount = hunger.GetPercentage();
        thirst.Uibar.fillAmount = thirst.GetPercentage();
        sleep.Uibar.fillAmount = sleep.GetPercentage();
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void Drink(float amount)
    {
        thirst.Add(amount);
    }

    public void Sleep(float amount)
    {
        sleep.Subtrack(amount);
    }

    
    public void TakePhysicsDamage(int amount)
    {
        health.Subtrack(amount);
        onTakeDamage?.Invoke();
    }

    public void Die()
    {
        Debug.Log("Player is dead");
    }

}

public interface IDamagable
{
    void TakePhysicsDamage(int damageAmount);
}

