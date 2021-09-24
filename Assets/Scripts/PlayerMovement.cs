using BlockRun;
using BlockRun.Enum;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    private float tumblingDuration = 0.2f;
    private bool isTumbling;

    private float tileSpawnPosY = -0.75f;

    private GameManager gameManager;

    private bool isGrounded = false;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (isGrounded)
        { 
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            if (horizontal != 0 && vertical != 0)
            {
                horizontal = 0;
            }
            else if (vertical != 0 && horizontal != 0)
            {
                vertical = 0;
            }

            Vector3 direction = new Vector3(horizontal, 0, vertical);

            if (direction != Vector3.zero && !isTumbling && gameManager.IsGameActive)
            {
                if (!CanMove(direction))
                {
                    gameManager.ChangeState(GameState.GameOver);
                }

                StartCoroutine(Tumble(direction));
            }
        }
    }

    private bool CanMove(Vector3 direction)
    {
        Vector3 positionToCheck = new Vector3(transform.position.x + direction.x, tileSpawnPosY, transform.position.z + direction.z);

        float radius = 0.25f;

        if (Physics.CheckSphere(positionToCheck, radius))
        {
            return true;
        }
        
        return false;
    }

    private IEnumerator Tumble(Vector3 direction)
    {
        isTumbling = true;

        var pivot = transform.position + Vector3.down * 0.5f + direction * 0.5f;
        var rotationAxis = Vector3.Cross(Vector3.up, direction);

        var startPosition = transform.position;
        var endPosition = startPosition + direction;

        var startRotation = transform.rotation;
        var endRotation = Quaternion.AngleAxis(90.0f, rotationAxis) * startRotation;

        var rotationSpeed = 90.0f / tumblingDuration;

        var t = 0.0f;

        while (t < tumblingDuration)
        {
            t += Time.deltaTime;

            if (t < tumblingDuration)
            {
                transform.RotateAround(pivot, rotationAxis, rotationSpeed * Time.deltaTime);
                yield return null;
            }        
            else
            {
                transform.position = endPosition;
                transform.rotation = endRotation;
            }
        }

        isTumbling = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }
}
