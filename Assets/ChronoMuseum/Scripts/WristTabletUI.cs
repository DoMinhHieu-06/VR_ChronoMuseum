using UnityEngine;
using TMPro;

namespace ChronoMuseum.VR
{
    public class WristTabletUI : MonoBehaviour
    {
        [Header("UI Text Displays")]
        [SerializeField] private TextMeshProUGUI objectiveTitleText;
        [SerializeField] private TextMeshProUGUI progressSummaryText;
        [SerializeField] private TextMeshProUGUI activeLoreText;

        [Header("Follow Wrist Option")]
        [SerializeField] private Transform wristAnchor;
        [SerializeField] private Vector3 localOffset = new Vector3(0, 0.1f, 0.05f);

        private void Start()
        {
            if (MuseumCuratorManager.Instance != null)
            {
                MuseumCuratorManager.Instance.OnPieceUpdated.AddListener(RefreshUI);
                MuseumCuratorManager.Instance.OnEraRestored.AddListener(OnEraFinished);
                MuseumCuratorManager.Instance.OnAllErasCompleted.AddListener(OnAllFinished);
            }

            UpdateInitialDisplay();
        }

        private void Update()
        {
            if (wristAnchor != null)
            {
                transform.position = wristAnchor.TransformPoint(localOffset);
                transform.rotation = wristAnchor.rotation;
            }
        }

        private void UpdateInitialDisplay()
        {
            if (objectiveTitleText != null)
            {
                objectiveTitleText.text = "<b>NHIỆM VỤ:</b> PHỤC HỒI CÁC CỔ VẬT THỜI KHÔNG";
            }

            if (progressSummaryText != null)
            {
                progressSummaryText.text =
                    "• Ai Cập: 0/3 mảnh\n" +
                    "• Kỷ Jura: 0/3 mảnh\n" +
                    "• Đông Sơn: 0/3 mảnh\n" +
                    "• Phục Hưng: 0/3 mảnh";
            }

            if (activeLoreText != null)
            {
                activeLoreText.text = "Dùng găng tay VR nhặt các mảnh vỡ và gắn vào bệ phục chế tương ứng.";
            }
        }

        private void RefreshUI(EraProgress eraData)
        {
            if (activeLoreText != null)
            {
                activeLoreText.text = $"<b>{eraData.eraName}</b>\nTiến độ: {eraData.piecesFound}/{eraData.totalPiecesRequired}\n<i>{eraData.historicalLore}</i>";
            }

            BuildSummary();
        }

        private void OnEraFinished(EraProgress eraData)
        {
            if (objectiveTitleText != null)
            {
                objectiveTitleText.text = $"<color=#00FF88>★ HOÀN TẤT {eraData.eraName.ToUpper()}! ★</color>";
            }

            BuildSummary();
        }

        private void OnAllFinished()
        {
            if (objectiveTitleText != null)
            {
                objectiveTitleText.text = "<color=#FFD700>★★★ XUẤT SẮC: TOÀN BỘ BẢO TÀNG ĐÃ ĐƯỢC CỨU! ★★★</color>";
            }

            if (activeLoreText != null)
            {
                activeLoreText.text = "Bạn đã hoàn thành vai trò Nhà Giám Tuyển Thời Không xuất sắc nhất lịch sử!";
            }
        }

        private void BuildSummary()
        {
            if (progressSummaryText == null || MuseumCuratorManager.Instance == null) return;

            string summary = "";
            foreach (var era in MuseumCuratorManager.Instance.GetAllEras())
            {
                string status = era.isRestored ? "<color=#00FF88>✓ Đã xong</color>" : $"{era.piecesFound}/{era.totalPiecesRequired}";
                summary += $"• {era.era}: {status}\n";
            }
            progressSummaryText.text = summary;
        }
    }
}
