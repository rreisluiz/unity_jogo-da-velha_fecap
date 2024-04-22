using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool isOccupied;
    public Vector3 worldPosition;
    public int objectType = -1;

    public GameObject gameObject;

    public Vector2 index { get; set; }

    public Node(Vector3 _worldPosition, Vector2 _objectIndex, bool _isOccupied) { 
        worldPosition = _worldPosition;
        index = _objectIndex;
        isOccupied = _isOccupied;
    }
}
