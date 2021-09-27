using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { set; get; }

    private bool mutedSFX = false;
    private bool mutedMusic = false;
    public AudioSource[] sfx ;
    public AudioSource music;
    public Toggle musicToggle;
    
    void Start()
    {
        // chequeamos si esta guardadas las preferencias del usuario...
        if(!PlayerPrefs.HasKey("mutedSFX"))
        {
            PlayerPrefs.SetInt("mutedSFX", 0);
            LoadSFXPref();
        }
        else
        {
            LoadSFXPref();
        }

     
        if (!PlayerPrefs.HasKey("mutedMusic"))
        {
            PlayerPrefs.SetInt("mutedMusic", 0);
            LoadMusicPref();
        }
        else
        {
            LoadMusicPref();
        }   

        if (!mutedMusic)
        {
            musicToggle.isOn = true;

            if (!music.isPlaying)
            {
                music.Play();
            }
            else
            {
                //music.Stop();
            }
            
        }
        else
        {
            musicToggle.isOn = false;            
        }

    }

 
    public void OnMusicPress()
    {
        if (!mutedMusic)
        {
            mutedMusic = true;        
            music.volume = 0;

        }
        else
        {
            mutedMusic = false;           
            music.volume = 1;
        }

        SaveMusicPref();
    }


    public void OnSoundPress()
    {     

        if (!mutedSFX)
        {
            mutedSFX = true;   


            for (int i = 0; i< sfx.Length; i++)
            {
                sfx[i].volume = 0;
            }           

        }
        else
        {
            mutedSFX = false;           

            for (int i = 0; i < sfx.Length; i++)
            {
                sfx[i].volume = 1;
            }
        }

        SaveSFXPref();
    }


    // funciones para guardar las preferencias del jugador
    private void LoadMusicPref()
    {       
        mutedMusic = PlayerPrefs.GetInt("mutedMusic") == 1;
    }

    private void SaveMusicPref()
    {
        // if muted = 1 => muted = true. Else if muted = 0 => muted = false    
        PlayerPrefs.SetInt("mutedMusic", mutedMusic ? 1 : 0);     
    }

    private void LoadSFXPref()
    {     
        mutedSFX = PlayerPrefs.GetInt("mutedSFX") == 1;  
    }

    private void SaveSFXPref()
    {       
        PlayerPrefs.SetInt("mutedSFX", mutedSFX ? 1 : 0);
    }
}
