﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityUIDll
{
    #region Defines

    #endregion

    #region MainClases
    public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        //  defines
        public struct InputPack
        {
            public GameObject mtargetObject;

            public Sprite mbaseSprite;

            public Vector2 mposition;
            public Vector2 mpivot;
            public Vector2 manchor;

            public InputPack(in GameObject targetObject, in Sprite baseSprite, in Vector2 position, in Vector2 pivot, in Vector2 anchor)
            {
                mtargetObject = targetObject;

                mbaseSprite = baseSprite;

                mposition = position;
                mpivot = pivot;
                manchor = anchor;
            }
        };

        public enum EButtonClickType
        {
            ShortClick = 1,
            LongClick = 2,
            ShortAndLongClick = 3
        };
        public enum EUIButtonAnimationKind
        {
            Idle = 1,
            ButtonPushed = 2
        };
        public enum EUIButtonSpriteKind
        {
            Poped = 1,
            Pushed = 2,
            OnMouseOrTouchPoped = 3,
            OnMouseOrTouchPushed = 4
        };



        //  propertys
            //  target GmaeObject
        private GameObject mtargetObject;

            //  related with button
        private Button.ButtonClickedEvent mbuttonClickedEvent;
        private RectTransform mrectTransform;
        private EButtonClickType mbuttonClickType;
        private float mrepeatPeriod;
        private Tuple<UnityAction, UnityAction> mfunctions;
        private bool mbisPushed;

            //  related with button GameObject Image Component
        private Image mbuttonImage;
        private Dictionary<EUIButtonSpriteKind, Sprite> mbuttonSpriteTable;

            //  related with button GameObject Animation
        private Animator mbuttonAnimator;
        private Dictionary<EUIButtonAnimationKind, string> mbuttonAnimationTable;

            //  button state bool variable
        private bool mbisEnter;
        private bool mbisClick;
        private bool mbisDown;
        private bool mbisUp;
        private bool mbisExit;

            //  using default
        private bool mbusingDefault;



        //  methods
            //  constructor
        public UIButton(in InputPack inputPack, in UnityAction shortClickFunction)
        {
            mbuttonClickType = EButtonClickType.ShortClick;

            mtargetObject = inputPack.mtargetObject;

            mbuttonClickedEvent = new Button.ButtonClickedEvent();
            mrectTransform = mtargetObject.AddComponent<RectTransform>() as RectTransform;
            mrectTransform.position = new Vector3(inputPack.mposition.x, inputPack.mposition.y, 0.0f);
            mrectTransform.pivot = new Vector2(inputPack.mpivot.x, inputPack.mpivot.y);
            mrectTransform.anchorMin = new Vector2(inputPack.manchor.x, inputPack.manchor.y);
            mrectTransform.anchorMax = new Vector2(inputPack.manchor.x, inputPack.manchor.y);
            mrepeatPeriod = -1.0f;
            mfunctions = new Tuple<UnityAction, UnityAction>(shortClickFunction, null);

            mbuttonImage = mtargetObject.AddComponent<Image>();
            mbuttonImage.sprite = inputPack.mbaseSprite;
            mbuttonSpriteTable = new Dictionary<EUIButtonSpriteKind, Sprite>();

            mbuttonAnimator = mtargetObject.AddComponent<Animator>() as Animator;
            mbuttonAnimationTable = new Dictionary<EUIButtonAnimationKind, string>();
        }
        public UIButton(in InputPack inputPack, float repeatPeriod, in UnityAction longClickFunction)
        {
            mbuttonClickType = EButtonClickType.LongClick;

            mtargetObject = inputPack.mtargetObject;

            mbuttonClickedEvent = new Button.ButtonClickedEvent();
            mrectTransform = mtargetObject.AddComponent<RectTransform>() as RectTransform;
            mrectTransform.position = new Vector3(inputPack.mposition.x, inputPack.mposition.y, 0.0f);
            mrectTransform.pivot = new Vector2(inputPack.mpivot.x, inputPack.mpivot.y);
            mrectTransform.anchorMin = new Vector2(inputPack.manchor.x, inputPack.manchor.y);
            mrectTransform.anchorMax = new Vector2(inputPack.manchor.x, inputPack.manchor.y);
            mrepeatPeriod = repeatPeriod;
            mfunctions = new Tuple<UnityAction, UnityAction>(null, longClickFunction);

            mbuttonImage = mtargetObject.AddComponent<Image>();
            mbuttonImage.sprite = inputPack.mbaseSprite;
            mbuttonSpriteTable = new Dictionary<EUIButtonSpriteKind, Sprite>();

            mbuttonAnimator = mtargetObject.AddComponent<Animator>() as Animator;
            mbuttonAnimationTable = new Dictionary<EUIButtonAnimationKind, string>();
        }
        public UIButton(in InputPack inputPack, float repeatPeriod, in UnityAction shortClickFunction, in UnityAction longClickFunction)
        {
            mbuttonClickType = EButtonClickType.ShortAndLongClick;

            mtargetObject = inputPack.mtargetObject;

            mbuttonClickedEvent = new Button.ButtonClickedEvent();
            mrectTransform = mtargetObject.AddComponent<RectTransform>() as RectTransform;
            mrectTransform.position = new Vector3(inputPack.mposition.x, inputPack.mposition.y, 0.0f);
            mrectTransform.pivot = new Vector2(inputPack.mpivot.x, inputPack.mpivot.y);
            mrectTransform.anchorMin = new Vector2(inputPack.manchor.x, inputPack.manchor.y);
            mrectTransform.anchorMax = new Vector2(inputPack.manchor.x, inputPack.manchor.y);
            mrepeatPeriod = repeatPeriod;
            mfunctions = new Tuple<UnityAction, UnityAction>(shortClickFunction, longClickFunction);

            mbuttonImage = mtargetObject.AddComponent<Image>();
            mbuttonImage.sprite = inputPack.mbaseSprite;
            mbuttonSpriteTable = new Dictionary<EUIButtonSpriteKind, Sprite>();

            mbuttonAnimator = mtargetObject.AddComponent<Animator>() as Animator;
            mbuttonAnimationTable = new Dictionary<EUIButtonAnimationKind, string>();
        }

        //  interface method
        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            mbisEnter = true;
            mbisExit = false;

            if (mbusingDefault)
            {
                if (mbisPushed == true)
                {
                    mbuttonImage.sprite = mbuttonSpriteTable[EUIButtonSpriteKind.OnMouseOrTouchPushed];
                }
                else
                {
                    mbuttonImage.sprite = mbuttonSpriteTable[EUIButtonSpriteKind.OnMouseOrTouchPoped];
                }
            }
        }
        public void OnPointerClick(PointerEventData pointerEventData)
        {
            mbisPushed = true;

            mbisClick = true;
            mbisUp = false;

            if(mbusingDefault)
            {
                mbuttonImage.sprite = mbuttonSpriteTable[EUIButtonSpriteKind.Pushed];

                mbuttonAnimator.Play(mbuttonAnimationTable[EUIButtonAnimationKind.ButtonPushed]);
            }
        }
        public void OnPointerDown(PointerEventData pointerEventData)
        {
            mbisDown = true;
        }
        public void OnPointerUp(PointerEventData pointerEventData)
        {
            mbisPushed = false;

            mbisUp = true;
            mbisClick = false;
            mbisDown = false;

            if(mbusingDefault)
            {
                mbuttonAnimator.Play(mbuttonAnimationTable[EUIButtonAnimationKind.Idle]);
            }
        }
        public void OnPointerExit(PointerEventData pointerEventData)
        {
            mbisExit = true;
            mbisEnter = false;

            if(mbusingDefault)
            {
                if (mbisPushed == true)
                {
                    mbuttonImage.sprite = mbuttonSpriteTable[EUIButtonSpriteKind.Pushed];
                }
                else
                {
                    mbuttonImage.sprite = mbuttonSpriteTable[EUIButtonSpriteKind.Poped];
                }
            }
        }

            //  get, set method
        public GameObject targetObject
        {
            get => mtargetObject;
        }
        public Button.ButtonClickedEvent buttonClickedEvent
        {
            get => mbuttonClickedEvent;
        }
        public RectTransform rectTransform
        {
            get => mrectTransform;
        }
        public EButtonClickType buttonClickType
        {
            get => mbuttonClickType;
        }
        public float repeatPeriod
        {
            get => mrepeatPeriod;
            set => mrepeatPeriod = value;
        }
        public Tuple<UnityAction, UnityAction> functions
        {
            get => mfunctions;
            set => mfunctions = value;
        }
        public bool isPushed
        {
            get => mbisPushed;
        }
        public Image buttonImage
        {
            get => mbuttonImage;
        }
        public Dictionary<EUIButtonSpriteKind, Sprite> buttonSpriteTable
        {
            get => mbuttonSpriteTable;
        }
        public Animator buttonAnimator
        {
            get => mbuttonAnimator;
        }
        public Dictionary<EUIButtonAnimationKind, string> buttonAnimationTable
        {
            get => mbuttonAnimationTable;
            set => mbuttonAnimationTable = value;
        }
        public bool isEnter
        {
            get => mbisEnter;
        }
        public bool isClick
        {
            get => mbisClick;
        }
        public bool isDown
        {
            get => mbisDown;
        }
        public bool isUp
        {
            get => mbisUp;
        }
        public bool isExit
        {
            get => mbisExit;
        }
        public bool usingDefault
        {
            get => mbusingDefault;
            set => mbusingDefault = value;
        }

            //  behavior method
        public void SwitchModeToShortClick(in UnityAction shortClickFunction)
        {
            mbuttonClickType = EButtonClickType.ShortClick;
            repeatPeriod = -1.0f;

            mfunctions = new Tuple<UnityAction, UnityAction>(shortClickFunction, null);
        }
        public void SwitchModeToLongClick(float repeatPeriod, in UnityAction longClickFunction)
        {
            mbuttonClickType = EButtonClickType.LongClick;
            mrepeatPeriod = repeatPeriod;

            mfunctions = new Tuple<UnityAction, UnityAction>(null, longClickFunction);
        }
        public void SwitchModeToShortAndLongClick(float repeatPeriod, in UnityAction shortClickFunction, in UnityAction longClickFunction)
        {
            mbuttonClickType = EButtonClickType.ShortAndLongClick;
            mrepeatPeriod = repeatPeriod;

            mfunctions = new Tuple<UnityAction, UnityAction>(shortClickFunction, longClickFunction);
        }
        public void SetButtonAnimation(string animationStateName, in EUIButtonAnimationKind buttonAnimationKind)
        {
            if(mbuttonAnimationTable.ContainsKey(buttonAnimationKind) == true)
            {
                mbuttonAnimationTable.Remove(buttonAnimationKind);
                mbuttonAnimationTable.Add(buttonAnimationKind, animationStateName);
            }
            else
            {
                mbuttonAnimationTable.Add(buttonAnimationKind, animationStateName);
            }
        }
        public void SetButtonSprite(in Sprite sprite, in EUIButtonSpriteKind buttonSpriteKind)
        {
            if(mbuttonSpriteTable.ContainsKey(buttonSpriteKind) == true)
            {
                mbuttonSpriteTable.Remove(buttonSpriteKind);
                mbuttonSpriteTable.Add(buttonSpriteKind, sprite);
            }
            else
            {
                mbuttonSpriteTable.Add(buttonSpriteKind, sprite);
            }
        }
    }
    #endregion
}