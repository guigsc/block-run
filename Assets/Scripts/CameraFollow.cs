using BlockRun;
using BlockRun.Enum;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Transform player;

    private Vector3 offset;
    private float smoothing = 0.5f;

    private GameManager gameManager;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);

        if (gameManager.IsGameActive &&
           (transform.position.z > player.position.z + 11 || 
            transform.position.x > player.position.x + 13 || 
            transform.position.x < player.position.x - 13))
        {
            gameManager.ChangeState(GameState.GameOver);
        }
    }
}
