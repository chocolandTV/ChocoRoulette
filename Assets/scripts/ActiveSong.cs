// ActiveSong
using UnityEngine;

public class ActiveSong : MonoBehaviour
{
	public string CurrentSong;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
		CurrentSong = other.gameObject.name;
		Debug.Log(other.gameObject.name);
	}
}
