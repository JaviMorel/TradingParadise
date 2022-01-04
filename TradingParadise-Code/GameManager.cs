using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Transform StartPosition;

    [SerializeField]
    Player Player = null;

    [SerializeField]
    Player Player1 = null;

    [SerializeField]
    Player Player2 = null;

    [SerializeField]
    List<Item> Items = new List<Item>();

    Merchant[] MerchantsLevel = new Merchant[0];

    [SerializeField]
    int NumberExchanges = 3;

    [SerializeField]
    bool MultiplayerActivated = false;

    [SerializeField]
    GameObject GamePanel = null;

    [SerializeField]
    GameObject MultiGamePanel = null;

    [SerializeField]
    GameObject FinalPanel = null;

    [SerializeField]
    GameObject LosePanel = null;

    [SerializeField]
    GameObject PausePanel = null;

    [SerializeField]
    Text FinalText = null;

    [SerializeField]
    Text TimerText = null;

    [SerializeField]
    float TimeSeconds = 0.0f;

    [SerializeField]
    AudioClip BackgroundSound = null;

    [SerializeField]
    AudioClip LoseSound = null;

    [SerializeField]
    AudioClip WinSound = null;

    AudioSource audioSource = null;

    bool GameFinished = false;

    MenuController mc = null;

    private void Awake()
    {
        MerchantsLevel = GameObject.FindObjectsOfType<Merchant>();
        mc = GameObject.FindObjectOfType<MenuController>();

        if(mc == null)
        {
            print("Menucontroller no encontrado");
        }
        else
        {
            print("El valor del juego es: " + mc.NumberExchanges);
            NumberExchanges = mc.NumberExchanges;
            MultiplayerActivated = mc.MultiplayerActivated;

            

        }

        if (NumberExchanges == 3)
        {
            TimeSeconds = 150.0f;
        }
        else if (NumberExchanges == 6)
        {
            TimeSeconds = 300.0f;
        }
        else
        {
            TimeSeconds = 600.0f;
        }

        if (MultiplayerActivated)
        {
            Player.gameObject.SetActive(false);
            Player1.gameObject.SetActive(true);
            Player2.gameObject.SetActive(true);

            GamePanel.SetActive(false);
            MultiGamePanel.SetActive(true);

            CreateMultiGame();
        }
        else
        {
            Player.gameObject.SetActive(true);
            Player1.gameObject.SetActive(false);
            Player2.gameObject.SetActive(false);

            GamePanel.SetActive(true);
            MultiGamePanel.SetActive(false);
            CreateGame();
        }

        PausePanel.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = BackgroundSound;
        audioSource.loop = true;
        audioSource.Play();

    }


    // Start is called before the first frame update
    void Start()
    {
        //GamePanel.SetActive(true);
        FinalPanel.SetActive(false);
        LosePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (!MultiplayerActivated && !GameFinished)
        {
            TimeSeconds -= Time.deltaTime;
            TimerText.text = "" + TimeSeconds.ToString("f0");

            if (TimeSeconds <= 0.2f)
            {
                LoseGame();

            }
        }

        if (!GameFinished)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OpenPauseMenu();
                GameFinished = true;
            }
        }

    }


    //Faltaria separar a los mercaderes por seccion y evitar que (en el intercambio ideal) se repita la misma seccion
    void CreateGame()
    {
        if (Items.Count <= 1) return;
        if (Player == null) return;

        List<Item> ItemsCopy = new List<Item>(Items);

        //Escojo el objeto inicial y el objeto objetivo
        int RandomNumber = Random.Range(0, ItemsCopy.Count - 1);
        Item GoalItem = ItemsCopy[RandomNumber];

        ItemsCopy.Remove(GoalItem);

        RandomNumber = Random.Range(0, ItemsCopy.Count - 1);
        Item InitialItem = ItemsCopy[RandomNumber];


        //Se los asigno al jugador
        Player.InventoryItem = InitialItem;
        Player.GoalItem = GoalItem;

        ItemsCopy.Clear();

        //Creo los intercambios
        ItemsCopy = new List<Item>(Items);

        ItemsCopy.Remove(GoalItem);
        ItemsCopy.Remove(InitialItem);

        List<Merchant> MerchantsCopy = new List<Merchant>(MerchantsLevel);  //Esta llevará la cuenta de los mercaderes ya usados
        List<Merchant> MerchantsCopy2 = new List<Merchant>(MerchantsLevel); //Este evitará que se repitan mercaderes de la misma seccion del mapa

        Item CurrentItem = null;
        //Creo el intercambio ideal
        for(int i = 0; i<NumberExchanges; ++i)
        {
            int r;
            Merchant m;
            Item i1;
            if(i == 0) //Si es la primera, le asigno el goal item al mercader
            {
                r = Random.Range(0, MerchantsCopy2.Count - 1);
                m = MerchantsCopy2[r];

                r = Random.Range(0, ItemsCopy.Count - 1);
                i1 = ItemsCopy[r];

                m.RequestItem = i1;
                m.SaleItem = GoalItem;

                print("Vendedor Final: "+m.name + " , seccion: "+m.SectionMap);

                MerchantsCopy.Remove(m);
                ItemsCopy.Remove(i1);

                CurrentItem = i1;
            }
            else if (i == NumberExchanges-1) //Si es el ultimo, le asigno el initial item al mercader
            {
                r = Random.Range(0, MerchantsCopy2.Count - 1);
                m = MerchantsCopy2[r];

                m.RequestItem = InitialItem;
                m.SaleItem = CurrentItem;

                print("Vendedor Inicial: " + m.name + " , seccion: " + m.SectionMap);

                MerchantsCopy.Remove(m);
            }
            else
            {
                r = Random.Range(0, MerchantsCopy2.Count - 1);
                m = MerchantsCopy2[r];

                r = Random.Range(0, ItemsCopy.Count - 1);
                i1 = ItemsCopy[r];
                
                m.RequestItem = i1;
                m.SaleItem = CurrentItem;

                print("Vendedor " + i + ": " + m.name + " , seccion: " + m.SectionMap);

                MerchantsCopy.Remove(m);
                ItemsCopy.Remove(i1);

                CurrentItem = i1;
            }

            //Elimino aquellos de la misma seccion
            for (int j = 0; j < MerchantsCopy2.Count; ++j)
            {
                Merchant m1 = MerchantsCopy2[j];
                if (m1.SectionMap == m.SectionMap)
                    MerchantsCopy2.Remove(m1);
            }

        }

        ItemsCopy.Clear();
        MerchantsCopy2.Clear();

        //Hago el resto de intercambios
        ItemsCopy = new List<Item>(Items);

        ItemsCopy.Remove(GoalItem);

        for(int i = 0; i<MerchantsCopy.Count;++i)
        {
            Merchant m = MerchantsCopy[i];

            int r1 = Random.Range(0, ItemsCopy.Count - 1);
            Item i1 = ItemsCopy[r1];

            int r2 = Random.Range(0, ItemsCopy.Count - 1);
            while(r2 == r1)
            {
                r2 = Random.Range(0, ItemsCopy.Count - 1);
            }
            Item i2 = ItemsCopy[r2];

            m.RequestItem = i1;
            m.SaleItem = i2;
        }

        MerchantsCopy.Clear();

    }

    void CreateMultiGame()
    {
        if (Items.Count <= 1) return;
        if (Player == null) return;

        List<Item> ItemsCopy = new List<Item>(Items);

        //Escojo el objeto inicial y el objeto objetivo
        int RandomNumber = Random.Range(0, ItemsCopy.Count - 1);
        Item GoalItem = ItemsCopy[RandomNumber];

        ItemsCopy.Remove(GoalItem);

        RandomNumber = Random.Range(0, ItemsCopy.Count - 1);
        Item InitialItem = ItemsCopy[RandomNumber];


        //Se los asigno a los jugadores
        Player1.InventoryItem = InitialItem;
        Player1.GoalItem = GoalItem;

        Player2.InventoryItem = InitialItem;
        Player2.GoalItem = GoalItem;

        ItemsCopy.Clear();

        //Creo los intercambios
        ItemsCopy = new List<Item>(Items);

        ItemsCopy.Remove(GoalItem);
        ItemsCopy.Remove(InitialItem);

        List<Merchant> MerchantsCopy = new List<Merchant>(MerchantsLevel);  //Esta llevará la cuenta de los mercaderes ya usados
        List<Merchant> MerchantsCopy2 = new List<Merchant>(MerchantsLevel); //Este evitará que se repitan mercaderes de la misma seccion del mapa

        Item CurrentItem = null;
        //Creo el intercambio ideal
        for (int i = 0; i < NumberExchanges; ++i)
        {
            int r;
            Merchant m;
            Item i1;
            if (i == 0) //Si es la primera, le asigno el goal item al mercader
            {
                r = Random.Range(0, MerchantsCopy2.Count - 1);
                m = MerchantsCopy2[r];

                r = Random.Range(0, ItemsCopy.Count - 1);
                i1 = ItemsCopy[r];

                m.RequestItem = i1;
                m.SaleItem = GoalItem;
                m.RequestItem2 = i1;
                m.SaleItem2 = GoalItem;

                print("Vendedor Final: " + m.name + " , seccion: " + m.SectionMap);

                MerchantsCopy.Remove(m);
                ItemsCopy.Remove(i1);

                CurrentItem = i1;
            }
            else if (i == NumberExchanges - 1) //Si es el ultimo, le asigno el initial item al mercader
            {
                r = Random.Range(0, MerchantsCopy2.Count - 1);
                m = MerchantsCopy2[r];

                m.RequestItem = InitialItem;
                m.SaleItem = CurrentItem;
                m.RequestItem2 = InitialItem;
                m.SaleItem2 = CurrentItem;

                print("Vendedor Inicial: " + m.name + " , seccion: " + m.SectionMap);

                MerchantsCopy.Remove(m);
            }
            else
            {
                r = Random.Range(0, MerchantsCopy2.Count - 1);
                m = MerchantsCopy2[r];

                r = Random.Range(0, ItemsCopy.Count - 1);
                i1 = ItemsCopy[r];

                m.RequestItem = i1;
                m.SaleItem = CurrentItem;
                m.RequestItem2 = i1;
                m.SaleItem2 = CurrentItem;

                print("Vendedor " + i + ": " + m.name + " , seccion: " + m.SectionMap);

                MerchantsCopy.Remove(m);
                ItemsCopy.Remove(i1);

                CurrentItem = i1;
            }

            //Elimino aquellos de la misma seccion
            for (int j = 0; j < MerchantsCopy2.Count; ++j)
            {
                Merchant m1 = MerchantsCopy2[j];
                if (m1.SectionMap == m.SectionMap)
                    MerchantsCopy2.Remove(m1);
            }

        }

        ItemsCopy.Clear();
        MerchantsCopy2.Clear();

        //Hago el resto de intercambios
        ItemsCopy = new List<Item>(Items);

        ItemsCopy.Remove(GoalItem);

        for (int i = 0; i < MerchantsCopy.Count; ++i)
        {
            Merchant m = MerchantsCopy[i];

            int r1 = Random.Range(0, ItemsCopy.Count - 1);
            Item i1 = ItemsCopy[r1];

            int r2 = Random.Range(0, ItemsCopy.Count - 1);
            while (r2 == r1)
            {
                r2 = Random.Range(0, ItemsCopy.Count - 1);
            }
            Item i2 = ItemsCopy[r2];

            m.RequestItem = i1;
            m.SaleItem = i2;
            m.RequestItem2 = i1;
            m.SaleItem2 = i2;
        }

        MerchantsCopy.Clear();

    }

    void LoseGame()
    {
        GameFinished = true;

        audioSource.Pause();

        audioSource.clip = LoseSound;

        audioSource.loop = false;

        audioSource.Play();

        Player.GetComponent<Player>().enabled = false;
        Player1.GetComponent<Player>().enabled = false;
        Player2.GetComponent<Player>().enabled = false;
        GamePanel.SetActive(false);
        MultiGamePanel.SetActive(false);

        LosePanel.SetActive(true);
    }

    public void WinGame(Player player)
    {
        GameFinished = true;
        audioSource.Pause();

        audioSource.clip = WinSound;

        audioSource.loop = false;

        audioSource.Play();

        Player.GetComponent<Player>().enabled = false;
        Player1.GetComponent<Player>().enabled = false;
        Player2.GetComponent<Player>().enabled = false;
        GamePanel.SetActive(false);
        MultiGamePanel.SetActive(false);


        FinalText.text = "Congrats, " + player.name + "!";

        FinalPanel.SetActive(true);
    }


    public void Replay()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ExitToMenu()
    {
        Destroy(mc.gameObject);
        SceneManager.LoadScene("Menu");
    }

    void OpenPauseMenu()
    {
        PausePanel.SetActive(true);
        GamePanel.SetActive(false);
        MultiGamePanel.SetActive(false);

        audioSource.Pause();
    }

    public void ClosePauseMenu()
    {
        audioSource.UnPause();
        PausePanel.SetActive(false);
        if (MultiplayerActivated)
        {
            GamePanel.SetActive(false);
            MultiGamePanel.SetActive(true);
        }
        else
        {
            GamePanel.SetActive(true);
            MultiGamePanel.SetActive(false);
        }
        GameFinished = false;
    }

}
