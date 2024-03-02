using System;
using System.Collections.Generic;
using System.Linq;

namespace Bonanza.SlotMachine
{
    public class SlotMachineModel
    {
        private int _columnCount;
        private int _rowCount;
        
        private List<CellModel> _cellsModels;

        public int ColumnCount => _columnCount;
        public int RowCount => _rowCount;
        
        public List<CellModel> CellsModels => _cellsModels;
        
        public event Action OnElementChanged;
        public event Action<List<CellModel>> OnWinsElementChanged;
        
        public SlotMachineModel(int columnCount, int rowCount, 
            IEnumerable<CellModel> cellsModels)
        {
            _columnCount = columnCount;
            _rowCount = rowCount;
            
            _cellsModels = cellsModels.ToList();
        }

        public void SetElementsIcons(List<ElementTypes> elementsTypes)
        {
            int SpritesCount = elementsTypes.Count;

            for (int i = 0; i < SpritesCount; i++)
                _cellsModels[i].SetElementType(elementsTypes[i]);
            
            OnElementChanged?.Invoke();
        }
        
        public void SetWinsElementsIcons(List<CellModel> models, List<ElementTypes> elementsTypes)
        {
            int counter = 0;
            foreach (var model in models)
            {
                model.SetElementType(elementsTypes[counter]);
                counter++;
            }
            
            OnWinsElementChanged?.Invoke(models);
        }
    }
}