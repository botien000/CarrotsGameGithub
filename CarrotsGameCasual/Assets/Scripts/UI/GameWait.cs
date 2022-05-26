using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameWait : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtReady;
    private void OnEnable()
    {
        txtReady.text = "Ready...";
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            GameManager.instance.SetState(GameManager.StateGame.GamePlay);
        }
    }
}
