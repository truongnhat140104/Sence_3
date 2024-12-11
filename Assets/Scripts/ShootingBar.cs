using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public Image shootingBar;

    public void updateBar(float fill)
    {
        shootingBar.fillAmount = fill;
    }
}
