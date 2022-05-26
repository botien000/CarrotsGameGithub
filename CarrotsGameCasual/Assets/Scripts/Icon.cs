using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icon : MonoBehaviour
{
    [SerializeField] private Sprite iconRight, iconWrong;
    [SerializeField] private float timeInVisible;

    private float curTime;
    private Image imgIcon;
    private void OnEnable()
    {
        ResetTime();
    }
    // Start is called before the first frame update
    void Start()
    {
        imgIcon = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        curTime -= Time.deltaTime;
        if(curTime <= 0)
        {
            ResetTime();
            gameObject.SetActive(false);
        }
    }
    public void Init(bool right)
    {
        ResetTime();
        if (right)
        {
            imgIcon.sprite = iconRight;
        }
        else
        {
            imgIcon.sprite = iconWrong;
        }
    }
    private void ResetTime()
    {
        curTime = timeInVisible;
    }
}
