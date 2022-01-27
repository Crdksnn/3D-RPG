using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{

    public Image bloodImage;

    private Coroutine fadeAway;

    public void Flash()
    {
        
        if(fadeAway != null)
        {
            StopCoroutine(fadeAway);
        }

        bloodImage.enabled = true;
        bloodImage.color = Color.white;
        fadeAway = StartCoroutine(FadeAway());
    }

    IEnumerator FadeAway()
    {
        float alphaImage = 1.0f;

        while (alphaImage > 0)
        {
            alphaImage -= Time.deltaTime;
            bloodImage.color = new Color(1.0f, 1.0f, 1.0f, alphaImage);
            yield return null;
        }
        
        bloodImage.enabled = false;
    }

}
