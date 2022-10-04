using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHandeler : MonoBehaviour
{
    private List<Key> _keys = new List<Key>();

    public void addKey(Key key)
    {
        _keys.Add(key);
    }

    public bool useKey(int id, Transform door)
    {
        for (int i = 0; i < _keys.Count; i++)
        {
            if (_keys[i].Id == id)
            {
                _keys[i].kill(door);
                _keys.RemoveAt(i);
                return true;
            }
        }
        return false;
    }
}
