using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{

	public static BackgroundMusic _instance; 
	
	private AudioSource _audioSource;
	private void Awake()
	{
		if (_instance == null) _instance = this;
		else Destroy(this.gameObject);
		
		DontDestroyOnLoad(transform.gameObject);
		_audioSource = GetComponent<AudioSource>();
	}
 
	public void PlayMusic()
	{
		if (_audioSource.isPlaying) return;
		_audioSource.Play();
	}
 
	public void StopMusic()
	{
		_audioSource.Stop();
	}
}
