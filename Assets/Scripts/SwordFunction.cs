using UnityEngine;

public class SwordFunction : MonoBehaviour
{
    public Transform player;

    public HingeJoint2D hinge;
    public Rigidbody2D playerRb; // drag player Rigidbody2D here

    /*for no accelerate and deccelerate
    public float speedMultiplier = 100f; // how much player speed affects spin
    public float forceMultiplier;
    public float baseSpeed = 180f; // minimum spin speed
    */

    private float playerSpeed;
    
    public Rigidbody2D swordRb;
    public float decelerationRate = 300f;
    public float accelerationRate = 100f;
    public float MaxAceMultiple = 250f;
    private float currentMotorSpeed; // store current motor speed

    public float WeaponDamage = 10f;

    private void Start()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        Collider2D weaponCollider = GetComponent<Collider2D>();
        foreach (GameObject Player in player)
        {
            Collider2D playerCollider = Player.GetComponent<Collider2D>();
            if (playerCollider != null)
            {
                Physics2D.IgnoreCollision(weaponCollider, playerCollider);
            }
        }
    }

    void Update()
    {
        
    }   

    void FixedUpdate()
    {
        /*
         * swordRb.AddForce(playerVelocity * forceMultiplier);
        */
        
        JointMotor2D motor = hinge.motor;
        currentMotorSpeed = motor.motorSpeed;
        // Calculate motor speed based on player velocity magnitude
        playerSpeed = playerRb.velocity.magnitude;

                /*
        // Determine spin direction
        Vector2 playerVelocity = playerRb.velocity;
        float directionMultiplier = 0f;

        if (playerVelocity.x > 0 || playerVelocity.y > 0)
        {
            directionMultiplier = 1f; // clockwise
        }
        else if (playerVelocity.x < 0 || playerVelocity.y < 0)
        {
            directionMultiplier = -1f; // counterclockwise
        }
        */

        if (playerSpeed > 0.01f)
        {
            //motor.motorSpeed = baseSpeed + (playerSpeed * speedMultiplier);
            motor.motorSpeed += (accelerationRate * playerSpeed) * Time.fixedDeltaTime;
            if (motor.motorSpeed >= (playerSpeed * MaxAceMultiple))
            {
                motor.motorSpeed -= (accelerationRate * playerSpeed) * Time.fixedDeltaTime;
            }

            motor.motorSpeed -= decelerationRate * Time.fixedDeltaTime;
            if (motor.motorSpeed <= 0f)
            {
                motor.motorSpeed = 0f;
            }
        }
        else
        {
            motor.motorSpeed -= decelerationRate * Time.fixedDeltaTime;
            if (motor.motorSpeed <= 0f)
            {
                motor.motorSpeed = 0f;
            }
        }

        hinge.motor = motor;
        ///Debug.Log("Player Speed: " + playerSpeed + ", Motor Speed: " + motor.motorSpeed);
    }

    void ConstantSlowDown()
    {
        
    }
}