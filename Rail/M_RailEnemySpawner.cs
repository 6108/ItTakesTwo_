using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class M_RailEnemySpawner : MonoBehaviour
{
    public bool isMinus = false;
    public GameObject railEnemyPrefab;
    public GameObject railEnemyPrefab2;
    public PathCreator path;

    public void Spawn(int i)
    {
        if (i == 1)
        {
            GameObject railEnemy = Instantiate(railEnemyPrefab);
            if (isMinus)
            {
                railEnemy.GetComponent<M_RailEnemy>().isMinus = true;
                railEnemy.GetComponent<M_RailEnemy>().distance = 1;
            }
            else
            {
                railEnemy.GetComponent<M_RailEnemy>().isMinus = false;
                railEnemy.GetComponent<M_RailEnemy>().distance = 0;
            }
            railEnemy.GetComponent<M_RailEnemy>().curPath = path;
        }
        else if (i == 2)
        {
            GameObject railEnemy = Instantiate(railEnemyPrefab2);
            if (isMinus)
            {
                railEnemy.GetComponent<M_RailEnemy2>().isMinus = true;
                railEnemy.GetComponent<M_RailEnemy2>().distance = 1;
            }
            else
            {
                railEnemy.GetComponent<M_RailEnemy2>().isMinus = false;
                railEnemy.GetComponent<M_RailEnemy2>().distance = 0;
            }
            railEnemy.GetComponent<M_RailEnemy2>().curPath = path;
        }
        
    }
}
