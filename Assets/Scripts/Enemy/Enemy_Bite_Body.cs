using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bite_Body : MonoBehaviour
{
    public Enemy enemy;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            if(enemy.PatternType==2)
                enemy._rushOn = false;
        }
    }
}
