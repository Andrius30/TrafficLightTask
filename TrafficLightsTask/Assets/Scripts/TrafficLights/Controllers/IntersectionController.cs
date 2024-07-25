using Andrius.Core.Timers;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public enum LightStates
{
    Red,
    RedAmber,
    Amber,
    Green
}

public class IntersectionController : MonoBehaviour
{
    [SerializeField] SignalSettings signalSettings;

    [ShowInInspector] List<IComandLight> lightsGroupA; // just for testing purposes show in inspector
    [ShowInInspector] List<IComandLight> lightsGroupB;

    LightStates groupA;
    LightStates groupB;
    public LightStates GroupALightState => groupA;
    public LightStates GroupBLightState => groupB;

    Timer switchStateTimer;

    int lightIndex;

    const int TOTAL_LIGHTS = 4;

    bool increasing;

    void Awake()
    {
        switchStateTimer = new Timer()
            .BuildWithStartTime(signalSettings.signalSwitchTime)
            .BuildWithOnDone(OnDone)
            .Build();
    }
    void Start()
    {
        InitializeLists();
        foreach (var light in lightsGroupA)
        {
            light.Init();
            light.Execute(LightStates.Red);
        }
        foreach (var light in lightsGroupB)
        {
            light.Init();
            light.Execute(LightStates.Green);
        }
    }


    void Update()
    {
        if (switchStateTimer == null) return;
        switchStateTimer.StartTimer();
    }

    void InitializeLists()
    {
        lightsGroupA = new List<IComandLight>();
        lightsGroupB = new List<IComandLight>();
        Transform groupA = transform.Find("LightGroupA");
        Transform groupB = transform.Find("LightGroupB");
        for (int i = 0; i < groupA.childCount; i++)
        {
            lightsGroupA.Add(groupA.GetChild(i).GetComponent<IComandLight>());
        }
        for (int i = 0; i < groupB.childCount; i++)
        {
            lightsGroupB.Add(groupB.GetChild(i).GetComponent<IComandLight>());
        }
    }
    void OnDone()
    {
        //lightIndex = lightIndex % TOTAL_LIGHTS;
        LightStates currentStateA, currentStateB;
        AssignLightStates(out currentStateA, out currentStateB);
        lightsGroupA.ForEach(x => x.Execute(currentStateA));
        lightsGroupB.ForEach(x => x.Execute(currentStateB));
        ControllLightIndex();
        switchStateTimer.SetTimer(signalSettings.signalSwitchTime, false);
    }
    void ControllLightIndex()
    {

        if (increasing)
        {
            if (lightIndex < TOTAL_LIGHTS - 1) // while light index is less than total lights  -1
            {
                lightIndex++;
            }
            else
            {
                increasing = false; // else we setting increasing to false and start decreasing index here to not skip 1 step
                lightIndex--;
            }
        }
        else
        {
            if (lightIndex > 0)
            {
                lightIndex--;
            }
            else
            {
                increasing = true;
                lightIndex++;
            }
        }
    }
    void AssignLightStates(out LightStates currentStateA, out LightStates currentStateB)
    {
        currentStateA = GetState(lightIndex);
        currentStateB = GetInverseState(lightIndex);
        groupA = currentStateA;
        groupB = currentStateB;
    }
    LightStates GetState(int index)
    {
        switch (index)
        {
            case 0:
                return LightStates.Red;
            case 1:
                return LightStates.RedAmber;
            case 2:
                return LightStates.Amber;
            case 3:
                return LightStates.Green;
            default: return LightStates.Red;
        }

    }
    LightStates GetInverseState(int index)
    {
        switch (index)
        {
            case 0:
                return LightStates.Green;
            case 1:
                return LightStates.Amber;
            case 2:
                return LightStates.RedAmber;
            case 3:
                return LightStates.Red;
            default: return LightStates.Green;
        }

    }
}
