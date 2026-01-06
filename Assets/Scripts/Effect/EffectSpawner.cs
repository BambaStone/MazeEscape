using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    public List<GameObject> RecycleList;
    public GameObject Effect;



    // Update is called once per frame
    public void EffectActive()
    {
        bool succes = false;//재활용 성공 상태
        for (int i = 0; i < this.RecycleList.Count; i++)//재활용 리스트만큼 반복
        {
            if (!(RecycleList[i].gameObject.activeSelf))//i번째 리스트의 오브젝트 비활성화 상태일때
            {
                RecycleList[i].transform.position = transform.position;//i번째 리스트의 위치를 pos로 이동
                RecycleList[i].transform.rotation = transform.rotation;
                RecycleList[i].SetActive(true);//활성화
                succes = true;//재활용 성공
                break;//반복 종료
            }
        }
        if (!succes)//재활용 실패시
        {
            RecycleList.Add(Instantiate(Effect, transform.position, transform.rotation));//pos에 소환
            RecycleList[RecycleList.Count - 1].transform.parent = transform;//스포너를 생성된 오브젝트의 부모오브젝트로 지정
        }
    }
    public void TargetEffectActive(Vector3 pos)
    {
        bool succes = false;//재활용 성공 상태
        for (int i = 0; i < this.RecycleList.Count; i++)//재활용 리스트만큼 반복
        {
            if (!(RecycleList[i].gameObject.activeSelf))//i번째 리스트의 오브젝트 비활성화 상태일때
            {
                RecycleList[i].transform.position = pos;//i번째 리스트의 위치를 pos로 이동
                RecycleList[i].transform.rotation = Quaternion.identity;
                RecycleList[i].SetActive(true);//활성화
                succes = true;//재활용 성공
                break;//반복 종료
            }
        }
        if (!succes)//재활용 실패시
        {
            RecycleList.Add(Instantiate(Effect, pos, Quaternion.identity));//pos에 소환
            RecycleList[RecycleList.Count - 1].transform.parent = transform;//스포너를 생성된 오브젝트의 부모오브젝트로 지정
        }
    }
}
