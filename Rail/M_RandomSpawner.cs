using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class M_RandomSpawner : MonoBehaviour
{
    public PathCreator[] pathCreators;
    public GameObject railEnemyPrefab;
    public GameObject railEnemyPrefab2;
    float time = 0;
    int randomPath = 0;
    float randomPos = 0.5f;
    int randomPlayer = 1;
    int miuns = 1;
    bool isMinus = false;

    void Update()
    {
        time += Time.deltaTime;
        if (time > 4)
            Spawn();
    }

    void Spawn()
    {
        time = 0;
        randomPath = Random.Range(0, pathCreators.Length);
        randomPos = Random.Range(0f, 1f);
        randomPlayer = Random.Range(1, 3);
        if (Random.Range(1, 3) == 1)
        {
            isMinus = true;
        }
        else
            isMinus = true;
        if (randomPlayer == 1)
        {
            GameObject railEnemy = Instantiate(railEnemyPrefab);
            if (isMinus)
            {
                railEnemy.GetComponent<M_RailEnemy>().isMinus = true;
                railEnemy.GetComponent<M_RailEnemy>().distance = randomPos;
            }
            else
            {
                railEnemy.GetComponent<M_RailEnemy>().isMinus = false;
                railEnemy.GetComponent<M_RailEnemy>().distance = randomPos;
            }
            railEnemy.GetComponent<M_RailEnemy>().curPath = pathCreators[randomPath];
        }
        else if (randomPlayer == 2)
        {
            GameObject railEnemy = Instantiate(railEnemyPrefab2);
            if (isMinus)
            {
                railEnemy.GetComponent<M_RailEnemy2>().isMinus = true;
                railEnemy.GetComponent<M_RailEnemy2>().distance = randomPos;
            }
            else
            {
                railEnemy.GetComponent<M_RailEnemy2>().isMinus = false;
                railEnemy.GetComponent<M_RailEnemy2>().distance = randomPos;
            }
            railEnemy.GetComponent<M_RailEnemy2>().curPath = pathCreators[randomPath];
        }
    }
}
