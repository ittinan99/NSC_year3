using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using UnityEngine.EventSystems;
public class ElementCardEnclopedia : NetworkBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerDownHandler
{
    public ElementCard E_Card;

    public TextMeshProUGUI CardText;
    public TextMeshProUGUI F_CardText;
    public Image Artwork = null;
    public float AmmoAmount;
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
        AmmoAmount = E_Card.Amount;
        if(E_Card.ArtWork != null)
        {
            Artwork.sprite = E_Card.ArtWork;
        }
    }
    private void Update()
    {
        //if(GameSystem.localTurnbased != null)
        //{
        //    if (Input.GetKeyUp(KeyCode.Mouse0) && IsAttack )
        //    {
        //        IsAttack = false;
        //        GameObject arrow = GameObject.Find("Arrow");
        //        arrow.GetComponent<Arrow>().Hide();
        //        GameSystem.localTurnbased.ATKcardFunc(this);
        //    }
        //    if (IsAttack)
        //    {
        //        if (_selection != null)
        //        {
        //            _selectionResponse.OnDeselect(_selection);
                    
        //        }

        //        #region MyRegion
        //        GameObject FB = GameSystem.localTurnbased.FlaskBarrel;
        //        Ray ray = CreateRay();
        //        //Physics.Raycast(FB.transform.position, FB.transform.forward

        //        _selection = null;

        //        if (Physics.Raycast(ray, out RaycastHit hit, 10000))
        //        {
        //            var selection = hit.transform;

        //            Debug.DrawRay(FB.transform.position, FB.transform.forward * 1000, Color.red);
        //            GameObject enemy = hit.transform.gameObject;

        //            if (selection.CompareTag("Player") || selection.CompareTag("canAttack"))
        //            {
        //                Debug.Log(selection.transform.gameObject.name);
        //                _selection = selection;
        //                GameSystem.localTurnbased.FlaskBarrel.transform.position = hit.transform.position;
        //            }

        //        }
        //        #endregion

        //        if (_selection != null)
        //        {
        //            Debug.Log("in_selection");
        //            _selectionResponse.OnSelect(_selection);
        //        }
        //    }
        //}     
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
            EncyclopediaPageManager PageManager = GameObject.Find("PageManager").GetComponent<EncyclopediaPageManager>();
            PlayerPrefs.SetInt("PageNow", PageManager.PageCount);
            EncyclopediaDisplayData Data = GameObject.Find("PageManager").GetComponent<EncyclopediaDisplayData>();
            Data.CardF_Name = E_Card.element_FName;
            Data.CardS_Name = E_Card.element_Name;
            Data.Description = E_Card.description;
            if (E_Card.ArtWork != null)
            {
                Data.Artwork = E_Card.ArtWork;
            }
            PageManager.PopUp.SetActive(true);
            PageManager.SetFalse();
            GameObject.Find("BackPage").SetActive(false);
            GameObject.Find("NextPage").SetActive(false);
        }
    }
}
