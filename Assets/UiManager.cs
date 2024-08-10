using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public GameObject[] hearts;
    public static UiManager Instance;

    void Awake()
    {
        Instance = this;
        
    }

    public void UpdateHearts(int amountOfLifes)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < amountOfLifes);
        }
    }
}
