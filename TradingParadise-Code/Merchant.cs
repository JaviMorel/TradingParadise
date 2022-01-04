using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{

    [SerializeField]
    public Item RequestItem = null;  //Objeto que pides

    [SerializeField]
    public Item SaleItem = null;  //Objeto que das

    [SerializeField]
    public Item RequestItem2 = null;  //Objeto que pides

    [SerializeField]
    public Item SaleItem2 = null;  //Objeto que das

    [SerializeField]
    public int SectionMap;

    public bool ExchangeMade = false;

    public bool ExchangeMade2 = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player == null) return;

            player.SetExchange(this);

        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player == null) return;

            player.UnsetExchange();

        }

    }
}
