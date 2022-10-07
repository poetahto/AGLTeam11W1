using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayBridge : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Collider2D platform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            platform.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            platform.enabled = false;
        }
    }
}
