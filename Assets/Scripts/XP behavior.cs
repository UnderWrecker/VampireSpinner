using UnityEngine;

public class XPbehavior : MonoBehaviour
{
    public float pullRange = 2f; // distance to start pulling
    public float pullSpeed = 0.5f; // speed of attraction
    public float xpValue = 100f; // XP amount granted

    private Transform player;
    public playerController PlayerXP;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance <= pullRange)
            {
                // Move towards player smoothly
                transform.position = Vector2.MoveTowards(transform.position, player.position, pullSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Add XP to player
            PlayerXP.EXPchange(xpValue);
            Destroy(gameObject); // destroy XPdrop after collection
        }
    }
}
