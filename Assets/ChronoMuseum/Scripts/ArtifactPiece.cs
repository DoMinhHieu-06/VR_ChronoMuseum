using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace ChronoMuseum.VR
{
    [RequireComponent(typeof(XRGrabInteractable))]
    [RequireComponent(typeof(Rigidbody))]
    public class ArtifactPiece : MonoBehaviour
    {
        [Header("Artifact Identity")]
        [SerializeField] private HistoricalEra belongsToEra;
        [SerializeField] private string pieceName = "Mảnh Vỡ Cổ Vật";
        [SerializeField] private int pieceIndex = 0;
        [SerializeField] private bool isPlacedInPedestal = false;

        [Header("Visual Effects")]
        [SerializeField] private bool bobAndRotateWhenFree = true;
        [SerializeField] private float bobSpeed = 1.5f;
        [SerializeField] private float bobHeight = 0.05f;
        [SerializeField] private float rotateSpeed = 30f;
        [SerializeField] private ParticleSystem auraParticle;

        [Header("Audio")]
        [SerializeField] private AudioClip grabSound;
        private AudioSource m_AudioSource;

        private XRGrabInteractable m_GrabInteractable;
        private Rigidbody m_Rb;
        private Vector3 m_StartPos;
        private bool m_IsBeingHeld = false;

        public HistoricalEra BelongsToEra => belongsToEra;
        public string PieceName => pieceName;
        public int PieceIndex => pieceIndex;
        public bool IsPlacedInPedestal => isPlacedInPedestal;

        private void Awake()
        {
            m_GrabInteractable = GetComponent<XRGrabInteractable>();
            m_Rb = GetComponent<Rigidbody>();
            m_StartPos = transform.position;

            m_AudioSource = GetComponent<AudioSource>();
            if (m_AudioSource == null)
            {
                m_AudioSource = gameObject.AddComponent<AudioSource>();
                m_AudioSource.spatialBlend = 1.0f; // 3D Spatial Audio
                m_AudioSource.playOnAwake = false;
                m_AudioSource.maxDistance = 10f;
            }
        }

        private void OnEnable()
        {
            m_GrabInteractable.selectEntered.AddListener(OnGrabbed);
            m_GrabInteractable.selectExited.AddListener(OnReleased);
        }

        private void OnDisable()
        {
            m_GrabInteractable.selectEntered.RemoveListener(OnGrabbed);
            m_GrabInteractable.selectExited.RemoveListener(OnReleased);
        }

        private void Update()
        {
            if (bobAndRotateWhenFree && !m_IsBeingHeld && !isPlacedInPedestal)
            {
                transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
                float newY = m_StartPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }
        }

        private void OnGrabbed(SelectEnterEventArgs args)
        {
            m_IsBeingHeld = true;
            if (auraParticle != null && auraParticle.isPlaying)
            {
                auraParticle.Stop();
            }

            if (grabSound != null && m_AudioSource != null)
            {
                m_AudioSource.PlayOneShot(grabSound);
            }
        }

        private void OnReleased(SelectExitEventArgs args)
        {
            m_IsBeingHeld = false;
            m_StartPos = transform.position;
            if (auraParticle != null && !isPlacedInPedestal)
            {
                auraParticle.Play();
            }
        }

        public void MarkAsPlacedInPedestal(Transform socketSnapPoint)
        {
            isPlacedInPedestal = true;
            m_GrabInteractable.enabled = false;
            if (m_Rb != null)
            {
                m_Rb.isKinematic = true;
                m_Rb.detectCollisions = false;
            }

            transform.SetParent(socketSnapPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            if (auraParticle != null)
            {
                auraParticle.Stop();
            }
        }
    }
}
