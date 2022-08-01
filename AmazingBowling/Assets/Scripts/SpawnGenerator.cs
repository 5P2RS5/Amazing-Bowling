using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnGenerator : MonoBehaviour
{
    public GameObject[] propPrefabs;
    
    BoxCollider area;
    
    public int count = 100;

    List<GameObject> props = new List<GameObject>();
    
    
    // Start is called before the first frame update
    void Start()
    {
        area = GetComponent<BoxCollider>();

        for (int i = 0; i < count; i++)
        {
            // 생성용 함수
            Spawn();
        }

        area.enabled = false; // props찍어내고 콜라이더 끄기
        
    }

    void Spawn()
    {
        int selection = Random.Range(0, propPrefabs.Length);

        GameObject selectPrefab = propPrefabs[selection];

        Vector3 spawnPos = GetRandomPosition();

        GameObject instance = Instantiate(selectPrefab, spawnPos, Quaternion.identity); // Quaternion.identity : 프리펩의 본래 회전값
        props.Add(instance); // 리스트에 등록
        // 리스트에 등록하는 이유는 매번 오브젝트를 생성, 삭제, 생성, 삭제하면 게임의 과부화가 발생할 수 있으므로.
        // 삭제 대신에 오브젝트를 disalbe하는 식으로 한다.
    }

    Vector3 GetRandomPosition() // 스폰할 랜덤위치 리턴
    {
        Vector3 basePos = transform.position; // 기본 위치
        Vector3 size = area.size;
        // 박스 콜라이더 영역 내의 위치에 랜덤하게 생성한다.
        float posX = basePos.x + Random.Range(-size.x / 2, size.x / 2);
        float posY = basePos.y + Random.Range(-size.y / 2, size.y / 2);
        float posZ = basePos.z + Random.Range(-size.z / 2, size.z / 2);

        Vector3 spawnPos = new Vector3(posX, posY, posZ);

        return spawnPos;
    }

    public void Reset() // 라운드가 넘어갈때마다 prop의 위치들 재수정
    {
        for (int i = 0; i < props.Count; i++)
        {
            props[i].transform.position = GetRandomPosition();
            props[i].SetActive(true);
        }
    }
}
