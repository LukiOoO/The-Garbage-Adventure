using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CollectionController : MonoBehaviour
{
    // Start is called before the first frame update
    public Item item;
    
    public float healthChange;
    
    public float moveSpeedChange;
    
    public float attackSpeedChange;
    
    public float bulletSizeChange;
    public int playerDmgChange;

    void Start()
    {
       GetComponent<SpriteRenderer>().sprite = item.itemImage;
       Destroy(GetComponent<PolygonCollider2D>());
       PolygonCollider2D polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
       polygonCollider.isTrigger = true;
    }

    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController.collectedAmount++;
            GameController.HealPlayer(healthChange);
            GameController.MoveSpeedChange(moveSpeedChange);
            GameController.FireRateChange(attackSpeedChange);
            GameController.BulletSizeChange(bulletSizeChange);
            GameController.PlayerDmgChange(playerDmgChange);
            GameController.instance.UpdateCollectedItems(this);
            Destroy(gameObject);
        }
    }
}
