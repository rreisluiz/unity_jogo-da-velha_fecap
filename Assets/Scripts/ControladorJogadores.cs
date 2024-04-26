using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class ControladorJogadores : MonoBehaviour
{
    public Transform estiloJogadorAzul;
    public Transform estiloJogadorVermelho;
    public LayerMask layerEstilo;

    internal int jogadorAtual;

    GridP grid;
    Transform estilo;
    ControladorJogo controladorJogo;

    private void Awake()
    {
        grid = GameObject.Find("GridP").GetComponent<GridP>();
        controladorJogo = GameObject.Find("GameController").GetComponent<ControladorJogo>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            JogarTurno();
        }
    }

    private void JogarTurno()
    {
        // Obtem posição do mouse
        Vector3 posicaoMouse = Input.mousePosition;
        posicaoMouse.z = Camera.main.transform.position.z;

        Ray ray = Camera.main.ScreenPointToRay(posicaoMouse);

        RaycastHit infoColisao;

        // Verifica se o mouse atingiu algum objeto
        if (Physics.Raycast(ray, out infoColisao, 1000, layerEstilo))
        {
            // Pega a posição da celula selecionada na matriz através da posição dela no mundo 
            Node celulaSelecionada = grid.ObterNodePelaPosicaoMundo(infoColisao.transform.position);

            // Verifica se a celula já está ocupada
            if (!celulaSelecionada.estaOcupado)
            {
                // Cria e ocupa a celula com objeto do jogador
                CriaObjeto(celulaSelecionada);

                // Verifica se o jogo terminou
                controladorJogo.VerificaFimDeJogo(celulaSelecionada);
            }
        }
    }

    private void CriaObjeto(Node celulaSelecionada)
    {
        string objetoPaiJogador = string.Empty;

        switch (jogadorAtual)
        {
            case 1:
                estilo = estiloJogadorAzul;
                objetoPaiJogador = "BluePlayerObjects";
                break;
            case 2:
                estilo = estiloJogadorVermelho;
                objetoPaiJogador = "RedPlayerObjects";
                break;
        }

        Vector3 posicaoObjeto = new Vector3(celulaSelecionada.posicaoMundo.x, celulaSelecionada.posicaoMundo.y + 1, celulaSelecionada.posicaoMundo.z);

        GameObject novoObjetoJogador = (GameObject)Instantiate(estilo.gameObject, posicaoObjeto, estilo.transform.rotation);
        novoObjetoJogador.transform.parent = GameObject.Find(objetoPaiJogador).transform;

        celulaSelecionada.estaOcupado = true;
        celulaSelecionada.tipoObjeto = jogadorAtual;
    }
}
