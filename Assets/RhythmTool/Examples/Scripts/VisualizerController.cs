using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

//In this "game", the controller uses data from the analyses is used to create some simple visuals.
public class VisualizerController : MonoBehaviour
{
    /// <summary>
    /// The RhythmEventProvider we're going to use to instantiate lines
    /// </summary>
    public RhythmEventProvider eventProvider;

    /// <summary>
    /// The prefab for the line we're going to instantiate
    /// </summary>
    public GameObject linePrefab;

    /// <summary>
    /// The list of all lines that are currently in the scene
    /// </summary>
    private List<Line> lines;
	
	public RhythmTool rhythmTool;	
		
    /// <summary>
    /// List of songs that can be assigned in the inspector
    /// </summary>
	public List<AudioClip> audioClips;

    private int currentSong;

    /// <summary>
    /// the array of smoothed magnitude we're going to use to determine the speed of the lines
    /// </summary>
    private ReadOnlyCollection<float> magnitudeSmooth;

	void Start ()
	{		
		currentSong = -1;
		Application.runInBackground = true;
        
        lines = new List<Line>();

        //subscribe to events
        eventProvider.onBeat.AddListener(OnBeat);
        eventProvider.onChange.AddListener(OnChange);

        //get smoothed magnitude
        magnitudeSmooth = rhythmTool.low.magnitudeSmooth;

        if (audioClips.Count <= 0)
			Debug.LogWarning ("no songs configured");
		else {
            //Go to the first song		
			NextSong();
		}
	}	
	
    //A new song has been loaded and can be played
	void OnReadyToPlay()
	{          
		//Start playing the song.
		rhythmTool.Play ();					
	}
	
    // OnEndOfSong is called by RhythmTool if the song has ended
	void OnEndOfSong()
	{		
		NextSong();	
	}
	
	//go to next song on list
	private void NextSong ()
	{
        //destroy all the lines in the scene
		ClearLines ();
		
		currentSong++;
		
		if (currentSong >= audioClips.Count)
			currentSong = 0;
			
        //Give the song to RhythmTool
		rhythmTool.NewSong (audioClips [currentSong]);	
	}
	
	void Update ()
	{		
		//Hit space to load another song.
		if (Input.GetKeyDown (KeyCode.Space)) {
			
			NextSong ();
			return;
		}		

		//Hit esc to quit.
		if (Input.GetKey (KeyCode.Escape)) {
			UnityEngine.Application.Quit ();
		}	

		//If no song loaded, don't do anything else.
		if (!rhythmTool.songLoaded) {						
			return;
		}

        //move and destroy lines
        UpdateLines();
        		
		rhythmTool.DrawDebugLines ();
	}

    private void UpdateLines()
    {
        //remove all lines that have occured already
        List<Line> toRemove = new List<Line>();
        foreach(Line l in lines)
        {
            //if the line's index is smaller than the current index it's moment in the song has passed, so destroy it
            if (l.index < rhythmTool.currentFrame || l.index > rhythmTool.currentFrame + eventProvider.offset)
            {
                Destroy(l.gameObject);
                toRemove.Add(l);
                continue;
            }
        }
        foreach (Line l in toRemove)
            lines.Remove(l);

        //calculate all cumulative magnitudeSmooth only once, so each line's position can be set efficiently
        float[] cumulativeMagnitudeSmooth = new float[eventProvider.offset + 1];
        float total = 0;
        for (int i = 0; i < cumulativeMagnitudeSmooth.Length; i++)
        {
            int index = Mathf.Min(rhythmTool.currentFrame + i, rhythmTool.totalFrames-1);

            total += magnitudeSmooth[index];
            cumulativeMagnitudeSmooth[i] = total;
        }

        //set each line's position based on magnitudeSmooth. 
        foreach (Line l in lines)
        {
            Vector3 pos = l.transform.position;
            pos.x = cumulativeMagnitudeSmooth[l.index - rhythmTool.currentFrame] * .2f;
            pos.x -= magnitudeSmooth[rhythmTool.currentFrame] * .2f * rhythmTool.interpolation;
            l.transform.position = pos;
        }        
    }

    //OnBeat has been added to the RhythmEventProvider in Start(). Alternatively it can be added through the inspector in the editor.
    //The RhythmEventProvider has been given an offset of 80, which means it's events are called 80 frames in advance, so the lines can be instantiated ahead of time.
    public void OnBeat(Beat beat)
    {
        //Instantiate a white line and give it the index at which the beat occurs
        lines.Add(CreateLine(beat.index, Color.white, 10, -40));       
    }

    public void OnChange(int index, float change)
    {
        if (change > 0)
            lines.Add(CreateLine(index, Color.yellow, 20, -60));
    }

    //OnOnset has been added to the RhythmEventProvider in the inspector.
    //Alternatively, methods can be added by using RhythmEventProvider.onOnset.AddListener(OnOnset)
    public void OnOnset(OnsetType type, Onset onset)
    {
        //ignore onsets that are too close to bigger onsets, and onsets that are too small
        if (onset.rank < 4 && onset < 5)
            return;

        //Instantiate a line depending on the type of onset. The line will be given the index at which the onset occurs, a color and a scale.
        switch (type)
        {
            case OnsetType.Low:
                lines.Add(CreateLine(onset.index, Color.blue, onset, -20));
                break;
            case OnsetType.Mid:
                lines.Add(CreateLine(onset.index, Color.green, onset, 0));
                break;
            case OnsetType.High:
                lines.Add(CreateLine(onset.index, Color.yellow, onset, 20));
                break;
            case OnsetType.All:
                lines.Add(CreateLine(onset.index, Color.magenta, onset, 40));
                break;
        }
    }
          
    public Line CreateLine(int index, Color color, float opacity, float yPosition)
    {
        GameObject lineObject = Instantiate(linePrefab) as GameObject;
        lineObject.transform.position = new Vector3(0, yPosition, 0);

        Line line = lineObject.GetComponent<Line>();
        line.Init(color, opacity, index);

        return line;
    }

    /// <summary>
    /// Destroy all lines
    /// </summary>
    public void ClearLines()
    {
        foreach (Line l in lines)
            Destroy(l.gameObject);

        lines.Clear();
    }
}
