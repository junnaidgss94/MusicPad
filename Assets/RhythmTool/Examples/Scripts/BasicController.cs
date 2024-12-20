using UnityEngine;
using System.Collections;

//This is a basic script that uses RhythmTool. It gives a song to the analyzer and starts playing and analyzing it.
//See DataController and the example scene for an example on how to get the data in a game.
public class BasicController : MonoBehaviour
{
    /// <summary>
    /// Reference to RhythmTool. It can be assigned in the inspector or with GetComponent
    /// </summary>
    public RhythmTool rhythmTool;
		
	/// <summary>
	/// AudioClip of a song.
	/// </summary>
	public AudioClip audioClip;

	// Use this for initialization
	void Start ()
	{
        //Get the RhythmTool Component.
        rhythmTool = GetComponent<RhythmTool>();

        //Give it a song.
        rhythmTool.NewSong(audioClip);
	}

    //OnReadyToPlay is called by RhythmTool after NewSong(), when RhythmTool is ready to start playing the song.
    //When RhythmTool is ready depends on lead and on whether preCalculate is enabled.
    void OnReadyToPlay()
	{
		//Start playing the song
		rhythmTool.Play ();	
	}


	// Update is called once per frame
	void Update ()
	{		
		//Draw graphs representing the data.
		rhythmTool.DrawDebugLines ();		
	}
}
