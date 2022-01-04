using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    GameManager gm = null;

    [SerializeField]
    int NumberPlayer = 0;

    [SerializeField]
    float speedMovement = 200.0f;

    public Item InventoryItem = null;

    public Item GoalItem = null;

    [SerializeField]
    GameObject ExchangePanel = null;

    [SerializeField]
    Image RequestItemImage = null;

    [SerializeField]
    Image SaleItemImage = null;

    Merchant Merchant = null;

    bool ExchangeActivated = false;

    [SerializeField]
    Image InventoryItemImage = null;

    [SerializeField]
    Image GoalItemImage = null;

    [SerializeField]
    Sprite TradePanelImageOriginal = null;

    [SerializeField]
    Sprite TradePanelImageDone = null;

    [SerializeField]
    Image TradePanelImage = null;

    Animator animator = null;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        animator = GetComponent<Animator>();

        ExchangePanel.SetActive(false);
        InventoryItemImage.sprite = InventoryItem.ItemImage;
        GoalItemImage.sprite = GoalItem.ItemImage; 
    }

    // Update is called once per frame
    void Update()
    {
        if(NumberPlayer == 0)
        {
            Move1();

            if (Input.GetKeyDown("e"))
            {
                if (ExchangeActivated)
                {
                    if (InventoryItem == Merchant.RequestItem)
                    {
                        InventoryItem = Merchant.SaleItem;

                        //Intercambio los objetos que pide
                        Item aux = Merchant.RequestItem;
                        Merchant.RequestItem = Merchant.SaleItem;
                        Merchant.SaleItem = aux;

                        //Change the value of the exchange
                        Merchant.ExchangeMade = !Merchant.ExchangeMade;

                        //Change the color of the panel if there was an exchange made before over this merchant
                        if (Merchant.ExchangeMade)
                        {
                            TradePanelImage.sprite = TradePanelImageDone;
                        }
                        else
                        {
                            TradePanelImage.sprite = TradePanelImageOriginal;
                        }

                        //---------Change the images--------------

                        InventoryItemImage.sprite = InventoryItem.ItemImage;
                        RequestItemImage.sprite = Merchant.RequestItem.ItemImage;
                        SaleItemImage.sprite = Merchant.SaleItem.ItemImage;
                    }
                }
            }
        }
        else
        {
            Move2();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (ExchangeActivated)
                {
                    if (InventoryItem == Merchant.RequestItem2)
                    {
                        InventoryItem = Merchant.SaleItem2;

                        //Intercambio los objetos que pide
                        Item aux = Merchant.RequestItem2;
                        Merchant.RequestItem2 = Merchant.SaleItem2;
                        Merchant.SaleItem2 = aux;

                        //Change the value of the exchange
                        Merchant.ExchangeMade2 = !Merchant.ExchangeMade2;

                        //Change the color of the panel if there was an exchange made before over this merchant
                        if (Merchant.ExchangeMade2)
                        {
                            TradePanelImage.sprite = TradePanelImageDone;
                        }
                        else
                        {
                            TradePanelImage.sprite = TradePanelImageOriginal;
                        }

                        //---------Change the images--------------

                        InventoryItemImage.sprite = InventoryItem.ItemImage;
                        RequestItemImage.sprite = Merchant.RequestItem2.ItemImage;
                        SaleItemImage.sprite = Merchant.SaleItem2.ItemImage;
                    }
                }
            }
        }

        CheckGoal();
    }

    private void Move1()
    {
        Vector2 pos;
        if (Input.GetKey("w"))
        {
            animator.SetBool("IsUpWalking", true);
            pos = new Vector2(0.0f, speedMovement);
            animator.SetBool("IsLeftWalking", false);
            animator.SetBool("IsRightWalking", false);
            animator.SetBool("IsDownWalking", false);

        }
        else if (Input.GetKey("s"))
        {
            animator.SetBool("IsDownWalking", true);
            pos = new Vector2(0.0f, -speedMovement);
            animator.SetBool("IsLeftWalking", false);
            animator.SetBool("IsRightWalking", false);
            animator.SetBool("IsUpWalking", false);

        }
        else if (Input.GetKey("d"))
        {
            animator.SetBool("IsRightWalking", true);
            pos = new Vector2(speedMovement, 0.0f);
            animator.SetBool("IsLeftWalking", false);
            animator.SetBool("IsDownWalking", false);
            animator.SetBool("IsUpWalking", false);

        }
        else if (Input.GetKey("a"))
        {
            animator.SetBool("IsLeftWalking", true);
            pos = new Vector2(-speedMovement, 0.0f);
            animator.SetBool("IsRightWalking", false);
            animator.SetBool("IsDownWalking", false);
            animator.SetBool("IsUpWalking", false);
        }
        else
        {
            pos = new Vector2(0.0f, 0.0f);
            animator.SetBool("IsLeftWalking", false);
            animator.SetBool("IsRightWalking", false);
            animator.SetBool("IsDownWalking", false);
            animator.SetBool("IsUpWalking", false);


        }
        this.transform.position += new Vector3(pos.x, pos.y, 0) * Time.deltaTime;
    }

    private void Move2()
    {
        Vector2 pos;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("IsUpWalking", true);
            pos = new Vector2(0.0f, speedMovement);
            animator.SetBool("IsLeftWalking", false);
            animator.SetBool("IsRightWalking", false);
            animator.SetBool("IsDownWalking", false);

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetBool("IsDownWalking", true);
            pos = new Vector2(0.0f, -speedMovement);
            animator.SetBool("IsLeftWalking", false);
            animator.SetBool("IsRightWalking", false);
            animator.SetBool("IsUpWalking", false);

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("IsRightWalking", true);
            pos = new Vector2(speedMovement, 0.0f);
            animator.SetBool("IsLeftWalking", false);
            animator.SetBool("IsDownWalking", false);
            animator.SetBool("IsUpWalking", false);

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetBool("IsLeftWalking", true);
            pos = new Vector2(-speedMovement, 0.0f);
            animator.SetBool("IsRightWalking", false);
            animator.SetBool("IsDownWalking", false);
            animator.SetBool("IsUpWalking", false);
        }
        else
        {
            pos = new Vector2(0.0f, 0.0f);
            animator.SetBool("IsLeftWalking", false);
            animator.SetBool("IsRightWalking", false);
            animator.SetBool("IsDownWalking", false);
            animator.SetBool("IsUpWalking", false);
        }
        this.transform.position += new Vector3(pos.x, pos.y, 0) * Time.deltaTime;
    }

    public void SetExchange(Merchant m)
    {
        Merchant = m;

        if(m.RequestItem == null && m.SaleItem == null)
        {
            print("Falta asignar al merchant " + m.name + " los items");
            return;
        }
            
        if(NumberPlayer == 0)
        {
            RequestItemImage.sprite = Merchant.RequestItem.ItemImage;
            SaleItemImage.sprite = Merchant.SaleItem.ItemImage;

            ExchangeActivated = true;

            //Change the color of the panel if there was an exchange made before over this merchant
            if (m.ExchangeMade)
            {
                TradePanelImage.sprite = TradePanelImageDone;
            }
            else
            {
                TradePanelImage.sprite = TradePanelImageOriginal;
            }
        }
        else
        {
            RequestItemImage.sprite = Merchant.RequestItem2.ItemImage;
            SaleItemImage.sprite = Merchant.SaleItem2.ItemImage;

            ExchangeActivated = true;

            //Change the color of the panel if there was an exchange made before over this merchant
            if (m.ExchangeMade2)
            {
                TradePanelImage.sprite = TradePanelImageDone;
            }
            else
            {
                TradePanelImage.sprite = TradePanelImageOriginal;
            }
        }

        

        ExchangePanel.SetActive(true);
    }

    public void UnsetExchange()
    {
        Merchant = null;

        ExchangeActivated = false;
        ExchangePanel.SetActive(false);
    }

    private void CheckGoal()
    {
        if(InventoryItem == GoalItem)
        {
            gm.WinGame(this);
        }
    }

}
