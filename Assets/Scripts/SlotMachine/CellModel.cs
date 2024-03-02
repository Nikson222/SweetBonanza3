using UnityEngine;

namespace Bonanza.SlotMachine
{
    public class CellModel
    {
        private ElementTypes _elementType;
        private int _cellIndex;
        
        public ElementTypes ElementType => _elementType;
        public int CellIndex => _cellIndex;


        public CellModel(ElementTypes elementType, int cellIndex)
        {
            _elementType = elementType;
            _cellIndex = cellIndex;
        }

        public void SetElementType(ElementTypes elementType)
        {
            _elementType = elementType;
        }
    }    
}

