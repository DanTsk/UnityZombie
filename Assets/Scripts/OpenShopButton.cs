using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class OpenShopButton : MonoBehaviour
{

    public UI2DSprite Shop;
    public OpenShopButton playButton;
    public UnityEvent signalOnClick = new UnityEvent();
    public void _onClick()
    {
        this.signalOnClick.Invoke();

    }

    void Start()
    {
       
        playButton.signalOnClick.AddListener(this.onPlay);
    }
    public void onPlay()
    {
        Shop.enabled = true;
        //Shop.UpdateVisibility(true, true);
        StartCoroutine(FadeIn());
       
        
        
    }

    public void CloseShop()
    {
        //Shop.UpdateVisibility(false, false);
        //Shop.enabled = false;
        StartCoroutine(FadeTo());
        Time.timeScale = 1;
    }

    public void ToggleSound()
    {
        SoundManager.Instance.switchMusic();
        SoundManager.Instance.switchSound();
        Debug.Log(SoundManager.Instance.isMusicOn());
    }
    public void onRestart()
    {
        SoundManager.Instance.switchSound();
        SceneManager.LoadScene("menu");
        
    }
    IEnumerator FadeTo()
    {
        while (Shop.alpha > 0)
        {
            Shop.alpha -= Time.deltaTime*3;

            yield return null;

        }
    }
    IEnumerator FadeIn()
    {
        
        while (Shop.alpha < 1)
        {
            Shop.alpha += Time.deltaTime*3;
            yield return null;
        }
        Time.timeScale = 0;
         //Shop.UpdateVisibility(true, true);
    }
}
