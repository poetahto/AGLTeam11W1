using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpriteGroup : ScriptableObject
{
    [SerializeField] private List<Sprite> sprites;

    public IReadOnlyList<Sprite> Sprites => sprites;
}