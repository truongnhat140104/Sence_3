using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public Image hpBar;

    public void updateHPBar(float recentHP,float maxHP)
    {
        hpBar.fillAmount = recentHP / maxHP;
    }
}
