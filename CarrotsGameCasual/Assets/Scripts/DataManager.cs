using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    private readonly int totalMap = 5;
    /// <summary>
    /// Singleton 
    /// </summary>
    public static DataManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private Map GetMap(int level)
    {
        Map map = new Map();
        map.open = PlayerPrefs.GetInt("OpenMap" + level, 0);
        map.star = PlayerPrefs.GetInt("StarOfMap" + level, 0);
        map.score = PlayerPrefs.GetInt("ScoreOfMap" + level, 0);
        return map;
    }
    ///<sumary>
    ///Lưu star,score,map vào PlayerPref
    ///</sumary>
    ///<param name="level">Level</param>
    ///<param name="star">Star</param>
    ///<param name="score">Score</param>
    public void SaveData(int level, int star, int score)
    {
        Map map = GetMap(level);
        if(map.star < star)
        {
            PlayerPrefs.SetInt("StarOfMap" + level, star);
        }
        if(map.score < score)
        {
            PlayerPrefs.SetInt("ScoreOfMap" + level, score);
        }
        if (star > 0 && level <= 5)
        {
            //open next map
            PlayerPrefs.SetInt("OpenMap" + (level + 1), 1);
        }
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Get toàn bộ dữ liệu của 5 map
    /// </summary>
    /// <returns>List Map</returns>
    public List<Map> GetAllMap()
    {
        List<Map> maps = new List<Map>();
        Map map;
        for (int i = 0; i < totalMap; i++)
        {
            map = new Map();
            //auto mở map 1
            if (i == 0)
            {
                map.open = PlayerPrefs.GetInt("OpenMap" + (i + 1), 1);
            }
            else
            {
                map.open = PlayerPrefs.GetInt("OpenMap" + (i + 1), 0);
            }
            map.star = PlayerPrefs.GetInt("StarOfMap" + (i + 1), 0);
            map.score = PlayerPrefs.GetInt("ScoreOfMap" + (i + 1), 0);
            maps.Add(map);
        }
        PlayerPrefs.Save();
        return maps;
    }
    /// <summary>
    /// Turn on or off sound
    /// </summary>
    /// <param name="type"></param>
    public void SetSound(int type)
    {
        PlayerPrefs.SetInt("Sound", type);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// GetSound
    /// </summary>
    /// <returns>Int</returns>
    public int GetSound()
    {
        return PlayerPrefs.GetInt("Sound", 1);
    }
    /// <summary>
    /// Turn on or off music
    /// </summary>
    /// <param name="type"></param>
    public void SetMusic(int type)
    {
        PlayerPrefs.SetInt("Music", type);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// GetMusic
    /// </summary>
    /// <returns>Int</returns>
    public int GetMusic()
    {
        return PlayerPrefs.GetInt("Music", 1);
    }
}
public class Map
{
    public int open;
    public int star;
    public int score;
}
