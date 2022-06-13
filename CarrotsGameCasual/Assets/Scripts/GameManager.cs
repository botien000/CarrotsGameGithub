using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    public enum StateGame
    {
        GameOver, GamePlay, GameWait,GameSetting
    }
    [SerializeField] private int level;
    [SerializeField] private GamePlay gamePlayUI;
    [SerializeField] private GameOver gameOverUI;
    [SerializeField] private GameWait gameWaitUI;
    [SerializeField] private GameSetting gameSettingUI;
    [SerializeField] private Transform[] posSpawnAnswer_Item;
    [SerializeField] private LevelScptObj levelScptObj;
    [SerializeField] private int lowerLimit, highestLimit;
    [SerializeField] private float speedInGame;
    [SerializeField] private float increaseSlowly;
    [SerializeField] private int scoreAchieve;
    [SerializeField] private int counterCarrot;
    [SerializeField] private CategoryItemSctbObj[] categoryItem;
    [SerializeField] private Button btnGameSetting;

    private Answer[] answers;
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
    private int curTurnItem;

    private int firstScoreAchieve;
    private float valueSlowInGame;

    private bool isPause;
    public bool IsPause => isPause;

    private int curScore;
    private float curTimeCounter;
    private int indexCounterCarrot;
    private float curSpeed;
    private SpawnManager instanceSM;
    private AudioManager instanceAM;
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
        answers = new Answer[posSpawnAnswer_Item.Length];
        curTurnInGame = 0;
        curIndexLvScptObj = 0;
        turnItem = 0;
        curScore = 0;
        curTimeCounter = 0;
        indexCounterCarrot = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        SetState(StateGame.GameWait);
        instanceAM = AudioManager.instance;
        instanceSM = SpawnManager.instance;
        curSpeed = speedInGame;
        firstScoreAchieve = scoreAchieve;
        RandomTurnItem();
    }

    // Update is called once per frame
    void Update()
    {
        curSpeed += increaseSlowly * Time.deltaTime;
        SetCounterCarrot();
    }
    private void SetCounterCarrot()
    {
        curTimeCounter += Time.deltaTime;
        gamePlayUI.SetImgCarrotPoint(indexCounterCarrot, curTimeCounter * 1 / counterCarrot);
        if(curTimeCounter >= counterCarrot)
        {
            indexCounterCarrot++;
            curTimeCounter = 0;
        }
    }
    #region Turn
    public void NextTurn()
    {
        try
        {


            curTurnInGame++;
            //Initial Item
            if (curTurnInGame == turnItem)
            {
                curTurnItem = turnItem;
                //spawn item
                Item item = instanceSM.itemPool.SpawnObjInPool(posSpawnAnswer_Item[RandomNumber(0, posSpawnAnswer_Item.Length - 1)]).GetComponent<Item>();
                item.Init(categoryItem[RandomNumber(0, categoryItem.Length - 1)]);
                RandomTurnItem();
                return;
            }
            //Initial Question
            gamePlayUI.ClearText();
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
            int indexRight = RandomNumber(0, posSpawnAnswer_Item.Length - 1);
            //tạo list để kiểm tra đã tồn tại số đó chưa
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
                    Answer answer = instanceSM.answersPool.SpawnObjInPool(posSpawnAnswer_Item[i]).GetComponent<Answer>();
                    answer.Init(wrongNumber, false);
                    answers[i] = answer;
                }
                else
                {
                    //spawn answer right
                    Answer answer = instanceSM.answersPool.SpawnObjInPool(posSpawnAnswer_Item[i]).GetComponent<Answer>();
                    answer.Init(rightAnswer, true);
                    answers[i] = answer;
                }
            }
        }catch(System.Exception e)
        {
            Debug.Log(e);
            Application.Quit();
        }
    }


    /// <summary>
    /// Khi player tương tác với câu trả lời thì xử lý huỷ bỏ
    /// </summary>
    /// <param name="_answer">Answer</param>
    public void RemoveAnswers(Answer _answer)
    {
        //check nếu turn item thì không xử lý nút transAnswer
        if (curTurnInGame == curTurnItem)
            return;
        foreach (var answer in answers)
        {
            answer.Interact = false;
            if (_answer == answer)
            {
                //hande remove answer is collide
                answer.HandleDie(true);
            }
            else
            {
                //hande remove answer isn't collide
                answer.HandleDie(false);
            }
        }
        NextTurn();

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
    }
    private int FindOperation(int[] _operation)
    {
        int index = RandomNumber(1, _operation.Length);
        return _operation[index - 1];
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
    #endregion
    public float GetSpeedMove()
    {
        return curSpeed;
    }
    public void SetScoreGamePlay()
    {
        curScore += scoreAchieve;
        gamePlayUI.SetTextScore(curScore);
    }
    public void HitPlayer(int heart)
    {
        gamePlayUI.ShowHeartPlayer(heart);
        if(heart == 0)
        {
            //gameover
            SetState(StateGame.GameOver);
        }
    }
    /// <summary>
    /// Lấy câu hỏi cho item
    /// </summary>
    /// <returns></returns>
    public Answer[] TakeAnswers()
    {
        return answers;
    }
    /// <summary>
    /// Thay đổi điểm nhận được từ item (biển clear để check nó hiện image thời gian
    /// </summary>
    /// <param name="number"></param>
    public void SetScoreFromItem(int number)
    {
        
        //nếu tham số truyền vào là 1 thì trả score về giá trị ban đầu
        scoreAchieve = firstScoreAchieve;
        scoreAchieve *= number;
        if (number != 1)
            gamePlayUI.TimeX2();
    }

    public void SetSlowInGameFromItem(int slowValue)
    {
        curSpeed += valueSlowInGame;
        valueSlowInGame = curSpeed - curSpeed / slowValue;
        curSpeed /= slowValue;
        gamePlayUI.TimeSlow();
    }
    public void ShowFlyScore(Transform pos)
    {
        ScoreFly scoreFly = instanceSM.scoreFlyPool.SpawnObjInPool(pos).GetComponent<ScoreFly>();
        scoreFly.Init(scoreAchieve.ToString());
    }
    public void FromSettingToPlay()
    {
        isPause = false;
        Time.timeScale = 1f;
        //Màn hình TabToPlay sẽ invisible
        gameWaitUI.gameObject.SetActive(false);
        btnGameSetting.gameObject.SetActive(true);
        gameSettingUI.gameObject.SetActive(false);
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
                isPause = false;
                Time.timeScale = 1f;
                //Màn hình TabToPlay sẽ invisible
                gameWaitUI.gameObject.SetActive(false);
                btnGameSetting.gameObject.SetActive(true);
                NextTurn();
                break;
            case StateGame.GameWait:
                isPause = false;
                Time.timeScale = 0f;
                gameWaitUI.gameObject.SetActive(true);
                btnGameSetting.gameObject.SetActive(false);
                //Màn hình TabToPlay sẽ visible
                break;
            case StateGame.GameOver:
                isPause = true;
                Time.timeScale = 0f;
                gameOverUI.gameObject.SetActive(true);
                //audio
                instanceAM.GameOverFx();
                gameOverUI.GetScore(curScore);
                gameOverUI.GetCarrotPoint(indexCounterCarrot - 1);
                //save db
                DataManager.instance.SaveData(level, indexCounterCarrot, curScore);
                break;
            case StateGame.GameSetting:
                isPause = true;
                Time.timeScale = 1f;
                gameSettingUI.gameObject.SetActive(true);
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
