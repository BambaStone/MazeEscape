using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(EffectUnActive());
    }
    
    IEnumerator EffectUnActive()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
