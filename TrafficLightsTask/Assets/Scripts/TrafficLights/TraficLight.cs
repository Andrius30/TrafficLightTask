using System;
using UnityEngine;

/// I made this class very simple, but it can be done with state machine also, so code would be a bit cleaner
public class TraficLight : MonoBehaviour, IComandLight
{
    [SerializeField] GameObject redLightObj;
    [SerializeField] GameObject amberLightObj;
    [SerializeField] GameObject greenLightObj;

    Renderer redRenderer;
    Renderer yellowRenderer;
    Renderer greenRenderer;

    public void Init() => CreateMaterialInstances();
    public void Execute(LightStates lightStates)
    {
        switch (lightStates)
        {
            case LightStates.Red:
                TurnOnRedLight();
                break;
            case LightStates.RedAmber:
                TurnOnRedAmberLight();
                break;
            case LightStates.Amber:
                TurnOnAmberLight();
                break;
            case LightStates.Green:
                TurnOnGreenLight();
                break;
        }

    }
    void CreateMaterialInstances()
    {
        redRenderer = redLightObj.GetComponent<Renderer>();
        yellowRenderer = amberLightObj.GetComponent<Renderer>();
        greenRenderer = greenLightObj.GetComponent<Renderer>();
        Material redMat = new Material(redRenderer.material);
        redRenderer.materials[0] = redMat;
        Material yelloMat = new Material(yellowRenderer.material);
        yellowRenderer.materials[0] = yelloMat;
        Material greenMat = new Material(greenRenderer.material);
        greenRenderer.materials[0] = greenMat;
    }
    void TurnOnRedLight()
    {
        DisableAllLights();
        EnableRenderer(redRenderer);
    }
    void TurnOnRedAmberLight()
    {
        DisableAllLights();
        EnableRenderer(redRenderer);
        EnableRenderer(yellowRenderer);

    }
    void TurnOnAmberLight()
    {
        DisableAllLights();
        EnableRenderer(yellowRenderer);
    }
    void TurnOnGreenLight()
    {
        DisableAllLights();
        EnableRenderer(greenRenderer);

    }
    void DisableAllLights()
    {
        DisableRenderer(redRenderer);
        DisableRenderer(yellowRenderer);
        DisableRenderer(greenRenderer);
    }
    void EnableRenderer(Renderer renderer) => renderer.materials[0].EnableKeyword("_EMISSION");
    void DisableRenderer(Renderer renderer) => renderer.materials[0].DisableKeyword("_EMISSION");

}
