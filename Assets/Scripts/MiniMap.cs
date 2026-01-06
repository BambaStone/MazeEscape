using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MiniMap : MonoBehaviour
{
    public GameObject ViewPorts;
    public List<GameObject> Walls;
    public List<GameObject> Enemy;
    public List<GameObject> Item;
    public GameObject Wall_MiniMap_Prefab;
    public GameObject Enemy_MiniMap_Prefab;
    public GameObject Item_MiniMap_Prefab;
    public List<GameObject> Walls_MiniMap;
    public List<GameObject> Enemy_MiniMap;
    public List<GameObject> Item_MiniMap;
    public RectTransform Player_MiniMap;
    public GameObject Player;
    public float LinePower = 1f;

    public void AddWall()
    {
        if(Walls_MiniMap.Count<Walls.Count)
        {
            Walls_MiniMap.Add(Instantiate(Wall_MiniMap_Prefab));
            Walls_MiniMap[Walls_MiniMap.Count-1].GetComponent<RectTransform>().SetParent(ViewPorts.GetComponent<RectTransform>());
            Walls_MiniMap[Walls_MiniMap.Count - 1].GetComponent<RectTransform>().localPosition = Vector3.zero;
            Walls_MiniMap[Walls_MiniMap.Count - 1].GetComponent<RectTransform>().localScale = Vector3.one;
            Walls_MiniMap[Walls_MiniMap.Count - 1].GetComponent<RectTransform>().localRotation=Quaternion.Euler(0,0,Walls[Walls_MiniMap.Count - 1].transform.rotation.eulerAngles.y-90);
        }
    }

    public void AddEnemy()
    {
        if(Enemy_MiniMap.Count < Enemy.Count)
        {
            Enemy_MiniMap.Add(Instantiate(Enemy_MiniMap_Prefab));
            Enemy_MiniMap[Enemy_MiniMap.Count - 1].GetComponent<RectTransform>().SetParent(ViewPorts.GetComponent<RectTransform>());
            Enemy_MiniMap[Enemy_MiniMap.Count - 1].GetComponent<RectTransform>().localPosition = Vector3.zero;
            Enemy_MiniMap[Enemy_MiniMap.Count - 1].GetComponent<RectTransform>().localScale = Vector3.one;
            Enemy_MiniMap[Enemy_MiniMap.Count - 1].GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, Enemy[Enemy.Count - 1].transform.rotation.eulerAngles.y *-1);
        }
    }


    // Update is called once per frame
    void Update()
    {
        Player_MiniMap.localRotation = Quaternion.Euler(0, 0, Player.transform.rotation.eulerAngles.y*-1);
        for(int i=0;i<Enemy_MiniMap.Count;i++)
        {
            if (Enemy[i].activeSelf)
            {
                Vector2 pos = new Vector2((Enemy[i].transform.position.x - Player.transform.position.x) * 10f, (Enemy[i].transform.position.z - Player.transform.position.z) * 10f);
                Enemy_MiniMap[i].GetComponent<RectTransform>().anchoredPosition = pos;
                Enemy_MiniMap[i].GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, Enemy[i].transform.rotation.eulerAngles.y * -1);
                Enemy_MiniMap[i].GetComponent<Image>().color = new Color(1, 0, 0, LinePower);
            }
            else
            {
                //Destroy(Enemy[i]);
                Enemy.RemoveAt(i);
                Destroy(Enemy_MiniMap[i]);
                Enemy_MiniMap.RemoveAt(i);
            }
        }
        for (int i=0;i<Walls_MiniMap.Count;i++)
        {
            Vector2 pos = new Vector2((Walls[i].transform.position.x - Player.transform.position.x) * 10f, (Walls[i].transform.position.z - Player.transform.position.z) * 10f);
            Walls_MiniMap[i].GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }
}
