using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    private ButtonItemManager instanceBtnItem;
    private CategoryItemSctbObj curCategory;
    private Rigidbody2D rgbody;
    private Image image;
    private GameManager instanceGM;
    private SpawnManager instanceSM;
    private Vector2 pos;
    public float speed;
    private void Awake()
    {
        rgbody = GetComponent<Rigidbody2D>();
        image = GetComponent<Image>();
    }
    private void OnEnable()
    {
        pos = transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        instanceGM = GameManager.instance;
        instanceSM = SpawnManager.instance;
        instanceBtnItem = ButtonItemManager.instance;
    }
    // Update is called once per frame
    void Update()
    {
        pos += Vector2.down * speed * Time.deltaTime;
        rgbody.MovePosition(pos);
        if (transform.position.y <= -6)
        {
            Die();
        }

    }
    public void Init(CategoryItemSctbObj category)
    {
        curCategory = category;
        image.sprite = curCategory.sprite;
    }
    private void Move(float _speed)
    {

    }
    public void DieByPlayer()
    {
        instanceGM.NextTurn();
        ButtonItem buttonItem = instanceBtnItem.SpawnButtonItem();
        buttonItem.Init(curCategory);
        instanceSM.itemPool.RemoveObjInPool(gameObject);
    }
    public void Die()
    {
        instanceGM.NextTurn();
        instanceSM.itemPool.RemoveObjInPool(gameObject);
    }
   
}
