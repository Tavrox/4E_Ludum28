using UnityEngine;
using System.Collections;

public class FESound : MonoBehaviour {

//	public MasterAudioGroup SoundGroup;
//	[Range (0,10f)] public float Delay;
//	[Range (0,1f)] public float Volume;
//	[Range (0,1f)] public float Pitch;
//	public enum SoundType {Oneshot, Continuous};
//	public SoundType type;
//	public float RepeatRate = 0.6f;
//
//	public void playSound()
//	{
//		MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay);
//	}
//	public void playModulatedSound(float _var1, float _var2)
//	{
//		float percent = (_var1 / _var2);	
//		Volume = percent;
//		MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay);
//	}
	public static float[] distCoeffs = new float[5] {1f,0.8f,0.6f,0.4f,0.2f};
	private static float volume, groupeVolume,res;
	private static Vector2 pos1,pos2;
//	public static MasterAudioGroup test;
//	public static void setValues () {
//		distCoeffs[1]=11f;distCoeffs[3]=11f;distCoeffs[2]=11f;distCoeffs[4]=11f;
//	}
	public static void playDistancedSound(string SoundGroup, Transform _obj1, Transform _obj2, float volumeMin, string action="play", float delay=0f)
	{
//		test = GameObject.Find("blob_run1").GetComponent<MasterAudioGroup>();
//		test.groupMasterVolume = 0f;
		//test.groupMasterVolume = 0;
		groupeVolume = MasterAudio.GetGroupVolume(SoundGroup);
		pos1 = new Vector2(_obj1.position.x, _obj1.position.y);
		pos2 = new Vector2(_obj2.position.x, _obj2.position.y);
		res = Vector2.Distance(pos1, pos2);
		if(res<=4f) volume = distCoeffs[0]*groupeVolume;
		else if (res > 4f && res <= 6f) volume = distCoeffs[1]*groupeVolume;
		else if (res > 6f && res <= 8f) volume = distCoeffs[2]*groupeVolume;
		else if (res > 8f && res <= 12f) volume = distCoeffs[3]*groupeVolume;
		else if (res > 12f && res <= 16f) volume = distCoeffs[4]*groupeVolume;
		else volume = volumeMin*groupeVolume;
		//Debug.Log ("Distance Sound" + res + " - " +volume);
		if(action=="fade") {
			MasterAudio.FadeOutAllOfSound(SoundGroup, delay);
		}
		if(action=="stop") {
			MasterAudio.StopAllOfSound(SoundGroup);
		}
		else MasterAudio.PlaySound(SoundGroup, volume,1f,delay);
	}
}