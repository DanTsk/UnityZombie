using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    public static LevelController Instance;
    public float betweenWavesPause;
    public int waves;


    public GameObject point1, point2;
    public GameObject zombie_1, zombie_2, zombie_3, zombie_4, zombie_5;

    public UILabel ammoLabel;
    public UILabel CoinLabel;
    public UILabel CoinLabelShop;
    public UILabel Lives;
    GameObject[] prefabs;
    public int coins;
    public int lives = 100;
    public int maxLives = 100;
    Vector3 downBorder, topBorder;
    bool wavesEnd;

    int currentWave, alreadySpawned, willSpawn, zombieAlive;

    float creepsPause, wavesPause;


    public Vector3 destination;

    void Awake()
    {
        Instance = this;
        destination = new Vector3(100f, 0, 16f);
        currentWave = 1;
        wavesEnd = false;

        creepsPause = 0;
        zombieAlive = 0;
        wavesPause = betweenWavesPause;
        prefabs = new GameObject[5] { zombie_1, zombie_2, zombie_3, zombie_4, zombie_5 };

    }

    void Start()
    {
        ammoLabel.text = PlayerPrefs.GetInt("ammo", 10)+"";
        downBorder = point1.transform.position;
        topBorder = point2.transform.position;

        willSpawn = getWavesZombies();
        zombieAlive = willSpawn;
        coins = 0;
    }



    public void onPointReached()
    {
        Debug.Log("zombie come");
        lives--;
        Lives.text = lives+"";
        
    }

    public void onZombieKilled()
    {
        zombieAlive--;
        Debug.Log(zombieAlive + "alive");
        Debug.Log("zombie killed");
        coins++;
        if (coins < 10) CoinLabel.text = "000"+coins ;
        else if (coins < 100) CoinLabel.text = "00" + coins;
        else if (coins < 10) CoinLabel.text = "0" + coins;
        else CoinLabel.text = "" + coins;

        if (coins < 10) CoinLabelShop.text = "000" + coins;
        else if (coins < 100) CoinLabelShop.text = "00" + coins;
        else if (coins < 10) CoinLabelShop.text = "0" + coins;
        else CoinLabelShop.text = "" + coins;
    }

    public void onGrenadeThrow(int grenadesLeft)
    {
        Debug.Log("grenafes left " + grenadesLeft);
    }

    public void onShooted(int hasAmmo)
    {
        ammoLabel.text = hasAmmo+"";
        Debug.Log("current ammo " + hasAmmo);
    }

    public void onReloaded(int hasAmmo)
    {
        ammoLabel.text = hasAmmo + "";
        Debug.Log("current ammo " + hasAmmo);
    }



    private void Update()
    {
        if (wavesEnd)
        {
            wavesPause -= Time.deltaTime;

            if (wavesPause <= 0 && zombieAlive <= 0)
            {
                wavesEnd = false;
                willSpawn = getWavesZombies();
                zombieAlive = willSpawn;
                wavesPause = betweenWavesPause;
            }
        }

        if (!wavesEnd)
        {
            creepsPause -= Time.deltaTime;

            if (creepsPause <= 0)
            {
                spawnZombies();
                creepsPause = Random.Range(.5f, 5f);
            }

        }

    }

    private void spawnZombies()
    {
        if (alreadySpawned == willSpawn)
        {
            wavesEnd = true;

            currentWave++;
            alreadySpawned = 0;
            willSpawn = getWavesZombies();

            return;
        }

        int zombieType = Random.Range(0, 5);

        GameObject obj = GameObject.Instantiate(prefabs[zombieType]);
        obj.transform.position = getSpawningPoint();
        alreadySpawned++;

    }

    private Vector3 getSpawningPoint()
    {
        Vector3 point = new Vector3(Random.Range(topBorder.x, downBorder.x), 0, Random.Range(downBorder.z, topBorder.z));
        return point;
    }

    private int getWavesZombies()
    {
        return (currentWave * 2) + 1;
    }
}