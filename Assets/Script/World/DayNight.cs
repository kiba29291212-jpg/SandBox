using UnityEngine;

public class DayNight : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private float dayDuration = 120f;
    [SerializeField] private float nightDuration = 60f;

    [Header("Lighting Settings")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private Gradient dayNightColorGradient;

    private float timeOfDay;
    private bool isDaytime;

    private void Update()
    {
        UpdateTimeOfDay();
        UpdateLighting();
    }

    private void UpdateTimeOfDay()
    {
        float duration = isDaytime ? dayDuration : nightDuration;
        timeOfDay += Time.deltaTime / duration;

        TimeCheck.DayCheck = isDaytime;

        if (timeOfDay >= 1f)
        {
            timeOfDay = 0f;
            isDaytime = !isDaytime;
            TimeCheck.DayCheck = isDaytime;
        }
    }

    private void UpdateLighting()
    {
        if (directionalLight != null)
        {
            directionalLight.color = dayNightColorGradient.Evaluate(timeOfDay);
            directionalLight.transform.rotation = Quaternion.Euler(new Vector3((timeOfDay * 360f) - 90f, 170f, 0));
        }
    }
}
