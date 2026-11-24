using UnityEngine;
using TMPro;

namespace ChronoMuseum.VR
{
    public class HiddenInscription : MonoBehaviour
    {
        [Header("Secret Lore")]
        [SerializeField] private string inscriptionTitle = "Cổ Ngữ Bí Ẩn";
        [TextArea] [SerializeField] private string secretText = "Vào thời cổ đại, chỉ có bậc đế vương mới được phép chạm tay vào báu vật này.";

        [Header("Visual Feedback")]
        [SerializeField] private MeshRenderer runeRenderer;
        [SerializeField] private Material glowingRuneMaterial;
        [SerializeField] private TextMeshPro worldTextDisplay;
        [SerializeField] private ParticleSystem revealSparkleVFX;

        private bool m_IsRevealed = false;
        public bool IsRevealed => m_IsRevealed;

        private void Start()
        {
            if (worldTextDisplay != null)
            {
                worldTextDisplay.gameObject.SetActive(false);
            }
        }

        public void RevealInscription()
        {
            if (m_IsRevealed) return;
            m_IsRevealed = true;

            Debug.Log($"[ChronoMuseum] Khám phá cổ ngữ: {inscriptionTitle} - {secretText}");

            if (runeRenderer != null && glowingRuneMaterial != null)
            {
                runeRenderer.material = glowingRuneMaterial;
            }

            if (worldTextDisplay != null)
            {
                worldTextDisplay.gameObject.SetActive(true);
                worldTextDisplay.text = $"<color=#FFD700><b>{inscriptionTitle}</b></color>\n{secretText}";
            }

            if (revealSparkleVFX != null)
            {
                revealSparkleVFX.Play();
            }
        }
    }
}
