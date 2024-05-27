using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class M_Rail : MonoBehaviour
{
    PathCreator curPath;
    public int pathDir = 1;

    void Start()
    {
        curPath = GetComponent<PathCreator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player1")
        {
            M_RailManager.instance.p1PathCountUp();
            print(M_RailManager.instance.p1PathCount);
            if (!M_Player.instance.path1)
            {
                M_Player.instance.path1 = M_RailManager.instance.pathCreators[M_RailManager.instance.p1PathCount];
                print(M_Player.instance.path1);
            }
            else if (!M_Player.instance.path2)
            {
                M_Player.instance.path2 = M_RailManager.instance.pathCreators[M_RailManager.instance.p1PathCount];
                print(M_Player.instance.path2);
            }
            if (M_RailManager.instance.p1PathCount == 0)
            {
                M_Player.instance.isRail = true;
            }
        }
        if (other.name.Contains("Player2"))
        {
            M_RailManager.instance.p2PathCountUp();
            if (!M_Player2.instance.path1)
            {
                M_Player2.instance.path1 = M_RailManager.instance.pathCreators[M_RailManager.instance.p2PathCount];
                print(M_Player2.instance.path1);
            }
            else if (!M_Player2.instance.path2)
            {
                M_Player2.instance.path2 = M_RailManager.instance.pathCreators[M_RailManager.instance.p2PathCount];
                print(M_Player2.instance.path2);
            }
            if (M_RailManager.instance.p2PathCount == 0)
            {
                M_Player2.instance.isRail = true;
            }
        }
    }
}
