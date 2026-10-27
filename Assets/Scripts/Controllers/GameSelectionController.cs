using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameSelectionController : MonoBehaviour
{
    [SerializeField] SGOGameTypeLibrary GameTypeLibrary;
    [SerializeField] GameObject GameTypePrefab;
    [SerializeField] GameObject ButtonSection;




    private void OnNavigate(InputAction.CallbackContext context) => StartNavigate(context.ReadValue<float>());
    private void OnNavigateEnd(InputAction.CallbackContext context) => EndNavigate();
    private void OnSelect(InputAction.CallbackContext context) => Select();
    float NavigationValue = 0f;
    float DelayBetweenNavigations = 0.5f;
    float CurrentDelayModifer = 1f;
    float TimeSinceLastNavigation = 0f;

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
        }
        else
        {
            Debug.LogError("GameTypeLibrary or GameTypePrefab is not assigned in the inspector.");
        }
        if (GameManager.Instance != null && GameManager.Instance.PlayerInput != null)
        {
            GameManager.Instance.PlayerInput.GameSelection.NavList.performed += OnNavigate;
            GameManager.Instance.PlayerInput.GameSelection.NavList.canceled += OnNavigateEnd;
            GameManager.Instance.PlayerInput.GameSelection.Select.performed += OnSelect;
        }
        else
        {
            Debug.LogError("GameManager instance or PlayerInput is not available.");
        }
        ButtonSection.transform.GetChild(ButtonSection.transform.childCount / 2 - 1).localScale = Vector3.one * 1.25f;
        ButtonSection.transform.GetChild(ButtonSection.transform.childCount / 2).localScale = Vector3.one * 1.5f;
        ButtonSection.transform.GetChild(ButtonSection.transform.childCount / 2 + 1).localScale = Vector3.one * 1.25f;
    }
    private void Select()
    {
        if (ButtonSection.transform.GetChild(ButtonSection.transform.childCount / 2).TryGetComponent<Button>(out var button))
        {
            button.onClick.Invoke();
        }
    }
    public void StartNavigate(float value)
    {
        if (value != 0f)
        {
            Navigate(value);
            NavigationValue = value;
        }
    }
    public void EndNavigate()
    {
        NavigationValue = 0f;
        CurrentDelayModifer = 1f;
        TimeSinceLastNavigation = 0f;
    }
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




    private void OnDestroy()
    {
        if (GameManager.Instance != null && GameManager.Instance.PlayerInput != null)
        {
            GameManager.Instance.PlayerInput.GameSelection.NavList.performed -= OnNavigate;
            GameManager.Instance.PlayerInput.GameSelection.NavList.canceled -= OnNavigateEnd;
            GameManager.Instance.PlayerInput.GameSelection.Select.performed -= OnSelect;
        }
    }
}
