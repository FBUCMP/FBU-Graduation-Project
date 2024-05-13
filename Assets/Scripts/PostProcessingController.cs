using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{
    Volume volume;
    Vignette vignette;
    LensDistortion lensDistortion;

    [Header("On Damage Settings")]
    public float damageDuration = 0.2f;
    [Range(0,1)] public float vignetteIntensityChangeOnDamage = 0.2f;
    public Color vignetteColorChangeOnDamage = Color.red; // value at 50 is good (HSV)
    [Range(-1, 1)] public float lensDistortionIntensityChange = -0.3f;
    
    [Header("On Shoot Settings")]
    public float shootDuration = 0.1f;
    [Range(0,1)] public float vignetteIntensityChangeOnShoot = 0.2f;
    public Color vignetteColorChangeOnShoot = Color.white;

    Color initalVignetteColor;
    float initialVignetteIntensity;
    float initialLensDistortionIntensity;

    Transform player;
    PlayerGunSelector playerGunSelector;
    void Start()
    {
        
        volume = GetComponent<Volume>();
        volume.profile.TryGet<Vignette>(out vignette);
        volume.profile.TryGet<LensDistortion>(out lensDistortion);

        initalVignetteColor = vignette.color.value;
        initialVignetteIntensity = vignette.intensity.value;
        initialLensDistortionIntensity = lensDistortion.intensity.value;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        if(player == null || volume == null)
        {
            Debug.LogWarning("PostProcessingController Script cannot find required object(s)!");
            return;
        }

        player.GetComponent<HPlayer>().OnTakeDamage += OnPlayerTakeDamage;
        playerGunSelector = player.GetComponent<PlayerGunSelector>();
        playerGunSelector.OnGunSetup += OnPlayerGunSetup;
        playerGunSelector.ActiveGun.OnShoot += OnPlayerShoot;
    }
    private void OnDisable()
    {
        if(player != null) player.GetComponent<HPlayer>().OnTakeDamage -= OnPlayerTakeDamage;
        if(playerGunSelector != null) playerGunSelector.OnGunSetup -= OnPlayerGunSetup;
        if(playerGunSelector != null) playerGunSelector.ActiveGun.OnShoot -= OnPlayerShoot;
    }

    void OnPlayerTakeDamage(int damage)
    {
        StopAllCoroutines();
        
        StartCoroutine(OnPlayerTakeDamageRoutine(this.damageDuration, Mathf.Lerp(0, vignetteIntensityChangeOnDamage, damage / 100f)));
    }
    void OnPlayerShoot(float power) // power is not used maybe it will be used in the future
    {
        StopAllCoroutines();
        StartCoroutine(OnPlayerShootRoutine(this.shootDuration));
    }
    void OnPlayerGunSetup(GunSO gun)
    {
        gun.OnShoot += OnPlayerShoot;
    }
    IEnumerator OnPlayerTakeDamageRoutine(float duration, float intensity)
    {
        // might make it smoother with lerp
        vignette.color.value = vignetteColorChangeOnDamage;
        vignette.intensity.value = initialVignetteIntensity + intensity;
        lensDistortion.intensity.value = initialLensDistortionIntensity - lensDistortionIntensityChange;
        yield return new WaitForSeconds(duration);

        vignette.color.value = initalVignetteColor;
        vignette.intensity.value = initialVignetteIntensity;
        lensDistortion.intensity.value = initialLensDistortionIntensity;
    }
    IEnumerator OnPlayerShootRoutine(float duration)
    {
        // might make it smoother with lerp
        vignette.color.value = Color.white;
        vignette.intensity.value = initialVignetteIntensity + vignetteIntensityChangeOnShoot;
        yield return new WaitForSeconds(duration);

        vignette.color.value = initalVignetteColor;
        vignette.intensity.value = initialVignetteIntensity;
    }
}
