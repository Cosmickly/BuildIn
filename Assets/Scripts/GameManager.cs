using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _startScreen;
    [SerializeField] private GameObject _endScreen;
    [SerializeField] private TextMeshProUGUI _endText;
    [SerializeField] private GameObject _restartButton;

    [Header("Prefabs")]
    [SerializeField]  private Vector3 _ballRespawn;
    [SerializeField] private Paddle _paddle;
    public Ball Ball;

    public bool Playing;

    private readonly Dictionary<string, Color32> _pico8Palette = new()
    {
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

    private void Start()
    {
        _endScreen.SetActive(false);
        _startScreen.SetActive(true);
        _restartButton.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Playing)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                StartGame();
            }
        }
    }

    public void LoseGame()
    {
        _endText.text = "You Lose!";
        EndGame();
    }

    public void WinGame()
    {
        _endText.text = "You Win!";
        EndGame();
        _paddle.BreakPaddle();
    }

    private void EndGame()
    {
        Playing = false;
        _endScreen.SetActive(true);
        _restartButton.SetActive(true);
        Ball.DestroyBall();
    }

    public void StartGame()
    {
        _startScreen.SetActive(false);
        _restartButton.SetActive(true);

        Ball = Instantiate(Ball, new Vector3(0, -1, 0), transform.rotation, transform);
        _paddle = Instantiate(_paddle, new Vector3(0, -3.5f, 0), transform.rotation, transform);

        Playing = true;
    }

    public IEnumerator RespawnBall()
    {
        Ball.gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
        Ball.gameObject.transform.position = _ballRespawn;
        Ball.RandomDirection();
        Ball.gameObject.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }
}