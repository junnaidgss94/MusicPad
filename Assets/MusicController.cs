using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public AudioClip[] SoundEffects;
    public AudioClip[] SampleLoading;
    public AudioClip[] PlayBacks;

    public AudioSource SoundEffectsAS;
    public AudioSource SampleLoadingAS;
    public AudioSource PlayBacksAS;

    public Transform SoundEffectsTransform;
    public Transform SampleLoadingTransform;
    public Transform PlayBacksTransform;

    public GameObject PadButton;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < SoundEffects.Length; i++)
        {
            int index = i;  // Create a local variable to capture the current value of i
            GameObject Go = Instantiate(PadButton, SoundEffectsTransform);
            Go.GetComponent<Button>().onClick.AddListener(() => _PlaySoundEffect(SoundEffects[index]));
            Go.GetComponent<Image>().color = Color.magenta;
        }

        for (int i = 0; i < SampleLoading.Length; i++)
        {
            int index = i;
            GameObject Go = Instantiate(PadButton, SampleLoadingTransform);
            Go.GetComponent<Button>().onClick.AddListener(() => _PlaySampleLoading(SampleLoading[index]));
            Go.GetComponent<Image>().color = Color.green;
        }

        for (int i = 0; i < PlayBacks.Length; i++)
        {
            int index = i;
            GameObject Go = Instantiate(PadButton, PlayBacksTransform);
            Go.GetComponent<Button>().onClick.AddListener(() => _PlayPlayBacks(PlayBacks[index]));
            Go.GetComponent<Image>().color = Color.cyan;
        }
    }


    public void _PlaySoundEffect(AudioClip audioClip)
    {
        SoundEffectsAS.PlayOneShot(audioClip);
    }

    public void _PlaySampleLoading(AudioClip audioClip)
    {
        SoundEffectsAS.PlayOneShot(audioClip);
    }

    public void _PlayPlayBacks(AudioClip audioClip)
    {
        PlayBacksAS.Stop();
        PlayBacksAS.clip = audioClip;
        PlayBacksAS.Play();
    }
}
