using UnityEngine;
using Bonanza.SlotMachine;
using Bonanza.Player;
using UnityEngine.UI;

namespace Bonanza
{
    public class Bootstraper : MonoBehaviour
    {
        [Header("Slots Settings")]
        [SerializeField] private ElementsStyleDataBase _elementsStyleDataBase;
        private SlotFactory _slotFactory;
        private SlotMachineController _slotMachineController;
        [SerializeField] private SlotMachineView _slotMachineView;
        
        [Header("Player Start Settings")]
        [SerializeField] private int _startBalance;
        [SerializeField] private int _startBetIndex;
        
        [SerializeField] private PlayerView _playerView;
        private PlayerModel _playerModel;
        private PlayerController _playerController;

        private void Awake()
        {
            _slotFactory = new SlotFactory();
            
            _playerModel = new PlayerModel(_startBetIndex, _startBalance);
            _playerController = new PlayerController(_playerModel, _playerView);
            
            _slotMachineController = _slotFactory.LoadSlotMachine(_slotMachineView, _elementsStyleDataBase);
        }

        private void Start()
        {
            _slotMachineController.OnSpinStarted += _playerModel.StartWaitResultSpinning;
            _slotMachineController.OnWin += _playerView.SetLastWinIcon;
            _playerModel.OnSuccessfulSpin += _slotMachineController.SpinSlot;
            _playerModel.OnWin += _playerView.SetLastWinValue;
            _slotMachineController.OnSpinEnded += _playerModel.EndWaitResultSpinning;
            
            _slotMachineController.FillCells();
        }
    }
}