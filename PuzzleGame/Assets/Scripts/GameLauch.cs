using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLauch : MonoBehaviour
{
    [SerializeField]
    Button btnEnter;
    [SerializeField]
    Button reset;

    void Awake()
    {
        LevelMgr.GetInstance().InitConfig();
        CacheMgr.GetInstance().InitCache();
    }

    void Start()
    {
        reset.onClick.AddListener(OnGameReset);
        btnEnter.onClick.AddListener(OnEnterClick);
    }

    private void OnEnterClick()
    {
        SceneManager.LoadSceneAsync(1);
    }

    private void OnGameReset()
    {
        CacheMgr.GetInstance().Reset();
    }

}
