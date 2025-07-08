using System.Collections.Generic;
using System.Xml.Serialization;
using NUnit.Framework;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Health = 50f;
    public float Speed = 10f;
    public Rigidbody2D Erb;
    public float knockback = 0.5f;
    public bool alive;
    public bool debuffing;
    public GameObject EnemyObject;

    public SwordFunction SwordF;

    public Transform playerPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       alive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (!alive) //when enemy dies
        {
            foreach(LootItem lootItem in lootTable)
            {
                if(Random.Range(0f,100f) <= lootItem.dropChance)
                {
                    InstantiateLoot(lootItem.itemPrefab);
                    //break;
                }
                
            }

            //ExperienceManager.Instance.AddExperience(EXPamount);
            GameObject.Destroy(EnemyObject);
        }

        if (playerPosition != null)
        {
            Vector2 enemyPos = Erb.position;
            Vector2 playerPos = playerPosition.position;
            Vector2 direction = (playerPos - enemyPos).normalized;
            Erb.linearVelocity = direction * Speed * Time.fixedDeltaTime;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("weapon"))
        {
            Health -= SwordF.WeaponDamage;
            //Debug.Log("Enemy hit!");

            if (Health <= 0)
            {
                alive = false;
            }

            Vector2 weaponPos = other.transform.position;
            Vector2 enemyPos = Erb.position;
            Vector2 knockbackDir = (enemyPos - weaponPos).normalized;
            Vector2 newPosition = enemyPos + knockbackDir * knockback;
            Erb.MovePosition(newPosition);
        }
    }


    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();
    

    public void InstantiateLoot(GameObject loot)
    {
        if(loot)
        {
            GameObject droppedLoot = Instantiate(loot, transform.position, Quaternion.identity);
            
        }
    }
    
}
