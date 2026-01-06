using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int HP = 3;
    public GameObject Player;
    private bool _goTarget = false;

    // Update is called once per frame
    void Update()
    {
        if(_goTarget)
        {
            transform.LookAt(Player.transform);
            transform.Translate(transform.forward*-1*Time.deltaTime*3);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Pulse"))
        {
            StartCoroutine(GoTarget());
        }
    }

    public void HitGun()
    {
        HP--;
        if(HP <=0)
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator GoTarget()
    {
        yield return new WaitForSeconds(0.5f);
        _goTarget = true;
        StartCoroutine(Stop());
    }
    IEnumerator Stop()
    {
        yield return new WaitForSeconds(5f);
        _goTarget = false;
    }

}
