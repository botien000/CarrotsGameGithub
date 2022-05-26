using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Answer : MonoBehaviour
{
    private Animator animator;
    private TextMeshProUGUI txtAnswer;
    private Rigidbody2D rgbody;
    private bool rightAnswer;
    private GameManager instanceGM;
    private SpawnManager instanceSM;
    private Vector2 pos;
    private bool interact;
    public float speed;

    public bool Interact { get => interact; set => interact = value; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rgbody = GetComponent<Rigidbody2D>();
        txtAnswer = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        interact = true;
        pos = transform.position;
        SetAnimation(false, true);
    }
    // Start is called before the first frame update
    void Start()
    {
        instanceGM = GameManager.instance;
        instanceSM = SpawnManager.instance;
        interact = true;
    }
    // Update is called once per frame
    void Update()
    {
        pos += Vector2.down * speed * Time.deltaTime;
        rgbody.MovePosition(pos);
    }
    public void Init(int text, bool answer)
    {
        txtAnswer.text = text.ToString();
        rightAnswer = answer;
    }
    private void Move(float _speed)
    {

    }
    public void HandleDie(bool collision)
    {
        SetAnimation(collision, false);
    }
    public void Die()
    {
        instanceSM.answersPool.RemoveObjInPool(gameObject);
    }
    public bool GetAnswer()
    {
        return rightAnswer;
    }
    /// <summary>
    /// Loại bỏ text đáp án khi iitem được kích hoạt
    /// </summary>
    public void ClearTextFromItem()
    {
        txtAnswer.text = "";
    }
    /// <summary>
    /// Animation
    /// </summary>
    /// <param name="mcInteract">Main tương tác</param>
    /// <param name="interact">Tên biến này dùng để xét nó có thể tương tác không</param>
    private void SetAnimation(bool mcInteract, bool interact)
    {
        animator.SetBool("MCinteract", mcInteract);
        animator.SetBool("Interact", interact);
    }
}
