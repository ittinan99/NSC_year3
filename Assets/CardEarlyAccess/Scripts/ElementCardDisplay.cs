using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using UnityEngine.EventSystems;
public class ElementCardDisplay : NetworkBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerDownHandler,IPointerUpHandler
{
    public ElementCard E_Card;

    public TextMeshProUGUI CardText;
    public TextMeshProUGUI F_CardText;
    public Image Artwork = null;

    public bool IsCombine;
    [SerializeField]
    private bool IsPressed;
    [SerializeField]
    private bool IsAttack;
    public Vector2 StartPos;
    
    public bool IsOutPutCard = false;
    public GameObject CombineSlot;

    private Transform _selection;

    private ISelectionResponse _selectionResponse;


    private void Awake()
    {
        _selectionResponse = GetComponent<ISelectionResponse>();
    }
    void Start()
    {
        IsAttack = false;
        IsCombine = false;
        IsPressed = false;
        StartPos = this.transform.position;
        CardText.text = E_Card.element_Name;
        F_CardText.text = E_Card.element_FName;
        if(E_Card.ArtWork != null)
        {
            Artwork.sprite = E_Card.ArtWork;
        }
    }
    private void Update()
    {
        if (IsPressed )
        {
            this.transform.position = Input.mousePosition;
        }
        if(GameSystem.localTurnbased != null)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0) && IsAttack && GameSystem.localTurnbased.PlayerState == TurnBaseSystem.GameState.Y_AttackTurn)
            {
                IsAttack = false;
                GameObject arrow = GameObject.Find("Arrow");
                arrow.GetComponent<Arrow>().Hide();
                GameSystem.localTurnbased.ATKcardFunc(this);
            }
            if (IsAttack)
            {
                if (_selection != null)
                {
                    _selectionResponse.OnDeselect(_selection);
                    
                }

                #region MyRegion
                GameObject FB = GameSystem.localTurnbased.FlaskBarrel;
                Ray ray = CreateRay();
                //Physics.Raycast(FB.transform.position, FB.transform.forward

                _selection = null;

                if (Physics.Raycast(ray, out RaycastHit hit, 10000))
                {
                    var selection = hit.transform;

                    Debug.DrawRay(FB.transform.position, FB.transform.forward * 1000, Color.red);
                    GameObject enemy = hit.transform.gameObject;

                    if (selection.CompareTag("Player") || selection.CompareTag("canAttack"))
                    {
                        Debug.Log(selection.transform.gameObject.name);
                        _selection = selection;
                        GameSystem.localTurnbased.FlaskBarrel.transform.position = hit.transform.position;
                    }

                }
                #endregion

                if (_selection != null)
                {
                    Debug.Log("in_selection");
                    _selectionResponse.OnSelect(_selection);
                }
            }
        }     
    }
    private static Ray CreateRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (GameSystem.gamePhase == GameSystem.GamePhase.CombineState)
            {
                IsPressed = true;
                if (IsCombine)
                {
                    if (CombineSlot != null)
                    {
                        CombineSlot.GetComponent<CardEvent>().IsEmpty = true;

                    }
                }
                if (IsOutPutCard)
                {
                    IsOutPutCard = false;
                    GameObject cardPanel = GameObject.Find("CardPanel");
                    GameObject combineSystem = GameObject.Find("CombineSystem");
                    combineSystem.GetComponent<CombineSystem>().ConfirmCombine();
                    cardPanel.GetComponent<CardPanel>().AddCard(this);
                }
            }
            else if (GameSystem.gamePhase == GameSystem.GamePhase.AttackState && E_Card.CanAttack && GameSystem.localTurnbased.PlayerState == TurnBaseSystem.GameState.Y_AttackTurn)
            {
                Debug.Log("Arrow");
                GameObject arrow = GameObject.Find("Arrow");
                arrow.GetComponent<Arrow>().Show();
                IsAttack = true;
                Arrow.startPoint = this.gameObject.transform.position;
            }
           
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsPressed = false;

        if (_selection != null)
        {
            _selectionResponse.OnDeselect(_selection);
        }

        if (IsCombine)
        {
            if(CombineSlot != null)
            {
                CombineSlot.GetComponent<CardEvent>().PlaceCard(this);
                this.transform.position = CombineSlot.transform.position;
            }
        }
        else
        {
            this.transform.position = StartPos;
            IsPressed = false;
        }
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CombineSlot") && other.gameObject.GetComponent
            <CardEvent>().IsEmpty)
        {
            IsCombine = true;
            CombineSlot = other.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("CombineSlot") && other.gameObject.GetComponent
            <CardEvent>().IsEmpty)
        {
            other.gameObject.GetComponent<CardEvent>().RemoveCard();
            IsCombine = false;
            CombineSlot = null;
        }
    }  
}
