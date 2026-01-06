using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseSpawner : MonoBehaviour
{
    public List<GameObject> RecycleList;
    public GameObject Pulse;
    public GameObject Cam;
    public MiniMap Map;
    public int MaxPulse;



    // Update is called once per frame
    public void PulseActive(Vector3 pos)
    {
        bool succes = false;//재활용 성공 상태
        for (int i = 0; i < this.RecycleList.Count; i++)//재활용 리스트만큼 반복
        {
            if (!(RecycleList[i].gameObject.activeSelf))//i번째 리스트의 오브젝트 비활성화 상태일때
            {
                RecycleList[i].transform.position = pos;//i번째 리스트의 위치를 pos로 이동
                RecycleList[i].SetActive(true);//활성화
                succes = true;//재활용 성공
                break;//반복 종료
            }
        }
        if (!succes)//재활용 실패시
        {
            RecycleList.Add(Instantiate(Pulse, pos, Quaternion.identity));//pos에 소환
            RecycleList[RecycleList.Count - 1].GetComponent<PulseEffectController>().Cam = Cam;
            RecycleList[RecycleList.Count - 1].GetComponent<PulseEffectController>().Map = Map;
            RecycleList[RecycleList.Count - 1].GetComponent<PulseEffectController>().Spawner = gameObject.GetComponent<PulseSpawner>();
            RecycleList[RecycleList.Count - 1].transform.parent = transform;//스포너를 생성된 오브젝트의 부모오브젝트로 지정
        }
    }

    public bool PulseOnChack()
    {
        int succes = 0;
        for (int i = 0; i < this.RecycleList.Count; i++)//재활용 리스트만큼 반복
        {
            if ((RecycleList[i].gameObject.activeSelf))//i번째 리스트의 오브젝트 활성화 상태일때
            {
                succes++;
                if (2 == succes)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
