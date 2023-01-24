using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button),typeof(Image))]
public class ToggleSetButton : MonoBehaviour
{
    [SerializeField]
    private Sprite _selectedSprite, _normalSprite;

    private Image _myImage;

    private void Awake()
    {
        _myImage = GetComponent<Image>();
    }

    public void Select()
    {
        _myImage.sprite = _selectedSprite;
    }
    public void Deselect()
    {
        _myImage.sprite = _normalSprite;
    }
}
