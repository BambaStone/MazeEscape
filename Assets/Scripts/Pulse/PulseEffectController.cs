using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseEffectController : MonoBehaviour
{

    public float ExpandSpeed = 5f;//파장 퍼지는 속도
    public float MaxScale = 50f;//파장 최대 크기
    public float CurrentScale = 0f;//현재 파장의 반지름
    public float StartLineWidth = 5f;//파장의 두께
    public float LightPower = 1f;//파장의 빛 세기
    public float LineWidth = 5f;
    public GameObject Cam;
    public PulseSpawner Spawner;
    public MiniMap Map;

    private void OnEnable()
    {
        LineWidth = StartLineWidth;
    }
    private void OnDisable()//활성화시 초기화
    {
        CurrentScale = 0f;
        LightPower=1f;
    }

    // Update is called once per frame
    void Update()
    {
        //파장크기를 퍼지는 속도에 맞춰서 크게
        CurrentScale += Time.deltaTime * ExpandSpeed;
        transform.localScale = Vector3.one * CurrentScale*2 ;
        //파장 크기에 맞춰서 두께 감소
        LineWidth = Mathf.Clamp01(10f - (CurrentScale / MaxScale));
        //파장 크기에 맞춰서 빛세기 감소
        LightPower = Mathf.Clamp01(1f - (CurrentScale / MaxScale));
        Cam.GetComponent<EdgeCommandBuffer>().LinePower = LightPower;
        Map.LinePower = LightPower;
        //파장 최대크기시 비활성화
        if (CurrentScale > MaxScale)
        {
            if (!Spawner.PulseOnChack())
            {
                Cam.GetComponent<EdgeCommandBuffer>().renderers1.Clear();
                for(int i=0;i<Map.Enemy.Count;i++)
                {
                    Map.Enemy[i].GetComponent<Enemy>()._triggerPulse = false;
                }
                Map.Enemy.Clear();
                Map.Enemy_MiniMap.Clear();
            }
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Objects"))
        {
            Cam.GetComponent<EdgeCommandBuffer>().renderers1.Add(other.gameObject.GetComponent<Renderer>());
        }
        Debug.Log(other.tag);
        if (other.CompareTag("Wall"))
        {
            if (!Map.Walls.Contains(other.gameObject))
            {
                Map.Walls.Add(other.gameObject);
                Map.AddWall();
            }
        }
        if(other.CompareTag("Enemy"))
        {
            
            if (!Map.Enemy.Contains(other.transform.parent.gameObject))
            {
                other.transform.parent.gameObject.GetComponent<Outline>().enabled=true;
                Map.Enemy.Add(other.transform.parent.gameObject);
                Map.AddEnemy();
            }
        }
        if (other.CompareTag("Objects"))
        {
            if (!Map.Objects.Contains(other.gameObject))
            {
                other.GetComponent<Outline>().enabled = true;
                Map.Objects.Add(other.gameObject);
                Map.AddObjects();
            }
        }
        if(other.CompareTag("Item"))
        {
            if (!Map.Item.Contains(other.gameObject))
            {
                other.GetComponent<Outline>().enabled = true;
                Map.Item.Add(other.gameObject);
                Map.AddItem();
            }
        }

    }

    
}
