using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    public TMP_Text F;
    public PlayerController PC;
    private bool _OnGun = false;


    private void Update()
    {
        if (_OnGun)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                F.text = "";
                if(PC.GunOn)
                {
                    PC.GunBulletNow = PC.GunBullet;
                    PC.GunBulletUI.text = PC.GunBulletNow + "";
                }
                else
                {
                    PC.GunOn = true;
                    PC.Gun.SetActive(true);
                }
                
                gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        gameObject.transform.Rotate(0, 1 * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            F.text = "F";
            _OnGun = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            F.text = "";
            _OnGun = false;
        }
    }
}
