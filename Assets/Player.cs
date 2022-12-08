using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float gravity;
    private Vector3 direction;
    public float jumpStrength;

    public Sprite[] sprites;
    private int spriteIdx = 0;
    private SpriteRenderer spriteRenderer;

   
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating("AnimateSprite", 0.2f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            direction.y = jumpStrength;
        }
        direction.y -= gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;
    }

    private void OnEnable()
    {
        // Bringing the player to the center
        transform.position = Vector3.zero;
        // Resetting direction (so that the bird doesn't fall faster if it was falling fast during the last play
        direction = new Vector3(0, 0, 0);
    }
    void AnimateSprite()
    {
        spriteIdx++;
        spriteIdx %= sprites.Length; // Making sure the idx doesn't go out of bound
        spriteRenderer.sprite = sprites[spriteIdx];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Obstacle")
        {
            FindObjectOfType<GameManager>().GameOver();
        }
        else if(collision.gameObject.tag == "Gap")
        {
            FindObjectOfType<GameManager>().IncreaseScore();
        }
    }
}
