using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool estaOcupado;
    public Vector3 posicaoMundo;
    public int tipoObjeto = -1;

    public GameObject gameObject;

    public Vector2 posicaoMatriz { get; set; }

    public Node(Vector3 _worldPosition, Vector2 _objectIndex, bool _isOccupied) { 
        posicaoMundo = _worldPosition;
        posicaoMatriz = _objectIndex;
        estaOcupado = _isOccupied;
    }
}
