using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public static BossHealthBar Instance;
    public Slider slider;
    public IHealthBar currentBoss;
    public CanvasGroup canvas;
    
    private void Start()
    {
        Instance = this;
    }
    private void Update()
    {
        if(currentBoss != null)
        {
            canvas.alpha = 1.0f;
            slider.value = currentBoss.GetHealthPer();
        }
        else
        {
            canvas.alpha = 0;
        }          
    }
    public void SetCurrent(IHealthBar b)
    {
        currentBoss = b;
    }
}
