using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorControler : MonoBehaviour
{
    [SerializeField]
    private int id;

    [SerializeField]
    private float doorSpeed;

    [SerializeField]
    private float moveDistance;

    [SerializeField]
    private Transform doorTransform;

    [Space(20)]
    [SerializeField]
    private UnityEvent keyTrigger;

    private bool _open = false;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.TryGetComponent<KeyHandeler>(out KeyHandeler keyHandler))
        {
            if (keyHandler.useKey(id, this.transform))
            {
                keyTrigger.Invoke(); 
            }
        }
    }

    public void openDoor()
    {
        _open = true;
    }


    private void Update()
    {
        if (_open && doorTransform.position.y < this.transform.position.y + moveDistance)
        {
            doorTransform.position += new Vector3(0, Time.deltaTime * doorSpeed, 0);
        }
    }
}
