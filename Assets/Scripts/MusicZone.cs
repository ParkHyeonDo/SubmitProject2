using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicZone : MonoBehaviour
{
    public AudioSource audioSource;
    public float fadeTime;
    public float maxVolume;
    private float targetVolume;

    // Start is called before the first frame update
    void Start()
    {
        targetVolume = 0;
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = targetVolume;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Mathf.Approximately(audioSource.volume, targetVolume)) // 근삿값 범위 안에 있으면 같은값으로 인식해줌
        {
            audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume,(maxVolume / fadeTime) * Time.deltaTime); // 소리를 점점 크게
        }  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            targetVolume = maxVolume;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        targetVolume = 0;
    }
}
