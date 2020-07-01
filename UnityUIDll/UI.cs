using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityUIDll
{
    #region MainClases
    public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        //  defines
        public struct InputPack_Simple
        {
            public GameObject mtargetObject;

            public Sprite mbaseSprite;

            public InputPack_Simple(in GameObject targetObject, in Sprite PopedSprite)
            {
                mtargetObject = targetObject;

                mbaseSprite = PopedSprite;
            }
        };
        public struct InputPack_Detail
        {
            public GameObject mtargetObject;

            public Sprite mbaseSprite;

            public Vector2 mposition;
            public Vector2 mpivot;
            public Vector2 manchor;

            public InputPack_Detail(in GameObject targetObject, in Sprite PopedSprite, in Vector2 position, in Vector2 pivot, in Vector2 anchor)
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



        //  properties
            //  target GmaeObject
        [SerializeField]
        private GameObject mtargetObject;

            //  related with button
        private Button.ButtonClickedEvent mbuttonClickedEventForShort;
        private Button.ButtonClickedEvent mbuttonClickedEventForLong;
        private RectTransform mrectTransform;
        private EButtonClickType mbuttonClickType;
        private float mrepeatPeriod;
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

            //  using default
        private bool mbusingDefault;
        private bool mbshortClickImmediately;
        private bool mbanimationClickProtection;



        //  methods
        //  constructor
        public void ConstructDefault(in InputPack_Simple inputPack, in UnityAction shortClickFunction)
        {
            mbuttonClickType = EButtonClickType.ShortClick;

            mtargetObject = inputPack.mtargetObject;

            mbuttonClickedEventForShort = new Button.ButtonClickedEvent();
            mbuttonClickedEventForLong = new Button.ButtonClickedEvent();
            if (mtargetObject.TryGetComponent<RectTransform>(out mrectTransform) == false)
            {
                mrectTransform = mtargetObject.AddComponent<RectTransform>();
            }
            mrepeatPeriod = -1.0f;

            mbuttonClickedEventForShort.AddListener(shortClickFunction);

            if (mtargetObject.TryGetComponent<Image>(out mbuttonImage) == false)
            {
                mbuttonImage = mtargetObject.AddComponent<Image>();
            }
            mbuttonImage.sprite = inputPack.mbaseSprite;
            mbuttonSpriteTable = new Dictionary<EUIButtonSpriteKind, Sprite>();
            mbuttonSpriteTable.Add(EUIButtonSpriteKind.Poped, inputPack.mbaseSprite);

            if (mtargetObject.TryGetComponent<Animator>(out mbuttonAnimator) == false)
            {
                mbuttonAnimator = mtargetObject.AddComponent<Animator>();
            }
            mbuttonAnimationTable = new Dictionary<EUIButtonAnimationKind, string>();
            mbuttonAnimator.enabled = false;

            usingDefault = true;
        }
        public void ConstructDefault(in InputPack_Simple inputPack, float repeatPeriod, in UnityAction longClickFunction)
        {
            mbuttonClickType = EButtonClickType.LongClick;

            mtargetObject = inputPack.mtargetObject;

            mbuttonClickedEventForShort = new Button.ButtonClickedEvent();
            mbuttonClickedEventForLong = new Button.ButtonClickedEvent();
            if (mtargetObject.TryGetComponent<RectTransform>(out mrectTransform) == false)
            {
                mrectTransform = mtargetObject.AddComponent<RectTransform>();
            }
            if (repeatPeriod > 0.0f)
            {
                mrepeatPeriod = repeatPeriod;
            }
            else
            {
                mrepeatPeriod = 0.1f;
            }

            mbuttonClickedEventForLong.AddListener(longClickFunction);

            if (mtargetObject.TryGetComponent<Image>(out mbuttonImage) == false)
            {
                mbuttonImage = mtargetObject.AddComponent<Image>();
            }
            mbuttonImage.sprite = inputPack.mbaseSprite;
            mbuttonSpriteTable = new Dictionary<EUIButtonSpriteKind, Sprite>();
            mbuttonSpriteTable.Add(EUIButtonSpriteKind.Poped, inputPack.mbaseSprite);

            if (mtargetObject.TryGetComponent<Animator>(out mbuttonAnimator) == false)
            {
                mbuttonAnimator = mtargetObject.AddComponent<Animator>();
            }
            mbuttonAnimationTable = new Dictionary<EUIButtonAnimationKind, string>();
            mbuttonAnimator.enabled = false;

            usingDefault = true;
        }
        public void ConstructDefault(in InputPack_Simple inputPack, float repeatPeriod, in UnityAction shortClickFunction, in UnityAction longClickFunction)
        {
            mbuttonClickType = EButtonClickType.ShortAndLongClick;

            mtargetObject = inputPack.mtargetObject;

            mbuttonClickedEventForShort = new Button.ButtonClickedEvent();
            mbuttonClickedEventForLong = new Button.ButtonClickedEvent();
            if (mtargetObject.TryGetComponent<RectTransform>(out mrectTransform) == false)
            {
                mrectTransform = mtargetObject.AddComponent<RectTransform>();
            }
            if (repeatPeriod > 0.0f)
            {
                mrepeatPeriod = repeatPeriod;
            }
            else
            {
                mrepeatPeriod = 0.1f;
            }

            mbuttonClickedEventForShort.AddListener(shortClickFunction);
            mbuttonClickedEventForLong.AddListener(longClickFunction);

            if (mtargetObject.TryGetComponent<Image>(out mbuttonImage) == false)
            {
                mbuttonImage = mtargetObject.AddComponent<Image>();
            }
            mbuttonImage.sprite = inputPack.mbaseSprite;
            mbuttonSpriteTable = new Dictionary<EUIButtonSpriteKind, Sprite>();
            mbuttonSpriteTable.Add(EUIButtonSpriteKind.Poped, inputPack.mbaseSprite);

            if (mtargetObject.TryGetComponent<Animator>(out mbuttonAnimator) == false)
            {
                mbuttonAnimator = mtargetObject.AddComponent<Animator>();
            }
            mbuttonAnimationTable = new Dictionary<EUIButtonAnimationKind, string>();
            mbuttonAnimator.enabled = false;

            usingDefault = true;
        }
        public void ConstructDefault(in InputPack_Detail inputPack, in UnityAction shortClickFunction)
        {
            mbuttonClickType = EButtonClickType.ShortClick;

            mtargetObject = inputPack.mtargetObject;

            mbuttonClickedEventForShort = new Button.ButtonClickedEvent();
            mbuttonClickedEventForLong = new Button.ButtonClickedEvent();
            if (mtargetObject.TryGetComponent<RectTransform>(out mrectTransform) == false)
            {
                mrectTransform = mtargetObject.AddComponent<RectTransform>();
            }
            mrectTransform.pivot = inputPack.mpivot;
            mrectTransform.anchorMin = inputPack.manchor;
            mrectTransform.anchorMax = inputPack.manchor;
            mrectTransform.anchoredPosition = inputPack.mposition;
            mrepeatPeriod = -1.0f;

            mbuttonClickedEventForShort.AddListener(shortClickFunction);

            if (mtargetObject.TryGetComponent<Image>(out mbuttonImage) == false)
            {
                mbuttonImage = mtargetObject.AddComponent<Image>();
            }
            else
            {
                mbuttonImage = mtargetObject.GetComponent<Image>();
            }
            mbuttonImage.sprite = inputPack.mbaseSprite;
            mbuttonSpriteTable = new Dictionary<EUIButtonSpriteKind, Sprite>();
            mbuttonSpriteTable.Add(EUIButtonSpriteKind.Poped, inputPack.mbaseSprite);

            if (mtargetObject.TryGetComponent<Animator>(out mbuttonAnimator) == false)
            {
                mbuttonAnimator = mtargetObject.AddComponent<Animator>();
            }
            mbuttonAnimationTable = new Dictionary<EUIButtonAnimationKind, string>();
            mbuttonAnimator.enabled = false;

            usingDefault = true;
        }
        public void ConstructDefault(in InputPack_Detail inputPack, float repeatPeriod, in UnityAction longClickFunction)
        {
            mbuttonClickType = EButtonClickType.LongClick;

            mtargetObject = inputPack.mtargetObject;

            mbuttonClickedEventForShort = new Button.ButtonClickedEvent();
            mbuttonClickedEventForLong = new Button.ButtonClickedEvent();
            if (mtargetObject.TryGetComponent<RectTransform>(out mrectTransform) == false)
            {
                mrectTransform = mtargetObject.AddComponent<RectTransform>();
            }
            mrectTransform.pivot = inputPack.mpivot;
            mrectTransform.anchorMin = inputPack.manchor;
            mrectTransform.anchorMax = inputPack.manchor;
            mrectTransform.anchoredPosition = inputPack.mposition;
            if (repeatPeriod > 0.0f)
            {
                mrepeatPeriod = repeatPeriod;
            }
            else
            {
                mrepeatPeriod = 0.1f;
            }

            mbuttonClickedEventForLong.AddListener(longClickFunction);

            if (mtargetObject.TryGetComponent<Image>(out mbuttonImage) == false)
            {
                mbuttonImage = mtargetObject.AddComponent<Image>();
            }
            mbuttonImage.sprite = inputPack.mbaseSprite;
            mbuttonSpriteTable = new Dictionary<EUIButtonSpriteKind, Sprite>();
            mbuttonSpriteTable.Add(EUIButtonSpriteKind.Poped, inputPack.mbaseSprite);

            if (mtargetObject.TryGetComponent<Animator>(out mbuttonAnimator) == false)
            {
                mbuttonAnimator = mtargetObject.AddComponent<Animator>();
            }
            mbuttonAnimationTable = new Dictionary<EUIButtonAnimationKind, string>();
            mbuttonAnimator.enabled = false;

            usingDefault = true;
        }
        public void ConstructDefault(in InputPack_Detail inputPack, float repeatPeriod, in UnityAction shortClickFunction, in UnityAction longClickFunction)
        {
            mbuttonClickType = EButtonClickType.ShortAndLongClick;

            mtargetObject = inputPack.mtargetObject;

            mbuttonClickedEventForShort = new Button.ButtonClickedEvent();
            mbuttonClickedEventForLong = new Button.ButtonClickedEvent();
            if (mtargetObject.TryGetComponent<RectTransform>(out mrectTransform) == false)
            {
                mrectTransform = mtargetObject.AddComponent<RectTransform>();
            }
            mrectTransform.pivot = inputPack.mpivot;
            mrectTransform.anchorMin = inputPack.manchor;
            mrectTransform.anchorMax = inputPack.manchor;
            mrectTransform.anchoredPosition = inputPack.mposition;
            if (repeatPeriod > 0.0f)
            {
                mrepeatPeriod = repeatPeriod;
            }
            else
            {
                mrepeatPeriod = 0.1f;
            }

            mbuttonClickedEventForShort.AddListener(shortClickFunction);
            mbuttonClickedEventForLong.AddListener(longClickFunction);

            if (mtargetObject.TryGetComponent<Image>(out mbuttonImage) == false)
            {
                mbuttonImage = mtargetObject.AddComponent<Image>();
            }
            mbuttonImage.sprite = inputPack.mbaseSprite;
            mbuttonSpriteTable = new Dictionary<EUIButtonSpriteKind, Sprite>();
            mbuttonSpriteTable.Add(EUIButtonSpriteKind.Poped, inputPack.mbaseSprite);

            if (mtargetObject.TryGetComponent<Animator>(out mbuttonAnimator) == false)
            {
                mbuttonAnimator = mtargetObject.AddComponent<Animator>();
            }
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

            if (mbusingDefault && !shortClickImmediately)
            {
                FunctionForShortClick();
            }
        }
        public void OnPointerDown(PointerEventData pointerEventData)
        {
            if(mbanimationClickProtection && mbuttonAnimator.enabled == false)
            {
                if (mbisPushed)
                {
                    mbisPushed = false;
                }
                else
                {
                    mbisPushed = true;
                }
                mbisDown = true;
            }
            else if(!mbanimationClickProtection)
            {
                if (mbisPushed)
                {
                    mbisPushed = false;
                }
                else
                {
                    mbisPushed = true;
                }
                mbisDown = true;
            }

            if (mbusingDefault)
            {
                if(mbanimationClickProtection)
                {
                    if(mbuttonAnimator.enabled == false)
                    {
                        if (mbisPushed && mbuttonSpriteTable.ContainsKey(EUIButtonSpriteKind.OnMouseOrTouchPushed))
                        {
                            mbuttonImage.sprite = mbuttonSpriteTable[EUIButtonSpriteKind.OnMouseOrTouchPushed];
                        }
                        else if (mbuttonSpriteTable.ContainsKey(EUIButtonSpriteKind.OnMouseOrTouchPoped))
                        {
                            mbuttonImage.sprite = mbuttonSpriteTable[EUIButtonSpriteKind.OnMouseOrTouchPoped];
                        }

                        
                        if (mbisPushed && mbuttonAnimationTable.ContainsKey(EUIButtonAnimationKind.ButtonPushed))
                        {
                            mbuttonAnimator.enabled = true;
                            mbuttonAnimator.Play(mbuttonAnimationTable[EUIButtonAnimationKind.ButtonPushed], 0, 0.0f);
                            Invoke("SetAnimatorEnableFalse", mbuttonAnimator.runtimeAnimatorController.animationClips[0].length);
                        }
                        else if (mbuttonAnimationTable.ContainsKey(EUIButtonAnimationKind.Idle))
                        {
                            mbuttonAnimator.enabled = true;
                            mbuttonAnimator.Play(mbuttonAnimationTable[EUIButtonAnimationKind.Idle], 0, 0.0f);
                            Invoke("SetAnimatorEnableFalse", mbuttonAnimator.runtimeAnimatorController.animationClips[1].length);
                        }

                        if (mbshortClickImmediately)
                        {
                            FunctionForShortClick();
                        }
                        if (mbuttonClickType == EButtonClickType.LongClick || mbuttonClickType == EButtonClickType.ShortAndLongClick)
                        {
                            InvokeRepeating("FunctionForLongClick", 0.0f, mrepeatPeriod);
                        }
                    }
                }
                else
                {
                    if (mbisPushed && mbuttonSpriteTable.ContainsKey(EUIButtonSpriteKind.OnMouseOrTouchPushed))
                    {
                        mbuttonImage.sprite = mbuttonSpriteTable[EUIButtonSpriteKind.OnMouseOrTouchPushed];
                    }
                    else if (mbuttonSpriteTable.ContainsKey(EUIButtonSpriteKind.OnMouseOrTouchPoped))
                    {
                        mbuttonImage.sprite = mbuttonSpriteTable[EUIButtonSpriteKind.OnMouseOrTouchPoped];
                    }

                    if (mbisPushed && mbuttonAnimationTable.ContainsKey(EUIButtonAnimationKind.ButtonPushed))
                    {
                        mbuttonAnimator.enabled = true;
                        mbuttonAnimator.Play(mbuttonAnimationTable[EUIButtonAnimationKind.ButtonPushed], 0, 0.0f);
                        Invoke("SetAnimatorEnableFalse", mbuttonAnimator.runtimeAnimatorController.animationClips[0].length);
                    }
                    else if (mbuttonAnimationTable.ContainsKey(EUIButtonAnimationKind.Idle))
                    {
                        mbuttonAnimator.enabled = true;
                        mbuttonAnimator.Play(mbuttonAnimationTable[EUIButtonAnimationKind.Idle], 0, 0.0f);
                        Invoke("SetAnimatorEnableFalse", mbuttonAnimator.runtimeAnimatorController.animationClips[1].length);
                    }

                    if (mbshortClickImmediately)
                    {
                        FunctionForShortClick();
                    }
                    if (mbuttonClickType == EButtonClickType.LongClick || mbuttonClickType == EButtonClickType.ShortAndLongClick)
                    {
                        InvokeRepeating("FunctionForLongClick", 0.0f, mrepeatPeriod);
                    }
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
        public bool isPushed
        {
            get => mbisPushed;
        }
        public Image image
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
        public bool shortClickImmediately
        {
            get => mbshortClickImmediately;
            set => mbshortClickImmediately = value;
        }
        public bool animationClickProtection
        {
            get => mbanimationClickProtection;
            set => mbanimationClickProtection = value;
        }
        public bool animationNowPlaying
        {
            get => mbuttonAnimator.enabled;
        }

            //  behaviour method
        public void SwitchModeToShortClick(in UnityAction shortClickFunction)
        {
            mbuttonClickType = EButtonClickType.ShortClick;
            repeatPeriod = -1.0f;

            mbuttonClickedEventForShort.RemoveAllListeners();
            mbuttonClickedEventForShort.AddListener(shortClickFunction);
        }
        public void SwitchModeToLongClick(float repeatPeriod, in UnityAction longClickFunction)
        {
            mbuttonClickType = EButtonClickType.LongClick;
            if (repeatPeriod > 0.0f)
            {
                mrepeatPeriod = repeatPeriod;
            }
            else
            {
                mrepeatPeriod = 0.1f;
            }

            mbuttonClickedEventForLong.RemoveAllListeners();
            mbuttonClickedEventForLong.AddListener(longClickFunction);
        }
        public void SwitchModeToShortAndLongClick(float repeatPeriod, in UnityAction shortClickFunction, in UnityAction longClickFunction)
        {
            mbuttonClickType = EButtonClickType.ShortAndLongClick;
            if (repeatPeriod > 0.0f)
            {
                mrepeatPeriod = repeatPeriod;
            }
            else
            {
                mrepeatPeriod = 0.1f;
            }

            mbuttonClickedEventForShort.RemoveAllListeners();
            mbuttonClickedEventForLong.RemoveAllListeners();

            mbuttonClickedEventForShort.AddListener(shortClickFunction);
            mbuttonClickedEventForLong.AddListener(longClickFunction);
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
        public void RemoveButtonAnimationState(in EUIButtonAnimationKind buttonAnimationKind)
        {
            if(mbuttonAnimationTable.ContainsKey(buttonAnimationKind))
            {
                mbuttonAnimationTable.Remove(buttonAnimationKind);
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
        public void RemoveButtonSprite(in EUIButtonSpriteKind buttonSpriteKind)
        {
            if(mbuttonSpriteTable.ContainsKey(buttonSpriteKind))
            {
                mbuttonSpriteTable.Remove(buttonSpriteKind);
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
                if(mbisEnter)
                {
                    mbuttonImage.sprite = mbuttonSpriteTable[EUIButtonSpriteKind.OnMouseOrTouchPushed];
                }
                else
                {
                    mbuttonImage.sprite = mbuttonSpriteTable[EUIButtonSpriteKind.Pushed];
                }
            }
            else if (mbuttonSpriteTable.ContainsKey(EUIButtonSpriteKind.OnMouseOrTouchPoped))
            {
                if(mbisEnter)
                {
                    mbuttonImage.sprite = mbuttonSpriteTable[EUIButtonSpriteKind.OnMouseOrTouchPoped];
                }
                else
                {
                    mbuttonImage.sprite = mbuttonSpriteTable[EUIButtonSpriteKind.Poped];
                }
            }
        }
    }
    #endregion
}
