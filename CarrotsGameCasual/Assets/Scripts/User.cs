using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class User : MonoBehaviour
{
    [SerializeField] private Transform[] transforms;

    private Rigidbody2D rgbody;
    private int curIndexPos;
    private Vector3 origin;
    private bool touch;
    private void Awake()
    {
        rgbody = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        curIndexPos = Mathf.RoundToInt(Random.Range(0f, (transforms.Length - 1) * 1f));
    }

    // Update is called once per frame
    void Update()
    {

        GetTouchMove();
    }
    private void GetTouchMove()
    {
        //handle swipe main character
        //nhận chạm đầu tiên
        if (Input.GetMouseButtonDown(0))
        {
            origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touch = true;
        }
        //nhận chạm đầu tiên sau đó vuốt màn hình
        if (Input.GetMouseButton(0))
        {
            Vector3 swipe = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dirSwipe = origin - swipe;
            if (dirSwipe.x < 0f && touch)
            {
                touch = false;
                GetPos(1);
            }
            else if (dirSwipe.x > 0f && touch)
            {
                touch = false;
                GetPos(-1);
            }
        }
        //trường hợp này khi không chạm touch return false
        if (Input.GetMouseButtonUp(0))
        {
            touch = false;
        }
    }
    private void GetPos(int index)
    {
        curIndexPos += index;
        if (curIndexPos == transforms.Length || curIndexPos < 0)
        {
            curIndexPos -= index;
            return;
        }
        rgbody.MovePosition(new Vector2(transforms[curIndexPos].position.x, transform.position.y));

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.tag == "item")
        {
            Item item = collision.gameObject.GetComponent<Item>();
            InstantItem(item.getCategory());
            Destroy(collision.gameObject);
        }
       if(collision.gameObject.tag == "answer")
        {
            Answer answer = collision.gameObject.GetComponent<Answer>();
            Result(answer.getRightAnswer());
        }
    }

    /// <summary>
    /// trợ giúp
    /// </summary>
    /// <param name="_category"></param>
    private void InstantItem(int _category)
    {
        switch (_category)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }  
    }
    private void Result(bool _rightAnswer)
    {
        if(_rightAnswer == true)
        {

        }
        else
        {

        }
    }
}
