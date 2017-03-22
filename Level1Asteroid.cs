using UnityEngine;
using System.Collections;

public class Level1Asteroid : MonoBehaviour {

    public int health;
    public float tumble;
    public float speed;
    public GameObject explosion;
    public Vector3 direction;
    

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
        GetComponent<Rigidbody>().velocity = direction * speed;
    }

    // When the asteroid is destroyed
    void Died()
    {
        Destroy(gameObject);
        var instance = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(instance, 2f);
        FindObjectOfType<WorldScript>().enemies -= 1;
    }

    // Asteroid can be hit by bullets or player
    void OnTriggerEnter(Collider con)
    {
        // Reduce health when hit by a bolt
        if (con.gameObject.tag == "PlayerBolt")
        {
            health = health - 1;
            Destroy(con.gameObject);
            if (health == 0)
            {
                Died();
            }
        }

        if (con.gameObject.name == "Player")
        {
            Destroy(con.gameObject);
            Died();
        }
    }
}
