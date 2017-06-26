using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopItems : MonoBehaviour
{
    int maxammo;
    int damage;
    public UI2DSprite Shop;
    public ShopItems playButton;
    public UnityEvent signalOnClick = new UnityEvent();
    public bool ammo;
    public bool hp;
    public bool dmg;
    public void _onClick()
    {
        this.signalOnClick.Invoke();

    }

    void Start()
    {

        playButton.signalOnClick.AddListener(this.onPlay);
        maxammo = PlayerPrefs.GetInt("ammo", 10);
        damage = PlayerPrefs.GetInt("damage", 10);
    }
    public void onPlay()
    {
        if (LevelController.Instance.coins > 50) 
        {
            LevelController.Instance.coins -= 50;
            if (dmg) upDamage();
            if (hp) upHp();
            if (ammo) upAmmo();
        }
    }

    void upDamage()
    {
        damage+=2;
        PlayerPrefs.SetInt("damage", damage);
        PlayerPrefs.Save();
    }

    void upHp()
    {
        LevelController.Instance.maxLives += 25;
        LevelController.Instance.lives += 25;
        LevelController.Instance.Lives.text = LevelController.Instance.lives+"";
    }

    void upAmmo()
    {
        maxammo += 2;
        PlayerPrefs.SetInt("ammo", maxammo);
        PlayerPrefs.Save();
    }
}

