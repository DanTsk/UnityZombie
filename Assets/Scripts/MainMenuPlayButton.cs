using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class MainMenuPlayButton : MonoBehaviour {

    public MainMenuPlayButton playButton;
    public UI2DSprite About;

    public UnityEvent signalOnClick = new UnityEvent();
    public void _onClick()
    {
        this.signalOnClick.Invoke();

    }

    void Start()
    {
        About.enabled = false;
        About.UpdateVisibility(false, false);
        playButton.signalOnClick.AddListener(this.onPlay);
    }
    public void onPlay()
    {

        SceneManager.LoadScene("Game");
    }
    public void aboutSectionOpen()
    {
        About.enabled = true;
        StartCoroutine(FadeIn());
        //About.UpdateVisibility(false, false);
    }
    public void aboutSectionClose()
    {
       // About.enabled = false;
        StartCoroutine(FadeTo());
    }
       
    public void fade()
    {
        
    }
    IEnumerator FadeTo()
    {
        while (About.alpha > 0)
        {
            About.alpha-=Time.deltaTime;
           
            yield return null;

        }
    }
    IEnumerator FadeIn()
    {
        while (About.alpha <1)
        {
            About.alpha += Time.deltaTime ;
            yield return null;
        }
    }
}
