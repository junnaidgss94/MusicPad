using UnityEngine;
using System.Collections;

public class EventsController : MonoBehaviour
{
    public Transform cube1Transform;
    public Transform cube2Transform;

    public RhythmEventProvider eventProvider;

    /// <summary>
    /// RhythmTool. 
    /// </summary>
    public RhythmTool rhythmTool;

    /// <summary>
    /// AudioClip of a song.
    /// </summary>
    public AudioClip audioClip;

    // Use this for initialization
    void Start()
    {
        rhythmTool = GetComponent<RhythmTool>();
        eventProvider = GetComponent<RhythmEventProvider>();

        eventProvider.onNewSong.AddListener(OnNewSong);
        eventProvider.onBeat.AddListener(OnBeat);
        eventProvider.onSubBeat.AddListener(OnSubBeat);

        rhythmTool.NewSong(audioClip);
    }

    void OnNewSong(string name, int totalFrames)
    {
        rhythmTool.Play();
    }
        
    void OnBeat(Beat beat)
    {
        //give cube 1 a random scale every beat
        cube1Transform.localScale = Random.insideUnitSphere;
    }

    void OnSubBeat(Beat beat, int count)
    {
        //give cube 2 a random scale every whole and half beat
        if(count == 0 || count == 2)
            cube2Transform.localScale = Random.insideUnitSphere;
    }

}
