using System;
using System.Collections.Generic;
using AdvancedPS.Core.System;
using UnityEngine;
using UnityEngine.UI;

namespace AdvancedPS.Core.Examples
{
    public class EasingShowcase : MonoBehaviour
    {
        [SerializeField] private DemoPopup _demoPopupPrefab;
        [SerializeField] private InfoBlock _infoBlockPrefab;
        
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Transform _popupsRoot;
        [SerializeField] private Transform _infoRoot;

        [SerializeField] private Button _buttonShow;
        [SerializeField] private Button _buttonHide;

        private List<DemoPopup> _popups;
        
        private void Awake()
        {
            _buttonShow.onClick.AddListener(() =>
            {
                AdvancedPopupSystem.PopupShow(PopupLayerEnum.HUB);
                _buttonShow.interactable = false;
                _buttonHide.interactable = true;
            });
            
            _buttonHide.interactable = false;
            _buttonHide.onClick.AddListener(() =>
            {
                AdvancedPopupSystem.PopupHide(PopupLayerEnum.HUB);
                _buttonShow.interactable = true;
                _buttonHide.interactable = false;
            });
        }

        private void Start()
        {
            _popups = GeneratePopups();
        }

        private List<DemoPopup> GeneratePopups()
        {
            List<DemoPopup> popups = new List<DemoPopup>();
            RectTransform canvasRectTransform = _canvas.GetComponent<RectTransform>();
            
            float canvasWidth = canvasRectTransform.rect.width;
            float canvasHeight = canvasRectTransform.rect.height;
            int easingTypesCount = Enum.GetValues(typeof(EasingType)).Length;
            
            float popupWidth = canvasWidth / (easingTypesCount + (easingTypesCount - 1) / 2.0f);
            float spacing = popupWidth / 2;
            
            float initialY = -250;
            float step = 25;
            int direction = 1;

            for (int i = 0; i < easingTypesCount; i++)
            {
                DemoPopup popup = Instantiate(_demoPopupPrefab, _popupsRoot);
                popup.RootTransform.sizeDelta = new Vector2(popupWidth, popup.RootTransform.sizeDelta.y);
                
                float posX = i * (popupWidth + spacing) - (canvasWidth / 2) + (popupWidth / 2);
                float posY = -canvasHeight / 2 - popup.RootTransform.sizeDelta.y / 2;
                popup.RootTransform.anchoredPosition = new Vector2(posX, posY);
                
                popup.Init();
                popup.SetCachedDisplay<SlideDisplay>(new SlideSettings
                {
                    Easing = (EasingType)i,
                    TargetPosition = new Vector3(posX, 0, 0)
                });
                popups.Add(popup);
                
                InfoBlock infoBlock = Instantiate(_infoBlockPrefab, _infoRoot);
                RectTransform infoRect = infoBlock.GetComponent<RectTransform>();
                infoRect.anchoredPosition = new Vector2(posX, initialY);

                infoBlock.SetText(Enum.GetName(typeof(EasingType), (EasingType)i));

                if (i % 3 == 0 && i != 0)
                {
                    direction *= -1;
                }
                initialY += direction * step;
            }
            
            return popups;
        }
    }
}