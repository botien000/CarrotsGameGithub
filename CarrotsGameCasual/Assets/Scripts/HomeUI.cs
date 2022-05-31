using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HomeUI : MonoBehaviour
{
    [SerializeField] private Button[] buttonMaps;
    [SerializeField] private List<CarrotMap> carrotMaps;
    [SerializeField] private TextMeshProUGUI[] txtScoreMap;
    [SerializeField] private Sprite imgMusicOn, imgMusicOff;
    [SerializeField] private Image imgMusic;
    [SerializeField] private AudioSource audioSource;

    private int stateMusic;//0:off  1:on
    private List<Map> maps;
    private DataManager instanceDM;
    // Start is called before the first frame update
    void Start()
    {
        instanceDM = DataManager.instance;
        //call all map
        maps = instanceDM.GetAllMap();
        //call volume
        stateMusic = instanceDM.GetMusic();
        HandleButtonMap();
        HandleAudio();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void HandleButtonMap()
    {
        if(maps.Count > buttonMaps.Length)
        {
            Debug.LogError("Bug logic.Fix please");
            return;
        }
        for (int i = 0; i < buttonMaps.Length; i++)
        {
            Debug.Log("Map " + (i + 1) + ":       Score  " + maps[i].score + "        Star   " + maps[i].star + "");
            HandleScoreMap(i, maps[i].score.ToString());
            HandleCarrotMap(i, maps[i].star - 1);
            if (maps[i].open == 1)
            {
                buttonMaps[i].interactable = true;
            }
            else
            {
                buttonMaps[i].interactable = false;
            }
        }
    }
    private void HandleScoreMap(int index,string txtScore)
    {
        txtScoreMap[index].text = "Score : " + txtScore;
    }
    private void HandleCarrotMap(int index,int indexCarrot)
    {
        for (int i = 0; i < carrotMaps[index].imgCarrots.Length; i++)
        {
            if(i <= indexCarrot)
            {
                carrotMaps[index].imgCarrots[i].gameObject.SetActive(true);
            }
            else
            {
                carrotMaps[index].imgCarrots[i].gameObject.SetActive(false);
            }
        }
    }
    private void HandleAudio()
    {
        if(stateMusic == 1)
        {
            //on
            audioSource.Play();
            imgMusic.sprite = imgMusicOn;
        }
        else
        {
            //off
            audioSource.Pause();
            imgMusic.sprite = imgMusicOff;
        }
        instanceDM.SetMusic(stateMusic);
    }
    public void BtnAudio()
    {
        if(stateMusic == 1)
        {
            stateMusic = 0;
        }
        else
        {
            stateMusic = 1;
        }
        HandleAudio();
    }
    public void BtnMap(int level)
    {
        SceneManager.LoadScene("Lv" + level);
    }
    [System.Serializable]
    class CarrotMap
    {
        public Image[] imgCarrots;
    }
}
