using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum SoundID
{
    BG_1,
}

public enum FxID
{
    CoinDrop,
    Failed,
    MeleeAtk,
    MeleeDeath_1,
    MeleeDeath_2,
    PopupNewCharacter,
    Popup_NewCharacter,
    RangeAtk,
    RangeDeath_1,
    RangeDeath_2,
    PopupSpinEnd,
    PopupSpinTick,
    Click,
    Victory,

}


public class SoundManager : Singleton<SoundManager>
{
    private AudioSource soundSource;
    private AudioSource fxSource;

    [SerializeField] private AudioClip[] soundAus;
    [SerializeField] private AudioClip[] fxAus;

    private bool isLoaded = false;
    private int indexSound;

    protected override void Awake()
    {
        base.Awake();

        soundSource = gameObject.AddComponent<AudioSource>();
        soundSource.loop = true;
        fxSource = gameObject.AddComponent<AudioSource>();
        fxSource.loop = false;
    }

    private void Start()
    {
        StartCoroutine(IELoad());
    }

    private IEnumerator IELoad()
    {
        if (soundAus.Length > 0)
        {
            yield return Cache.GetWFS(1f);
            isLoaded = true;

            indexSound = Random.Range(0, soundAus.Length);
            PlaySound((SoundID)indexSound);
        }
    }


    public void PlaySound(SoundID ID)
    {
        soundSource.clip = soundAus[(int)ID];

        if (isLoaded)
        {
            soundSource.Play();
            //Debug.Log(ID);
        }
    }

    public void NextSound()
    {
        indexSound = indexSound >= soundAus.Length - 1 ? 0 : indexSound + 1;
        PlaySound((SoundID)indexSound);
    }

    public void PlayMusic(bool play)
    {
        if (play)
        {
            soundSource.Play();
        }
        else
        {
            soundSource.Stop();
        }
    }


    public void PlayFx(FxID ID)
    {
        if (isLoaded)
        {
            fxSource.PlayOneShot(fxAus[(int)ID]);

            //Debug.Log(ID);
        }
    }

    public void ChangeSound(SoundID ID, float time)
    {
        if (isLoaded)
        {
            float spacetime = time / 2;

            ChangeVol(.1f, spacetime,
                () =>
                {
                    PlaySound(ID);
                    ChangeVol(1, spacetime, null);
                });
        }
    }

    public void ChangeVol(float vol, float time, UnityAction callBack)
    {
        StartCoroutine(ChangeVolume(vol, time, callBack));
    }

    private IEnumerator ChangeVolume(float vol, float time, UnityAction callBack)
    {
        float stepVol = (vol - soundSource.volume) / 10;
        float stepTime = time / 10;

        for (int i = 0; i < 10; i++)
        {
            soundSource.volume += stepVol;
            yield return Cache.GetWFS(stepTime);
        }

        callBack?.Invoke();
    }


}
