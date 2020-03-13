using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 20.0f;
    [SerializeField]Rigidbody2D rb;
    public AudioSource scoreSound;
    public Transform enemy;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "DeadZone")
        {
            enemy = collision.gameObject.transform;
            Vector3 targetLocation;
            targetLocation = enemy.position;
            targetLocation.x -= 100;
            enemy.position = targetLocation;
            Destroy(gameObject);
            scoreSound.Play();
            GameManager.Instance.score += 1;

        }
    }
}

