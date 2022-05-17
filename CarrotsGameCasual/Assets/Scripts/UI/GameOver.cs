using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshPro txtScore;
    [SerializeField] private Image[] imgCarrotsPoint;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// Hiện điểm khi kết thúc game
    /// </summary>
    /// <param name="score"></param>
    public void GetScore(int score)
    {
        txtScore.text = score.ToString();
    }
    /// <summary>
    /// Hiện ảnh cà rốt khi kết thúc game
    /// </summary>
    /// <param name="curPoint"></param>
    /// <param name="totalPoint"></param>
    public void GetCarrotPoint(int curPoint, int totalPoint)
    {
        imgCarrotsPoint[curPoint / totalPoint].gameObject.SetActive(true);
    }
    /// <summary>
    /// Chơi lại màn hiện tại
    /// </summary>
    public void BtnPlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    /// <summary>
    /// Trở về màn hình trang chủ
    /// </summary>
    public void BtnHome()
    {
        //Load Home Screen
    }
}
