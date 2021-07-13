using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BarType {
    healthBar,
    manaBar
}

public class IndicatorBar : MonoBehaviour
{

    private Slider slider;
    public BarType Type;
    private PlayerController _playerController;

    void Start()
    {
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        slider = GetComponent<Slider>();
        switch (Type)
        {
            case BarType.healthBar:
                slider.maxValue = PlayerController.MAX_HEALTH;
                break;
            case BarType.manaBar:
                slider.maxValue = PlayerController.MAX_MANA;
                break;
        }
    }


    void Update()    {
        switch (Type)
        {
            case BarType.healthBar:
                slider.value = _playerController.GetHealth();
                break;
            case BarType.manaBar:
                slider.value = _playerController.GetMana();
                break;
        }
    }
}
