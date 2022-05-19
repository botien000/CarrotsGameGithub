using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Answer : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;

    public TextMeshProUGUI answer;
    private bool rightAnswer;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = new Vector2(0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    internal void Init(int _answer, bool _rightAnswer)
    {
        answer.text = "" + _answer;
        rightAnswer = _rightAnswer;
    }
    internal void Move(float _speed)
    {
        rigidbody2D.velocity = new Vector2(0, _speed);
    }
    internal void Die()
    {
        Destroy(gameObject);
    }
    internal bool getRightAnswer()
    {
        return rightAnswer;
    }
}
