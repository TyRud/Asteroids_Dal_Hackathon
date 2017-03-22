using UnityEngine;
using System.Collections;

public class Level2Asteroid : MonoBehaviour {

    public int health;
    public float tumble;
    public float speed;
    public GameObject explosion;
    public GameObject asteroid;
    public Vector3 direction;
    

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
        GetComponent<Rigidbody>().velocity = direction * speed;
    }

    // Asteroid can be hit by bullets or player
    void OnTriggerEnter(Collider con)
    {
        // Reduce health when hit by a bolt
        if (con.gameObject.tag == "PlayerBolt" || con.gameObject.name == "Player")
        {
            health = health - 1;
            Destroy(con.gameObject);

            // Change Color of the asteroid
            Renderer renderer = gameObject.GetComponent<Renderer>();
            renderer.material.color = renderer.material.color + new Color(1f, 0, 0, 0);
            
            if (health == 0)
            {
                // Explode
                var instance = Instantiate(explosion, transform.position, transform.rotation);
                Destroy(instance, 2);

                // Spawn more asteroids xD
                SpawnAsteroid();
                SpawnAsteroid();
                SpawnAsteroid();

                // Destroy this object
                Destroy(gameObject);
            }
        }
    }

    // Random value for the direction of the asteroids
    float Randomize()
    {
        float value;
        value = Random.Range(0.25f, 1f);
        return (Random.value > 0.5) ? value * 1 : value * -1;
    }

    // Spawn an additional Asteroid with random direction
    void SpawnAsteroid()
    {
        var test = Instantiate(asteroid, transform.position, transform.rotation) as GameObject;
        var asteroidScript = test.GetComponent<Level1Asteroid>();
        asteroidScript.direction = new Vector3(Randomize(), 0, Randomize());
    }
}
