using UnityEngine;
using TMPro;

namespace ChronoMuseum.VR
{
    public class ExhibitInfoPlacard : MonoBehaviour
    {
        [Header("Exhibit Information")]
        [SerializeField] private string exhibitName = "Hiện Vật Lịch Sử";
        [SerializeField] private string eraAndOrigin = "Kỷ nguyên / Xuất xứ";
        [TextArea(3, 8)] [SerializeField] private string historicalDescription = "Mô tả chi tiết ý nghĩa lịch sử...";

        [Header("UI Text References")]
        [SerializeField] private TextMeshPro titleTMP;
        [SerializeField] private TextMeshPro originTMP;
        [SerializeField] private TextMeshPro descriptionTMP;

        [Header("Audio Guide")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip narrationAudioClip;

        private void Start()
        {
            ApplyPlacardData();

            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    audioSource = gameObject.AddComponent<AudioSource>();
                    audioSource.spatialBlend = 1.0f; // 3D Audio
                    audioSource.playOnAwake = false;
                }
            }
        }

        private void OnValidate()
        {
            ApplyPlacardData();
        }

        public void ApplyPlacardData()
        {
            if (titleTMP != null) titleTMP.text = exhibitName;
            if (originTMP != null) originTMP.text = $"<color=#88CCFF>{eraAndOrigin}</color>";
            if (descriptionTMP != null) descriptionTMP.text = historicalDescription;
        }

        public void PlayAudioGuide()
        {
            if (audioSource != null && narrationAudioClip != null)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
                else
                {
                    audioSource.clip = narrationAudioClip;
                    audioSource.Play();
                }
            }
        }
    }
}
