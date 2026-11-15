using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameSelectionController : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] SGOGameTypeLibrary GameTypeLibrary;
    [SerializeField] GameObject GameTypePrefab;
    [SerializeField] GameObject ButtonSection;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Handle the beginning of a drag event if needed
        eventData.eligibleForClick = false; // Prevent click events during drag
    }
    public void OnDrag(PointerEventData eventData)
    {
        // Handle the drag event if needed
    }


    private void OnNavigate(InputAction.CallbackContext context) => StartNavigate(context.ReadValue<float>());
    private void OnNavigateEnd(InputAction.CallbackContext context) => EndNavigate();
    float NavigationValue = 0f;

    private void Start()
    {
        if (GameTypeLibrary != null && GameTypePrefab != null)
        {
            foreach (var gameType in GameTypeLibrary.Types)
            {
                GameObject newGameType = Instantiate(GameTypePrefab, ButtonSection.transform);
                
                if (newGameType.TryGetComponent<GameTypeButtonController>(out var button))
                {
                    button.Initialize(gameType);
                }
                else
                {
                    Debug.LogError("GameTypePrefab does not have a GameTypeButton component.");
                }
            }
            /*
            */
            if (ButtonSection != null && ButtonSection.TryGetComponent<VerticalLayoutGroup>(out var verticalLayoutGroup))
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(ButtonSection.GetComponent<RectTransform>());
                verticalLayoutGroup.enabled = false;
            }
            else
            {
                Debug.LogError("ButtonSection does not have a VerticalLayoutGroup component.");
            }
            if (ButtonSection != null && ButtonSection.TryGetComponent<ContentSizeFitter>(out var contentSizeFitter))
            {
                //contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
                contentSizeFitter.enabled = false;
            }
            else
            {
                Debug.LogError("ButtonSection does not have a ContentSizeFitter component.");
            }
        }
        else
        {
            Debug.LogError("GameTypeLibrary or GameTypePrefab is not assigned in the inspector.");
        }
        if (GameManager.Instance != null && GameManager.Instance.PlayerInput != null)
        {
            GameManager.Instance.PlayerInput.GameSelection.NavList.performed += OnNavigate;
            GameManager.Instance.PlayerInput.GameSelection.NavList.canceled += OnNavigateEnd;
        }
        else
        {
            Debug.LogError("GameManager instance or PlayerInput is not available.");
        }
        Navigate(0f);
    }
    public void StartNavigate(float value)
    {
        if (value != 0f)
        {
            //Navigate(value);
            NavigationValue = value;
        }
    }
    public void EndNavigate()
    {
        //NavigationValue = 0f;
    }

    //PC Version of navigation is handled in FixedUpdate, but this is not needed for mobile, so it is commented out.
    /*
    private void FixedUpdate()
    {
        if (NavigationValue != 0f)
        {
            TimeSinceLastNavigation += Time.fixedDeltaTime;
            if (TimeSinceLastNavigation >= DelayBetweenNavigations * CurrentDelayModifer)
            {
                Navigate(NavigationValue);
                TimeSinceLastNavigation = 0f;
                CurrentDelayModifer *= 0.8f;
                if (CurrentDelayModifer < 0.1f)
                {
                    CurrentDelayModifer = 0.1f;
                }
            }
        }
    }
    private void Navigate(float value)
    {
        ButtonSection.transform.GetChild(ButtonSection.transform.childCount / 2 - 1).localScale = Vector3.one;
        ButtonSection.transform.GetChild(ButtonSection.transform.childCount / 2).localScale = Vector3.one;
        ButtonSection.transform.GetChild(ButtonSection.transform.childCount / 2 + 1).localScale = Vector3.one;
        if (value > 0f)
        {
            ButtonSection.transform.GetChild(0).SetAsLastSibling();
        }
        else if (value < 0f)
        {
            ButtonSection.transform.GetChild(ButtonSection.transform.childCount-1).SetAsFirstSibling();
        }
        ButtonSection.transform.GetChild(ButtonSection.transform.childCount / 2 - 1).localScale = Vector3.one * 1.25f;
        ButtonSection.transform.GetChild(ButtonSection.transform.childCount / 2).localScale = Vector3.one * 1.5f;
        ButtonSection.transform.GetChild(ButtonSection.transform.childCount / 2 + 1).localScale = Vector3.one * 1.25f;
    }

    */

    // Mobile version of navigation
    private void Update()
    {
        if (NavigationValue != 0f)
        {
            Navigate(NavigationValue);
            NavigationValue -= NavigationValue * 0.05f;
            if (Mathf.Abs(NavigationValue) < 0.01f)
            {
                NavigationValue = 0f;
            }
            /*
            TimeSinceLastNavigation += Time.fixedDeltaTime;
            if (TimeSinceLastNavigation >= DelayBetweenNavigations * CurrentDelayModifer)
            {
                Navigate(NavigationValue);
                TimeSinceLastNavigation = 0f;
                CurrentDelayModifer *= 0.8f;
                if (CurrentDelayModifer < 0.1f)
                {
                    CurrentDelayModifer = 0.1f;
                }
                
            }
            */
        }
    }
    private void Navigate(float value)
    {
        float sectionTop = ButtonSection.GetComponent<RectTransform>().rect.yMin;
        float sectionBottom = ButtonSection.GetComponent<RectTransform>().rect.yMax;
        float sectionHeight = sectionBottom - sectionTop;
        float sectionMiddle = (sectionTop + sectionBottom) / 2f;

        foreach (Transform child in ButtonSection.transform)
        {
            child.position += new Vector3(0f, value, 0f);

            float childTop = child.GetComponent<RectTransform>().rect.yMin + child.localPosition.y;
            float childBottom = child.GetComponent<RectTransform>().rect.yMax + child.localPosition.y;
            float childLoc = child.localPosition.y;
            float childMiddle = (childTop + childBottom) / 2f;

            child.localScale = Vector3.one * Mathf.Clamp(1f - Mathf.Abs(childMiddle - sectionMiddle) / (sectionHeight / 2f), 0.5f, 1f);

            if (childLoc <= -sectionHeight / 2f)
            {
                child.localPosition = new Vector3(child.localPosition.x, child.localPosition.y + sectionHeight, child.localPosition.z);
            }
            else if (childLoc >= sectionHeight / 2f)
            {
                child.localPosition = new Vector3(child.localPosition.x, child.localPosition.y - sectionHeight, child.localPosition.z);
            }
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null && GameManager.Instance.PlayerInput != null)
        {
            GameManager.Instance.PlayerInput.GameSelection.NavList.performed -= OnNavigate;
            GameManager.Instance.PlayerInput.GameSelection.NavList.canceled -= OnNavigateEnd;
        }
    }
}
