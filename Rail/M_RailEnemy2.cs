using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class M_RailEnemy2 : MonoBehaviour
{
    public bool isMinus = false; //어느 방향인지
    public PathCreator curPath;
    EndOfPathInstruction end;
    public float speed = 5;
    public float distance = 0;

    void Update()
    {
        if (isMinus)
        {
            if (distance < 0.04)
            {

                Destroy(gameObject);
            }

            distance -= speed * Time.deltaTime / curPath.path.length;
        }
        else if (isMinus)
        {
            if (distance > 0.97)
            {
                Destroy(gameObject);
            }

            distance += speed * Time.deltaTime / curPath.path.length;
        }
        transform.position = curPath.path.GetPointAtTime(distance, end);
        transform.rotation = curPath.path.GetRotation(distance, end);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name.Contains("Player2"))
        {
            M_Player2.instance.Hit();
            M_Player2.instance.Hit();
        }
    }
}
