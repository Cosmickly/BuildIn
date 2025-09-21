using System.Collections.Generic;
using Configurations;
using Factories;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    /// <summary>
    ///     Manages the game state and scene.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("UI")] [SerializeField] private GameObject _startScreen;
        [SerializeField] private GameObject _endScreen;
        [SerializeField] private TextMeshProUGUI _endText;
        [SerializeField] private GameObject _restartButton;

        [Header("Prefabs")] [SerializeField] private Paddle _paddle;
        [SerializeField] private Ball _ball;
        [SerializeField] private PlayingBrickView _playingBrickViewPrefab;
        [SerializeField] private ProtoBrickView _protoBrickViewPrefab;
        [SerializeField] private OverlayView _overlayPrefab;

        [Header("Transforms")] [SerializeField]
        private Transform _gridTransform;

        [SerializeField] private Transform _brickQueueTransform;
        [SerializeField] private Transform _selectionAreaTransform;

        [Header("Sprites")]
        [SerializeField] private Sprite[] _brickSprites = new Sprite[3];

        [SerializeField] private Vector2Int _gridSize;
        private bool _playing;

        private readonly Dictionary<string, Color32> _pico8Palette = new()
        {
            { "black", new Color32(0, 0, 0, 255) },
            { "dark-blue", new Color32(29, 43, 83, 255) },
            { "dark-purple ", new Color32(126, 37, 83, 255) },
            { "dark-green ", new Color32(0, 135, 81, 255) },
            { "brown", new Color32(171, 82, 54, 255) },
            { "dark-grey ", new Color32(95, 87, 79, 255) },
            { "light-grey ", new Color32(194, 195, 199, 255) },
            { "white", new Color32(255, 241, 232, 255) },
            { "red", new Color32(255, 0, 77, 255) },
            { "orange", new Color32(255, 163, 0, 255) },
            { "yellow", new Color32(255, 236, 39, 255) },
            { "green", new Color32(0, 228, 54, 255) },
            { "blue", new Color32(41, 173, 255, 255) },
            { "lavender", new Color32(131, 118, 156, 255) },
            { "pink", new Color32(255, 119, 168, 255) },
            { "light-peach", new Color32(255, 204, 170, 255) }
        };

        private GridManager _gridManager;
        private BrickQueueManager _brickQueueManager;
        private OverlayManager _overlayManager;
        private ProtoBrickManager _protoBrickManager;

        private void Awake()
        {
            var gridConfig = new GridConfig(_gridSize.x, _gridSize.y, 1, 0.5f);
            var brickFactory = new BrickFactory(_playingBrickViewPrefab, _protoBrickViewPrefab, _brickSprites);
            var overlayFactory = new OverlayFactory(_overlayPrefab, gridConfig);

            _gridManager = new GridManager(gridConfig, brickFactory, _gridTransform);
            _brickQueueManager = new BrickQueueManager(gridConfig, brickFactory, _brickQueueTransform);
            _overlayManager = new OverlayManager(gridConfig, overlayFactory, _selectionAreaTransform);
            _protoBrickManager = new ProtoBrickManager(gridConfig, brickFactory, _selectionAreaTransform);
        }

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

            if (_playing)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Restart();
                }

                // if (Input.GetKeyDown(KeyCode.LeftArrow))
                // {
                //     MoveSelectedOverlay(-1);
                // }
                //
                // if (Input.GetKeyDown(KeyCode.RightArrow))
                // {
                //     MoveSelectedOverlay(1);
                // }
                //
                // if (Input.GetKeyDown(KeyCode.Space))
                // {
                //     AddTopBrick(_selectedOverlay);
                // }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    StartGame();
                }
            }
        }

        private void StartGame()
        {
            Debug.Log("Starting Game");
            _restartButton.SetActive(true);
            _startScreen.SetActive(false);
            _ball.SetBallActive(true);
            _playing = true;

            _brickQueueManager.InitialiseBrickQueue();
            _gridManager.InitialisePlayingBricks();
            _overlayManager.InitialiseOverlays();
            _protoBrickManager.InitialiseProtoBricks();
        }

        public void LoseGame()
        {
            _endText.text = "You Lose!";
            _endScreen.SetActive(true);
            _ball.SetBallActive(false);
            _playing = false;
        }

        public void WinGame()
        {
            _endText.text = "You Win!";
            _endScreen.SetActive(true);
            _ball.SetBallActive(false);
            _playing = false;
        }

        public void Restart()
        {
            SceneManager.LoadScene(0);
        }

        public void Exit()
        {
            Application.Quit();
        }

        public Ball GetBall()
        {
            return _ball;
        }
    }
}