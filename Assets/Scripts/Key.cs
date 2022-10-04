using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField]
    private int id;

    private bool _collected = false;

    public int Id => id;

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if ( other.TryGetComponent<KeyHandeler>(out KeyHandeler keyHandler))
        {
            keyHandler.addKey(this);
            _collected = true;
        }
    }

    void Update()
    {
        if (_collected)
        {

        }
        
    }
}
