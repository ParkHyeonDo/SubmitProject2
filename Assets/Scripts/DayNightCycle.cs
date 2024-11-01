using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time; 
    public float fullDayLength; // 하루의 길이
    public float startTime = 0.4f; // 0.5일때 정오 말 그대로 게임 시작시 시간
    private float timeRate;
    public Vector3 noon; //Vector 90 0 0

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionIntensityMultiplier;


    // Start is called before the first frame update
    void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f; // %를 써야 time이 0f ~ 1.0f 을 계속 돔

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time); //풍경광. 낮에는 더 밝게할수도 더 어둡게할수도
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time); // 물체에서 반사되는 빛의 양
    }

    void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve) 
    {
        float intensity = intensityCurve.Evaluate(time); //Evaluate : 보간. 0f ~ 1f 의 사이의 값을 출력

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f ; // ex) ( 0.5 - 0.25 ) * 90 * 4 = 90도
        lightSource.color = gradient.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
        {
            go.SetActive(false);
        } else if (lightSource.intensity > 0 && !go.activeInHierarchy) 
        {
            go.SetActive(true);
        }
    }

}
