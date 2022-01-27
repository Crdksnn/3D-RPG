using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Needs
{

    [HideInInspector]
    public float currentValue;
    public float maxValue;
    public float startValue;
    public float regenrate;
    public float decayRate;
    public Image Uibar;

    public void Add(float amount)
    {
        currentValue = Mathf.Min(currentValue + amount, maxValue);
    }

    public void Subtrack(float amount)
    {
        currentValue = Mathf.Max(currentValue - amount, 0.0f);
    }

    public float GetPercentage()
    {
        return currentValue / maxValue;
    }

}
