using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform pointA, pointB;
    [SerializeField] private float speed;
    [SerializeField] private int wait;
    private Transform current;
    private bool isMoving;
    [SerializeField] private bool isAutomatic;

    // Start is called before the first frame update
    void Start()
    {
        current = pointB;
        if (isAutomatic)
        {
            isMoving = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            Move();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            if (!isAutomatic && collision.transform.position.y > transform.position.y)
            {
                isMoving = true;
            }
        }
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, current.position, speed * Time.deltaTime);
        if (transform.position == current.position)
        {
            if (isAutomatic)
            {
                StartCoroutine(WaitStop(wait));
            }
            else
            {
                isMoving = false;
                if (current == pointA)
                {
                    current = pointB;
                }
                else
                {
                    current = pointA;
                }
            }
        }
    }

    IEnumerator WaitStop(int sec)
    {
        isMoving = false;
        yield return new WaitForSeconds(sec);
        if (current == pointA)
        {
            current = pointB;
        }
        else
        {
            current = pointA;
        }
        isMoving = true;
    }
}
