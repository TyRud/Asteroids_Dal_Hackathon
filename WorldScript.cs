using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// This class will decide the state of the game for each level
public class WorldScript : MonoBehaviour {

    public Camera worldCamera;      // Reference to the camrea to calculate bounds

    // World Boundaries 
    float xlimit;                   // Limit of the game horizontally
    float zlimit;                   // Limit of the game vertically
    public GameObject player;       // Reference to the player prefab
    public GameObject enemy;        // Reference to the enemy prefab
    public GameObject asteroid1;    // Reference to the Level 1 Asteroid
    public GameObject asteroid2;    // Reference to the Level 2 Asteroid

    public Image one;
    public Image two;
    public Image three;
    public Image victory;
    public Image defeat;
    

    public int enemies;             // Keeps track of the number of enemies in the scene
    bool playing;


	// Use this for initialization
	void Start () {
        playing = true;

        // Calculate the bounds of the game
        zlimit = 2f * worldCamera.orthographicSize;
        xlimit = zlimit * worldCamera.aspect;
        var collider = GetComponent<BoxCollider>();
        collider.size = new Vector3(xlimit, 2, zlimit);

        if (Application.loadedLevel == 0)
        {
            StartCoroutine(Spawnlevel1());
        }

        if (Application.loadedLevel == 1)
        {
            StartCoroutine(Spawnlevel2());
        }
	}
	
	// Update is called once per frame
	void Update () {
        if ( enemies == 0 )
        {
            EndLevel(true);
        }

        if(!playing && Input.GetKey(KeyCode.Return))
        {
            Application.LoadLevel(0);
        }
    }

    void OnTriggerExit(Collider con)
    {   
        var posToInvert = con.gameObject.transform.position;

        // Check case if hitting the top/bottom vs the left/right
        if (Mathf.Abs(posToInvert.x) > (xlimit / 2)-1)
        {
            posToInvert.x *= -1;
            posToInvert.x = (posToInvert.x > 0) ? posToInvert.x - 1 : posToInvert.x + 1;
        }

        if (Mathf.Abs(posToInvert.z) > (zlimit / 2)-1)
        {
            posToInvert.z *= -1;
            posToInvert.z = (posToInvert.z > 0) ? posToInvert.z -1 : posToInvert.z + 1;
        }

        con.gameObject.transform.position = posToInvert;
    }

    // Setting up everything for level 1
    IEnumerator Spawnlevel1()
    {
        enemies = 5;
        GameObject instance;
        
        // First asteroid of the level
        instance = Instantiate(asteroid1, transform.position, transform.rotation) as GameObject;
        instance.GetComponent<Level1Asteroid>().direction = new Vector3(Randomize(), 0, Randomize());

        instance = Instantiate(asteroid1, transform.position, transform.rotation) as GameObject;
        instance.GetComponent<Level1Asteroid>().direction = new Vector3(Randomize(), 0, Randomize());

        instance = Instantiate(asteroid2, transform.position, transform.rotation) as GameObject;
        instance.GetComponent<Level2Asteroid>().direction = new Vector3(Randomize(), 0, Randomize());

        three.color = new Color(three.color.r, three.color.g, three.color.b, 255);

        yield return new WaitForSeconds(1f);
        Destroy(three);

        two.color = new Color(two.color.r, two.color.g, two.color.b, 255);
        yield return new WaitForSeconds(1f);
        Destroy(two);
        
        one.color = new Color(one.color.r, one.color.g, one.color.b, 255);
        yield return new WaitForSeconds(1f);
        Destroy(one);

        Instantiate(player, transform.position, transform.rotation);
    }

    // Setting up everything for level 2
    IEnumerator Spawnlevel2()
    {
        enemies = 9;
        GameObject instance;

        var playerInstance = Instantiate(player, transform.position, transform.rotation) as GameObject;
        playerInstance.SetActive(false);

        // Setting the referene to the player for the enemy to follow
        var enemyinstance = Instantiate(enemy, transform.position, transform.rotation) as GameObject;
        enemyinstance.GetComponent<Enemy>().player = playerInstance;
        enemyinstance.GetComponent<Enemy>().direction = new Vector3(Randomize(), 0, Randomize());
        enemyinstance.GetComponent<Enemy>().fireRate = 5f;

        instance = Instantiate(asteroid1, transform.position, transform.rotation) as GameObject;
        instance.GetComponent<Level1Asteroid>().direction = new Vector3(Randomize(), 0, Randomize());

        instance = Instantiate(asteroid1, transform.position, transform.rotation) as GameObject;
        instance.GetComponent<Level1Asteroid>().direction = new Vector3(Randomize(), 0, Randomize());

        instance = Instantiate(asteroid2, transform.position, transform.rotation) as GameObject;
        instance.GetComponent<Level2Asteroid>().direction = new Vector3(Randomize(), 0, Randomize());

        instance = Instantiate(asteroid2, transform.position, transform.rotation) as GameObject;
        instance.GetComponent<Level2Asteroid>().direction = new Vector3(Randomize(), 0, Randomize());


        three.color = new Color(three.color.r, three.color.g, three.color.b, 255);

        yield return new WaitForSeconds(1f);
        Destroy(three);

        two.color = new Color(two.color.r, two.color.g, two.color.b, 255);
        yield return new WaitForSeconds(1f);
        Destroy(two);

        one.color = new Color(one.color.r, one.color.g, one.color.b, 255);
        yield return new WaitForSeconds(1f);
        Destroy(one);

        // Enable the player
        playerInstance.SetActive(true);
    }

    // Random value for the direction of the asteroids
    float Randomize()
    {
        float value;
        value = Random.Range(0.25f, 1f);
        return (Random.value > 0.5) ? value*1 : value * -1;
    }

    // How to proceed
    public void EndLevel(bool win)
    {
        // Did the player die?
        if(!win)
        {
            defeat.color = new Color(defeat.color.r, defeat.color.g, defeat.color.b, 255);
            playing = false;
        }
        else
        {
            if (Application.loadedLevel == 0)
            {
                Application.LoadLevel(1);
            }
            else
            {
                victory.color = new Color(victory.color.r, victory.color.g, victory.color.b, 255);
                playing = false;
            }
        }
    }
}
