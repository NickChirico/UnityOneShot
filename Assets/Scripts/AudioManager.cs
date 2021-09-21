using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _audioManager;
    public static AudioManager GetAudioManager { get { return _audioManager; } }

    [Header("Player Audio")]
    public AudioSource GunSource;
    public AudioSource ReloadSource;
    public AudioSource ChargeSource;
    public AudioSource BoostSource;
    public AudioSource AltFireSource;
    [Space(10)]
    public AudioClip[] shotSounds;
    public AudioClip[] reloadSounds;
    public AudioClip chargeSound;
    public AudioClip[] boostSounds;
    [Space(5)]
    public AudioClip[] shotgunSounds;
    public AudioClip burstSound;

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
        }
    }

    public void PlayShotSound(int hit)
    {
        switch (hit)
        {
            case 1: 
                // Hit something
                GunSource.clip = shotSounds[Random.Range(0, shotSounds.Length)];
                GunSource.Play();
                break;
            case 2:
                // Miss? Hit Terrain?
                break;

        }
        isCharging = false;
    }

    public void PlayReloadSound()
    {
        ReloadSource.clip = reloadSounds[Random.Range(0, reloadSounds.Length)];
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
}
