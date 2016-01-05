using UnityEngine;
using System.Collections;

[System.Serializable]
public class Ground {

    public enum Type {
        NULL,
        GRASSLAND,
        DESERT,
        BEACH,
        SEA
    }
    public Type _type;

    [Range(0, 1)]
    public float _mana;

    [Range(0, 1)]
    public float _fertility;

    [Range(0, 1)]
    public float _solidity;

    public sbyte _altitude;

}
