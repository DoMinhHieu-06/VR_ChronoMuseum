using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace ChronoMuseum.VR
{
    public class ArtifactRestorationPedestal : MonoBehaviour
    {
        [Header("Pedestal Configuration")]
        [SerializeField] private HistoricalEra era;
        [SerializeField] private Transform[] pieceSnapSockets;
        [SerializeField] private GameObject brokenArtifactGroup;
        [SerializeField] private GameObject fullyRestoredArtifactModel;

        [Header("Effects & Feedback")]
        [SerializeField] private ParticleSystem restorationFlashVFX;
        [SerializeField] private Light pedestalSpotlight;
        [SerializeField] private Color completedSpotlightColor = new Color(1f, 0.85f, 0.3f, 1f); // Golden glow
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip snapPieceSFX;
        [SerializeField] private AudioClip restoredFanfareSFX;

        [Header("Linked Living Exhibit")]
        [SerializeField] private LivingExhibitController livingExhibitController;

        public UnityEvent OnRestorationCompleted;

        private HashSet<int> m_SlottedIndices = new HashSet<int>();
        private bool m_IsFullyRestored = false;

        private void Start()
        {
            if (fullyRestoredArtifactModel != null)
            {
                fullyRestoredArtifactModel.SetActive(false);
            }

            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.spatialBlend = 1.0f; // 3D Audio
                audioSource.playOnAwake = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (m_IsFullyRestored) return;

            var piece = other.GetComponent<ArtifactPiece>();
            if (piece == null || piece.IsPlacedInPedestal) return;

            if (piece.BelongsToEra == this.era)
            {
                SlotPiece(piece);
            }
        }

        public void SlotPiece(ArtifactPiece piece)
        {
            if (m_IsFullyRestored || piece.IsPlacedInPedestal) return;

            int idx = piece.PieceIndex;
            Transform targetSocket = (pieceSnapSockets != null && idx < pieceSnapSockets.Length && pieceSnapSockets[idx] != null)
                ? pieceSnapSockets[idx]
                : transform;

            piece.MarkAsPlacedInPedestal(targetSocket);
            m_SlottedIndices.Add(idx);

            if (snapPieceSFX != null && audioSource != null)
            {
                audioSource.PlayOneShot(snapPieceSFX);
            }

            // Register in global manager
            if (MuseumCuratorManager.Instance != null)
            {
                MuseumCuratorManager.Instance.RegisterPieceCollected(era);
            }

            var eraData = MuseumCuratorManager.Instance != null ? MuseumCuratorManager.Instance.GetEraData(era) : null;
            int requiredCount = eraData != null ? eraData.totalPiecesRequired : (pieceSnapSockets != null ? pieceSnapSockets.Length : 3);

            if (m_SlottedIndices.Count >= requiredCount)
            {
                CompleteRestoration();
            }
        }

        private void CompleteRestoration()
        {
            m_IsFullyRestored = true;
            Debug.Log($"[ChronoMuseum] Bệ phục chế {era} đã hoàn tất lắp ráp!");

            if (brokenArtifactGroup != null)
            {
                brokenArtifactGroup.SetActive(false);
            }

            if (fullyRestoredArtifactModel != null)
            {
                fullyRestoredArtifactModel.SetActive(true);
            }

            if (restorationFlashVFX != null)
            {
                restorationFlashVFX.Play();
            }

            if (pedestalSpotlight != null)
            {
                pedestalSpotlight.color = completedSpotlightColor;
                pedestalSpotlight.intensity *= 1.5f;
            }

            if (restoredFanfareSFX != null && audioSource != null)
            {
                audioSource.PlayOneShot(restoredFanfareSFX);
            }

            OnRestorationCompleted?.Invoke();

            if (livingExhibitController != null)
            {
                livingExhibitController.ActivateLivingExhibit();
            }
        }
    }
}
