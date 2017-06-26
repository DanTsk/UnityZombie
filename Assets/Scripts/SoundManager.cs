using UnityEngine;

public class SoundManager
{
    public static SoundManager Instance = new SoundManager();

    bool is_sound_on = true;
    bool is_music_on = true;


    public bool isSoundOn()
    {
        return is_sound_on;
    }

    public bool isMusicOn()
    {
        return is_music_on;
    }



    public void switchSound()
    {
        if (is_sound_on)
            PlayerPrefs.SetInt("sound",0);
        else
            PlayerPrefs.SetInt("sound", 1);

        is_sound_on = !is_sound_on;
    }

    public void switchMusic()
    {
        if (is_music_on)
            PlayerPrefs.SetInt("music", 0);
        else
            PlayerPrefs.SetInt("music", 1);

        is_music_on = !is_music_on;
    }

    
    SoundManager()
    {
        is_sound_on = PlayerPrefs.GetInt("sound", 1) == 1;
        is_music_on = PlayerPrefs.GetInt("music", 1) == 1;
    }

}