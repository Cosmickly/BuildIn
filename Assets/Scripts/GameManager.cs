using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject endScreen;
    public TextMeshProUGUI endText;
    public GameObject restartButton;

    public BallMovement ball;
    public PaddleMovement paddle;

    public bool playing = false;

    private void Start() {
        endScreen.SetActive(false);
        startScreen.SetActive(true);
        restartButton.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R) && playing) {
            Restart();
        }
    }

    public void LoseGame() {
        endText.text = "You Lose!";
        EndGame();
    }

    public void WinGame() {
        endText.text = "You Win!";
        EndGame();
    }

    private void EndGame() {
        playing = false;
        endScreen.SetActive(true);
        ball.DestroyBall();
    }
    public void StartGame() {
        startScreen.SetActive(false);
        restartButton.SetActive(true);

        ball = Instantiate(ball, new Vector3(0, -1, 0), transform.rotation, transform);
        paddle = Instantiate(paddle, new Vector3(0, -3.5f, 0), transform.rotation, transform);

        playing = true;
    }

    public void Restart() {
        SceneManager.LoadScene(0);
    }

    public void Quit() {
        Application.Quit();
    }
}
