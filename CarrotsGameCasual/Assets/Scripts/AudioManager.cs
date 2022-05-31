using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private AudioSource audioSourceSFX;

    [SerializeField] private AudioClip home;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip getRightAnswer;
    [SerializeField] private AudioClip getWrongAnswer;
    [SerializeField] private AudioClip getItem;
    [SerializeField] private AudioClip gameOver;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip breakShield;
    [SerializeField] private GameSetting gameSettingUI;

    private DataManager instanceDM;
    private int statusSound;
    private int statusMusic;
    //music:
    //nhạc trang chủ(done)
    //nhac gameplay or nhạc 4 con đường chạy trong game (đã lấy nhạc trang chủ)
    //sfx:
    //click(done)
    //thỏ ăn đúng trái cà rốt
    //thỏ ăn sai trái cà rốt
    //thỏ nhảy ngang(done)
    //thỏ nhận item(done)
    //kết thúc game

    /// <summary>
    /// Singleton 
    /// </summary>
    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        instanceDM = DataManager.instance;
        statusSound = instanceDM.GetSound();
        statusMusic = instanceDM.GetMusic();
        SetSoundSetting(false);
        SetMusicSetting(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetSoundSetting(bool isPressed)
    {
        //khi nhấn nút
        if (isPressed)
        {
            if (statusSound == 1)
            {
                statusSound = 0;
                gameSettingUI.SetImageSound(false);
            }
            else
            {
                statusSound = 1;
                gameSettingUI.SetImageSound(true);
            }
        }
        else
        {
            //khi không nhấn nút
            if (statusSound == 1)
            {
                gameSettingUI.SetImageSound(true);
            }
            else
            {
                gameSettingUI.SetImageSound(false);
            }
        }
        SetMute(statusSound == 1 ? false : true,1);
        //save db
        instanceDM.SetSound(statusSound);
    }
    public void SetMusicSetting(bool isPressed)
    {
        //khi nhấn nút
        if (isPressed)
        {
            if (statusMusic == 1)
            {
                statusMusic = 0;
                gameSettingUI.SetImageMusic(false);
            }
            else
            {
                statusMusic = 1;
                gameSettingUI.SetImageMusic(true);
            }
        }
        else
        {
            //khi không nhấn nút
            if (statusMusic == 1)
            {
                gameSettingUI.SetImageMusic(true);
            }
            else
            {
                gameSettingUI.SetImageMusic(false);
            }   
        }
        SetMute(statusMusic == 1 ? false : true, 0);
        //save db
        instanceDM.SetSound(statusMusic);
    }
    private void SetMute(bool mute,int type)
    {
        if(type == 0)
        {
            audioSourceMusic.mute = mute;
            instanceDM.SetMusic(mute ? 0 : 1);
            return;
        }
        audioSourceSFX.mute = mute;
        instanceDM.SetSound(mute ? 0 : 1);
    }
    public void JumpFx()
    {
        audioSourceSFX.PlayOneShot(jump);
    }
    public void ClickFx()
    {
        audioSourceSFX.PlayOneShot(click);
    }
    public void GetItemFx()
    {
        audioSourceSFX.PlayOneShot(getItem);
    }
    public void RightAnswerFx()
    {
        audioSourceSFX.PlayOneShot(getRightAnswer);
    }
    public void WrongAnswerFx()
    {
        audioSourceSFX.PlayOneShot(getWrongAnswer);
    }
    public void GameOverFx()
    {
        audioSourceSFX.PlayOneShot(gameOver);
    }
    public void BreakShieldFx()
    {
        audioSourceSFX.PlayOneShot(breakShield);
    }
}
