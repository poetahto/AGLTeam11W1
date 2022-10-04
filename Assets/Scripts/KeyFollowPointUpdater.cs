using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFollowPointUpdater : MonoBehaviour
{
    [SerializeField]
    Collider2D KeyFollowPoint;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == KeyFollowPoint)
        {
            this.transform.position = KeyFollowPoint.transform.position;
        }
    }
}
