

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 


public class GameController : MonoBehaviour
{
    public static GameController instance;

    private static float health;
    private static int maxHealth = 6;
    private static float moveSpeed = 5f;
    private static float fireRate = 0.5f;
    private static float bulletSize = 0.5f;
    private bool screwCollected = false;
    private  bool bananaCollected = false;
    private  bool kokardkaCollected = false;
    private  bool cigaretCollected = false;
    private  bool sniperCollected = false;
    private static int playerDmg = 5;
    private static float playerCritChance = 0f;
    private static int playerCritMulti = 0;

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
    public static float PlayerCritChance { get => playerCritChance; set => playerCritChance = value; }
    public static int PlayerCritMulti { get => playerCritMulti; set => playerCritMulti = value; }

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


    public static void CritMultiChange(int critMulti)
    {
        playerCritMulti += critMulti;
    }
    public static void CritChaneChange(float critChance)
    {
        playerCritChance += critChance;
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

    public static int CritOrNo()
    {
        System.Random random = new System.Random();
        double randomValue = random.NextDouble();
        Debug.Log(randomValue);
        
        if (randomValue <= playerCritChance)
        {
            return  playerDmg = playerDmg * playerCritMulti;
        }        
        else
        {
            return playerDmg;
        }
    }



    public void UpdateCollectedItems(CollectionController item)
    {
        collectedNames.Add(item.item.name);



       

        // Check collected items
        foreach (string i in collectedNames)
        {
            switch (i)
            {
                case "Screw":
                    screwCollected = true;
                    break;
                case "Banana":
                    bananaCollected = true;
                    break;
                case "Kokardka":
                    kokardkaCollected = true;
                    break;
                case "Cigaret":
                    cigaretCollected = true;
                    break;
                case "Snipe":
                    sniperCollected = true;
                    break;
            }
        }

        if (kokardkaCollected && cigaretCollected && screwCollected)
        {
            BulletSizeChange(5);
            playerDmg = 80;
            FireRateChange(-2f);
        }
        if (kokardkaCollected && cigaretCollected && bananaCollected)
        {
            BulletSizeChange(1);
            playerDmg = 1;
            FireRateChange(400f);
        }
        if (kokardkaCollected && cigaretCollected && sniperCollected)
        {
        
            BulletSizeChange(0.5f);
            playerDmg = 1;
            FireRateChange(-0.5f);
            playerCritChance = 0.1f;
            CritMultiChange(1000);

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
        screwCollected = false;
        bananaCollected = false;
        kokardkaCollected = false;
        cigaretCollected = false;
        sniperCollected = false;
        collectedNames.Clear();
    }

    public void LoadEndSceneAfterDelay(float delay)
    {
        StartCoroutine(LoadEndSceneCoroutine(delay));
    }

    private IEnumerator LoadEndSceneCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("The End");
    }
}
