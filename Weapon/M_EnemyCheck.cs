using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//모든 적 저장, 가까이 있는 적 추적 공격
public class M_EnemyCheck : MonoBehaviour
{
    public Camera playerCamera;
    GameObject[] allEnemies;
    GameObject nearEnemy;

    void Start()
    {
        allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Update()
    {
        //모든 적 체크
        for (int i = 0; i < allEnemies.Length; i++)
        {
            Vector3 enemyPos = playerCamera.WorldToViewportPoint(allEnemies[i].transform.position);
            //자동 에임 안에 들어온 모든 적 체크, 카메라와 제일 가까운 적 체크 
            if (enemyPos.x > 0.25f && enemyPos.x < 0.75f && enemyPos.y > 0.25f && enemyPos.y < 0.75f)
            {
                if (!nearEnemy)
                    nearEnemy = allEnemies[i];
                else if (Vector3.Distance(transform.position, allEnemies[i].transform.position) <=
                    Vector3.Distance(transform.position, nearEnemy.transform.position))
                {
                    nearEnemy = allEnemies[i];
                }
            }
        }
        if (nearEnemy)
        {
            Vector3 nearEnemyPos = playerCamera.WorldToViewportPoint(nearEnemy.transform.position);
            if (nearEnemyPos.x <= 0.25f || nearEnemyPos.x >= 0.75f || nearEnemyPos.y <= 0.25f || nearEnemyPos.y >= 0.75f)
            {
                nearEnemy = null;
            }
        }
    }
}
