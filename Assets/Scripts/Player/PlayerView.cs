using System;
using UnityEngine;
using UnityEngine.UI;

namespace Bonanza.Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Text _balanceText;
        [SerializeField] private Text _betText;

        [SerializeField] private Text _lastWinValueText;
        [SerializeField] private Image _lastWinIcon;
        [SerializeField] private GameObject _lastWinPanel;
        
        [Space(10)]
        [SerializeField] private Button _spinButton;
        [SerializeField] private Button _increaseBetButton;
        [SerializeField] private Button _decreaseBetButton;


        public void SetBalance(int balance) => _balanceText.text = balance.ToString();
        public void SetBet(int bet) => _betText.text = bet.ToString();
        
        public void AddListenerOnSpinButton(Action callback) => _spinButton.onClick.AddListener(() => callback());
        public void AddListenerOnIncreaseBetButton(Action callback) => _increaseBetButton.onClick.AddListener(() => callback());
        public void AddListenerOnDecreaseBetButton(Action callback) => _decreaseBetButton.onClick.AddListener(() => callback());

        public void SetLastWinIcon(Sprite icon)
        {
            //if(!_lastWinIcon.IsActive())
                //_lastWinPanel.SetActive(true);
            
            //_lastWinIcon.sprite = icon;
        }

        public void SetLastWinValue(int value)
        {
            //if(!_lastWinIcon.IsActive())
              //  _lastWinPanel.SetActive(true);
            
            //_lastWinValueText.text = value.ToString();
        }
    }
}