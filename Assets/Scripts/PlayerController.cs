using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float speed;
    public Rigidbody2D rigidBody2D;
    public Text collectedText;
    public static int collectedAmount = 0;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    private float lastFire;
    public float fireDelay;
    public Animator animator;
    private bool isDead = false;
    public float delayBeforeLoading = 2f;
    public int playerDmg;

    public float playerCritChance;

    public int playerCritMulti;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        ResetPlayer(); 
    }

    void Update()
    {
        if (isDead) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
        animator.SetFloat("Speed", speed);

        fireDelay = GameController.FireRate;
        speed = GameController.MoveSpeed;
        playerDmg = GameController.PlayerDmg;
        playerCritMulti = GameController.PlayerCritMulti;
        playerCritChance = GameController.PlayerCritChance;
   


        float shootHorizontal = Input.GetAxis("ShootHorizontal");
        float shootVertical = Input.GetAxis("ShootVertical");
        if ((shootHorizontal != 0 || shootVertical != 0) && Time.time > lastFire + fireDelay)
        {
            Shoot(shootHorizontal, shootVertical);
            lastFire = Time.time;
        }

        rigidBody2D.velocity = new Vector3(horizontal * speed, vertical * speed, 0);
        collectedText.text = "Items Collected: " + collectedAmount;
    }

    void Shoot(float x, float y)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb == null)
        {
            bulletRb = bullet.AddComponent<Rigidbody2D>();
        }
        bulletRb.gravityScale = 0;
        bulletRb.velocity = new Vector3(
            (x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed,
            (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed,
            0
        );
        
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (bulletController != null) 
        {
            bulletController.SetDamage(GameController.CritOrNo());
        }
    }

    public void KillPlayer()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("Player_Death"); 
        StartCoroutine(LoadMainMenuAfterDelay(delayBeforeLoading));
    }

    private IEnumerator LoadMainMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Main Menu");
    }

    public void ResetPlayer()
    {
        collectedAmount = 0;
        isDead = false;
    }
}