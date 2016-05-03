using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class menuTienda : MonoBehaviour
{



    public  GameObject[] totalChapas;
    public List<Vector3> totalFormaciones;
    private static GameObject[] chapas;
    public static int  precioItem;
    public static int dinero = 500;
    public static string nombreBoton;

    // Use this for initialization
    void Awake()
    {
 
        //Cargamos las chapas
        chapas = playerController.chapas;
        //Cargamos el dinero
        dinero = PlayerPrefs.GetInt("PlayerMoney");
        //Cargamos las chapas compradas

        for(int i = 0; i<totalChapas.Length; i++) 
        {
            string index = totalChapas[i].GetComponent<ShopItemProperties>().itemIndex.ToString();
            string useButton = totalChapas[i].GetComponent<ShopItemProperties>().useButton;
            string shopItem = "Chapa-" + index;

			Debug.Log (shopItem);
          
            if (PlayerPrefs.GetInt(shopItem) == 1)
            {
                //Encontramos el boton y lo desactivamos
                GameObject BuyButton = GameObject.Find(index.ToString());
                BuyButton.SetActive(false);
                //Encontramos el boton de usar y lo activamos
                GameObject ActiveButton = GameObject.Find(useButton);
                ActiveButton.GetComponent<Button>().interactable = true;
            }
        }

            for (int j = 0; j < totalAuras.Length; j++)
            {
                string indexAura = totalAuras[j].GetComponent<ShopItemProperties>().itemIndex.ToString();
                string useButtonAura = totalAuras[j].GetComponent<ShopItemProperties>().useButton;
                string shopItemAura = "Aura-" + indexAura;
			Debug.Log (shopItemAura);
                if (PlayerPrefs.GetInt(shopItemAura) == 1)
                {
                    //Encontramos el boton y lo desactivamos
                    GameObject BuyButton = GameObject.Find(indexAura.ToString());
                    BuyButton.SetActive(false);
                    //Encontramos el boton de usar y lo activamos
                    GameObject ActiveButton = GameObject.Find(useButtonAura);
                    ActiveButton.GetComponent<Button>().interactable = true;
                }
            }

    }
    void Start()
    {
       GameObject.Find("Chapas").SetActive(false);
       GameObject.Find("Auras").SetActive(false);
       GameObject.Find("Formaciones").SetActive(false);
       GameObject.Find("PanelChapas").SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {

    }

    //******************************
    //Metodo cuando hace click boton comprar
    //******************************
    public void comprarChapa(int index)
    {
        precioItem=totalChapas[index].GetComponent<ShopItemProperties>().itemPrice;
        nombreBoton = totalChapas[index].GetComponent<ShopItemProperties>().useButton;

        if (dinero >= precioItem)
                {
                //Decrementamos el dinero 
                dinero = dinero - precioItem;
                PlayerPrefs.SetInt("PlayerMoney", dinero);

                //Guardamos el objeto
                PlayerPrefs.SetInt("Chapa-" + index.ToString(),1);

                //Encontramos el boton y lo desactivamos
                GameObject BuyButton = GameObject.Find(index.ToString());
                BuyButton.SetActive(false);
                //Encontramos el boton de usar y lo activamos
                GameObject ActiveButton = GameObject.Find(nombreBoton);
                ActiveButton.GetComponent<Button>().interactable = true;
       
            }
        }

    public void comprarAura(int index)
    {
        precioItem = totalAuras[index].GetComponent<ShopItemProperties>().itemPrice;
        nombreBoton = totalAuras[index].GetComponent<ShopItemProperties>().useButton;

        if (dinero >= precioItem)
        {
            //Decrementamos el dinero 
            dinero = dinero - precioItem;
            PlayerPrefs.SetInt("PlayerMoney", dinero);

            //Guardamos el objeto
            PlayerPrefs.SetInt("Aura-" + index.ToString(),1);

            //Encontramos el boton y lo desactivamos
            GameObject BuyButton = GameObject.Find(index.ToString());
            BuyButton.SetActive(false);
            //Encontramos el boton de usar y lo activamos
            GameObject ActiveButton = GameObject.Find(nombreBoton);
            ActiveButton.GetComponent<Button>().interactable = true;

        }
    }

   public void setIndex(int index)
    {
        //Debug
        Debug.Log(index);
        PlayerPrefs.SetInt("Skin", index);
    }


        }
       

    

 


            
