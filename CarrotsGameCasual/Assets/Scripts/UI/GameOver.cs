using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtScore;
    [SerializeField] private Image[] imgCarrotsPoint;
    [SerializeField] private LoadingScreen loadingScreen;

    private AudioManager instanceAM;
    // Start is called before the first frame update
    void Start()
    {
        instanceAM = AudioManager.instance;
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
        txtScore.text = "Score:" + score.ToString();
    }
    /// <summary>
    /// Hiện ảnh cà rốt khi kết thúc game
    /// </summary>
    /// <param name="curPoint"></param>
    /// <param name="numberOfPoint"></param>
    public void GetCarrotPoint(int index)
    {
        for (int i = 0; i < imgCarrotsPoint.Length; i++)
        {
            if (i <= index)
            {
                imgCarrotsPoint[i].fillAmount = 1;
            }
            else
            {
                imgCarrotsPoint[i].fillAmount = 0;
            }
        }
    }

    /// <summary>
    /// Chơi lại màn hiện tại
    /// </summary>
    public void BtnPlayAgain()
    {
        instanceAM.ClickFx();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    /// <summary>
    /// Trở về màn hình trang chủ
    /// </summary>
    public void BtnHome()
    {
        //Load Home Screen
        instanceAM.ClickFx();
        Time.timeScale = 1f;
        //SceneManager.LoadScene("Home");
        loadingScreen.gameObject.SetActive(true);
        loadingScreen.LoadSceneHome();

    }


}
