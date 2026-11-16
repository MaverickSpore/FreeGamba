using UnityEngine;
using UnityEngine.UI;

public class WidgetSwitcher : MonoBehaviour
{
    [SerializeField] bool IsGrid = false;
    [SerializeField] float GridWidth = 100f;
    [SerializeField] float GridHeight = 100f;
    private int CurrentWidgetIndex = 0;
    public int GetCurrentWidgetIndex() => CurrentWidgetIndex;
    public void SetWidgetindex(int index)
    {
        if (index < 0)
            index = 0;
        else if (index >= transform.childCount)
            index = transform.childCount - 1;
        CurrentWidgetIndex = index;
        UpdateWidgetVisibility();
    }
    public void NextWidget()
    {
        CurrentWidgetIndex++;
        if (CurrentWidgetIndex >= transform.childCount)
            CurrentWidgetIndex = 0;
        UpdateWidgetVisibility();
    }
    public void PreviousWidget()
    {
        CurrentWidgetIndex--;
        if (CurrentWidgetIndex < 0)
            CurrentWidgetIndex = transform.childCount - 1;
        UpdateWidgetVisibility();
    }
    public void AddWidget()
    {
        GameObject newWidget = new GameObject("Widget_" + transform.childCount, typeof(RectTransform), typeof(Canvas));
        newWidget.transform.SetParent(transform);
        newWidget.transform.localScale = Vector3.one;

        var rectTransform = newWidget.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        UpdateWidgetVisibility();
        ConfirmGrid();
    }
    public void InsertWidget(int index)
    {
        if (index < 0)
            index = 0;
        else if (index >= transform.childCount)
            index = transform.childCount - 1;
        GameObject newWidget = new GameObject("InsertedWidget_" + index, typeof(RectTransform), typeof(Canvas));
        newWidget.transform.SetParent(transform);
        newWidget.transform.SetSiblingIndex(index);
        newWidget.transform.localScale = Vector3.one;

        var rectTransform = newWidget.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        UpdateWidgetVisibility();
        ConfirmGrid();
    }

    private void UpdateWidgetVisibility()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == CurrentWidgetIndex);
        }
    }

    private void Start()
    {
        UpdateWidgetVisibility();
        ConfirmGrid();
    }
    private void ConfirmGrid()
    {
        if (IsGrid)
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<GridLayoutGroup>() == null)
                {
                    child.gameObject.AddComponent<GridLayoutGroup>();
                }
                GridLayoutGroup gridLayout = child.GetComponent<GridLayoutGroup>();
                gridLayout.cellSize = new Vector2(GridWidth, GridHeight);
                gridLayout.spacing = new Vector2(10, 10);
                gridLayout.padding = new RectOffset(10, 10, 10, 10);
                gridLayout.childAlignment = TextAnchor.MiddleCenter;
            }
        }
    }
}
