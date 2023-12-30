using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(TextMeshProUGUI))]
public class AmmoDisplayer : MonoBehaviour
{
    [SerializeField]
    private PlayerGunSelector GunSelector;
    private TextMeshProUGUI ammoText;

    private void Awake()
    {
        ammoText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        ammoText.SetText(
           $"{GunSelector.ActiveGun.ammoConfig.currentClipAmmo} / "
           + $"{GunSelector.ActiveGun.ammoConfig.clipSize}"
       );
    }
}