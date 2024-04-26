using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorJogo : MonoBehaviour
{
    public GameObject gridP;
    public GameObject gameOverScreen;

    Node[,] grid;
    Vector2 tamanhoGrid;
    ControladorJogadores controladorJogadores;
    int jogadorAtual;

    List<Node> celulasValidas;

    private void Start()
    {
        grid = gridP.GetComponent<GridP>().ObterNodeGrid();
        tamanhoGrid = gridP.GetComponent<GridP>().tamanhoGrid;

        controladorJogadores = GameObject.Find("PlayersController").GetComponent<ControladorJogadores>();
        controladorJogadores.jogadorAtual = Random.Range(1, 3); // Gera um n�mero aleat�rio entre 1 e 2 que define o jogador / 1 = Azul, 2 = Vermelho
        jogadorAtual = controladorJogadores.jogadorAtual; // Guarda o valor do jogador na vari�vel global
    }

    private void Update()
    {
        // Sai do jogo ao clicar ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void VerificaFimDeJogo(Node celulaSelecionada)
    {
        // Obt�m a posi��o na matriz da celula selecionada
        int posicaoCelulaX = (int) celulaSelecionada.posicaoMatriz.x;
        int posicaoCelulaY = (int) celulaSelecionada.posicaoMatriz.y;

        // Verifica coluna, linha e diagonal para saber se o jogo terminou
        bool jogoTerminou = VerificaObjetosNaColuna(posicaoCelulaX) || VerificaObjetosNaLinha(posicaoCelulaY) || VerificaObjetosNaDiagonal(posicaoCelulaX, posicaoCelulaY);

        // Verifica se jogo terminou
        if (jogoTerminou)
        {
            // Termina jogo
            FimDeJogo();
        }
        else
        {
            // Verifica se todas as celulas estao ocupadas
            if (TodasCelulasOcupadas())
            {
                // Termina jogo
                FimDeJogo();
            }

            // Troca o turno do jogador
            TrocarJogador();
        }
    }

    private bool TodasCelulasOcupadas()
    {
        // Percorre todas as celulas do grid
        foreach (Node celula in grid)
        {
            // Retorna falso se alguma das celulas n�o est� ocupada
            // Entende-se que todas as c�lulas devem estar ocupadas para o jogo finalizar
            if (!celula.estaOcupado) return false;
        }
        return true;
    }

    private bool VerificaObjetosNaColuna(int _x)
    {
        // Inicia lista vazia que vai armazenar as celulas ocupadas/validas
        List<Node> celulasJogadorAtual = new List<Node>();

        // Percorre as c�lulas da coluna do objeto selecionado, trvando o eixo X e alterando o eixo Y
        // Ex: (0,0), (0,1), (0,2)
        for (int i = 0; i < tamanhoGrid.x; i++)
        {
            Node celulaAtual = grid[_x, i];

            // Verifica se a celula est� ocupada
            if (celulaAtual.estaOcupado)
            {
                // Verifica se o tipo da celula � igual ao jogador atual
                if (celulaAtual.tipoObjeto == jogadorAtual)
                {
                    // Adiciona a celula verificada na lista de celulas ocupadas/validas
                    celulasJogadorAtual.Add(celulaAtual);
                }
                else
                {
                    return false;
                }
            } 
            else
            {
                // Caso alguma celula n�o esteja ocupada, retorna Falso
                // Entende-se que se uma coluna n�o est� totalmente preenchida, o jogo n�o terminou
                return false;
            }
        }

        // Verifica se a quantidade de c�lulas validas � igual ao tamanho total da coluna do Grid
        if (celulasJogadorAtual.Count == tamanhoGrid.x) 
        {
            // Retorna Verdadeiro
            print("Ganhou na COLUNA");
            celulasValidas = celulasJogadorAtual;
            return true;
        }

        return false;
    }

    private bool VerificaObjetosNaLinha(int _y)
    {
        // Inicia lista vazia que vai armazenar as celulas ocupadas/validas
        List<Node> celulasJogadorAtual = new List<Node>();

        // Percorre as c�lulas da coluna do objeto selecionado, trvando o eixo Y e alterando o eixo X
        // Ex: (0,0), (1,0), (2,0)
        for (int i = 0; i < tamanhoGrid.y; i++)
        {
            Node celulaAtual = grid[i, _y];

            // Verifica se a celula est� ocupada
            if (celulaAtual.estaOcupado)
            {
                // Verifica se o tipo da celula � igual ao jogador atual
                if (celulaAtual.tipoObjeto == jogadorAtual)
                {
                    // Adiciona a celula verificada na lista de celulas ocupadas/validas
                    celulasJogadorAtual.Add(celulaAtual);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // Caso alguma celula n�o esteja ocupada, retorna Falso
                // Entende-se que se uma coluna n�o est� totalmente preenchida, o jogo n�o terminou
                return false;
            }
        }

        // Verifica se a quantidade de c�lulas validas � igual ao tamanho total da coluna do Grid
        if (celulasJogadorAtual.Count == tamanhoGrid.y)
        {
            // Retorna Verdadeiro
            print("Ganhou na LINHA");
            celulasValidas = celulasJogadorAtual;
            return true;
        }

        return false;
    }

    private bool VerificaObjetosNaDiagonal(int _x, int _y)
    {
        // Inicia lista vazia que vai armazenar as celulas ocupadas/validas
        List<Node> celulasJogadorAtual = new List<Node>();

        // Verifica em qual diagonal a c�lula selecionada est�
        // A posi��o X e Y das c�lulas na diagonal que come�a na inferior esquerda sempre ser�o iguais
        // Ex: (0,0), (1,1), (2,2)
        if (_x == _y)
        {
            // Percorre a diagonal sempre incrementando os dois valores, X e Y
            for (int i = 0; i < tamanhoGrid.x; i++)
            {
                Node celulaAtual = grid[i, i];

                // Verifica se a celula est� ocupada
                if (celulaAtual.estaOcupado)
                {
                    // Verifica se o tipo da celula � igual ao jogador atual
                    if (celulaAtual.tipoObjeto == jogadorAtual)
                    {
                        // Adiciona a celula verificada na lista de celulas ocupadas/validas
                        celulasJogadorAtual.Add(celulaAtual);
                    }
                }
            }
        }

        // Verifica se a quantidade de c�lulas validas � igual ao tamanho total da coluna do Grid
        if (celulasJogadorAtual.Count == tamanhoGrid.x)
        {
            // Retorna Verdadeiro
            print("Ganhou na DIAGONAL PRINCIPAL");
            celulasValidas = celulasJogadorAtual;
            return true;
        }
        else
        {
            // Se a quantidade for diferente, limpa a lista
            celulasJogadorAtual.Clear();
        }

        int tamanhoMaximoGrid = (int) tamanhoGrid.x - 1;

        // Verifica em qual diagonal a c�lula selecionada est�
        // A soma das posi��es X e Y das c�lulas na diagonal que come�a na superior esquerda sempre ser�o iguais ao valor m�ximo de posi��o X ou Y
        // Ex: (0,2), (1,1), (2,0) / A Soma dos eixos X e Y sempre ser� 2 
        if (_x + _y == tamanhoMaximoGrid)
        {
            // Percorre a diagonal, incrementando o valor X e definindo Y como sendo ( posi��o m�xima - contador (i) )
            for (int i = 0; i < tamanhoGrid.x; i++)
            {
                Node celulaAtual = grid[i, tamanhoMaximoGrid - i];

                // Verifica se a celula est� ocupada
                if (celulaAtual.estaOcupado)
                {
                    // Verifica se o tipo da celula � igual ao jogador atual
                    if (celulaAtual.tipoObjeto == jogadorAtual)
                    {
                        // Adiciona a celula verificada na lista de celulas ocupadas/validas
                        celulasJogadorAtual.Add(celulaAtual);
                    }
                }
            }
        }

        // Verifica se a quantidade de c�lulas validas � igual ao tamanho total da coluna do Grid
        if (celulasJogadorAtual.Count == tamanhoGrid.x)
        {
            // Retorna Verdadeiro
            print("Ganhou na OUTRA DIAGONAL");
            celulasValidas = celulasJogadorAtual;
            return true;
        }

        return false;
    }

    private void FimDeJogo()
    {
        if (celulasValidas != null)
        {
            foreach (Node node in celulasValidas)
            {
                string quad;

                if (node.tipoObjeto == 1)
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

    private void TrocarJogador()
    {
        // Verifica qual � o jogador atual e muda o valor para o outro jogador
        int proximoJogador = (jogadorAtual == 1) ? 2 : 1;
        controladorJogadores.jogadorAtual = proximoJogador;
        jogadorAtual = proximoJogador;
    }
}
