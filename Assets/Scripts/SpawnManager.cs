using BlockRun.Enum;
using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject firstTile;
    [SerializeField] GameObject timePowerup;

    public float spawnRate = 4f;
    public float spawnRateIncrease = 0.5f;

    private float nextSpawn;
    private float spawnPosY = -0.8f;

    private GameObject previousTile;
    private CameraFollow cameraFollow;
    private GameManager gameManager;
    private ScoreManager scoreManager;

    private Direction previousDirection;

    [SerializeField] private int minScorePowerupSpawn = 80;
    [SerializeField] private int maxScorePowerupSpawn = 121;

    private int nextScoreSpawn;
    private int randomScoreRangePowerupSpawn;
    
    private bool isPaused = false;
    private float pauseTimeInSeconds = 2f;

    private void Start()
    {
        previousTile = firstTile;
        cameraFollow = GameObject.Find("CameraTarget").GetComponent<CameraFollow>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

        SetPowerupSpawnParameters();
    }

    void Update()
    {
        if (isPaused)
        {
            StartCoroutine(PauseTileSpawn());
        }
        else
        {
            if (nextSpawn < Time.time && gameManager.IsGameActive)
            {
                nextSpawn = Time.time + 1f / spawnRate;

                SpawnTile();
                SetCameraTarget();
            }
        }
    }

    private void SetCameraTarget()
    {
        cameraFollow.target = previousTile.transform;
    }

    private void SpawnTile()
    {
        Vector3 direction = GetRandomDirection();
        
        float spawnPosX = previousTile.transform.position.x + direction.x;
        float spawnPosZ = previousTile.transform.position.z + direction.z;

        previousTile = Instantiate(tilePrefab, new Vector3(spawnPosX, spawnPosY, spawnPosZ), tilePrefab.transform.rotation);

        SpawnPowerup();
    }

    private Vector3 GetRandomDirection()
    {
        Direction randomDirection = (Direction)Random.Range(0, 3);
        while (!IsValidDirection(randomDirection))
        {
            randomDirection = (Direction)Random.Range(0, 3);
        }

        previousDirection = randomDirection;
        
        Vector3 direction;

        if (randomDirection == Direction.Left)
        {
            direction = Vector3.left;
        }
        else if (randomDirection == Direction.Forward)
        {
            direction = Vector3.forward;
        }
        else
        {
            direction = Vector3.right;
        }

        return direction;
    }

    private void SpawnPowerup()
    {
        if (CanSpawnPowerup())
        {
            Vector3 position = new Vector3(previousTile.transform.position.x, 0, previousTile.transform.position.z);
            
            var timePowerupInstance = Instantiate(timePowerup, position, timePowerup.transform.rotation);
            
            var timePowerupScript = timePowerupInstance.GetComponentInChildren<TimePowerup>();
            timePowerupScript.onPowerupPickup.AddListener(this.OnTimePowerupPickup);
            
            SetPowerupSpawnParameters();
        }
    }

    private bool CanSpawnPowerup()
    {
        return scoreManager.Score == nextScoreSpawn;
    }

    private void SetPowerupSpawnParameters()
    {
        randomScoreRangePowerupSpawn = Random.Range(minScorePowerupSpawn, maxScorePowerupSpawn);
        nextScoreSpawn = scoreManager.Score + randomScoreRangePowerupSpawn;
    }

    private bool IsValidDirection(Direction direction)
    {
        if (direction == Direction.Left && previousDirection == Direction.Right)
            return false;
        
        if (direction == Direction.Right && previousDirection == Direction.Left)
            return false;

        return true;
    }

    public void IncreaseSpawnRate()
    {
        spawnRate += spawnRateIncrease;
    }

    public void OnTimePowerupPickup()
    {
        isPaused = true;
    }

    public IEnumerator PauseTileSpawn()
    {
        yield return new WaitForSeconds(pauseTimeInSeconds);
        isPaused = false;
    }
}
