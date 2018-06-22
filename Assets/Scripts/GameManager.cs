using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class GameManager : Singleton<GameManager>
{

    public TowerButton ClickedButton { get; set; }



    public int Currency
    {
        get
        {
            return currency;
        }

        set
        {
            currency = value;
            currencyText.text = "<color=yellow>$</color> " + value.ToString();
        }
    }

    private int currency;

    [SerializeField]
    private Text currencyText;

    public ObjectPool Pool { get; set; }

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }



    // Use this for initialization
    void Start ()
    {
        Currency = 500;	
	}
	
	// Update is called once per frame
	void Update ()
    {
        HandleEscape();	
	}

    public void PickTower(TowerButton towerButton)
    {
        if (Currency >= towerButton.Price)
        {
            this.ClickedButton = towerButton;
            Hover.Instance.Activate(towerButton.Sprite);
        }

    }

    public void BuyTower()
    {
        //Reduce the currency
        if (Currency >= ClickedButton.Price)
        {
            Currency -= ClickedButton.Price;

            Hover.Instance.Deactivate();
        }
        
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hover.Instance.Deactivate();
        }
    }

    public void StartWave()
    {
        StartCoroutine(SpawnWave());
    }

    // Spawns a wave of monsters

    private IEnumerator SpawnWave()
    {
        LevelManager.Instance.GeneratePath(); // Can possibly call on update?

        int monsterIndex = Random.Range(0, 4);

        string type = string.Empty;

        switch (monsterIndex)
        {
            case 0:
                type = "BlueMonster";
                break;
            case 1:
                type = "GreenMonster";
                break;
            case 2:
                type = "PurpleMonster";
                break;
            case 3:
                type = "RedMonster";
                break;
        }

        //Requests the Monster from the Object Pool
        Monster monster = Pool.GetObject(type).GetComponent<Monster>();
        monster.Spawn();


        yield return new WaitForSeconds(2.5f);
    }
}
