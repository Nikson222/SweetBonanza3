using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bonanza.SlotMachine
{
    [CreateAssetMenu(fileName = "ElementsStyleDataBase", menuName = "Datas/ElementsStyleDataBase", order = 51)]
    public class ElementsStyleDataBase : ScriptableObject
    {
        public List<ElementStyle> ElementsStyles;

        public Sprite GetElementIcon(ElementTypes elementType)
        {
            foreach (var elementStyle in ElementsStyles)
            {
                if(elementStyle.ElementType == elementType)
                    return elementStyle.Icon;
            }

            return ElementsStyles.First().Icon;
        }
    }

    [Serializable]
    public class ElementStyle
    {
        public Sprite Icon;
        public ElementTypes ElementType;
    }
}