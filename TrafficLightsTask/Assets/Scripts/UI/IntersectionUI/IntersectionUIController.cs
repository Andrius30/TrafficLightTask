using Andrius.Core.Timers;
using System;
using TMPro;
using UnityEngine;

public class IntersectionUIController : MonoBehaviour
{
    [SerializeField] IntersectionController intersectionController;
    [SerializeField] TextMeshProUGUI infoText;
    [SerializeField] TextMeshProUGUI currentLightText;
    [SerializeField] float infoTextDurationTime = 1f;

    Timer infoTextDurationTimer;

    private void Awake()
    {
        infoTextDurationTimer = new Timer()
            .BuildWithOnDone(OnDone)
            .Build();
    }
    private void Update()
    {
        if (infoTextDurationTimer == null) return;
        infoTextDurationTimer.StartTimer();
    }

    public void Drive()
    {
        infoTextDurationTimer.SetTimer(infoTextDurationTime, false);
        if (intersectionController == null)
        {
            Debug.LogError($"intersectionn controller not assigned!");
            return;
        }
        LightStates lightStates = intersectionController.GroupALightState;
        GetResponse(lightStates);
    }
    public void PrintLighState(LightStates lightStates) => currentLightText.text = $"{lightStates}";

    void GetResponse(LightStates lightStates)
    {
        switch (lightStates)
        {
            case LightStates.Red:
                PrintText("Can't Drive", Color.red);
                break;
            case LightStates.RedAmber:
                PrintText("Can't Drive", Color.red);
                break;
            case LightStates.Amber:
                PrintText("Traffic may not pass the stop line or enter the intersection unless it cannot safely stop when the light shows!", Color.yellow, 14);
                break;
            case LightStates.Green:
                PrintText("Can Drive", Color.green);
                break;
        }
    }
    void PrintText(string text, Color color, int fontSize = 36)
    {
        infoText.color = color;
        infoText.fontSize = fontSize;
        infoText.text = text;
    }
    void ResetText() => infoText.text = "";
    void OnDone() => ResetText();

    void OnEnable()
    {
        intersectionController.OnStateChanged += PrintLighState;
    }
    void OnDisable()
    {
        intersectionController.OnStateChanged -= PrintLighState;
    }
}
