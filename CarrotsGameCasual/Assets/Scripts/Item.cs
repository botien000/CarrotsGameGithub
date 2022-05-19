using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    public int category;
    User user;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        user = FindObjectOfType<User>();
        rigidbody2D.velocity = new Vector2(0, -2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    internal void Init(int _category)
    {
        
    }
    internal void Move(float _speed)
    {
        rigidbody2D.velocity = new Vector2(0, _speed);
    }
    internal void Die()
    {
        Destroy(gameObject);
    }
    /// <summary>
    /// 1. 50/50   2. x2   3. đổi câu hỏi  4. khiên
    /// </summary>
    /// <returns></returns>
    internal int getCategory()
    {
        return category;
    }

    /// <summary>
    /// Giảm 1 nửa đáp án
    /// </summary>
    public void HalfAnswer()
    {
        
    }

    /// <summary>
    /// Khiên bảo vệ
    /// </summary>
    public void Shield()
    {
        user.Shield(true);
    }
    /// <summary>
    /// đổi đáp án
    /// </summary>
    public void transAnswer()
    {

    }
    /// <summary>
    /// nhân đôi điểm
    /// </summary>
    public void DoublePoint()
    {

    }
}
