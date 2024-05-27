using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��� �� ����, ������ �ִ� �� ���� ����
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
        //��� �� üũ
        for (int i = 0; i < allEnemies.Length; i++)
        {
            Vector3 enemyPos = playerCamera.WorldToViewportPoint(allEnemies[i].transform.position);
            //�ڵ� ���� �ȿ� ���� ��� �� üũ, ī�޶�� ���� ����� �� üũ 
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
