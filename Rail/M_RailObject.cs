using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class M_RailObject : MonoBehaviour
{
    public PathCreator path1;
    public PathCreator path2;
    PathCreator curPath;
    EndOfPathInstruction end;
    public float railSpeed = 5;
    float distance = 0;
    public bool isRail = false;
    CharacterController cc;
    public float yVelocity;

    public static M_RailObject instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (isRail)
        {
            //지금 길이 마지막 길이라면
            
            if (!curPath)
                curPath = path1;
            if (curPath.path.GetClosestTimeOnPath(transform.position) > 0.98f)
                isRail = false;
            if (path1)
            {
                if (path1.path.GetClosestTimeOnPath(transform.position) > 0.98f)
                    path1 = null;
            }
            if (path2)
            {
                if (path2.path.GetClosestTimeOnPath(transform.position) > 0.98f)
                {
                    path2 = null;
                }
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                if (curPath == path1)
                {
                    if (path2)
                    {
                        distance = path2.path.GetClosestTimeOnPath(transform.position);
                        curPath = path2;
                    }
                    else
                        isRail = false;
                }
                else if (curPath == path2)
                {
                    if (path1)
                    {
                        distance = path1.path.GetClosestTimeOnPath(transform.position);
                        curPath = path1;
                    }
                    else
                        isRail = false;
                }
            }
            distance += railSpeed * Time.deltaTime / curPath.path.length;
            transform.position = curPath.path.GetPointAtTime(distance, end);
            transform.rotation = curPath.path.GetRotation(distance, end);
            
        }
        else
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 dir = new Vector3(h, 0, v);
            yVelocity += -10 * Time.deltaTime;
            dir.y = yVelocity;
        }
       
    }
}
