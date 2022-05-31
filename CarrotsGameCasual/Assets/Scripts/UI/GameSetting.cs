using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSetting : MonoBehaviour
{
    [SerializeField] private Sprite imgSoundOn, imgSoundOff;
    [SerializeField] private Image imgSound;
    [SerializeField] private Sprite imgMusicOn, imgMusicOff;
    [SerializeField] private Image imgMusic;

    private AudioManager instanceAM;
    private GameManager instanceGM;
    // Start is called before the first frame update
    void Start()
    {
        instanceAM = AudioManager.instance;
        instanceGM = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BtnHome()
    {
        SceneManager.LoadScene("Home");
    }
    public void BtnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void BtnExit()
    {
        instanceGM.FromSettingToPlay();
    }
    public void BtnSound()
    {
        instanceAM.ClickFx();
        instanceAM.SetSoundSetting(true);
    }
    public void BtnMusic()
    {
        instanceAM.ClickFx();
        instanceAM.SetMusicSetting(true);
    }
    public void SetImageSound(bool on)
    {
        if (on)
        {
            imgSound.sprite = imgSoundOn;
        }
        else
        {
            imgSound.sprite = imgSoundOff;
        }
    }
    public void SetImageMusic(bool on)
    {
        if (on)
        {
            imgMusic.sprite = imgMusicOn;
        }
        else
        {
            imgMusic.sprite = imgMusicOff;
        }
    }
}
