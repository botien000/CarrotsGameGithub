using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum StateGame
    {
        GameOver, GamePlay, GameWait
    }
    [SerializeField] private GamePlay gamePlayUI;
    [SerializeField] private GameOver gameOverUI;
    [SerializeField] private GameWait gameWaitUI;
    [SerializeField] private Transform[] posSpawnAnswer_Item;
    [SerializeField] private LevelScptObj levelScptObj;
    [SerializeField] private int lowerLimit, highestLimit;
    [SerializeField] private float speedInGame;
    [SerializeField] private float increaseSlowly;
    [SerializeField] private int scoreAchieve;
    [SerializeField] private int totalCarrotPoint;

    public Text txtQuestion;
    public Text txtRightAnswer;

    private List<int> listParams;
    private List<int> paramAfter, opeAfter;
    private List<int> paramFull, operationFull;
    private List<Group> groups;
    int firstParameter, secondParameter;
    int indexGroup;
    private int curTurnInGame;
    private int rightAnswer;
    private int curIndexLvScptObj;
    private int turnItem;

    private int curScore;
    private int curCarrotPoint;
    private float curSpeed;

    private SpawnManager instanceSM;
    /// <summary>
    /// Singleton
    /// </summary>
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;

        listParams = new List<int>();
        paramAfter = new List<int>();
        opeAfter = new List<int>();
        paramFull = new List<int>();
        operationFull = new List<int>();
        groups = new List<Group>();
        curTurnInGame = 0;
        curIndexLvScptObj = 0;
        turnItem = 0;
        curScore = 0;
        curCarrotPoint = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        instanceSM = SpawnManager.instance;
        curSpeed = speedInGame;
        RandomTurnItem();
    }

    // Update is called once per frame
    void Update()
    {
        curSpeed += increaseSlowly;
    }
    #region Turn
    public void NextTurn()
    {
        curTurnInGame++;
        //Initial Item
        if (curTurnInGame == turnItem)
        {
            //spawn item
            RandomTurnItem();
            return;
        }

        //Initial Question
        for (int i = levelScptObj.turns.Count - 1; i >= 0; i--)
        {
            //Increase Speed
            if (curTurnInGame == levelScptObj.turns[i].turn)
                curSpeed += levelScptObj.turns[i].speedUp;

            if (curTurnInGame >= levelScptObj.turns[i].turn)
            {
                curIndexLvScptObj = i;
                HandleQuestion(levelScptObj.turns[i].numberOfOpeSptObj, levelScptObj.turns[i].GetOperations());
                break;
            }
        }
        //Initial Answer
        int indexRight = RandomNumber(1, posSpawnAnswer_Item.Length);
        List<int> wrongNumbers = new List<int>();
        int wrongNumber;
        for (int i = 0; i < posSpawnAnswer_Item.Length; i++)
        {
            if (i != indexRight)
            {
                do
                {
                    wrongNumber = RandomNumber(lowerLimit, highestLimit);
                } while (wrongNumber == rightAnswer || wrongNumbers.Contains(wrongNumber));
                wrongNumbers.Add(wrongNumber);
                //spawn answer wrong
            }
            else
            {
                //spawn answer right

            }
        }



    }
    private void RandomTurnItem()
    {
        turnItem += RandomNumber(levelScptObj.turns[curIndexLvScptObj].turnItemRangeFrom, levelScptObj.turns[curIndexLvScptObj].turnItemRangeTo);
    }
    private void HandleQuestion(int numberOfOperation, int[] operations)
    {
        listParams.Clear();
        paramAfter.Clear();
        opeAfter.Clear();
        paramFull.Clear();
        operationFull.Clear();
        groups.Clear();
        indexGroup = -1;
        //for numberOfOperation tìm nhân chia để gộp hoặc tách
        for (int i = 0; i < numberOfOperation; i++)
        {
            int operation = FindOperation(operations);
            operationFull.Add(operation);
            //tìm giá trị chung x or / để gộp
            if (i > 0)
            {
                if ((operationFull[i] == 1 || operationFull[i] == 2))
                {
                    //vế riêng (là phép + -)
                    opeAfter.Add(operationFull[i]);
                }
                else if ((operationFull[i] == 3 || operationFull[i] == 4) && (operationFull[i - 1] == 3 || operationFull[i - 1] == 4))
                {
                    //gộp
                    groups[indexGroup].operationsInGroup.Add(operationFull[i]);
                }
                else
                {
                    //tách
                    indexGroup++;
                    Group group = new Group(this);
                    group.operationsInGroup.Add(operationFull[i]);
                    group.indexGroup = i;
                    groups.Add(group);
                }
            }
            else
            {
                //i = 0
                if (operation == 1 || operation == 2)
                {
                    opeAfter.Add(operation);
                }
                else
                {
                    indexGroup++;
                    Group group = new Group(this);
                    group.operationsInGroup.Add(operationFull[i]);
                    group.indexGroup = i;
                    groups.Add(group);
                }
            }
        }
        // đây là trường hợp mà trong 4 phép tính được random ban đầu không có + - chỉ có nhân chia 
        if (opeAfter.Count == 0)
        {
            paramAfter.Add(RandomNumber(lowerLimit, highestLimit));
        }
        else
        {
            paramAfter = HandleLogicOperation(opeAfter, lowerLimit, highestLimit);
        }

        bool y_n = false;
        for (int i = 0; i < paramAfter.Count; i++)
        {
            for (int j = 0; j < groups.Count; j++)
            {
                if (groups[j].indexGroup == i)
                {
                    groups[j].GetAnswers(paramAfter[i]);
                    y_n = true;
                    foreach (var item in listParams)
                    {
                        paramFull.Add(item);
                    }
                    break;
                }
            }
            if (!y_n)
                paramFull.Add(paramAfter[i]);
        }
        for (int i = 0; i < paramFull.Count; i++)
        {
            if (i == paramFull.Count - 1)
            {
                ShowQuestionTextGamePlay(paramFull[i], 0);
            }
            else
            {
                ShowQuestionTextGamePlay(paramFull[i], operationFull[i]);
            }
        }
        //xử lý lấy rightansnwer
        for (int i = 0; i < paramFull.Count; i++)
        {
            if (i == 0)
            {
                rightAnswer = paramFull[i];
            }
            else
            {
                rightAnswer = Calculate(rightAnswer, paramFull[i], operationFull[i - 1]);
            }
        }
        ShowRightAnswerText(rightAnswer);

    }
    private int FindOperation(int[] _operation)
    {
        int index = RandomNumber(1, _operation.Length);
        return _operation[index];
    }
    private int RandomNumber(int first, int second)
    {
        return Mathf.RoundToInt(Random.Range((float)first, (float)second));
    }
    private int Calculate(int first, int second, int ope)
    {
        int kq = 0;
        switch (ope)
        {
            // +
            case 1:
                kq = first + second;
                break;
            // -
            case 2:
                kq = first - second;
                break;
            // *
            case 3:
                kq = first * second;
                break;
            // /
            case 4:
                kq = first / second;
                break;
        }
        return kq;
    }
    private List<int> HandleLogicOperation(List<int> listOperation, int lowerLimit, int highestLimit)
    {
        listParams.Clear();
        for (int i = 0; i < listOperation.Count; i++)
        {
            if (i == 0)
            {
                // *
                if (listOperation[i] == 3)
                {
                    //ở trường hợp phép nhân này ta phải random một giá trị nhưng phải xét điều kiện nó có chia hết hay không,không thì lặp cho đến khi tìm được số chia hết
                    do
                    {
                        firstParameter = Mathf.RoundToInt(Random.Range(lowerLimit, highestLimit));
                    } while (highestLimit % firstParameter != 0);
                    listParams.Add(firstParameter);
                    firstParameter = LogicTwoParameters(firstParameter, listOperation[i], lowerLimit, highestLimit);
                }
                // :
                else if (listOperation[i] == 4)
                {
                    //ở trường hợp phép nhân này ta phải random một giá trị nhưng phải xét điều kiện nó có chia hết hay không,không thì lặp cho đến khi tìm được số chia hết
                    do
                    {
                        firstParameter = Mathf.RoundToInt(Random.Range(lowerLimit, highestLimit));
                    } while (firstParameter % lowerLimit != 0);
                    listParams.Add(firstParameter);
                    firstParameter = LogicTwoParameters(firstParameter, listOperation[i], lowerLimit, highestLimit);
                }
                // + or -
                else
                {
                    firstParameter = RandomNumber(lowerLimit, highestLimit);
                    listParams.Add(firstParameter);
                    firstParameter = LogicTwoParameters(firstParameter, listOperation[i], lowerLimit, highestLimit);
                }
            }
            else
            {
                switch (listOperation[i])
                {
                    case 1:
                        firstParameter = LogicTwoParameters(firstParameter, listOperation[i], lowerLimit, highestLimit);
                        break;
                    case 2:
                        firstParameter = LogicTwoParameters(firstParameter, listOperation[i], lowerLimit, highestLimit);
                        break;
                    case 3:
                        firstParameter = LogicTwoParameters(firstParameter, listOperation[i], lowerLimit, highestLimit);
                        break;
                    case 4:
                        firstParameter = LogicTwoParameters(firstParameter, listOperation[i], lowerLimit, highestLimit);
                        break;
                }
            }
        }
        return listParams;
    }
    //function này xử lý tìm tham số thứ 2 từ 1 vd:first (operation) second = lowewrLimit or highestLimit
    //add tham số thứ 2 vào listParams trả về tham số 1
    private int LogicTwoParameters(int first, int operation, int lowerLimit, int highestLimit)
    {
        int second = 0;
        switch (operation)
        {
            case 1:
                second = RandomNumber(lowerLimit, highestLimit - first);
                break;
            case 2:
                second = RandomNumber(lowerLimit, first - lowerLimit);
                break;
            case 3:
                second = Mathf.RoundToInt(Random.Range(lowerLimit, highestLimit / first));
                break;
            case 4:
                do
                {
                    second = Mathf.RoundToInt(Random.Range(lowerLimit, first / lowerLimit));
                } while (first % second != 0);
                break;
        }
        listParams.Add(second);
        firstParameter = Calculate(first, second, operation);
        return firstParameter;
    }
    private void ShowQuestionTextGamePlay(int param, int operation)
    {
        string txtOperation = "";
        switch (operation)
        {
            case 1:
                txtOperation = "+";
                break;
            case 2:
                txtOperation = "-";
                break;
            case 3:
                txtOperation = "x";
                break;
            case 4:
                txtOperation = ":";
                break;
            default:
                txtOperation = "";
                break;
        }
        gamePlayUI.ShowTextQuestion(param.ToString() + txtOperation);
    }
    private void ShowRightAnswerText(int right)
    {
        txtRightAnswer.text = right.ToString();
    }

    #endregion
    public float GetSpeedMove()
    {
        return speedInGame;
    }
    public void SetScore_CarrotPointGamePlay()
    {
        curScore += scoreAchieve;
        gamePlayUI.SetTextScore(curScore);
        curCarrotPoint++;
        gamePlayUI.SetTextCarrotPoint(curCarrotPoint, totalCarrotPoint);
    }
    #region StateGame
    /// <summary>
    /// Trạng thái game
    /// </summary>
    /// <param name="state"></param>
    public void SetState(StateGame state)
    {
        switch (state)
        {
            case StateGame.GamePlay:
                //Màn hình TabToPlay sẽ invisible
                Time.timeScale = 1f;
                break;
            case StateGame.GameWait:
                //Màn hình TabToPlay sẽ visible
                Time.timeScale = 0f;
                break;
            case StateGame.GameOver:
                gameOverUI.GetScore(curScore);
                gameOverUI.GetCarrotPoint(curCarrotPoint,totalCarrotPoint);
                Time.timeScale = 0f;
                break;
        }
    }
    #endregion
    class Group
    {
        public List<int> operationsInGroup = new List<int>();
        public int indexGroup;

        private int lowerLimit;
        private GameManager gameManager;

        public Group(GameManager gameManager)
        {
            this.gameManager = gameManager;
            lowerLimit = this.gameManager.lowerLimit;
        }

        public void GetAnswers(int resultGroup)
        {
            gameManager.HandleLogicOperation(operationsInGroup, lowerLimit, resultGroup);
        }
    }
}
