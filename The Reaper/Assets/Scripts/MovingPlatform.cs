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

    // Start is called before the first frame update
    void Start()
    {
        current = pointA;
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            Move();
        }
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, current.position, speed * Time.deltaTime);
        if (transform.position == current.position)
        {
            StartCoroutine(Stop(wait));
        }
    }

    IEnumerator Stop(int sec)
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
