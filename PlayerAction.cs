using UnityEngine;
using System.Collections;

public class PlayerAction : MonoBehaviour {

    // Player Variables
    public float playerSpeed;
    public float turnSpeed;
    public float fireRate;
    public float maxSpeed;

    // Weapon System
    public GameObject bolt;
    public Transform boltSpawn;

    // Particle effects to play
    public ParticleSystem leftthrust;
    public ParticleSystem rightthrust;
    public ParticleSystem redthruster;
    public ParticleSystem bluethruster;

    // Player explosion
    public GameObject explosion;

    Rigidbody body;     // Rigidbody for the player
    float nextFire;
    

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        // Spawning in the bolts
        if(Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            var bulletInstance = Instantiate(bolt, boltSpawn.position, boltSpawn.rotation);
            GetComponent<AudioSource>().Play();
            Destroy(bulletInstance, 1.5f);
        }
	}

    void FixedUpdate()
    {
        var up = Input.GetAxis("Vertical");
        var turn = Input.GetAxis("Horizontal");

        // Gradually decrease Velocity 
        if (body.velocity.magnitude > 0)
        {
            body.velocity -= body.velocity.normalized * Time.deltaTime;
        }

        MovePlayer(turn, up);
    }

    // How the player moves
    void MovePlayer(float x, float z)
    {      

        // Apply force to the player to move
        if (z != 0)
        {
            if (!redthruster.enableEmission && !bluethruster.enableEmission)
            {
                redthruster.Play();
                bluethruster.Play();
                redthruster.enableEmission = true;
                bluethruster.enableEmission = true;
            }
            
            if (z > 0)
                body.AddForce(body.gameObject.transform.forward * playerSpeed);
            else
                body.AddForce(-body.gameObject.transform.forward * playerSpeed);
        }
        else
        {
            redthruster.Stop();
            bluethruster.Stop();

            bluethruster.enableEmission = false;
            redthruster.enableEmission = false;
        }

        if (x > 0)
        {
            if (!leftthrust.enableEmission)
            {
                leftthrust.Play();
                leftthrust.enableEmission = true;
            }
        }
        else
        {
            leftthrust.Stop();
            leftthrust.enableEmission = false;
        }

        if (x < 0)
        {
            if (!rightthrust.enableEmission)
            {
                rightthrust.Play();
                rightthrust.enableEmission = true;
            }
        }
        else
        {
            rightthrust.Stop();
            rightthrust.enableEmission = false;
        }

        transform.Rotate(Vector3.up * x * turnSpeed);

        // Limiting the speed of the player
        if (body.velocity.magnitude > maxSpeed)
            body.velocity = body.velocity.normalized * maxSpeed;
    }

    // When player is hit
    void OnTriggerEnter(Collider con)
    {
        if (con.gameObject.tag == "bad")
        {
            Destroy(gameObject);
            Instantiate(explosion, transform.position, transform.rotation);
            FindObjectOfType<WorldScript>().EndLevel(false);
        }
    }
}
