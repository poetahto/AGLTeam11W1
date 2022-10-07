using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField]
    private int id;

    [SerializeField]
    private Transform keyFollowPoint;

    [SerializeField]
    private GameObject killEffect;

    [SerializeField]
    private float killAfter = .6f;

    private float _lerp = 2;

    private bool _collected = false;

    float _killTime = -1;

    public int Id => id;

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if ( other.TryGetComponent<KeyHandeler>(out KeyHandeler keyHandler))
        {
            keyHandler.addKey(this);
            _collected = true;
        }
    }

    public void kill(Transform door)
    {
        keyFollowPoint = door;
        _killTime = Time.time;
    }

    void Update()
    {
        if (_collected)
        {
            this.transform.position = Vector3.Lerp(this.transform.position,
                keyFollowPoint.position + ((_killTime != -1) ? new Vector3(0,0,0) : new Vector3(-1f*id,.25f*id,0)),
                _lerp * Time.deltaTime);

            if (_killTime != -1 && Time.time > _killTime + killAfter)
            {
                Instantiate(killEffect).transform.position = this.transform.position;
                Destroy(this.gameObject);
            }
        }

        
        
    }
}
