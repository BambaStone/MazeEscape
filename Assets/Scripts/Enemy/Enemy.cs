using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int HP = 3;
    public GameObject Player;
    public int PatternType = 0;
    public Outline outlines;

    public bool _rushOn=false;
    public bool _triggerPulse = false;

    private void Start()
    {
        outlines = gameObject.GetComponent<Outline>();
    }

    private void FixedUpdate()
    {
        
        switch(PatternType)
        {
            case 0:
                if(_triggerPulse)
                {
                    transform.LookAt(Player.transform);
                    transform.Translate(Vector3.forward * Time.deltaTime*3);
                }
                break;
            case 1:
                if (!_triggerPulse)
                {
                    transform.LookAt(Player.transform);
                    transform.Translate(Vector3.forward * Time.deltaTime);
                }
                break;
            case 2:
                if(_triggerPulse)
                {
                    transform.LookAt(Player.transform);
                    _triggerPulse = false;
                    StartCoroutine(RushOn());
                }
                if(_rushOn)
                {
                    transform.Translate(Vector3.forward * Time.deltaTime * 10);
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Pulse"))
        {
            _triggerPulse = true;
        }
    }


    public void HitGun()
    {
        
        HP--;
        if(HP <=0)
        {
            switch(PatternType)
            {
                case 0:
                    gameObject.SetActive(false);
                    break;
                case 1:
                    HP = 2;
                    transform.Translate(Vector3.forward * 50*-1);
                    break;
                case 2:
                    gameObject.SetActive(false);
                    break;
            }
            
        }
    }

    IEnumerator RushOn()
    {
        yield return new WaitForSeconds(1f);
        transform.LookAt(Player.transform);
        _rushOn = true;
        StartCoroutine(RushOff());
    }
    IEnumerator RushOff()
    {
        yield return new WaitForSeconds(2f);
        _rushOn = false;
    }
}
