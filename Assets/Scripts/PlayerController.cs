using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    public float tapForce = 0;
    public Vector3 startPos;
    

    public AudioSource takeDamageAudio;
    public AudioSource jumpAudio;
    public AudioSource scoreAudio;
    public AudioSource shootAudio;

    public Transform firePoint;
    public bool shooting = false;
    public GameObject bulletPrefab;

    Rigidbody2D rb;
    public bool jumping = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        rb.simulated = false;

    }

    private void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    private void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void OnGameStarted()
    {
        rb.velocity = Vector3.zero;
        rb.simulated = true;
    }

    void OnGameOverConfirmed()
    {
        transform.localPosition = startPos;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Time.timeScale += 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ScoreZone")
        {
            scoreAudio.Play();
            OnPlayerScored();
        }
    }
    public void Jump()
    {
        if (!jumping)
        {
            jumpAudio.Play();
            rb.AddForce(Vector2.up * 7.0f, ForceMode2D.Impulse);
            jumping = true;
            StartCoroutine(JumpDelay());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "DeadZone")
        {
            rb.simulated = false;
            GameManager.Instance.started = false;
            takeDamageAudio.Play();
            OnPlayerDied();
        }
    }

    IEnumerator JumpDelay()
    {
        yield return new WaitForSeconds(2);
        jumping = false;
        GameManager.Instance.JumpButton.interactable = true;
    }
    public void Shoot()
    {
        if (shooting) return;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        shooting = true;
        StartCoroutine(ShootDelay());
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(3);
        shooting = false;
        GameManager.Instance.ShootButton.interactable = true;
    }
}
