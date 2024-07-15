using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// <para>功用: 放音效</para>
/// <para>使用方式: 在需要的地方new 一個 Pack出來播就行了, 你要自己叫SoundSystem也行</para>
/// </summary>
public class SoundSystem : MonoBehaviour
{
    public static SoundSave LastSoundSave = new(0.8f, 0.8f, 0.8f);
    [SerializeField] SoundPack PlayOnAwake;


    readonly List<AudioSource> AudioPlayers = new();
    readonly List<AudioSource> SFXPlayers = new();
    readonly List<AudioSource> MusicPlayers = new();
    readonly List<AudioSource> UIPlayers = new();

    [Header ("Mixer")]
    [SerializeField] AudioMixer GameAudioMixer;
    [SerializeField] AudioMixerGroup MasterMixer;
    [SerializeField] AudioMixerGroup SFXMixer;
    [SerializeField] AudioMixerGroup MusicMixer;
    [SerializeField] AudioMixerGroup UIMixer;

   public  AudioMixerSnapshot DefaultSnapShot;

    readonly static Dictionary<int, AudioSource> Locker = new();

    public static System.Action<SoundPack, float> PlayAudioCall;
    public static System.Action<RandomPlaySoundPack, float> PlayRandomAudioCall;

    public static System.Action<SoundPack, float> PlayAudioWithLockerCall;


  

    public static int LockerKeyTamp;


    public static (SoundPack Clip, float Time, int Key) ContinuePlayAudio;
    public enum SoundType
    {
        SFX,
        Music,
        UI,
    }

    #region 火星文 
   
    private void Awake()
    {
        PlayAudioCall += PlayAudio;
        PlayRandomAudioCall += PlayAudio;
        PlayAudioWithLockerCall += PlayAudioWithLocker;

        if (ContinuePlayAudio.Clip != null)
        {
            AudioSource AudioPlayer = FindAduioPlayerAvaliable(ContinuePlayAudio.Clip.SoundType);
            if (AudioPlayer == null) { return; }
            AudioPlayer.clip = ContinuePlayAudio.Clip.Clip;
            AudioPlayer.loop = ContinuePlayAudio.Clip.Loop;
            AudioPlayer.volume = ContinuePlayAudio.Clip.Volume;
            AudioPlayer.pitch = ContinuePlayAudio.Clip.Pitch;
            AudioPlayer.time = ContinuePlayAudio.Time;
            AudioPlayer.Play();
            int Newkey = Locker.Count;
            Locker.Add(Newkey, AudioPlayer);
            LockerKeyTamp = Newkey;
            ContinuePlayAudio.Key = Newkey;
        }
    }
    private void Start()
    {
        if (PlayOnAwake.GetLockerAudio() != null) { SoundSystem.FadeInEffect(PlayOnAwake.GetLockerAudio(), 1f); }
    }
    void Update()
    {
        MasterMixer.audioMixer.SetFloat("Master Volume", -80 + 100 * LastSoundSave.Master);
        MusicMixer.audioMixer.SetFloat("Music Volume", -80 + 100 * LastSoundSave.Music);

        SFXMixer.audioMixer.SetFloat("SFX Volume", -80 + 100 * LastSoundSave.SFX);
        UIMixer.audioMixer.SetFloat("UI Volume", -80 + 100 * LastSoundSave.UI);
    }
    public void SetSnapShot(AudioMixerSnapshot snapshot)
    {
        snapshot.TransitionTo(0.01f);
    }

    void OnDestroy()
    {
        PlayAudioCall -= PlayAudio;
        PlayRandomAudioCall -= PlayAudio;
        PlayAudioWithLockerCall -= PlayAudioWithLocker;
        SetSnapShot(DefaultSnapShot);
    }
    public void PlayAudio(SoundPack soundpack, float Delay = 0)
    {
        if (soundpack.Clip == null) { return; }
        AudioSource AudioPlayer = FindAduioPlayerAvaliable(soundpack.SoundType);
        if (AudioPlayer == null) { return; }
        SetPlayer(AudioPlayer, soundpack, Delay);
    }
    public void PlayAudio(RandomPlaySoundPack soundpack, float Delay = 0)
    {
        int randomindex = Random.Range(0, soundpack.SoundPackList.Count);
        AudioSource AudioPlayer = FindAduioPlayerAvaliable(soundpack.SoundPackList[randomindex].SoundType);
        if (AudioPlayer == null) { return; }
        SetPlayer(AudioPlayer, soundpack.SoundPackList[randomindex], Delay);
    }
    void PlayAudioWithLocker(SoundPack soundpack, float Delay = 0)
    {
        AudioSource AudioPlayer = FindAduioPlayerAvaliable(soundpack.SoundType);
        if (AudioPlayer == null) { return; }
        SetPlayer(AudioPlayer, soundpack, Delay);

        int Newkey = Locker.Count;
        Locker.Add(Newkey, AudioPlayer);
        LockerKeyTamp = Newkey;
    }
    public static async void FadeInEffect(AudioSource Audio, float FadeTime)
    {

        float VolumeDistent = Audio.volume;

        for (float i = 0; i < FadeTime; i += 0.02f)
        {
            if (Audio == null) { break; }

            Audio.volume = (i / FadeTime) * VolumeDistent;
            await System.Threading.Tasks.Task.Delay(20);
        }
        Audio.volume = VolumeDistent;
    }
    public static async void FadeOutEffect(AudioSource Audio, float FadeTime)
    {
        float VolumeDistent = Audio.volume;

        for (float i = 0; i < FadeTime; i += Time.deltaTime)
        {
            if (Audio == null)
            {
                break;
            }
            Audio.volume = ((FadeTime - i) / FadeTime) * VolumeDistent;
            await System.Threading.Tasks.Task.Delay(Mathf.RoundToInt(Time.unscaledDeltaTime * 1000));
        }
    }
    public static AudioSource GetLockerAudio(int Key)
    {
        if (Locker.ContainsKey(Key))
        {
            return Locker[Key];
        }
        else
        {
            return null;
        }
    }
    public static void RemoveLockerAudio(int Key)
    {
        Locker.Remove(Key);
    }
    public static void SwitchSoundClip(AudioSource audio, SoundPack NewSound, float Delay)
    {
        audio.Stop();
        if (NewSound.Clip == null) { return; }
        audio.clip = NewSound.Clip;
        audio.volume = NewSound.Volume;
        audio.pitch = NewSound.Pitch;
        audio.PlayDelayed(Delay);

    }
    void SetPlayer(AudioSource AudioPlayer, SoundPack soundpack, float Delay)
    {
        AudioPlayer.clip = soundpack.Clip;
        AudioPlayer.volume = soundpack.Volume;
        AudioPlayer.pitch = soundpack.Pitch;
        if (soundpack.RandomPitch)
        {
            AudioPlayer.pitch = Random.Range(soundpack.RandomPitchBottom, soundpack.RandomPitchTop);
        }
        AudioPlayer.loop = soundpack.Loop;
        AudioPlayer.PlayDelayed(Delay);

    }
    AudioSource FindAduioPlayerAvaliable(SoundType soundType)
    {
        AudioSource Result = null;


        if (soundType == SoundType.SFX)
        {
            foreach (AudioSource audioSource in SFXPlayers)
            {
                if (!audioSource.isPlaying && !Locker.ContainsValue(audioSource))
                {
                    Result = audioSource;
                }
            }
        }
        else if (soundType == SoundType.Music)
        {
            foreach (AudioSource audioSource in MusicPlayers)
            {
                if (!audioSource.isPlaying && !Locker.ContainsValue(audioSource))
                {
                    Result = audioSource;
                }
            }
        }
        else if (soundType == SoundType.UI)
        {
            foreach (AudioSource audioSource in UIPlayers)
            {
                if (!audioSource.isPlaying && !Locker.ContainsValue(audioSource))
                {
                    Result = audioSource;
                }
            }
        }

        if (Result == null)
        {
            if (AudioPlayers.Count > 150f)
            {
                return null;
            }

            Result = gameObject.AddComponent<AudioSource>();
            AudioPlayers.Add(Result);


            switch (soundType)
            {
                case SoundType.SFX:
                    Result.outputAudioMixerGroup = SFXMixer;
                    SFXPlayers.Add(Result);
                    break;
                case SoundType.Music:
                    Result.outputAudioMixerGroup = MusicMixer;
                    MusicPlayers.Add(Result);
                    break;
                case SoundType.UI:
                    Result.outputAudioMixerGroup = UIMixer;
                    UIPlayers.Add(Result);
                    break;
                default:
                    break;
            }

        }

        return Result;
    }
 #endregion

    [System.Serializable]
    public class SoundSave
    {
        public SoundSave(float _Master = 1f, float _SFX = 1f, float _Music = 1f, float _UI = 1f)
        {
            Master = _Master;
            SFX = _SFX;
            Music = _Music;
            UI = _UI;

        }
        public float Master;
        public float SFX;
        public float Music;
        public float UI;
    }
}


//在需要的地方叫下面這個就好,功能都在裡面

/// <summary>
/// <para>用法: 在外面把音效拉好之後程式直接  Play(); </para>
/// <para>後面可以串更多功能，像這樣 => Hitsound.PlayLocker().FadeIn(3);</para>
/// <para>使用PlayLocker()讓音效標記成不可複寫狀態,只有Locker才能使用漸進和淡出</para>
/// </summary>
[System.Serializable]
public class SoundPack
{
    public SoundSystem.SoundType SoundType;
    public AudioClip Clip;
    public float Volume = 1;
    public float Pitch = 1;
    public bool RandomPitch = false;
    public float RandomPitchTop = 0;
    public float RandomPitchBottom = 0;
    public bool Loop = false;

    [HideInInspector] public int LockerKey;
    AudioSource Source;


    //用法: 在外面把音效拉好之後直接 .Play(); 
    //後面可以串更多功能，像這樣=>  Hitsound.PlayLocker().FadeIn(3);
    // 使用PlayLocker()讓音效標記成不可複寫狀態,只有Locker才能使用漸進和淡出

    public SoundPack Play()
    {
        SoundSystem.PlayAudioCall?.Invoke(this, 0);
        return this;
    }
    public SoundPack Play(float delay)
    {
        SoundSystem.PlayAudioCall?.Invoke(this, delay);
        return this;
    }
    public SoundPack PlayLocker()
    {
        SoundSystem.PlayAudioWithLockerCall?.Invoke(this, 0);
        LockerKey = SoundSystem.LockerKeyTamp;
        Source = SoundSystem.GetLockerAudio(LockerKey);
        return this;
    }
    public AudioSource GetLockerAudio()
    {
        return Source;
    }
    public SoundPack FadeIn(float time)
    {
        if (Source != null)
        {
            SoundSystem.FadeInEffect(Source, time);
        }
        return this;
    }
    public SoundPack FadeOut(float time)
    {
        if (Source != null)
        {
            SoundSystem.FadeOutEffect(Source, time);
        }
        return this;
    }
}




//其他特殊版本的SoundPack

[System.Serializable]
public class RandomPlaySoundPack
{
    public List<SoundPack> SoundPackList;
}


[System.Serializable]
public class UISoundPack
{
    public SoundPack ClickSound = new();
    public SoundPack HighLightSound = new();
    public SoundPack UnHighlightSound = new();
    public SoundPack SelectSound = new();
    public SoundPack UnSelectSound = new();
}
