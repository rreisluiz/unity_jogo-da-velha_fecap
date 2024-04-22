using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    private void OnEnable()
    {
        print("FIM DE JOGO");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            restartGame();
        }
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
