using System;
using System.Collections.Generic;
using UnityEditorInternal;
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

            public InputPack(in GameObject targetObject, in Sprite PopedSprite, in Vector2 position, in Vector2 pivot, in Vector2 anchor)
            {
                mtargetObject = targetObject;

                mbaseSprite = PopedSprite;

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
        [SerializeField]
        private GameObject mtargetObject;

        //  related with button
        private Button.ButtonClickedEvent mbuttonClickedEventForShort;
        private Button.ButtonClickedEvent mbuttonClickedEventForLong;
        private RectTransform mrectTransform;
        private EButtonClickType mbuttonClickType;
        private float mrepeatPeriod;
        private Tuple<UnityAction, UnityAction> mfunctions;
        private bool mbisPushed;

        //  related with button GameObject Image Component
        private Image mbuttonImage;
        [SerializeField]
        private Dictionary<EUIButtonSpriteKind, Sprite> mbuttonSpriteTable;

        //  related with button GameObject Animation
        private Animator mbuttonAnimator;
        [SerializeField]
        private Dictionary<EUIButtonAnimationKind, string> mbuttonAnimationTable;

        //  button state bool variable
        private bool mbisEnter;
        private bool mbisClick;
        private bool mbisDown;

        //  using default
        private bool mbusingDefault;



        //  methods
            //  constructor
        public void ConstructDefault(in InputPack inputPack, in UnityAction shortClickFunction)
        {
            mbuttonClickType = EButtonClickType.ShortClick;

            mtargetObject = inputPack.mtargetObject;

            mbuttonClickedEventForShort = new Button.ButtonClickedEvent();
            mbuttonClickedEventForLong = new Button.ButtonClickedEvent();
            mrectTransform = mtargetObject.AddComponent<RectTransform>() as RectTransform;
            mrectTransform.pivot = new Vector2(inputPack.mpivot.x, inputPack.mpivot.y);
            mrectTransform.anchorMin = new Vector2(inputPack.manchor.x, inputPack.manchor.y);
            mrectTransform.anchorMax = new Vector2(inputPack.manchor.x, inputPack.manchor.y);
            mrectTransform.anchoredPosition = new Vector3(inputPack.mposition.x, inputPack.mposition.y, 0.0f);
            mrepeatPeriod = -1.0f;
            mfunctions = new Tuple<UnityAction, UnityAction>(shortClickFunction, null);

            mbuttonClickedEventForShort.AddListener(shortClickFunction);

            mbuttonImage = mtargetObject.AddComponent<Image>();
            mbuttonImage.sprite = inputPack.mbaseSprite;
            mbuttonSpriteTable = new Dictionary<EUIButtonSpriteKind, Sprite>();
            mbuttonSpriteTable.Add(EUIButtonSpriteKind.Poped, inputPack.mbaseSprite);

            mbuttonAnimator = mtargetObject.AddComponent<Animator>();
            mbuttonAnimationTable = new Dictionary<EUIButtonAnimationKind, string>();
            mbuttonAnimator.enabled = false;

            usingDefault = true;
        }
        public void ConstructDefault(in InputPack inputPack, float repeatPeriod, in UnityAction longClickFunction)
        {
            mbuttonClickType = EButtonClickType.LongClick;

            mtargetObject = inputPack.mtargetObject;

            mbuttonClickedEventForShort = new Button.ButtonClickedEvent();
            mbuttonClickedEventForLong = new Button.ButtonClickedEvent();
            mrectTransform = mtargetObject.AddComponent<RectTransform>() as RectTransform;
            mrectTransform.pivot = new Vector2(inputPack.mpivot.x, inputPack.mpivot.y);
            mrectTransform.anchorMin = new Vector2(inputPack.manchor.x, inputPack.manchor.y);
            mrectTransform.anchorMax = new Vector2(inputPack.manchor.x, inputPack.manchor.y);
            mrectTransform.anchoredPosition = new Vector3(inputPack.mposition.x, inputPack.mposition.y, 0.0f);
            mrepeatPeriod = repeatPeriod;
            mfunctions = new Tuple<UnityAction, UnityAction>(null, longClickFunction);

            mbuttonClickedEventForLong.AddListener(longClickFunction);

            mbuttonImage = mtargetObject.AddComponent<Image>();
            mbuttonImage.sprite = inputPack.mbaseSprite;
            mbuttonSpriteTable = new Dictionary<EUIButtonSpriteKind, Sprite>();
            mbuttonSpriteTable.Add(EUIButtonSpriteKind.Poped, inputPack.mbaseSprite);

            mbuttonAnimator = mtargetObject.AddComponent<Animator>();
            mbuttonAnimationTable = new Dictionary<EUIButtonAnimationKind, string>();
            mbuttonAnimator.enabled = false;

            usingDefault = true;
        }
        public void ConstructDefault(in InputPack inputPack, float repeatPeriod, in UnityAction shortClickFunction, in UnityAction longClickFunction)
        {
            mbuttonClickType = EButtonClickType.ShortAndLongClick;

            mtargetObject = inputPack.mtargetObject;

            mbuttonClickedEventForShort = new Button.ButtonClickedEvent();
            mbuttonClickedEventForLong = new Button.ButtonClickedEvent();
            mrectTransform = mtargetObject.AddComponent<RectTransform>() as RectTransform;
            mrectTransform.pivot = new Vector2(inputPack.mpivot.x, inputPack.mpivot.y);
            mrectTransform.anchorMin = new Vector2(inputPack.manchor.x, inputPack.manchor.y);
            mrectTransform.anchorMax = new Vector2(inputPack.manchor.x, inputPack.manchor.y);
            mrectTransform.anchoredPosition = new Vector3(inputPack.mposition.x, inputPack.mposition.y, 0.0f);
            mrepeatPeriod = repeatPeriod;
            mfunctions = new Tuple<UnityAction, UnityAction>(shortClickFunction, longClickFunction);

            mbuttonClickedEventForShort.AddListener(shortClickFunction);
            mbuttonClickedEventForLong.AddListener(longClickFunction);

            mbuttonImage = mtargetObject.AddComponent<Image>();
            mbuttonImage.sprite = inputPack.mbaseSprite;
            mbuttonSpriteTable = new Dictionary<EUIButtonSpriteKind, Sprite>();
            mbuttonSpriteTable.Add(EUIButtonSpriteKind.Poped, inputPack.mbaseSprite);

            mbuttonAnimator = mtargetObject.AddComponent<Animator>();
            mbuttonAnimationTable = new Dictionary<EUIButtonAnimationKind, string>();
            mbuttonAnimator.enabled = false;

            usingDefault = true;
        }

            //  interface method
        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            mbisEnter = true;

            if (mbusingDefault)
            {
                if (mbisPushed == true)
                {
                    if(mbuttonSpriteTable.ContainsKey(EUIButtonSpriteKind.OnMouseOrTouchPushed))
                    {
                        mbuttonImage.sprite = mbuttonSpriteTable[EUIButtonSpriteKind.OnMouseOrTouchPushed];
                    }
                }
                else
                {
                    if(mbuttonSpriteTable.ContainsKey(EUIButtonSpriteKind.OnMouseOrTouchPoped))
                    {
                        mbuttonImage.sprite = mbuttonSpriteTable[EUIButtonSpriteKind.OnMouseOrTouchPoped];
                    }
                }
            }
        }
        public void OnPointerClick(PointerEventData pointerEventData)
        {
            mbisClick = true;

            if (mbusingDefault)
            {
                FunctionForShortClick();
            }
        }
        public void OnPointerDown(PointerEventData pointerEventData)
        {
            if(mbisPushed)
            {
                mbisPushed = false;
            }
            else
            {
                mbisPushed = true;
            }
            mbisDown = true;

            if (mbusingDefault)
            {
                if (mbisPushed && mbuttonSpriteTable.ContainsKey(EUIButtonSpriteKind.OnMouseOrTouchPushed))
                {
                    mbuttonImage.sprite = mbuttonSpriteTable[EUIButtonSpriteKind.OnMouseOrTouchPushed];
                }
                else if (mbuttonSpriteTable.ContainsKey(EUIButtonSpriteKind.OnMouseOrTouchPoped))
                {
                    mbuttonImage.sprite = mbuttonSpriteTable[EUIButtonSpriteKind.OnMouseOrTouchPoped];
                }

                mbuttonAnimator.enabled = true;
                if (mbisPushed && mbuttonAnimationTable.ContainsKey(EUIButtonAnimationKind.ButtonPushed))
                {
                    mbuttonAnimator.Play(mbuttonAnimationTable[EUIButtonAnimationKind.ButtonPushed]);
                    Invoke("SetAnimatorEnableFalse", mbuttonAnimator.GetCurrentAnimatorStateInfo(0).length);
                }
                else if(mbuttonAnimationTable.ContainsKey(EUIButtonAnimationKind.Idle))
                {
                    mbuttonAnimator.Play(mbuttonAnimationTable[EUIButtonAnimationKind.Idle]);
                    Invoke("SetAnimatorEnableFalse", mbuttonAnimator.GetCurrentAnimatorStateInfo(0).length);
                }

                if (mbuttonClickType == EButtonClickType.LongClick || mbuttonClickType == EButtonClickType.ShortAndLongClick)
                {
                    InvokeRepeating("FunctionForLongClick", 0.0f, mrepeatPeriod);
                }
            }
        }
        public void OnPointerUp(PointerEventData pointerEventData)
        {
            mbisClick = false;
            mbisDown = false;

            if (mbusingDefault)
            {
                CancelInvoke("FunctionForLongClick");
            }
        }
        public void OnPointerExit(PointerEventData pointerEventData)
        {
            mbisEnter = false;

            if (mbusingDefault)
            {
                if (mbisPushed == true)
                {
                    if(mbuttonSpriteTable.ContainsKey(EUIButtonSpriteKind.Pushed))
                    {
                        mbuttonImage.sprite = mbuttonSpriteTable[EUIButtonSpriteKind.Pushed];
                    }
                }
                else
                {
                    if(mbuttonSpriteTable.ContainsKey(EUIButtonSpriteKind.Poped))
                    {
                        mbuttonImage.sprite = mbuttonSpriteTable[EUIButtonSpriteKind.Poped];
                    }
                }
            }
        }

            //  get, set method
        public GameObject targetObject
        {
            get => mtargetObject;
        }
        public Button.ButtonClickedEvent buttonClickedEventForshort
        {
            get => mbuttonClickedEventForShort;
        }
        public Button.ButtonClickedEvent buttonClickedEventForLong
        {
            get => mbuttonClickedEventForLong;
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
        public void SetButtonAnimationState(string animationStateName, in EUIButtonAnimationKind buttonAnimationKind)
        {
            if (mbuttonAnimationTable.ContainsKey(buttonAnimationKind) == true)
            {
                mbuttonAnimationTable.Remove(buttonAnimationKind);
                mbuttonAnimationTable.Add(buttonAnimationKind, animationStateName);
            }
            else
            {
                mbuttonAnimationTable.Add(buttonAnimationKind, animationStateName);
            }
        }
        public void SetButtonAnimationController(in RuntimeAnimatorController animatorController)
        {
            mbuttonAnimator.runtimeAnimatorController = animatorController;
        }
        public void SetButtonSprite(in Sprite sprite, in EUIButtonSpriteKind buttonSpriteKind)
        {
            if (mbuttonSpriteTable.ContainsKey(buttonSpriteKind) == true)
            {
                mbuttonSpriteTable.Remove(buttonSpriteKind);
                mbuttonSpriteTable.Add(buttonSpriteKind, sprite);
            }
            else
            {
                mbuttonSpriteTable.Add(buttonSpriteKind, sprite);
            }
        }

            //  private method
        private void FunctionForShortClick()
        {
            mbuttonClickedEventForShort.Invoke();
        }
        private void FunctionForLongClick()
        {
            mbuttonClickedEventForLong.Invoke();
        }
        private void SetAnimatorEnableFalse()
        {
            mbuttonAnimator.enabled = false;

            if (mbisPushed && mbuttonSpriteTable.ContainsKey(EUIButtonSpriteKind.OnMouseOrTouchPushed))
            {
                mbuttonImage.sprite = mbuttonSpriteTable[EUIButtonSpriteKind.OnMouseOrTouchPushed];
            }
            else if (mbuttonSpriteTable.ContainsKey(EUIButtonSpriteKind.OnMouseOrTouchPoped))
            {
                mbuttonImage.sprite = mbuttonSpriteTable[EUIButtonSpriteKind.OnMouseOrTouchPoped];
            }
        }
    }
    #endregion
}
