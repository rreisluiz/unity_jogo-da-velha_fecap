using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject gridP;
    public GameObject gameOverScreen;

    Node[,] grid;
    Vector2 gridSize;
    PlayersController playersController;
    int currentPlayer;

    List<Node> validNodes;

    private void Start()
    {
        grid = gridP.GetComponent<GridP>().GetNodeGrid();
        gridSize = gridP.GetComponent<GridP>().gridWorldSize;

        playersController = GameObject.Find("PlayersController").GetComponent<PlayersController>();
        playersController.currentPlayer = Random.Range(1, 3);
        currentPlayer = playersController.currentPlayer;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void CheckGameFinish(Node _selectedNode)
    {
        int nodeIndexX = (int) _selectedNode.index.x;
        int nodeIndexY = (int) _selectedNode.index.y;

        bool gameFinished = CheckObjectsInColumn(nodeIndexX) || CheckObjectsInRow(nodeIndexY) || CheckObjectsInDiagonal(nodeIndexX, nodeIndexY);

        if (gameFinished)
        {
            EndGame();
        }
        else
        {
            if (allCellsFilled(nodeIndexX, nodeIndexY))
            {
                EndGame();
            }
            ChangePlayer();
        }
    }

    private bool allCellsFilled(int nodeIndexX, int nodeIndexY)
    {
        foreach (Node node in grid)
        {
            if (!node.isOccupied) return false;
        }
        return true;
    }

    private bool CheckObjectsInColumn(int _x)
    {
        List<Node> currentPlayerNodes = new List<Node>();

        for (int i = 0; i < gridSize.x; i++)
        {
            Node currentNode = grid[_x, i];

            if (currentNode.isOccupied)
            {
                if (currentNode.objectType == currentPlayer)
                {
                    currentPlayerNodes.Add(currentNode);
                }
            } 
            else
            {
                return false;
            }
        }
        if (currentPlayerNodes.Count == gridSize.x) 
        {
            validNodes = currentPlayerNodes;
            return true;
        }

        return false;
    }

    private bool CheckObjectsInRow(int _y)
    {
        List<Node> currentPlayerNodes = new List<Node>();

        for (int i = 0; i < gridSize.y; i++)
        {
            Node currentNode = grid[i, _y];

            if (currentNode.isOccupied)
            {
                if (currentNode.objectType == currentPlayer)
                {
                    currentPlayerNodes.Add(currentNode);
                }
            }
            else
            {
                return false;
            }
        }
        if (currentPlayerNodes.Count == gridSize.y)
        {
            validNodes = currentPlayerNodes;
            return true;
        }

        return false;
    }

    private bool CheckObjectsInDiagonal(int _x, int _y)
    {
        List<Node> currentPlayerNodes = new List<Node>();

        if (_x == _y)
        {
            for (int i = 0; i < gridSize.x; i++)
            {
                Node currentNode = grid[i, i];

                if (currentNode.objectType == currentPlayer)
                {
                    currentPlayerNodes.Add(currentNode);
                }
            }
        }

        if (currentPlayerNodes.Count == gridSize.x)
        {
            validNodes = currentPlayerNodes;
            return true;
        }
        else
        {
            currentPlayerNodes.Clear();
        }

        int maxGridSize = (int) gridSize.x - 1;

        if (_x + _y == maxGridSize)
        {
            for (int i = 0; i < gridSize.x; i++)
            {
                Node currentNode = grid[i, maxGridSize - i];

                if (currentNode.objectType == currentPlayer)
                {
                    currentPlayerNodes.Add(currentNode);
                }
            }
        }

        if (currentPlayerNodes.Count == gridSize.x)
        {
            validNodes = currentPlayerNodes;
            return true;
        }

        return false;
    }

    private void EndGame()
    {
        if (validNodes != null)
        {
            foreach (Node node in validNodes)
            {
                string quad;

                if (node.objectType == 1)
                {
                    quad = "BlueQuad";
                }
                else
                {
                    quad = "RedQuad";
                }

                Material material = node.gameObject.transform.Find(quad).gameObject.GetComponent<Renderer>().material;
                material.SetColor("_EmissionColor", Color.yellow);
            }
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gameOverScreen.SetActive(true);
    }

    private void ChangePlayer()
    {
        int nextPlayer = (currentPlayer == 1) ? 2 : 1;
        playersController.currentPlayer = nextPlayer;
        currentPlayer = nextPlayer;
    }
}
