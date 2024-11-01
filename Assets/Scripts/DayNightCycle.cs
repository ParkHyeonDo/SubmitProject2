using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time; 
    public float fullDayLength; // �Ϸ��� ����
    public float startTime = 0.4f; // 0.5�϶� ���� �� �״�� ���� ���۽� �ð�
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
        time = (time + timeRate * Time.deltaTime) % 1.0f; // %�� ��� time�� 0f ~ 1.0f �� ��� ��

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time); //ǳ�汤. ������ �� ����Ҽ��� �� ��Ӱ��Ҽ���
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time); // ��ü���� �ݻ�Ǵ� ���� ��
    }

    void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve) 
    {
        float intensity = intensityCurve.Evaluate(time); //Evaluate : ����. 0f ~ 1f �� ������ ���� ���

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f ; // ex) ( 0.5 - 0.25 ) * 90 * 4 = 90��
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
