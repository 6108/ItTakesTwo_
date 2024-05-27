using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_HoneyBullet : MonoBehaviour
{
    public GameObject honeyPrefab; //´Ù¸¥ ¿ÀºêÁ§Æ®¿¡ ºÙÀ» ²Ü, µå·¡±×·Î °¡Á®¿È
    public float speed = 5;
    Vector3 startPosition;
    Vector3 midPosition;
    Vector3 targetPosition;
    float distance;
    float startTime;

    void Start()
    {
        startPosition = transform.position;
        startTime = Time.time;
        Destroy(gameObject, 3);
        midPosition = Vector3.Lerp(startPosition, targetPosition, 0.5f) + new Vector3(0, 5f, 0);
        distance = Vector3.Distance(startPosition, targetPosition);
    }
    float time;
    void Update()
    {
        if (time >= 1f)
            Destroy(gameObject);
        time += Time.deltaTime;
        gameObject.transform.position = Bezier(startPosition, midPosition, targetPosition, time);
    }

    //²Ü ÃÑ¾Ë ³¯¾Æ°¡´Â °Í ÈÖ¾îÁö´Â ´À³¦
    public Vector3 Bezier(Vector3 p1, Vector3 p2, Vector3 p3, float value)
    {
        Vector3 A = Vector3.Lerp(p1, p2, value);
        Vector3 B = Vector3.Lerp(p2, p3, value);
        Vector3 C = Vector3.Lerp(A, B, value);

        return C;
    }

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GameObject honey = Instantiate(honeyPrefab, transform.position, Quaternion.identity);
            honey.transform.parent = collision.gameObject.transform;
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "BossBody")
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Honey")
        {
            collision.gameObject.GetComponent<M_Honey>().SizeUp();
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag != "InteractionObject")
        {
            GameObject honey = Instantiate(honeyPrefab, transform.position, Quaternion.identity);
            honey.transform.parent = collision.gameObject.transform.parent;
            Destroy(gameObject);
        }
        else
        {
            GameObject honey = Instantiate(honeyPrefab, transform.position, Quaternion.identity);
            honey.transform.parent = collision.transform;
            if (honey.transform.parent.GetComponent<M_Counter>())
                honey.transform.parent.GetComponent<M_Counter>().count++;
            Destroy(gameObject);
        }
    }
}
