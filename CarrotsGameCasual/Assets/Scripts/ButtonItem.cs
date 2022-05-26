using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonItem : MonoBehaviour
{
    private Image image;
    private CategoryItemSctbObj category;
    private GameManager instanceGM;
    private ButtonItemManager instanceBtnItemM;
    private User player;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<User>();
        instanceBtnItemM = ButtonItemManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BtnItemClicked()
    {
        //HandleItem
        switch (category.curCategory)
        {
            case CategoryItemSctbObj.Category.HalfAnswer:
                HalfAnswer();
                break;
            case CategoryItemSctbObj.Category.Shield:
                break;
            case CategoryItemSctbObj.Category.TransAnswer:
                break;
            case CategoryItemSctbObj.Category.DoublePoint:
                break;
        }
        //RemoveButton
        instanceBtnItemM.RemoveObjInPool(this);
    }
    public void Init(CategoryItemSctbObj cate)
    {
        category = cate;
        image.sprite = category.sprite;
    }
    public CategoryItemSctbObj GetCategory()
    {
        return category;
    }
    /// <summary>
    /// Giảm nửa đáp án (cụ thể là 2 đáp án)
    /// </summary>
    /// Thiếu bắt điều kiện nếu trong kho có 2 item 50/50 trở lên thì chỉ cho chọn 1 item 
    public void HalfAnswer()
    {
        //2 biến sẽ loại bỏ phần text của answer
        int one, two;
        List<Answer> wrongAnswers = new List<Answer>();
        Answer[] answers = instanceGM.TakeAnswers();
        foreach (var answer in answers)
        {
            if (!answer.GetAnswer())
            {
                wrongAnswers.Add(answer);
            }
        }
        one = Mathf.RoundToInt(Random.Range(1, wrongAnswers.Count));
        do
        {
            two = Mathf.RoundToInt(Random.Range(1, wrongAnswers.Count));
        } while (two == one);
        answers[one].ClearTextFromItem();
        answers[two].ClearTextFromItem();
    }
    /// <summary>
    /// Khiên bảo vệ
    /// </summary>
    ///Shield đang hình ảnh mạng,xử lý mạng và ảnh khiên 
    public void Shield()
    {
        player.Shield(true);
    }
    /// <summary>
    /// Đổi đáp án
    /// </summary>
    /// Chưa làm gì
    public void TransAnswer()
    {

    }
    /// <summary>
    /// Nhân đôi điểm
    /// </summary>
    /// Thiếu thời gian và hình ảnh
    public void DoublePoint()
    {
        //Sẽ có thời gian giới hạn cho nhân đôi điểm   
        //x2
        instanceGM.SetScoreFromItem(2);
    }
}
