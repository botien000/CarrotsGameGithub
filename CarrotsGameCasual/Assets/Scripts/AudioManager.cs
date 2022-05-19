using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private AudioSource auidoSourceSFX;

    [SerializeField] private AudioClip home;
    [SerializeField] private AudioClip road;
    [SerializeField] private AudioClip clickToMap;
    [SerializeField] private AudioClip getRightAnswer;
    [SerializeField] private AudioClip getWrongAnswer;
    [SerializeField] private AudioClip getItem;
    [SerializeField] private AudioClip gameOver;

    //music:
    //nhạc trang chủ
    //nhac gameplay or nhạc 4 con đường chạy trong game
    //sfx:
    //click chọn map
    //thỏ ăn đúng trái cà rốt
    //thỏ ăn sai trái cà rốt
    //thỏ nhảy ngang(tạm thời bỏ)
    //thỏ nhận item
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
