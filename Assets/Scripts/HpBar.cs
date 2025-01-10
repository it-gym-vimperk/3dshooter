using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Image progressImage;

    public void Set(float progress)
    {
        progressImage.fillAmount = progress;
    }
}