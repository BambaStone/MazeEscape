using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Battery : MonoBehaviour
{
    public TMP_Text F;
    public PlayerController PC;
    private bool _OnBattery = false;


    private void Update()
    {
        if(_OnBattery)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                F.text = "";
                PC.PulseBatteryNow = PC.PulseBattery;
                PC.PulseBatteryUI.fillAmount = 1;
                gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        gameObject.transform.Rotate(0,1*Time.deltaTime,0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            F.text = "F";
            _OnBattery = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            F.text = "";
            _OnBattery = false;
        }
    }
}
