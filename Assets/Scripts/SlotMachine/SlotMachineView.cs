using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Bonanza.SlotMachine
{
    public class SlotMachineView : MonoBehaviour
    {
        [SerializeField] private Transform _cellContainer;

        private float _ySpacing;
        private float _yCellSize;
        
        private List<CellView> _cellViews;
        public Transform CellContainer => _cellContainer;
        
        public float YSpacing => _ySpacing;
        public float YCellSize => _yCellSize;
        private void Awake()
        {
            var gridLayoutGroup = GetComponent<GridLayoutGroup>();
            _ySpacing = gridLayoutGroup.spacing.y;
            _yCellSize = gridLayoutGroup.cellSize.y;
        }

        public void SetCellViews(List<CellView> cellViews)
        {
            _cellViews = cellViews;
        }

        public Sequence SetElementsIcons(List<Sprite> elementsIcons)
        {
            Sequence seq = DOTween.Sequence();

            int SpritesCount = elementsIcons.Count;

            for (int i = 0; i < SpritesCount; i++)
            {
                seq = SimpleAnimateSlot(i, elementsIcons[i]);
            }

            return seq;
        }
        
        public Sequence SetWinsElementsIcons(List<int> cellIndexes, List<Sprite> elementsIcons)
        {
            Sequence seq = DOTween.Sequence();
            int counter = 0;

            foreach (var element in cellIndexes)
            {
                seq = AnimateWinSlot(element, elementsIcons[counter]);
                counter++;
            }

            return seq;
        }

        
        private Sequence AnimateSlot(int index, Sprite icon)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(_cellViews[index].transform.DOLocalRotate(new Vector3(0, 90, 0), 0.25f).SetEase(Ease.InSine)
                .OnComplete(
                () =>
                {
                    _cellViews[index].Image.enabled = false;
                }));

            seq.Append(_cellViews[index].transform.DOLocalRotate(new Vector3(0, 180, 0), 0.25f));

            seq.Append(_cellViews[index].transform.DOLocalRotate(new Vector3(0, 90, 0), 0.25f).SetEase(Ease.OutSine)
                .OnComplete((() =>
                {
                    SetElementIcon(index, icon);

                    _cellViews[index].Image.enabled = true;
                    _cellViews[index].transform.DOLocalRotate(new Vector3(0, 0, 0), 0.25f);
                })));

            return seq;
        }

        private Sequence SimpleAnimateSlot(int index, Sprite icon)
        {
            Sequence startSeq = DOTween.Sequence();
            Sequence endSeq = DOTween.Sequence();

            endSeq.Pause();
            
            startSeq.Append(_cellViews[index].Image.transform.DOScale(new Vector3(0, 0, 0), 0.25f).SetEase(Ease.InSine)).
                OnComplete(() => { SetElementIcon(index, icon);
                endSeq.Play();
            });;

            endSeq.Append(_cellViews[index].Image.transform.DOScale(new Vector3(.5f, .5f, .5f), 0.5f).SetEase(Ease.InSine));
            
            endSeq.Append(_cellViews[index].Image.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f)).SetEase(Ease.InSine);

            endSeq.Append(_cellViews[index].Image.transform.DOScale(new Vector3(1, 1, 1), 0.25f));
            endSeq.Append(_cellViews[index].Image.transform.DOScale(new Vector3(1, 1, 1), 0.55f));

            return endSeq;
        }
        
        private Sequence AnimateWinSlot(int index, Sprite icon)
        {
            Sequence startSeq = DOTween.Sequence();
            Sequence endSeq = DOTween.Sequence();

            endSeq.Pause();
            
            startSeq.Append(_cellViews[index].Image.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).SetEase(Ease.InSine));

            startSeq.Append(_cellViews[index].Image.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 1f).SetEase(Ease.InSine));
            
            startSeq.Append(_cellViews[index].Image.transform.DOScale(new Vector3(0, 0, 0), 0.25f))
                .OnComplete(() => { SetElementIcon(index, icon);
                    endSeq.Play();
                });

            endSeq.Append(_cellViews[index].Image.transform.DOScale(new Vector3(1, 1, 1), 0.25f));
            endSeq.Append(_cellViews[index].Image.transform.DOScale(new Vector3(1, 1, 1), 0.55f));
            

            return endSeq;
        }

        
        public void SetElementIcon(int index, Sprite icon) => _cellViews[index].SetElementIcon(icon);
        
        public void StartSlotCoroutine(IEnumerator coroutine) => StartCoroutine(coroutine);
    }
}