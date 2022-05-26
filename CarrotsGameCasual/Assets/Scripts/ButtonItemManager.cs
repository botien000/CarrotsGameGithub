using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonItemManager : MonoBehaviour
{
    [SerializeField] private List<ButtonItem> activeBtnItems;
    [SerializeField] private List<ButtonItem> inactiveBtnItems;

    /// <summary>
    /// Singleton 
    /// </summary>
    public static ButtonItemManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //Khi vào game tất cả button invisible
        foreach (var item in inactiveBtnItems)
        {
            item.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public ButtonItem SpawnButtonItem()
    {
        ButtonItem buttonItem;
        //Lấy button đang inVisible
        if (inactiveBtnItems.Count > 0)
        {
            //nếu có tồn tại gameObject thì lấy ra một gameobj
            buttonItem = inactiveBtnItems[0];
            activeBtnItems.Add(buttonItem);
            inactiveBtnItems.Remove(buttonItem);
            buttonItem.gameObject.SetActive(true);
        }
        //trường hợp không còn button nào inVisible thì bỏ button đầu sắp xếp lại để button mới nằm cuối
        else
        {
            SortOfButtonItem(0);
            buttonItem = activeBtnItems[activeBtnItems.Count - 1];
        }
        return buttonItem;
    }
    public void RemoveObjInPool(ButtonItem buttonItem)
    {
        if (activeBtnItems.Contains(buttonItem))
        {
            int index = activeBtnItems.IndexOf(buttonItem);
            SortOfButtonItem(index);
            //remove button cuoi 
            buttonItem = activeBtnItems[activeBtnItems.Count - 1];
            inactiveBtnItems.Add(buttonItem);
            activeBtnItems.Remove(buttonItem);
            buttonItem.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Not found " + gameObject.name + "Bug logic.Fix Bug!!!");
        }
    }
    public void ClearObjInPool()
    {
        activeBtnItems.Clear();
        inactiveBtnItems.Clear();
    }
    private void SortOfButtonItem(int index)
    {
        while(index < activeBtnItems.Count)
        {
            //if(index == active.count - 1) => remove =>inactive.add
            //a[index].sprite = a[index - 1]
            if(index == activeBtnItems.Count - 1)
            {
                break;
            }
            activeBtnItems[index].Init(activeBtnItems[index + 1].GetCategory());
            index++;
        }
    }
}
