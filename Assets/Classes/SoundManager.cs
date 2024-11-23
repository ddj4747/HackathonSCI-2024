using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    
}

public enum SoundEffect
{
    foootsteps,
    click,
    cofanie,
    door,
    skakanie
}

public enum Music
{
    Hackathon1,
    Hackathon2
}


public class SoundManager : MonoBehaviour
{
    private static float musicStart;

    [SerializeField]
    private AudioClip[] soundEffectList;

    [SerializeField]
    private AudioClip[] musicList;

    [SerializeField]
    private AudioSource soundEffectSource;

    [SerializeField]
    private AudioSource musicSource;
    private static SoundManager instance;
    private Vector2 defaultPitchRange;
    private float startPitch;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        defaultPitchRange = new Vector2(instance.soundEffectSource.pitch, instance.soundEffectSource.pitch);
        startPitch = soundEffectSource.pitch;
    }

    public static void SaveState()
    {
        musicStart = instance.musicSource.time;
    }

    public static void PlaySoundEffect(SoundEffect effect, float volume = 1f, Vector2 pitchRandomizer = default)
    {
        if (pitchRandomizer == default)
        {
            pitchRandomizer = instance.defaultPitchRange;
        }

        float randomizedPitch = UnityEngine.Random.Range(pitchRandomizer.x, pitchRandomizer.y);
        instance.soundEffectSource.pitch = randomizedPitch;

        instance.soundEffectSource.PlayOneShot(instance.soundEffectList[(int)effect], volume);

        instance.soundEffectSource.pitch = instance.startPitch;
    }

    public static void PlayMusic(Music music, float volume = 1f, bool start = true)
    {
        instance.musicSource.clip = instance.musicList[(int)music];
        instance.musicSource.volume = volume;

        if (start)
        {
            instance.musicSource.time = 0;
        }
        else
        {
            instance.musicSource.time = musicStart;
        }

        instance.musicSource.Play();
    }
}

