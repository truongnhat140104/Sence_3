using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reload : MonoBehaviour
{
    public Image reloadBar;

    public float duration = 1f; // Thời gian để fill từ 0 đến 1

    public void updateBar(float seconds)
    {
        StartCoroutine(fillAmount(seconds));
    }

    IEnumerator fillAmount(float seconds)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < seconds)
        {
            reloadBar.fillAmount = Mathf.Lerp(0.0f, 1.0f, elapsedTime / seconds);
            elapsedTime += 0.01f; // Tăng một lượng nhỏ thời gian mỗi khung hình
            yield return new WaitForSeconds(0.01f); // Chờ 0.01 giây trước khi thực hiện vòng lặp tiếp theo
        }

        reloadBar.fillAmount = 1.0f; // Đảm bảo fillAmount đạt giá trị 1 khi hoàn thành
    }
}
