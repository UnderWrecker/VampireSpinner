using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float SPEED = 4f;
    public float Rspeed = 8f;

    private Vector2 moveInput;
    private bool _Running;
    public InputActionReference move;
    public InputActionReference run;

    public bool MoveRight;
    public bool MoveLeft;
    public bool MoveUp;
    public bool MoveDown;

    //public int playerDirection;

    public GameObject faceR;
    public GameObject faceL;
    public GameObject faceU;
    public GameObject faceD;
    public GameObject faceHurt;
    public GameObject Idle;
    public Animator playerAnimations;

    public bool hurting = false;
    public float healthCount;
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public float knockbackForce = 3f;

    public LogicScript logic; //gotta rewatch flappy bird shit when spawning
    
    void Start()
    {
        move.action.Enable();
        run.action.Enable();

        healthCount = 3.0f;
    }

    void Update()
    {
        /*moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();*/

        moveInput = move.action.ReadValue<Vector2>();

        _Running = run.action.IsPressed();

        if (!hurting)
        {
            if (moveInput.x > 0)
            {
                MoveRight = true;
                faceR.SetActive(true);
                faceD.SetActive(false);
                faceL.SetActive(false);
                faceU.SetActive(false);
                Idle.SetActive(false);
                playerAnimations.Play("walkR");
            }
            else
            {
                MoveRight = false;
            }

            if (moveInput.x < 0)
            {
                MoveLeft = true;
                faceR.SetActive(false);
                faceD.SetActive(false);
                faceL.SetActive(true);
                faceU.SetActive(false);
                Idle.SetActive(false);
                playerAnimations.Play("walkL");
            }
            else
            {
                MoveLeft = false;
            }

            if (moveInput.y > 0)
            {
                MoveUp = true;
                faceR.SetActive(false);
                faceD.SetActive(false);
                faceL.SetActive(false);
                faceU.SetActive(true);
                Idle.SetActive(false);
                playerAnimations.Play("walkU");
            }
            else
            {
                MoveUp = false;
            }

            if (moveInput.y < 0)
            {
                MoveDown = true;
                faceR.SetActive(false);
                faceD.SetActive(true);
                faceL.SetActive(false);
                faceU.SetActive(false);
                Idle.SetActive(false);
                playerAnimations.Play("walkD");
            }
            else
            {
                MoveDown = false;
            }

            if (moveInput.x == 0 && moveInput.y == 0)
            {
                playerAnimations.Play("PlayerAnimation");
            }
        }


        if (_Running)
        {
            playerAnimations.speed = 1.7f;
        }
        else
        {
            playerAnimations.speed = 1.0f; // normal speed
        }

        if (healthCount >= 0)
        {
            if (healthCount == 3)
            {
                heart1.SetActive(true);
                heart2.SetActive(true);
                heart3.SetActive(true);
            }
            else if (healthCount == 2)
            {
                heart1.SetActive(true);
                heart2.SetActive(true);
                heart3.SetActive(false);
            }
            else if (healthCount == 1)
            {
                heart1.SetActive(true);
                heart2.SetActive(false);
                heart3.SetActive(false);
            }
            else
            {
                heart1.SetActive(false);
                heart2.SetActive(false);
                heart3.SetActive(false);
            }
        }
    }

    void FixedUpdate()
    {
        float currentSpeed = _Running ? Rspeed : SPEED;
        rb.linearVelocity = new Vector2(moveInput.x * currentSpeed, moveInput.y * currentSpeed);

    }

    IEnumerator HurtAnimation()
    {
        hurting = true;
        faceR.SetActive(false);
        faceD.SetActive(false);
        faceL.SetActive(false);
        faceU.SetActive(false);
        Idle.SetActive(false);
        faceHurt.SetActive(true);
        playerAnimations.Play("playerHurt");

        yield return new WaitForSeconds(3f);

        hurting = false;
        faceHurt.SetActive(false);
        Idle.SetActive(true);
        playerAnimations.Play("PlayerAnimation"); // or revert to any default animation
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("enemy") && !hurting)
        {
            healthCount -= 1;

            if (healthCount == 0)
            {
                logic.gameOver();
            }

            Vector2 enemyPos = other.transform.position;
            Vector2 playerPos = rb.position;
            Vector2 knockbackDir = (playerPos - enemyPos).normalized;
            // Calculate new position
            Vector2 newPosition = playerPos + knockbackDir * knockbackForce;

            // Move player instantly
            rb.MovePosition(newPosition);
            //rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
            StartCoroutine(HurtAnimation());
            
        }
    }

}