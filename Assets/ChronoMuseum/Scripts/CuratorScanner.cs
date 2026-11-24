using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace ChronoMuseum.VR
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class CuratorScanner : MonoBehaviour
    {
        [Header("Scanner Beam")]
        [SerializeField] private Transform scanRayOrigin;
        [SerializeField] private float scanDistance = 5f;
        [SerializeField] private LayerMask scanLayers = ~0;
        [SerializeField] private Light scanSpotlight;
        [SerializeField] private LineRenderer scanLaserBeam;

        [Header("Audio")]
        [SerializeField] private AudioSource scannerAudio;
        [SerializeField] private AudioClip scanHummingLoop;
        [SerializeField] private AudioClip secretDiscoveredSFX;

        private XRGrabInteractable m_GrabInteractable;
        private bool m_IsHeld = false;

        private void Awake()
        {
            m_GrabInteractable = GetComponent<XRGrabInteractable>();
            if (scanRayOrigin == null) scanRayOrigin = transform;

            if (scannerAudio == null)
            {
                scannerAudio = gameObject.AddComponent<AudioSource>();
                scannerAudio.spatialBlend = 1f;
                scannerAudio.loop = true;
                scannerAudio.clip = scanHummingLoop;
            }
        }

        private void OnEnable()
        {
            m_GrabInteractable.selectEntered.AddListener(OnGrab);
            m_GrabInteractable.selectExited.AddListener(OnRelease);
        }

        private void OnDisable()
        {
            m_GrabInteractable.selectEntered.RemoveListener(OnGrab);
            m_GrabInteractable.selectExited.RemoveListener(OnRelease);
        }

        private void OnGrab(SelectEnterEventArgs args)
        {
            m_IsHeld = true;
            if (scanSpotlight != null) scanSpotlight.enabled = true;
            if (scanLaserBeam != null) scanLaserBeam.enabled = true;
            if (scannerAudio != null && scanHummingLoop != null) scannerAudio.Play();
        }

        private void OnRelease(SelectExitEventArgs args)
        {
            m_IsHeld = false;
            if (scanSpotlight != null) scanSpotlight.enabled = false;
            if (scanLaserBeam != null) scanLaserBeam.enabled = false;
            if (scannerAudio != null && scannerAudio.isPlaying) scannerAudio.Stop();
        }

        private void Update()
        {
            if (!m_IsHeld) return;

            Ray ray = new Ray(scanRayOrigin.position, scanRayOrigin.forward);
            Vector3 endPos = scanRayOrigin.position + scanRayOrigin.forward * scanDistance;

            if (Physics.Raycast(ray, out RaycastHit hit, scanDistance, scanLayers))
            {
                endPos = hit.point;

                var inscription = hit.collider.GetComponent<HiddenInscription>();
                if (inscription != null && !inscription.IsRevealed)
                {
                    inscription.RevealInscription();
                    if (secretDiscoveredSFX != null && scannerAudio != null)
                    {
                        scannerAudio.PlayOneShot(secretDiscoveredSFX);
                    }
                }
            }

            if (scanLaserBeam != null && scanLaserBeam.enabled)
            {
                scanLaserBeam.SetPosition(0, scanRayOrigin.position);
                scanLaserBeam.SetPosition(1, endPos);
            }
        }
    }
}
