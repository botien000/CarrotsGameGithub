using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamePlay : MonoBehaviour
{
    [SerializeField] private TextMeshPro txtScore;
    [SerializeField] private Image[] imgCarrotPoint;
    [SerializeField] private TextMeshPro txtQuestion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Hiện điểm trên UI
    /// </summary>
    /// <param name="score">Score</param>
    public void SetTextScore(int score)
    {
        txtScore.text = score.ToString();
    }
    /// <summary>
    /// Hiện điểm cà rốt trên UI
    /// </summary>
    /// <param name="curPoint">curPoint</param>
    /// <param name="totalPoint">totalPoint</param>
    public void SetTextCarrotPoint(int curPoint,int totalPoint)
    {
        imgCarrotPoint[curPoint / totalPoint].fillAmount = (curPoint % totalPoint) * 20 / totalPoint;
    }
    public void ShowTextQuestion(string question)
    {
        txtQuestion.text += question;
    }
    public void BtnEnd()
    {
        GameManager.instance.SetState(GameManager.StateGame.GameOver);
    }
}
