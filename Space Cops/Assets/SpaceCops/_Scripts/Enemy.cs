using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    [Header("Enemy Stats")]
    public int score;
    public float speed = 20f;
    public float bulletSpeed = 200f;
    private int enemyHealth;
    public int enemyTopHealth = 5;
    [Header("Patrol Stats")]
    public float startPatrol = 5f;
    public float waitPatrol = 5f;
    private float waitTime;
    public float startWaitTime;
    public Transform[] patrolPoints;
    [Header("Hunt Stats")]
    public float startHunt = 5f;
    public float waitHunt = 5f;
    public float minDist = 10f;
    [Header("Evade Stats")]
    public float startEvade = 5f;
    public float waitEvade = 5f;
    // # seconds for a full sine wave
    public float waveFrequency = 2;
    // sine wave width in meters
    public float waveWidth = 4;
    public float waveRotY = 45;
    private float x0; // The initial x value of pos
    private float birthTime;
    [Header("Set Transforms")]
    public Transform player;
    public GameObject bulletPrefab;
    public GameObject locBulletSpawnPrefab;
    [Header("Set Audio")]
    public AudioClip audioClip;
    private AudioSource audioSource;
    [Header("Set Others")]
    public Text scoreGT;
    public EBehavior behavior;
    public enum EBehavior
    {
        Patrol, Hunt, Evade
    }

    private int firstBehavior;
    private int topBehavior;
    private Rigidbody rigid;

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
        birthTime = Time.time;
        float x0 = transform.position.x;
        enemyHealth = enemyTopHealth;
        InvokeRepeating("PickBehavior", 1, 30);

        GameObject scoreGO = GameObject.Find("ScoreCounter");
        scoreGT = scoreGO.GetComponent<Text>();
        scoreGT.text = "0";
    }
	
	// Update is called once per frame
	void Update () {
        if (firstBehavior == 0)
        {
            behavior = EBehavior.Patrol;
        } else if(firstBehavior == 1)
        {
            behavior = EBehavior.Hunt;
        } else if(firstBehavior == 2)
        {
            behavior = EBehavior.Evade;
        }
        // -----------------------------Enemy Movement--------------------------
        switch (behavior)
        {
            case EBehavior.Patrol:
                InvokeRepeating("Patrol", startPatrol, waitPatrol);
                break;
            case EBehavior.Hunt:
                InvokeRepeating("Hunt", startHunt, waitHunt);
                break;
            case EBehavior.Evade:
                InvokeRepeating("Evade", startEvade, waitEvade);
                break;
        }
        if (enemyHealth <= 0)
        {
            int score = int.Parse(scoreGT.text);
            score += 1000;
            scoreGT.text = score.ToString();
            Destroy(this.gameObject);
        }
    }

    void Patrol()
    {
        float acceleration = speed * Time.deltaTime;
        int randomPoint = Random.Range(0, patrolPoints.Length);
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[randomPoint].position, acceleration);
        if (Vector3.Distance(transform.position, patrolPoints[randomPoint].position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                randomPoint = Random.Range(0, patrolPoints.Length);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    void Hunt()
    {
        float acceleration = speed * Time.deltaTime;
        int randomPoint = Random.Range(0, patrolPoints.Length);
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[randomPoint].position, acceleration);
        if (Vector3.Distance(transform.position, patrolPoints[randomPoint].position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                randomPoint = Random.Range(0, patrolPoints.Length);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
        //this.transform.position += transform.forward * speed * Time.deltaTime;
        Vector3 distance = player.transform.position - this.transform.position;
        float viewAngle = Vector3.Angle(distance, this.transform.forward);
        if (viewAngle < 25)
        {
            this.transform.LookAt(player);
            this.transform.position += transform.forward * speed * Time.deltaTime;
        }
        float distToPlayer = Vector3.Distance(player.transform.position, this.transform.position);
        if (distToPlayer < minDist)
        {
            GameObject go = Instantiate(bulletPrefab, locBulletSpawnPrefab.transform.position, locBulletSpawnPrefab.transform.rotation);
            Rigidbody rigidBullet = go.GetComponent<Rigidbody>();
            rigidBullet.AddForce(go.transform.forward * bulletSpeed);
            audioSource.PlayOneShot(audioClip);
        }
    }

    void PickBehavior()
    {
        topBehavior = Random.Range(0, 3);
        firstBehavior = topBehavior;
    }

    void Evade()
    {
        // theta adjusts based on time
        float age = Time.time - birthTime;
        float theta = Mathf.PI * 4 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        float movePos = x0 + waveWidth * sin;
        // Rotate a bit about y
        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);
        this.transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;
        
        if (collidedWith.tag == "Bullet")
        {
            enemyHealth--;
        } else if (collidedWith.tag == "Player")
        {
            enemyHealth--;
        }
    }
}
