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

    public Vector3 ballRespawn;
    public Ball ball;
    public Paddle paddle;
    public float paddleSpeed;

    public bool playing = false;

    public Dictionary<string, Color32> pico8Palette = new() {
        {"black", new Color32(0,0,0,255)},
        {"dark-blue", new Color32(29,43,83,255)},
        {"dark-purple ", new Color32(126,37,83, 255)},
        {"dark-green ", new Color32(0,135,81, 255)},
        {"brown", new Color32(171,82,54, 255)},
        {"dark-grey ", new Color32(95,87,79, 255)},
        {"light-grey ", new Color32(194,195,199, 255)},
        {"white", new Color32(255,241,232, 255)},
        {"red", new Color32(255,0,77, 255)},
        {"orange", new Color32(255,163,0, 255)},
        {"yellow", new Color32(255,236,39, 255)},
        {"green", new Color32(0,228,54, 255)},
        {"blue", new Color32(41,173,255, 255)},
        {"lavender", new Color32(131,118,156, 255)},
        {"pink", new Color32(255,119,168, 255)},
        {"light-peach", new Color32(255,204,170, 255)}
    };

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
        paddle.BreakPaddle();
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

    public IEnumerator RespawnBall() {
        ball.gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
        ball.gameObject.transform.position = ballRespawn;
        ball.RandomDirection();
        ball.gameObject.SetActive(true);
    }

    public void Restart() {
        SceneManager.LoadScene(0);
    }

    public void Quit() {
        Application.Quit();
    }
}
