using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace ChronoMuseum.VR
{
    public class VRMuseumSceneBuilder : MonoBehaviour
    {
        [Header("Auto Build Settings")]
        [SerializeField] private bool buildOnStartIfEmpty = true;

        [Header("Materials")]
        [SerializeField] private Material floorMaterial;
        [SerializeField] private Material wallMaterial;
        [SerializeField] private Material pedestalMaterial;
        [SerializeField] private Material goldMaterial;
        [SerializeField] private Material bronzeMaterial;
        [SerializeField] private Material boneMaterial;
        [SerializeField] private Material glowingCyanMaterial;

        private void Start()
        {
            if (buildOnStartIfEmpty)
            {
                CheckAndBuild();
            }
        }

        [ContextMenu("Build VR Museum Environment")]
        public void CheckAndBuild()
        {
            if (transform.Find("Museum_Hall") != null)
            {
                Debug.Log("[ChronoMuseum] Bảo tàng đã tồn tại, không xây dựng lại.");
                return;
            }

            BuildMuseum();
        }

        public void BuildMuseum()
        {
            GameObject hall = new GameObject("Museum_Hall");
            hall.transform.SetParent(transform);

            // Ensure Curator Manager exists
            #if UNITY_2023_1_OR_NEWER
            if (FindFirstObjectByType<MuseumCuratorManager>() == null)
            #else
            if (FindObjectOfType<MuseumCuratorManager>() == null)
            #endif
            {
                GameObject managerObj = new GameObject("MuseumCuratorManager");
                managerObj.AddComponent<MuseumCuratorManager>();
                managerObj.transform.SetParent(hall.transform);
            }

            // 1. Grand Floor
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "Museum_Floor";
            floor.transform.SetParent(hall.transform);
            floor.transform.position = new Vector3(0, -0.1f, 0);
            floor.transform.localScale = new Vector3(30, 0.2f, 30);
            SetColor(floor, new Color(0.12f, 0.14f, 0.18f)); // Dark polished marble

            // 2. Centerpiece Info Stand & Curator Scanner Tool
            BuildCentralScannerStation(hall.transform);

            // 3. Build 4 Quadrant Exhibit Halls
            // Ai Cập (North: Z + 8)
            BuildExhibitSection(hall.transform, HistoricalEra.AncientEgypt,
                "Ai Cập Cổ Đại", "Mặt Nạ Vàng Pharaoh",
                new Vector3(0, 0, 8), new Color(1f, 0.8f, 0.2f), PrimitiveType.Sphere);

            // Kỷ Jura (South: Z - 8)
            BuildExhibitSection(hall.transform, HistoricalEra.JurassicPrehistoric,
                "Kỷ Jura Tiền Sử", "Hộp Sọ T-Rex Khổng Lồ",
                new Vector3(0, 0, -8), new Color(0.9f, 0.9f, 0.8f), PrimitiveType.Cube);

            // Đông Sơn (East: X + 8)
            BuildExhibitSection(hall.transform, HistoricalEra.DongSonCulture,
                "Văn Hóa Đông Sơn", "Trống Đồng Hoàng Gia",
                new Vector3(8, 0, 0), new Color(0.8f, 0.5f, 0.2f), PrimitiveType.Cylinder);

            // Phục Hưng (West: X - 8)
            BuildExhibitSection(hall.transform, HistoricalEra.Renaissance,
                "Thời Kỳ Phục Hưng", "Cỗ Máy Bay Da Vinci",
                new Vector3(-8, 0, 0), new Color(0.4f, 0.7f, 1f), PrimitiveType.Capsule);

            Debug.Log("[ChronoMuseum] ★ Đã tự động tạo lập toàn bộ 4 Sảnh Triển Lãm & Hiện Vật tương tác VR thành công! ★");
        }

        private void BuildCentralScannerStation(Transform parent)
        {
            GameObject station = new GameObject("Central_Station");
            station.transform.SetParent(parent);
            station.transform.position = new Vector3(0, 0, 0);

            // Center Table
            GameObject table = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            table.name = "Scanner_Table";
            table.transform.SetParent(station.transform);
            table.transform.position = new Vector3(0, 0.45f, 1.5f);
            table.transform.localScale = new Vector3(1.2f, 0.45f, 1.2f);
            SetColor(table, new Color(0.2f, 0.25f, 0.3f));

            // Scanner Tool (Interactable)
            GameObject scannerObj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            scannerObj.name = "Curator_UV_Scanner";
            scannerObj.transform.SetParent(station.transform);
            scannerObj.transform.position = new Vector3(0, 0.95f, 1.5f);
            scannerObj.transform.localScale = new Vector3(0.08f, 0.15f, 0.08f);
            scannerObj.transform.rotation = Quaternion.Euler(90, 0, 0);
            SetColor(scannerObj, new Color(0.1f, 0.8f, 0.9f));

            var grab = scannerObj.AddComponent<XRGrabInteractable>();
            var scannerComp = scannerObj.AddComponent<CuratorScanner>();

            // Spotlight on scanner
            GameObject lightObj = new GameObject("ScannerLight");
            lightObj.transform.SetParent(scannerObj.transform);
            lightObj.transform.localPosition = new Vector3(0, 1f, 0);
            lightObj.transform.localRotation = Quaternion.Euler(90, 0, 0);
            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Spot;
            light.color = Color.cyan;
            light.range = 8f;
            light.spotAngle = 40f;
            light.enabled = false;
        }

        private void BuildExhibitSection(Transform parent, HistoricalEra era, string eraTitle, string artifactName, Vector3 centerPos, Color themeColor, PrimitiveType pieceShape)
        {
            GameObject section = new GameObject($"Exhibit_{era}");
            section.transform.SetParent(parent);
            section.transform.position = centerPos;

            // Pedestal Base
            GameObject pedestal = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pedestal.name = "Pedestal_Base";
            pedestal.transform.SetParent(section.transform);
            pedestal.transform.position = centerPos + new Vector3(0, 0.5f, 0);
            pedestal.transform.localScale = new Vector3(1.4f, 0.5f, 1.4f);
            SetColor(pedestal, new Color(0.25f, 0.25f, 0.28f));

            // Pedestal Component
            var pedestalComp = pedestal.AddComponent<ArtifactRestorationPedestal>();

            // Trigger Zone for pieces
            var boxCol = pedestal.AddComponent<BoxCollider>();
            boxCol.isTrigger = true;
            boxCol.size = new Vector3(2f, 2f, 2f);
            boxCol.center = new Vector3(0, 1f, 0);

            // Fully Restored Center Model (hidden initially)
            GameObject restoredModel = GameObject.CreatePrimitive(pieceShape);
            restoredModel.name = "Restored_Model";
            restoredModel.transform.SetParent(pedestal.transform);
            restoredModel.transform.localPosition = new Vector3(0, 1.5f, 0);
            restoredModel.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            SetColor(restoredModel, themeColor);
            restoredModel.SetActive(false);

            // Spotlight overhead
            GameObject spotObj = new GameObject("Exhibit_Spotlight");
            spotObj.transform.SetParent(section.transform);
            spotObj.transform.position = centerPos + new Vector3(0, 4.5f, 0);
            spotObj.transform.rotation = Quaternion.Euler(90, 0, 0);
            Light spot = spotObj.AddComponent<Light>();
            spot.type = LightType.Spot;
            spot.range = 7f;
            spot.spotAngle = 45f;
            spot.intensity = 3f;
            spot.color = themeColor;

            // Living Exhibit Controller
            var livingComp = section.AddComponent<LivingExhibitController>();

            // Placard Stand
            GameObject placardObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            placardObj.name = "Info_Placard";
            placardObj.transform.SetParent(section.transform);
            placardObj.transform.position = centerPos + new Vector3(-1.2f, 0.7f, -0.8f);
            placardObj.transform.localScale = new Vector3(0.6f, 0.8f, 0.1f);
            placardObj.transform.rotation = Quaternion.Euler(0, 35, 0);
            SetColor(placardObj, new Color(0.15f, 0.15f, 0.18f));

            var placardComp = placardObj.AddComponent<ExhibitInfoPlacard>();

            // Spawn 3 Artifact Pieces around the hall for player to find
            for (int i = 0; i < 3; i++)
            {
                float angle = (i * 120f) * Mathf.Deg2Rad;
                Vector3 pieceOffset = new Vector3(Mathf.Cos(angle) * 2.2f, 0.6f, Mathf.Sin(angle) * 2.2f);

                GameObject pieceObj = GameObject.CreatePrimitive(pieceShape);
                pieceObj.name = $"{era}_Piece_{i + 1}";
                pieceObj.transform.SetParent(section.transform);
                pieceObj.transform.position = centerPos + pieceOffset;
                pieceObj.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                SetColor(pieceObj, themeColor);

                var rb = pieceObj.AddComponent<Rigidbody>();
                rb.mass = 1f;

                var grab = pieceObj.AddComponent<XRGrabInteractable>();
                var pieceComp = pieceObj.AddComponent<ArtifactPiece>();
            }
        }

        private void SetColor(GameObject obj, Color color)
        {
            var renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
                renderer.material.color = color;
            }
        }
    }
}
