using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GameManager Instance => instance;

    public GameState gameState;

    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private Button playButton;

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
    }

    private void Start()
    {
        ChangeState(GameState.Play);

        playButton.onClick.AddListener(() =>
        {
            gameState = GameState.Play;
            SceneManager.LoadScene(0);
        });
    }

    public void ChangeState(GameState state)
    {
        gameState = state;

        switch (state)
        {
            case GameState.Play:
                Play();
                break;
            case GameState.GameOver:
                GameOver();
                break;
            case GameState.Victory:
                Victory();
                break;
        }
    }

    private void Play()
    {
        gameOverCanvas.gameObject.SetActive(false);
    }

    private void Victory()
    {
        gameOverCanvas.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        gameOverCanvas.gameObject.SetActive(true);
    }
}
public enum GameState
{
    Play,
    GameOver,
    Victory
}