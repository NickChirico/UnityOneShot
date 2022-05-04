using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _audioManager;
    public static AudioManager GetAudioManager { get { return _audioManager; } }

    [Header("Audio Mixer")]
    [SerializeField] string _masterVolumeParameter = "MasterVolume";
    [SerializeField] AudioMixer _mixer;
    [SerializeField] Slider _slider;
    [SerializeField] private float _multiplier = 30f;


    [Header("Player Audio")]
    public AudioSource GunSource;
    public AudioSource ReloadSource;
    public AudioSource ChargeSource;
    public AudioSource BoostSource;
    public AudioSource AltFireSource;
    public AudioSource DashSource;
    public AudioSource MeleeSource;
    public AudioSource ExplosionSource;
    public AudioSource sfxSource;
    //public AudioSource musicSource;
    [Space(10)]
    public AudioClip[] shotSounds;
    public AudioClip[] rechamberSounds;
    public AudioClip reloadSound;
    public AudioClip chargeSound;
    public AudioClip[] boostSounds;
    public AudioClip dashSound;
    public AudioClip explosionSound;
    public AudioClip seraphPickupSound;
    [Space(10)]
    public AudioClip[] shotgunSounds;
    public AudioClip burstSound;
    [Space(10)]
    public AudioClip[] meleeSounds;

    [Header("Enemy Audio")]
    public AudioSource gruntSource;
    public AudioSource banelingSource;
    public AudioSource duelistSource;
    public AudioSource riflemanSource;

    bool isCharging;

    private void Awake()
    {
        if (_audioManager != null && _audioManager != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _audioManager = this;
            _slider.onValueChanged.AddListener(HandleSliderValueChanged);
        }
    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat(_masterVolumeParameter, _slider.value);   
    }
    private void HandleSliderValueChanged(float val)
    {
        _mixer.SetFloat(_masterVolumeParameter, Mathf.Log10(val) * _multiplier);
    }

    private void Start()
    {
        _slider.value = PlayerPrefs.GetFloat(_masterVolumeParameter, _slider.value);
    }

    public void PlayShotSound(int hit)
    {
        switch (hit)
        {
            case 1: 
                // Hit something
                GunSource.clip = shotSounds[Random.Range(0, shotSounds.Length)];
                GunSource.PlayOneShot(GunSource.clip);
                break;
            case 2:
                // Miss? Hit Terrain?
                break;

        }
        isCharging = false;
    }

    public void PlayRechamberSound()
    {
        ReloadSource.clip = rechamberSounds[Random.Range(0, rechamberSounds.Length)];
        ReloadSource.Play();
    }

    public void PlayFullReloadSound()
    {
        ReloadSource.clip = reloadSound;
        ReloadSource.Play();
    }

    public void PlayChargeSound()
    {
        if (!isCharging)
        {
            ChargeSource.clip = chargeSound;
            ChargeSource.Play();
        }
        isCharging = true;
    }

    public void PlayEnemyDeathSound(string enemyType)
    {
        switch (enemyType)
        {
            case "Grunt":
                gruntSource.PlayOneShot(gruntSource.clip);
                break;
            case "Baneling":
                banelingSource.PlayOneShot(banelingSource.clip);
                break;
            case "Duelist":
                duelistSource.PlayOneShot(duelistSource.clip);
                break;
            case "Rifleman":
                riflemanSource.PlayOneShot(riflemanSource.clip);
                break;
            default:
                gruntSource.PlayOneShot(gruntSource.clip);
                break;
        }
    }

    public void StopChargeSound()
    {
        isCharging = false;
        ChargeSource.Stop();
    }

    public void PlayBoostSound(bool isBig)
    {
        if (isBig)
        {
            BoostSource.clip = boostSounds[1];
        }
        else
        {
            BoostSource.clip = boostSounds[0];
        }
        BoostSource.Play();
    }

    public void PlayAltFireSound(string alt, bool hit)
    {
        switch (alt)
        {
            case "Shotgun":
                AltFireSource.clip = shotgunSounds[Random.Range(0, shotgunSounds.Length)];
                break;

            case "Burst":
                AltFireSource.clip = burstSound;
                break;
        }

        AltFireSource.Play();
    }

    public void StopAltSound()
    {
        AltFireSource.Stop();
    }

    public void PlayDashSound()
    {
        DashSource.Play();
    }
    public void PlayMeleeSound(int interval)
    {
        switch(interval)
        { 
            case 1:
                MeleeSource.clip = meleeSounds[0];
                break;
            case 2:
                MeleeSource.clip = meleeSounds[1];
                break;
            case 3:
                MeleeSource.clip = meleeSounds[2];
                break;
            default:
                MeleeSource.clip = meleeSounds[0];
                break;
        }

        MeleeSource.Play();
    }

    // SET AUDIOCLIP[]'s

    public void SetShotSounds(AudioClip[] sounds)
    {
        shotSounds = sounds;
    }

    public void SetReloadSound(AudioClip sound)
    {
        reloadSound = sound;
    }
}
