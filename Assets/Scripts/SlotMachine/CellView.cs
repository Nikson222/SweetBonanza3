using UnityEngine;
using UnityEngine.UI;

namespace Bonanza.SlotMachine
{
    [RequireComponent(typeof(Image))]
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        
        public Image Image => _image;
        
        public void SetElementIcon(Sprite icon)
        {
            _image.enabled = true;
            _image.sprite = icon;
        }
    }
}