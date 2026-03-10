using ModConfig.UI;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ModConfig.Utilities
{
    public class ConfigHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string description;
        private static GameObject tooltipBox;
        private static TextMeshProUGUI tooltipText;
        private Coroutine hoverCoroutine;
        private RectTransform labelRect;

        private void Awake()
        {
            labelRect = GetComponent<RectTransform>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!string.IsNullOrEmpty(description))
                hoverCoroutine = StartCoroutine(WaitAndShow(0.5f));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (hoverCoroutine != null) StopCoroutine(hoverCoroutine);
            if (tooltipBox != null) tooltipBox.SetActive(false);
        }

        private IEnumerator WaitAndShow(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            ShowTooltip();
        }

        private void ShowTooltip()
        {
            if (tooltipBox == null) CreateTooltipPrefab();

            tooltipBox.SetActive(true);
            tooltipText.text = description;

            Vector3[] labelCorners = new Vector3[4];
            labelRect.GetWorldCorners(labelCorners);

            float centerX = (labelCorners[0].x + labelCorners[2].x) / 2f;
            float topY = labelCorners[1].y;

            tooltipBox.transform.position = new Vector3(centerX, topY + 5, 0);
            tooltipBox.transform.SetAsLastSibling();

            Canvas.ForceUpdateCanvases();
            ClampToScreen();
        }

        private void CreateTooltipPrefab()
        {
            var canvas = GameObject.Find("Canvas")?.transform;

            tooltipBox = new GameObject("DescriptionTooltip", typeof(RectTransform), typeof(HorizontalLayoutGroup), typeof(ContentSizeFitter));
            tooltipBox.transform.SetParent(canvas ?? transform.root, false);

            var rect = tooltipBox.GetComponent<RectTransform>();
            rect.pivot = new Vector2(0.5f, 0);
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.zero;

            var rootLayout = tooltipBox.GetComponent<HorizontalLayoutGroup>();
            rootLayout.childControlHeight = true;
            rootLayout.childControlWidth = true;

            var rootFitter = tooltipBox.GetComponent<ContentSizeFitter>();
            rootFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            rootFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

            var bgGo = new GameObject("TooltipCard", typeof(RectTransform), typeof(Image), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter), typeof(Outline));
            bgGo.transform.SetParent(tooltipBox.transform, false);

            var bgImage = bgGo.GetComponent<Image>();
            bgImage.color = new Color(1f, 0.98f, 0.933f, 1f);
            bgImage.raycastTarget = false;

            var outline = bgGo.GetComponent<Outline>();
            outline.effectColor = new Color(0.878f, 0.89f, 0.843f, 1f);
            outline.effectDistance = new Vector2(4, -4);

            var bgLayout = bgGo.GetComponent<VerticalLayoutGroup>();
            bgLayout.padding = new RectOffset(12, 12, 12, 12);
            bgLayout.childControlHeight = true;
            bgLayout.childControlWidth = true;

            var bgFitter = bgGo.GetComponent<ContentSizeFitter>();
            bgFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            bgFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

            var textGo = new GameObject("DescriptionText", typeof(RectTransform), typeof(TextMeshProUGUI), typeof(LayoutElement));
            textGo.transform.SetParent(bgGo.transform, false);

            tooltipText = textGo.GetComponent<TextMeshProUGUI>();
            tooltipText.font = UICloner.baseFont;
            tooltipText.fontSize = 18;
            tooltipText.color = Color.black;
            tooltipText.alignment = TextAlignmentOptions.Center;
            tooltipText.textWrappingMode = TextWrappingModes.Normal;
            tooltipText.raycastTarget = false;

            var textLayout = textGo.GetComponent<LayoutElement>();
            textLayout.preferredWidth = 550;

            tooltipBox.SetActive(false);
        }

        private void ClampToScreen()
        {
            var rect = tooltipBox.GetComponent<RectTransform>();
            Vector3[] corners = new Vector3[4];
            rect.GetWorldCorners(corners);

            float width = corners[2].x - corners[0].x;
            float height = corners[2].y - corners[0].y;
            Vector3 pos = tooltipBox.transform.position;

            if (pos.x - (width / 2) < 0)
                pos.x = (width / 2) + 10;
            else if (pos.x + (width / 2) > Screen.width)
                pos.x = Screen.width - (width / 2) - 10;

            if (pos.y + height > Screen.height)
            {
                Vector3[] labelCorners = new Vector3[4];
                labelRect.GetWorldCorners(labelCorners);
                pos.y = labelCorners[0].y - 10;
                rect.pivot = new Vector2(0.5f, 1f);
            }
            else
            {
                rect.pivot = new Vector2(0.5f, 0f);
            }

            tooltipBox.transform.position = pos;
        }
    }
}