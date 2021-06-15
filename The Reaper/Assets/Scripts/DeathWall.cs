using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWall : MonoBehaviour
{
    [SerializeField] private Transform checkpoint;
    [SerializeField] private Transform player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            player.position = checkpoint.position;
        }
    }
}
