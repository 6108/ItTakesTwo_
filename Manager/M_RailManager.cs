using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class M_RailManager : MonoBehaviour
{
    public PathCreator[] pathCreators;
    public GameObject p1PathObj1;
    public GameObject p1PathObj2;
    public GameObject p2PathObj1;
    public GameObject p2PathObj2;
    public int p1PathCount = -1;
    public int p2PathCount = -1;


    public static M_RailManager instance;

    private void Awake()
    {
        instance = this;
    }
    
    void Update()
    {
        if (M_Player.instance.isRail == false)
        {
            M_Player.instance.distance = 0;
            M_Player.instance.curPath = null;
            p1PathCount = -1;
            M_Player.instance.path1 = null;
            M_Player.instance.path2 = null;
        }
        if (M_Player2.instance.isRail == false)
        {
            M_Player2.instance.distance = 0;
            M_Player2.instance.curPath = null;
            p2PathCount = -1;
            M_Player2.instance.path1 = null;
            M_Player2.instance.path2 = null;
        }
    }

    public void p1PathCountUp()
    {
        p1PathCount++;
    }

    public void p2PathCountUp()
    {
        p2PathCount++;
    }

    public void SetP1PathObject1(GameObject po)
    {
        p1PathObj1 = po;
    }

    public void SetP1PathObject2(GameObject po)
    {
        p1PathObj2 = po;
    }
    public void SetP2PathObject1(GameObject po)
    {
        p2PathObj1 = po;
    }
    public void SetP2PathObject2(GameObject po)
    {
        p2PathObj2 = po;
    }
}
