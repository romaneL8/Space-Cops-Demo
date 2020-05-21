using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private int health;
    public int topHealth = 5;
    public float startSpeed = 0f;
    public float topSpeed = 40f;
    private float currentSpeed = 0f;
    public float bulletSpeed = 200f;
    public float accelRate = 5f;
    public float deAccelRate = 25f;
    public float power = 10.0f;
    public GameObject bulletPrefab;
    public GameObject locBulletSpawnPrefab;
    public AudioClip audioClip;
    public AudioClip bullAudioClip;
    public AudioClip exAudioClip;

    private AudioSource audioSource;

    private Rigidbody rigid;

    // Use this for initialization
    void Start()
    {
        health = topHealth;
        rigid = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // -----------------------------Player Movement--------------------------
        if (Input.GetKey(KeyCode.W))
        {
            currentSpeed = currentSpeed + (accelRate * Time.deltaTime);
            transform.Translate(0, 0, currentSpeed);
        }
        else
        {
            currentSpeed = currentSpeed - (deAccelRate * Time.deltaTime);
            if (currentSpeed > 0)
            {
                transform.Translate(0, 0, currentSpeed);
            }
            else
            {
                transform.Translate(0, 0, 0);
            }
        }
        currentSpeed = Mathf.Clamp(currentSpeed, startSpeed, topSpeed);
        // -----------------------------Bullet Movement--------------------------
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject go = Instantiate(bulletPrefab, locBulletSpawnPrefab.transform.position, locBulletSpawnPrefab.transform.rotation);
            Rigidbody rigidBullet = go.GetComponent<Rigidbody>();
            rigidBullet.AddForce(go.transform.forward * bulletSpeed);
            audioSource.PlayOneShot(audioClip);
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;
        GameObject bullCollidedWith = bulletPrefab.gameObject;

        if (collidedWith.tag == "Enemy")
        {
            health--;
            audioSource.PlayOneShot(bullAudioClip);
        }
        else if (collidedWith.tag == "Environment")
        {
            Destroy(collidedWith);
            audioSource.PlayOneShot(exAudioClip);
            Invoke("LoadLevel", 1f);
        }

        if (bullCollidedWith.tag == "Enemy")
        {
            Destroy(bullCollidedWith);
            audioSource.PlayOneShot(bullAudioClip);
        }
    }

    void LoadLevel()
    {
        SceneManager.LoadScene("Main");
    }
}
