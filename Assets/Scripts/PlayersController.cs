using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class PlayersController : MonoBehaviour
{
    public Transform bluePlayerShape;
    public Transform redPlayerShape;
    public LayerMask shapeLayer;

    internal int currentPlayer;

    GridP grid;
    Transform shape;
    GameController gameController;

    private void Awake()
    {
        grid = GameObject.Find("GridP").GetComponent<GridP>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MakePlayerTurn();
        }
    }

    private void MakePlayerTurn()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.z;

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 1000, shapeLayer))
        {
            Node selectedNode = grid.GetNodeFromWorldPosition(hitInfo.transform.position);

            if (!selectedNode.isOccupied)
            {
                SpawnObject(selectedNode);
                gameController.CheckGameFinish(selectedNode);
            }
        }
    }

    private void SpawnObject(Node _selectedNode)
    {
        string playerParentObject = string.Empty;

        switch (currentPlayer)
        {
            case 1:
                shape = bluePlayerShape;
                playerParentObject = "BluePlayerObjects";
                break;
            case 2:
                shape = redPlayerShape;
                playerParentObject = "RedPlayerObjects";
                break;
        }

        Vector3 pointPos = new Vector3(_selectedNode.worldPosition.x, _selectedNode.worldPosition.y + 1, _selectedNode.worldPosition.z);

        GameObject newPlayerObject = (GameObject)Instantiate(shape.gameObject, pointPos, shape.transform.rotation);
        newPlayerObject.transform.parent = GameObject.Find(playerParentObject).transform;

        _selectedNode.isOccupied = true;
        _selectedNode.objectType = currentPlayer;
    }
}
