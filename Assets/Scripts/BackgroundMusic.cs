using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
	public enum Track
	{
		IntroTrack = 0,
		GhostTrack = 1,
		GhostScaredTrack = 2,
		GhostDeadTrack = 3
	}

	public AudioSource audioSource;
	public AudioClip[] trackClips;
	private Track currentTrack;

	void PlayTrack(Track track)
	{
		audioSource.clip = trackClips[(int) track];
		audioSource.Play();
	}

	void Start()
	{
		PlayTrack(Track.IntroTrack);
	}

	void Update()
	{
		if (currentTrack == Track.IntroTrack && !audioSource.isPlaying)
			PlayTrack(Track.GhostTrack);
	}
}
