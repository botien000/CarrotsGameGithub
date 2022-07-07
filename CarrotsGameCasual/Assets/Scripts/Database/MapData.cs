using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public class MapData
{
    public int typeMap;

    public Level[] levels = new Level[10];
    //public List<int> listInt = new List<int>(){1,2};

    //public MapData()
    //{
    //    for (int i = 0; i < levels.Length; i++)
    //    {
    //        levels[i] = new Level();
    //    }
    //}

    public MapData(int typeMap)
    {
        this.typeMap = typeMap;
    }

    public int GetScore(int level)
    {
        return levels[level - 1].score;
    }
    public int GetCarrotStar(int level)
    {
        return levels[level - 1].carrotStar;
    }
    public void SetScore(int level, int score)
    {
        levels[level - 1].score = score;
        levels[level - 1].lv = level;
    }
    public void SetCarrotStar(int level, int star)
    {
        levels[level - 1].carrotStar = star;
    }
    public int[] GetHighScore()
    {
        Level lv = new Level();
        var highscoreLv = from level in levels
                          orderby level.score descending
                          select level;
        foreach (var item in highscoreLv)
        {
            lv = item;
            break;
        }
        return new int[3] { lv.lv, lv.score, lv.carrotStar };
    }
    [Serializable]
    public class Level
    {
        public int lv;
        public int score;
        public int carrotStar;
    }
}

