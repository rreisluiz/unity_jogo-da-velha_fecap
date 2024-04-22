using UnityEngine;

public class GridP : MonoBehaviour
{
    public Transform tile;
    public Vector2 gridWorldSize;
    public Vector2 cellSize;
    public LayerMask gridMask;

    public Transform selectionTileBlue;
    public Transform selectionTileRed;

    PlayersController playersController;

    Node[,] grid;

    public Node[,] GetNodeGrid()
    {
        return grid;
    }

    private void Awake()
    {
        grid = new Node[(int) gridWorldSize.x, (int) gridWorldSize.y];
    }

    private void Start()
    {
        playersController = GameObject.Find("PlayersController").GetComponent<PlayersController>();
        CreateGrid();
    }

    void CreateGrid()
    {
        Vector2 realGridSize = gridWorldSize * cellSize;

        for (int x = 0; x < gridWorldSize.x; x++)
        {
            for (int z = 0; z < gridWorldSize.y; z++)
            {
                float pointX = x * cellSize.x - ((realGridSize.x - cellSize.x) / 2);
                float pointZ = z * cellSize.y - ((realGridSize.y - cellSize.y) / 2);

                Vector3 worldPoint = new Vector3(pointX, 0.5f, pointZ);

                Node currentNode = new Node(worldPoint, new Vector2(x, z), false);

                grid[x, z] = currentNode;

                if (grid != null)
                {
                    SpawnTile(currentNode);
                }
            }
        }
    }

    private void SpawnTile(Node currentNode)
    {
        if (currentNode != null)
        {
            GameObject newTile = (GameObject)Instantiate(tile.gameObject, currentNode.worldPosition, Quaternion.identity);
            currentNode.gameObject = newTile;
            newTile.transform.parent = GameObject.Find("GridTiles").transform;
        }
    }

    public Node GetNodeFromWorldPosition(Vector3 worldPosition)
    {
        float percentX = Mathf.Clamp01((worldPosition.x + (gridWorldSize.x * cellSize.x) / 2) / (gridWorldSize.x * cellSize.x));
        float percentZ = Mathf.Clamp01((worldPosition.z + (gridWorldSize.y * cellSize.y) / 2) / (gridWorldSize.y * cellSize.y));

        int x = Mathf.RoundToInt((gridWorldSize.x - 1) * percentX);
        int y = Mathf.RoundToInt((gridWorldSize.y - 1) * percentZ);

        return grid[x, y];
    }


    private void OnDrawGizmos()
    {
        //if (grid != null)
        //{
        //    Vector3 mousePos = Input.mousePosition;
        //    mousePos.z = Camera.main.transform.position.z;

        //    Vector3 dataPosition = new Vector3();

        //    Ray ray = Camera.main.ScreenPointToRay(mousePos);

        //    RaycastHit hitInfo;
        //    bool isHit = Physics.Raycast(ray, out hitInfo, 1000, gridMask);

        //    if (isHit)
        //    {
        //        dataPosition = hitInfo.transform.position;
        //    }

        //    foreach (Node node in grid)
        //    {
        //        if (isHit)
        //        {
        //            string quad;

        //            if (playersController.currentPlayer == 1)
        //            {
        //                quad = "BlueQuad";
        //            }
        //            else
        //            {
        //                quad = "RedQuad";
        //            }

        //            if (GetNodeFromWorldPosition(dataPosition) == node)
        //            {
        //                Gizmos.color = (node.isOccupied) ? Color.red : Color.green;
        //                Gizmos.DrawWireCube(node.worldPosition, new Vector3(cellSize.x - 0.1f, 0.05f, cellSize.y - 0.1f));

        //                if (!node.isOccupied)
        //                {
        //                    node.gameObject.transform.Find(quad).gameObject.SetActive(true);
        //                }
        //            } 
        //            else
        //            {
        //                if (!node.isOccupied)
        //                {
        //                    node.gameObject.transform.Find(quad).gameObject.SetActive(false);
        //                }
        //            }
        //        }
        //    }
        //}
    }

    private void Update()
    {
        if (grid != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.transform.position.z;

            Vector3 dataPosition = new Vector3();

            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            RaycastHit hitInfo;
            bool isHit = Physics.Raycast(ray, out hitInfo, 1000, gridMask);

            if (isHit)
            {
                dataPosition = hitInfo.transform.position;
            }

            foreach (Node node in grid)
            {
                if (isHit)
                {
                    string quad;

                    if (playersController.currentPlayer == 1)
                    {
                        quad = "BlueQuad";
                    }
                    else
                    {
                        quad = "RedQuad";
                    }

                    if (GetNodeFromWorldPosition(dataPosition) == node)
                    {
                        if (!node.isOccupied)
                        {
                            node.gameObject.transform.Find(quad).gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        if (!node.isOccupied)
                        {
                            node.gameObject.transform.Find(quad).gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }
}
