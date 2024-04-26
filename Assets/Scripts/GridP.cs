using UnityEngine;

public class GridP : MonoBehaviour
{
    public Transform estiloDaCelula;
    public Vector2 tamanhoGrid;
    public Vector2 tamanhoCelula;
    public LayerMask gridMask;

    public Transform estiloSelecaoJogadorAzul;
    public Transform estiloSelecaoJogadorVermelho;

    ControladorJogadores controladorJogadores;

    Node[,] grid;

    public Node[,] ObterNodeGrid()
    {
        return grid;
    }

    private void Awake()
    {
        grid = new Node[(int) tamanhoGrid.x, (int) tamanhoGrid.y];
    }

    private void Start()
    {
        controladorJogadores = GameObject.Find("PlayersController").GetComponent<ControladorJogadores>();
        IniciarGrid();
    }

    void IniciarGrid()
    {
        // Calcula o tamanho total do grid
        Vector2 tamanhoRealGrid = tamanhoGrid * tamanhoCelula;

        // Inicia laço de repetição de acordo com o eixo X
        for (int x = 0; x < tamanhoGrid.x; x++)
        {
            // Inicia laço de repetição de acordo com o eixo Y
            for (int z = 0; z < tamanhoGrid.y; z++)
            {
                // Calcula as posições X e Y da celula a ser criada no mundo
                float posicaoX = x * tamanhoCelula.x - ((tamanhoRealGrid.x - tamanhoCelula.x) / 2);
                float posicaoZ = z * tamanhoCelula.y - ((tamanhoRealGrid.y - tamanhoCelula.y) / 2);

                // Armazena em um vetor de 3 posições
                Vector3 posicaoMundo = new Vector3(posicaoX, 0.5f, posicaoZ);
                
                // Cria um novo objeto 
                Node novoNode = new Node(posicaoMundo, new Vector2(x, z), false);

                // Guarda o objeto na Matriz, na posição selecionada 
                grid[x, z] = novoNode;

                if (grid != null)
                {
                    // Cria nova célula no mundo
                    CriarCelula(novoNode);
                }
            }
        }
    }

    private void CriarCelula(Node node)
    {
        if (node != null)
        {
            GameObject novaCelula = (GameObject) Instantiate(estiloDaCelula.gameObject, node.posicaoMundo, Quaternion.identity);
            node.gameObject = novaCelula;
            novaCelula.transform.parent = GameObject.Find("GridTiles").transform;
        }
    }

    public Node ObterNodePelaPosicaoMundo(Vector3 posicaoMundo)
    {
        // Calcula posicão da celula de acordo com a posição no mundo selecionada
        float porcentagemX = Mathf.Clamp01((posicaoMundo.x + (tamanhoGrid.x * tamanhoCelula.x) / 2) / (tamanhoGrid.x * tamanhoCelula.x));
        float porcentagemY = Mathf.Clamp01((posicaoMundo.z + (tamanhoGrid.y * tamanhoCelula.y) / 2) / (tamanhoGrid.y * tamanhoCelula.y));

        int x = Mathf.RoundToInt((tamanhoGrid.x - 1) * porcentagemX);
        int y = Mathf.RoundToInt((tamanhoGrid.y - 1) * porcentagemY);

        // Acessa a matriz e retorna a celula selecionada
        return grid[x, y];
    }

    private void Update()
    {
        if (grid != null)
        {
            Vector3 posicaoMouse = Input.mousePosition;
            posicaoMouse.z = Camera.main.transform.position.z;

            Vector3 posicaoColisaoMouse = new Vector3();

            Ray ray = Camera.main.ScreenPointToRay(posicaoMouse);

            RaycastHit informacaoColisao;
            bool colidiu = Physics.Raycast(ray, out informacaoColisao, 1000, gridMask);

            if (colidiu)
            {
                posicaoColisaoMouse = informacaoColisao.transform.position;
            }

            foreach (Node node in grid)
            {
                if (colidiu)
                {
                    string quad;

                    if (controladorJogadores.jogadorAtual == 1)
                    {
                        quad = "BlueQuad";
                    }
                    else
                    {
                        quad = "RedQuad";
                    }

                    if (ObterNodePelaPosicaoMundo(posicaoColisaoMouse) == node)
                    {
                        if (!node.estaOcupado)
                        {
                            node.gameObject.transform.Find(quad).gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        if (!node.estaOcupado)
                        {
                            node.gameObject.transform.Find(quad).gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }
}
