using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
    private void Awake()
    {
        if (instance != null)
            instance = this;
    }
    #endregion

    public bool[] isSelected = new bool[6];

    private void Update()
    {
        
    }

}
