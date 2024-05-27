using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Bridge : MonoBehaviour
{
    public float yAngle;
    public float zAngle;
    public float startAngle;
    public float minAngle;
    public float curAngle; 

    private void Update()
    {
        curAngle = startAngle - 2 * GetComponent<M_Counter>().count;
        if (curAngle < minAngle)
            curAngle = minAngle;
        if (curAngle > startAngle)
            curAngle = startAngle;
        transform.rotation = Quaternion.Euler(curAngle, yAngle, zAngle);
    }

    IEnumerator BridgeUp()
    {
        while (180 - transform.eulerAngles.z > curAngle)
        {
            yield return new WaitForSeconds(0.02f);
            transform.Rotate(0, 0, -0.5f);
        }
    }
}
