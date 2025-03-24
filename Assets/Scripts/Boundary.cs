using UnityEngine;

public class Boundary : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    //TODO: crash when respawning ball during block placement
    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
}