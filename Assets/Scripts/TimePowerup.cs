using UnityEngine;
using UnityEngine.Events;

public class TimePowerup : MonoBehaviour
{
    public UnityEvent onPowerupPickup;

    [SerializeField] AudioClip pickupSound;
    [SerializeField] AudioClip endEffectSound;
    [SerializeField] float effectDuration;


    private AudioSource audioSource;
    private Renderer[] renderers;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        DisableRenderers();

        audioSource.PlayOneShot(pickupSound, 1f);

        audioSource.clip = endEffectSound;
        audioSource.PlayDelayed(effectDuration);

        if (onPowerupPickup != null)
            onPowerupPickup.Invoke();
        
        Destroy(gameObject, pickupSound.length + effectDuration);
    }

    private void DisableRenderers()
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = false;
        }
    }
}
