using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bonanza.SlotMachine
{
    public class SlotMachineController
    {
        private SlotMachineModel _slotMachineModel;
        private SlotMachineView _slotMachineView;

        private ElementsStyleDataBase _elementsStyleDataBase;

        private bool _isSpinning = false;
        private bool _isWin = false;
        private bool _isSlotInited = false;

        private int _winCounter = 0;
        
        private SelectedElement _lastSelectedElement;

        public Action<int> OnSpinEnded;
        public Action<Sprite> OnWin;

        public SlotMachineController(SlotMachineModel slotMachineModel, SlotMachineView slotMachineView,
            ElementsStyleDataBase elementsStyleDataBase)
        {
            _slotMachineModel = slotMachineModel;
            _slotMachineView = slotMachineView;
            _elementsStyleDataBase = elementsStyleDataBase;

            _slotMachineModel.OnElementChanged += OnElementsChanged;
            _slotMachineModel.OnWinsElementChanged += OnWinsElementsChanged;
        }

        private void OnWinsElementsChanged(List<CellModel> elementsModels)
        {
            List<Sprite> elementsIcons = new List<Sprite>();
            List<int> elementsIndexes = new List<int>();
            
            foreach (var elementsModel in elementsModels)
            {
                Sprite icon = _elementsStyleDataBase.GetElementIcon(elementsModel.ElementType);
                elementsIcons.Add(icon);
                elementsIndexes.Add(elementsModel.CellIndex);
            }

            var seq = _slotMachineView.SetWinsElementsIcons(elementsIndexes, elementsIcons);

            seq.OnComplete(() =>
            {
                if (_isSlotInited)
                {
                    if (IsWin())
                    {
                        _isWin = true;
                        _winCounter++;
                        
                        _slotMachineView.StartSlotCoroutine(SpinSlotAgain());
                    }
                    else
                    {
                        _isWin = false;
                        _isSpinning = false;
                        OnSpinEnded?.Invoke(_winCounter);

                        if (_winCounter > 0)
                            OnWin?.Invoke(
                                _elementsStyleDataBase.GetElementIcon(_lastSelectedElement.Value));
                        
                        _winCounter = 0;
                    }
                }
                else
                {
                    _isSlotInited = true;
                    _isSpinning = false;
                    _winCounter = 0;
                    
                    OnSpinEnded?.Invoke(_winCounter);
                }
            });
        }


        public event Action OnSpinStarted;

        private void OnElementsChanged()
        {
            int myEnumMemberCount = Enum.GetNames(typeof(ElementTypes)).Length;
            int slotCellsCount = _slotMachineModel.ColumnCount * _slotMachineModel.RowCount;

            List<Sprite> elementsIcons = new List<Sprite>();

            for (int i = 0; i < slotCellsCount; i++)
            {
                Sprite icon = _elementsStyleDataBase.GetElementIcon(_slotMachineModel.CellsModels[i].ElementType);
                elementsIcons.Add(icon);
            }

            var seq = _slotMachineView.SetElementsIcons(elementsIcons);

            seq.OnComplete(() =>
            {
                if (_isSlotInited)
                {
                    if (IsWin())
                    {
                        _isWin = true;
                        _winCounter++;

                        _slotMachineView.StartSlotCoroutine(SpinSlotAgain());

                        Debug.Log($"{_lastSelectedElement.Value} - {_lastSelectedElement.Count}");
                    }
                    else
                    {
                        _isWin = false;
                        _isSpinning = false;
                        OnSpinEnded?.Invoke(_winCounter);

                        if (_winCounter > 0)
                            OnWin?.Invoke(_elementsStyleDataBase.GetElementIcon(_lastSelectedElement.Value));
                        
                        _winCounter = 0;
                    }
                }
                else
                {
                    _isSlotInited = true;
                    _isSpinning = false;
                    _winCounter = 0;
                    
                    OnSpinEnded?.Invoke(_winCounter);
                }
            });
        }

        public void SpinSlot()
        {
            if (_isSpinning || _isWin)
                return;

            _isSpinning = true;

            FillCells();
        }

        public void FillCells()
        {
            OnSpinStarted?.Invoke();

            int myEnumMemberCount = Enum.GetNames(typeof(ElementTypes)).Length;
            int slotCellsCount = _slotMachineModel.ColumnCount * _slotMachineModel.RowCount;

            List<ElementTypes> elementTypes = new List<ElementTypes>();
            
            for (int i = 0; i < slotCellsCount; i++)
            {
                ElementTypes elementType = (ElementTypes)Random.Range(0, myEnumMemberCount - 1);
                elementTypes.Add(elementType);
            }
            
            _slotMachineModel.SetElementsIcons(elementTypes);
        }


        public void FillWinsCells()
        {
            OnSpinStarted?.Invoke();
            
            int myEnumMemberCount = Enum.GetNames(typeof(ElementTypes)).Length;

            List<ElementTypes> elementTypes = new List<ElementTypes>();
            List<CellModel> cellsModels = new List<CellModel>();

            int indexCounter = 0;
            
            foreach (var model in _slotMachineModel.CellsModels)
            {
                if (model.ElementType == _lastSelectedElement.Value)
                {
                    ElementTypes elementType = (ElementTypes)Random.Range(0, myEnumMemberCount - 1);
                    elementTypes.Add(elementType);

                    cellsModels.Add(model);
                }

                indexCounter++;
            }
            
            _slotMachineModel.SetWinsElementsIcons(cellsModels, elementTypes);
        }
        
        private IEnumerator SpinSlotAgain()
        {
            yield return new WaitForSeconds(0.5f);

            FillWinsCells();
        }

        public bool IsWin()
        {
            var maxElement = GetSomeMaxElementCount();

            if (maxElement.Count >= 8)
            {
                _lastSelectedElement = maxElement;
                return true;
            }
            else
                return false;
        }

        private SelectedElement GetSomeMaxElementCount()
        {
            var q = _slotMachineModel.CellsModels.GroupBy(x => x.ElementType)
                .Select(g => new SelectedElement { Value = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count).FirstOrDefault();

            return q;
        }

        private struct SelectedElement
        {
            public ElementTypes Value { get; set; }
            public int Count { get; set; }


            public SelectedElement(ElementTypes value, int count)
            {
                Value = value;
                Count = count;
            }
        }
    }
}