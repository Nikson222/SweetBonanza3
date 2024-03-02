using System.Collections.Generic;
using UnityEngine;

namespace Bonanza.SlotMachine
{
    public class SlotFactory
    {
        private const int COLUMN_COUNT = 6;
        private const int ROW_COUNT = 5;

        public SlotMachineController SlotMachineController { get; private set; }
        public SlotMachineModel SlotMachineModel { get; private set; }
        public SlotMachineView SlotMachineView { get; private set; }


        public SlotMachineController LoadSlotMachine(SlotMachineView slotMachineView,
            ElementsStyleDataBase elementsStyleDataBase)
        {
            SlotMachineModel = CreateSlotMachineModel();

            SlotMachineView = CreateSlotMachineView(slotMachineView);
            
            SlotMachineController = new SlotMachineController(SlotMachineModel, SlotMachineView, elementsStyleDataBase);

            return SlotMachineController;
        }

        private SlotMachineModel CreateSlotMachineModel()
        {
            List<CellModel> cellModels = new List<CellModel>();
            int modelsCount = COLUMN_COUNT * ROW_COUNT;

            for (int i = 0; i < modelsCount; i++)
            {
                CellModel cellModel = new CellModel((ElementTypes)0, i);
                cellModels.Add(cellModel);
            }

            return new SlotMachineModel(COLUMN_COUNT, ROW_COUNT, cellModels);
        }

        private SlotMachineView CreateSlotMachineView(SlotMachineView slotMachineView)
        {
            List<CellView> cellsViews = new List<CellView>();

            //SlotMachineView slotMachinePrefab = Resources.Load<SlotMachineView>("SlotMachine");
            CellView cellViewPrefab = Resources.Load<CellView>("CellView");

            //SlotMachineView slotMachineInstance = Object.Instantiate(slotMachinePrefab);
            //var slotMachineSpawnPosition = slotMachineInstance.transform.position;

            //slotMachineInstance.transform.SetParent(canvasTransform);
            //slotMachineInstance.transform.localScale = Vector3.one;
            //slotMachineInstance.transform.localPosition = Vector3.zero;
            
            var ySpacing = slotMachineView.YSpacing + slotMachineView.YCellSize/2;
            
            slotMachineView.transform.localPosition = 
                new Vector3(slotMachineView.transform.localPosition.x, 
                    slotMachineView.transform.localPosition.y - (ySpacing*ROW_COUNT) + slotMachineView.YSpacing, 0);
            
            for (int i = 0; i < COLUMN_COUNT; i++)
            {
                for (int j = 0; j < ROW_COUNT; j++)
                {
                    CellView cellViewInstance = Object.Instantiate(cellViewPrefab);

                    cellViewInstance.transform.SetParent(slotMachineView.CellContainer);
                    cellViewInstance.transform.localScale = Vector3.one;

                    var localCellPosition = cellViewInstance.transform.localPosition;
                    cellViewInstance.transform.localPosition = new Vector3(localCellPosition.x, localCellPosition.y, 0);

                    cellsViews.Add(cellViewInstance);
                }
            }

            slotMachineView.SetCellViews(cellsViews);

            return slotMachineView;
        }
    }
}