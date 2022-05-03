using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public PlayerLoader myLoader;

    public MapGenerator myGenerator;

    public PathManager myManager;

    void Awake()
    {
        myLoader = GameObject.Find("Player Loader").GetComponent<PlayerLoader>();
        myGenerator = GameObject.Find("Map Generator").GetComponent<MapGenerator>();
        myManager = GameObject.Find("Path Manager").GetComponent<PathManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        Destroy(myLoader.gameObject);
        Destroy(myGenerator.gameObject);
        Destroy(myManager.gameObject);
        SceneManager.LoadScene("TitleScreen");
    }
}
