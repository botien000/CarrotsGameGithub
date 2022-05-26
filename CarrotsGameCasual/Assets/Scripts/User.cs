using UnityEngine;
using UnityEngine.UI;

public class User : MonoBehaviour
{
    enum State
    {
        Move, Jump
    }
    [SerializeField] private Transform[] transforms;
    [SerializeField] private Transform posFlyScore;
    [SerializeField] private int heart;
    [SerializeField] private float speedJump;
    [SerializeField] private Text teext;

    private State curState;
    private Animator animator;
    private GameManager instanceGM;
    private Rigidbody2D rgbody;
    private Icon icon;
    private int curIndexPos;
    private int curHeart;
    private Vector3 origin;
    private bool touch, shield = false;
    private void Awake()
    {
        rgbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        instanceGM = GameManager.instance;
        curHeart = heart;
        curIndexPos = Mathf.RoundToInt(Random.Range(0f, (transforms.Length - 1) * 1f));
        icon = GetComponentInChildren<Icon>();
        icon.gameObject.SetActive(false);
        transform.position =(Vector2) transforms[curIndexPos].position;
    }
    // Update is called once per frame
    void Update()
    {
        GetTouchMove();
        transform.position = Vector2.MoveTowards(transform.position,transforms[curIndexPos].position, speedJump * Time.deltaTime);
        if (transform.position.x == transforms[curIndexPos].position.x)
        {
            SetAnimation(State.Move);
        }
        else
        {
            SetAnimation(State.Jump);
        }
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
            Vector3 dirVector = swipe - origin;
            float angleSwipe = Mathf.Atan2(dirVector.y, dirVector.x) * Mathf.Rad2Deg;
            if (dirSwipe.x < 0f && touch && (angleSwipe <= 45 && angleSwipe >= -45))
            {

                touch = false;
                GetPos(1);
            }
            else if (dirSwipe.x > 0f && touch && (angleSwipe <= -135 || angleSwipe >= 135))
            {
                touch = false;
                GetPos(-1);

            }
        }
        //trường hợp này khi chạm không vuốt touch return false
        if (Input.GetMouseButtonUp(0))
        {
            touch = false;
        }
    }
    private void GetPos(int index)
    {
        // điều kiện để nhảy sang ô tiếp theo chỉ được khi vị trí của player đã vào đúng chỗ
        if (transform.position.x != transforms[curIndexPos].position.x)
        {
            teext.text = "ok" + "   " + transform.position + "      " + transforms[curIndexPos].position;
            return;
        }

        curIndexPos += index;
        if (curIndexPos == transforms.Length || curIndexPos < 0)
        {
            curIndexPos -= index;
            return;
        }
        SetDirection(index);
    }
    private void SetDirection(int dir)
    {
        //left
        if (dir < 0)
        {
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }
        //right
        else if (dir > 0)
        {
            transform.rotation = Quaternion.AngleAxis(0, Vector3.zero);
        }
    }
    private void GetIcon(bool type)
    {
        icon.gameObject.SetActive(true);
        icon.Init(type);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            Item item = collision.GetComponent<Item>();
            item.DieByPlayer();
        }
        else if (collision.gameObject.tag == "Answer")
        {
            Answer answer = collision.GetComponent<Answer>();
            if (answer.Interact)
            {
                //nếu bằng true thì mới cho phép tương tác xử lý
                instanceGM.RemoveAnswers(answer);
                Result(answer.GetAnswer());
            }
        }
    }
    /// <summary>
    /// Kiểm tra đáp án người chơi vừa chọn là sai hay đúng
    /// </summary>
    /// <param name="rightAnswer"></param>
    private void Result(bool rightAnswer)
    {
        if (rightAnswer == true)
        {
            instanceGM.SetScore_CarrotPointGamePlay();
            instanceGM.ShowFlyScore(posFlyScore);
        }
        else
        {
            if (shield == false)
            {
                // Không có khiên chắn thì sẽ mất mạng
                curHeart--;
                instanceGM.HitPlayer(curHeart);
            }
            else
            {
                // Có khiên chắn sẽ không làm mất mạng
            }
        }
        GetIcon(rightAnswer);
    }

    // Truyền khiên bảo vệ từ item
    public void Shield(bool haveShield)
    {
        shield = haveShield;
    }
    /// <summary>
    /// Khi tương tác với item sau đó mất khiên
    /// </summary>
    private void BreakShield()
    {
        shield = false;
        //thay đổi ảnh có khiên nhưng mà chưa có
    }
    private void SetAnimation(State state)
    {
        if (curState == state)
            return;

        animator.SetInteger("State", (int)state);
        animator.SetTrigger("Change");
        curState = state;
    }

}
