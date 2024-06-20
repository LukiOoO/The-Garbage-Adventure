using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private static float health;
    private static int maxHealth = 6;
    private static float moveSpeed = 5f;
    private static float fireRate = 0.5f;
    private static float bulletSize = 0.5f;
    private bool bootCollected = false;
    private bool screwCollected = false;
    private static int playerDmg = 3;

    public List<string> collectedNames = new List<string>();
    public Text healthText;

    public static float Health
    {
        get => health;
        set
        {
            health = value;
            if (health <= 0)
            {
                instance.KillPlayer();
            }
        }
    }

    public static int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public static float FireRate { get => fireRate; set => fireRate = value; }
    public static float BulletSize { get => bulletSize; set => bulletSize = value; }
    public static int PlayerDmg { get => playerDmg; set => playerDmg = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        ResetGame(); 
    }

    public static void PlayerDmgChange(int dmg)
    {
        playerDmg += dmg;
    }

    void Update()
    {
        healthText.text = "Health: " + health;
    }

    public static void DamagePlayer(int damage)
    {
        Health -= damage;
    }

    public static void HealPlayer(float healAmount)
    {
        Health = Mathf.Min(MaxHealth, health + healAmount);
    }

    public static void MoveSpeedChange(float speed)
    {
        MoveSpeed += speed;
    }

    public static void FireRateChange(float rate)
    {
        FireRate -= rate;
    }

    public static void BulletSizeChange(float size)
    {
        BulletSize += size;
    }

    public void UpdateCollectedItems(CollectionController item)
    {
        collectedNames.Add(item.item.name);

        foreach (string i in collectedNames)
        {
            switch (i)
            {
                case "Boot":
                    bootCollected = true;
                    break;
                case "Screw":
                    screwCollected = true;
                    break;
            }
        }
        if (bootCollected && screwCollected)
        {
            FireRateChange(2.5f);
        }
    }

    private void KillPlayer()
    {
        if (PlayerController.instance != null)
        {
            PlayerController.instance.KillPlayer();
        }
    }

    public void ResetGame()
    {
        health = maxHealth;
        moveSpeed = 5f;
        fireRate = 0.5f;
        bulletSize = 0.5f;
        bootCollected = false;
        screwCollected = false;
        collectedNames.Clear();
    }
}