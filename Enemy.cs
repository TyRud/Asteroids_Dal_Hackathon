using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public int health;              // Toughness of the enemy
    public float speed;             // How fast the enemy travels
    public float fireRate;          // How fast the enemy fires
    public Transform boltSpawn;     // Enemy Bolts
    public Vector3 direction;       // Direction for the enemy
    public GameObject explosion;    // Enemy explosion
    public GameObject bolt;         // Enemy bolts
    public ParticleSystem chargeUp; // Enemy Charging animation
    public GameObject player;       // Reference to the player

    Rigidbody body;                 // Rigidbody for the enemy
    float nextFire;                 // How long until the next fire

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody>();
        body.velocity = speed * direction;
        nextFire = Time.deltaTime + fireRate;
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(player.transform);

        // Control when the enemy fires a bullet
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            StartCoroutine(Fire(1f));
        }
	}

    // Fires the enemy bolts after alowing the animation to play
    IEnumerator Fire(float t)
    {
        SpawnCharge();

        // Calculate the rotation to apply to the dirction of the enemy bullets
        yield return new WaitForSeconds(t);
        SpawnBolt(boltSpawn.rotation);
    }

    // Spawns the enemy particle effect before destroying it
    void SpawnCharge()
    {
        var instance = Instantiate(chargeUp, boltSpawn.position, chargeUp.transform.rotation) as ParticleSystem;
        instance.transform.parent = boltSpawn.transform;
    }

    // Spawns the enemy bolts based on a base rotation in the direction of the player
    void SpawnBolt(Quaternion baseRotation)
    {
        var instance = Instantiate(bolt, boltSpawn.position, baseRotation);
        Destroy(instance, 1.5f);
        instance = Instantiate(bolt, boltSpawn.position, baseRotation * Quaternion.Euler(Vector3.up * 20));
        Destroy(instance, 1.5f);
        instance = Instantiate(bolt, boltSpawn.position, baseRotation * Quaternion.Euler(Vector3.up * -20));
        Destroy(instance, 1.5f);
    }

    // When the enemy dies
    void Died()
    {
        Destroy(gameObject);
        Instantiate(explosion, transform.position, transform.rotation);
        FindObjectOfType<WorldScript>().enemies -= 1;
    }


    // Decide what to do when the enemy is hit by other objects
    void OnTriggerEnter(Collider con)
    {
        if (con.gameObject.tag == "bad")
        {
            // Do nothing
            return;
        }

        // hit by the player....
        if (con.gameObject.name == "Player")
        {
            Destroy(gameObject);
            Died();
        }

        // Hit by a bolt from the player
        if (con.gameObject.tag == "PlayerBolt")
        {
            Destroy(con.gameObject);
            health = health - 1;

            // Change Color of the Enemy
            Renderer renderer = gameObject.GetComponent<Renderer>();
            renderer.material.color = renderer.material.color + new Color(1f, 0, 0, 0);

            if (health < 1)
            {
                Died();
            }
        }
    }
}
