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

    public BallMovement ballPrefab;
    public BallMovement currentBall;
    public PaddleMovement paddlePrefab;

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
        Debug.Log("You Lose!");
        playing = false;
        endText.text = "You Lose!";
        endScreen.SetActive(true);
    }

    public void WinGame() {
        Debug.Log("You Win!");
        playing = false;
        endText.text = "You Win!";
        endScreen.SetActive(true);
    }

    public void StartGame() {
        startScreen.SetActive(false);
        restartButton.SetActive(true);
        currentBall = Instantiate(ballPrefab, transform);
        ballPrefab.transform.position = new Vector3(0, -1, 0);

        Instantiate(paddlePrefab, transform);
        paddlePrefab.transform.position = new Vector3(0, -3.5f, 0);
        playing = true;
    }

    public void Restart() {
        SceneManager.LoadScene(0);
    }

    public void Quit() {
        Application.Quit();
    }
}
