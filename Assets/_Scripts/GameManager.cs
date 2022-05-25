using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GameManager Instance => instance;

    public GameState gameState;

    [Header("UI Info")]
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private Image winLoseImage;
    [SerializeField] private Button playButton;
    [SerializeField] private Sprite winSprite;
    [SerializeField] private Sprite loseSprite;

    private void Awake()
    {
        ChangeFrameRate();

        if (instance != null)
            Destroy(this);
        instance = this;
    }

    private void ChangeFrameRate()
    {
        if(Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
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
        gameOverCanvas.SetActive(false);
    }

    private void Victory()
    {
        winLoseImage.sprite = winSprite;
        gameOverCanvas.SetActive(true);
    }

    private void GameOver()
    {
        winLoseImage.sprite = loseSprite;
        gameOverCanvas.SetActive(true);
    }
}
public enum GameState
{
    Play,
    GameOver,
    Victory
}