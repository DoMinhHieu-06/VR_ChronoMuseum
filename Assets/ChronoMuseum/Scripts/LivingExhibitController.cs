using System.Collections;
using UnityEngine;

namespace ChronoMuseum.VR
{
    public class LivingExhibitController : MonoBehaviour
    {
        [Header("Exhibit Identity")]
        [SerializeField] private HistoricalEra era;
        [SerializeField] private string exhibitTitle = "Cổng Ký Ức Thời Không";

        [Header("Living Hologram & Environment")]
        [SerializeField] private GameObject historicalDioramaGroup;
        [SerializeField] private ParticleSystem timePortalVFX;
        [SerializeField] private AudioSource eraAmbientAudioSource;
        [SerializeField] private AudioClip portalOpenSFX;
        [SerializeField] private AudioClip historicalAmbienceClip;

        [Header("Interactive Elements")]
        [SerializeField] private Animator exhibitAnimator;
        [SerializeField] private string triggerAnimationName = "Activate";
        [SerializeField] private Light exhibitAtmosphereLight;
        [SerializeField] private Color ancientAtmosphereColor = new Color(0.2f, 0.8f, 1f, 1f);

        [Header("Duration Settings")]
        [SerializeField] private bool autoDimAfterTime = false;
        [SerializeField] private float activeDurationSeconds = 30f;

        private bool m_IsActive = false;

        private void Start()
        {
            if (historicalDioramaGroup != null)
            {
                historicalDioramaGroup.SetActive(false);
            }
        }

        public void ActivateLivingExhibit()
        {
            if (m_IsActive) return;
            m_IsActive = true;

            Debug.Log($"[ChronoMuseum] KÍCH HOẠT TRIỂN LÃM SỐNG: {exhibitTitle} ({era})");
            StartCoroutine(SequenceActivation());
        }

        private IEnumerator SequenceActivation()
        {
            // 1. Time Portal VFX & Audio
            if (timePortalVFX != null)
            {
                timePortalVFX.Play();
            }

            if (eraAmbientAudioSource != null && portalOpenSFX != null)
            {
                eraAmbientAudioSource.PlayOneShot(portalOpenSFX);
            }

            yield return new WaitForSeconds(1.2f);

            // 2. Reveal Historical Diorama / Hologram
            if (historicalDioramaGroup != null)
            {
                historicalDioramaGroup.SetActive(true);
            }

            if (exhibitAtmosphereLight != null)
            {
                exhibitAtmosphereLight.enabled = true;
                exhibitAtmosphereLight.color = ancientAtmosphereColor;
            }

            // 3. Play Historical Ambience loop (Dino roar / Egypt chants / Bronze drum)
            if (eraAmbientAudioSource != null && historicalAmbienceClip != null)
            {
                eraAmbientAudioSource.clip = historicalAmbienceClip;
                eraAmbientAudioSource.loop = true;
                eraAmbientAudioSource.spatialBlend = 1.0f; // 3D Spatial Audio immersion
                eraAmbientAudioSource.Play();
            }

            // 4. Trigger animations (e.g. dinosaur roar or Da Vinci wings flapping)
            if (exhibitAnimator != null && !string.IsNullOrEmpty(triggerAnimationName))
            {
                exhibitAnimator.SetTrigger(triggerAnimationName);
            }

            // 5. Optional auto-dim
            if (autoDimAfterTime)
            {
                yield return new WaitForSeconds(activeDurationSeconds);
                // Settle into steady ambient state
                if (timePortalVFX != null) timePortalVFX.Stop();
            }
        }
    }
}
