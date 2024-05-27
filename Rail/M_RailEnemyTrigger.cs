using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_RailEnemyTrigger : MonoBehaviour
{
    public GameObject railEnemySpawner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Player1"))
        {
            railEnemySpawner.GetComponent<M_RailEnemySpawner>().Spawn(1);
        }
        if (other.name.Contains("Player2"))
        {
            railEnemySpawner.GetComponent<M_RailEnemySpawner>().Spawn(2);
        }
    }
}
