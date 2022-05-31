using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelsData", menuName = "ScriptableObject/Create Levels")]
public class LevelScptObj : ScriptableObject
{
    public enum Operation
    {
        Plus = 1,
        SubTract = 2,
        Multiply = 3,
        Divide = 4
    }
    public List<TurnLevel> turns;
    [System.Serializable]
    public class TurnLevel
    {
        [Range(1, 10)]
        public int turnItemRangeFrom;
        [Range(1, 10)]
        public int turnItemRangeTo;
        public int turn;
        public float speedUp;
        public Operation[] operationsEnumSptObj;
        public int numberOfOpeSptObj;
        [HideInInspector]
        private int[] operationsSptObj;
        public int[] GetOperations()
        {
            operationsSptObj = new int[operationsEnumSptObj.Length];
            for (int i = 0; i < operationsEnumSptObj.Length; i++)
            {
                operationsSptObj[i] = (int)operationsEnumSptObj[i];

            }
            return operationsSptObj;
        }
    }
}
