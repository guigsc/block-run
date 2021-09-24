using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] 
    private Text scoreText;

    [SerializeField]
    private Text highScoreText;

    private int score;
    private int highScore;

    private SpawnManager spawnManager;

    public int Score => score;

    private void Start()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        highScore = PlayerPrefs.GetInt("highScore");
        SetHighscoreText();
    }

    public void AddScore(int scorePoints)
    {
        score += scorePoints;
        scoreText.text = "Score: " + score;

        if (score % 50 == 0)
        {
            spawnManager.IncreaseSpawnRate();
        }
    }

    public void SetHighscore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highScore", highScore);
            SetHighscoreText();
        }
    }

    void SetHighscoreText() 
    {
        highScoreText.text = "Best: " + highScore;
    }
}
