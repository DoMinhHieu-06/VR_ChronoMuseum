using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ChronoMuseum.VR
{
    public enum HistoricalEra
    {
        AncientEgypt,
        JurassicPrehistoric,
        DongSonCulture,
        Renaissance
    }

    [System.Serializable]
    public class EraProgress
    {
        public HistoricalEra era;
        public string eraName;
        public bool isRestored;
        public int piecesFound;
        public int totalPiecesRequired;
        [TextArea] public string historicalLore;
    }

    public class MuseumCuratorManager : MonoBehaviour
    {
        public static MuseumCuratorManager Instance { get; private set; }

        [Header("Museum Progression")]
        [SerializeField] private List<EraProgress> eraList = new List<EraProgress>()
        {
            new EraProgress {
                era = HistoricalEra.AncientEgypt,
                eraName = "Ai Cập Cổ Đại (Mặt Nạ Vàng Tutankhamun)",
                totalPiecesRequired = 3,
                historicalLore = "Mặt nạ tang lễ bằng vàng ròng thế kỷ 14 TCN của pharaoh Tutankhamun, biểu tượng đỉnh cao của nền văn minh sông Nile."
            },
            new EraProgress {
                era = HistoricalEra.JurassicPrehistoric,
                eraName = "Kỷ Jura Tiền Sử (Hóa Thạch T-Rex)",
                totalPiecesRequired = 3,
                historicalLore = "Hộp sọ Tyrannosaurus Rex hoàn chỉnh niên đại 66 triệu năm trước với lực cắn khủng khiếp bậc nhất lịch sử."
            },
            new EraProgress {
                era = HistoricalEra.DongSonCulture,
                eraName = "Văn Hóa Đông Sơn (Trống Đồng Hoàng Gia)",
                totalPiecesRequired = 3,
                historicalLore = "Đỉnh cao nghệ thuật đúc đồng Việt cổ thiên niên kỷ 1 TCN, khắc họa ngôi sao 14 cánh và đàn chim Lạc bay lượn."
            },
            new EraProgress {
                era = HistoricalEra.Renaissance,
                eraName = "Thời Kỳ Phục Hưng (Cỗ Máy Bay Da Vinci)",
                totalPiecesRequired = 3,
                historicalLore = "Bản thảo phác thảo cỗ máy bay cánh chim (Ornithopter) của thiên tài Leonardo da Vinci năm 1485."
            }
        };

        [Header("Audio & Effects")]
        [SerializeField] private AudioSource museumAudioSource;
        [SerializeField] private AudioClip pieceAttachedSFX;
        [SerializeField] private AudioClip eraRestoredFanfareSFX;
        [SerializeField] private ParticleSystem globalCelebrationVFX;

        [Header("Events")]
        public UnityEvent<EraProgress> OnPieceUpdated;
        public UnityEvent<EraProgress> OnEraRestored;
        public UnityEvent OnAllErasCompleted;

        private int m_CompletedErasCount = 0;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            if (museumAudioSource == null)
            {
                museumAudioSource = gameObject.AddComponent<AudioSource>();
                museumAudioSource.spatialBlend = 0f; // 2D master audio for UI / global chimes
            }
        }

        public EraProgress GetEraData(HistoricalEra era)
        {
            return eraList.Find(e => e.era == era);
        }

        public IReadOnlyList<EraProgress> GetAllEras()
        {
            return eraList.AsReadOnly();
        }

        public void RegisterPieceCollected(HistoricalEra era)
        {
            var data = GetEraData(era);
            if (data == null || data.isRestored) return;

            data.piecesFound = Mathf.Min(data.piecesFound + 1, data.totalPiecesRequired);
            Debug.Log($"[ChronoMuseum] Tiến độ {data.eraName}: {data.piecesFound}/{data.totalPiecesRequired}");

            if (pieceAttachedSFX != null && museumAudioSource != null)
            {
                museumAudioSource.PlayOneShot(pieceAttachedSFX);
            }

            OnPieceUpdated?.Invoke(data);

            if (data.piecesFound >= data.totalPiecesRequired)
            {
                CompleteEra(data);
            }
        }

        private void CompleteEra(EraProgress data)
        {
            data.isRestored = true;
            m_CompletedErasCount++;
            Debug.Log($"[ChronoMuseum] ★ ĐÃ PHỤC HỒI THÀNH CÔNG: {data.eraName} ★");

            if (eraRestoredFanfareSFX != null && museumAudioSource != null)
            {
                museumAudioSource.PlayOneShot(eraRestoredFanfareSFX);
            }

            if (globalCelebrationVFX != null)
            {
                globalCelebrationVFX.Play();
            }

            OnEraRestored?.Invoke(data);

            if (m_CompletedErasCount >= eraList.Count)
            {
                Debug.Log("[ChronoMuseum] TẤT CẢ CÁC ĐẠI SẢNH ĐÃ ĐƯỢC CỨU VÃN! KHÁCH THAM QUAN ĐÃ HOÀN TẤT BẢO TÀNG THỜI KHÔNG!");
                OnAllErasCompleted?.Invoke();
            }
        }
    }
}
