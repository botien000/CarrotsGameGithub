using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamePlay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtScore;
    [SerializeField] private Image[] imgCarrotPoint;
    [SerializeField] private TextMeshProUGUI txtQuestion;
    [SerializeField] private Image[] imgHeart;

    private void OnEnable()
    {
        ClearUI();
    }
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
        txtScore.text = "Score:" + score.ToString();
    }
    /// <summary>
    /// Hiện điểm cà rốt trên UI
    /// </summary>
    /// <param name="curPoint">curPoint</param>
    /// <param name="totalPoint">totalPoint</param>
    public void SetImgCarrotPoint(float curPoint,float totalPoint)
    {
        if(totalPoint < imgCarrotPoint.Length || totalPoint % imgCarrotPoint.Length != 0)
        {
            Debug.LogError("Bug logic.Fix!!!");
            return;
        }
        //nếu điểm hiện tại lớn hơn điểm tổng thì không cần xử lý hình ảnh cà rốt
        if (curPoint > totalPoint)
            return;
        int index = (int)(curPoint / (totalPoint / imgCarrotPoint.Length));
        //trường hợp tìm ra index nhưng chia không dư thì giảm index một giá trị
        if ((curPoint % (totalPoint / imgCarrotPoint.Length)) == 0)
        {
            index -= 1;
        }
        imgCarrotPoint[index].fillAmount = curPoint / (totalPoint / imgCarrotPoint.Length) - (float)index;
    }   
    public void ShowTextQuestion(string question)
    {
        txtQuestion.text += question;
    }
    public void ClearText()
    {
        txtQuestion.text = "";
    }
    public void BtnEnd()
    {
        GameManager.instance.SetState(GameManager.StateGame.GameOver);
    }
    private void ClearUI()
    {
        SetTextScore(0);
        foreach (var img in imgCarrotPoint)
        {
            img.fillAmount = 0;
        }
    }
    public void ShowHeartPlayer(int heart)
    {
        if (heart <= imgHeart.Length)
        {
            for (int i = 0; i < imgHeart.Length; i++)
            {
                if (i < heart)
                {
                    imgHeart[i].gameObject.SetActive(true);
                }
                else
                {
                    imgHeart[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
