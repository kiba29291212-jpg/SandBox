using UnityEngine;

public class DayNight : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private float dayDuration = 600f;
    [SerializeField] private float nightDuration = 300f;

    [Header("Lighting Settings")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private Gradient dayNightColorGradient;
    private float timeOfDay;
    private bool isDaytime = true;
    public int DayCount { get; private set; } = 1;
    public float TimeOfDay => timeOfDay;
    public bool IsDaytime => isDaytime;
    public bool IsEntityNight { get; private set; }

    private void Update()
    {
        UpdateTimeOfDay();
        UpdateLighting();
    }

    private void UpdateTimeOfDay()
    {
        float duration = isDaytime ? dayDuration : nightDuration;

        if (duration <= 0f)
            return;

        timeOfDay += Time.deltaTime / duration;

        if (timeOfDay >= 1f)
        {
            timeOfDay = 0f;

            if (isDaytime)
            {
                isDaytime = false;
                IsEntityNight = DayCount % 2 == 0;
            }
            else
            {
                isDaytime = true;
                DayCount++;
                IsEntityNight = false;
            }
        }

        TimeCheck.DayCheck = isDaytime;
    }

    private void UpdateLighting()
    {
        if (directionalLight == null)
            return;

        if (dayNightColorGradient != null)
        {
            directionalLight.color = dayNightColorGradient.Evaluate(timeOfDay);
        }

        directionalLight.transform.rotation = Quaternion.Euler((timeOfDay * 360f) - 90f, 170f, 0f);
    }
}
