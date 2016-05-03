using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class menuTienda : MonoBehaviour
{



    public  GameObject[] totalChapas;
    public GameObject[] totalAuras;
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

            if (PlayerPrefs.GetInt(shopItem) == 1)
            {
                //Encontramos el boton y lo desactivamos
                GameObject BuyButton = GameObject.Find(index.ToString());
                BuyButton.SetActive(false);
                //Encontramos el boton de usar y lo activamos
                GameObject ActiveButton = GameObject.Find(useButton);
                ActiveButton.GetComponent<Button>().interactable = true;
            }

            string indexAura = totalAuras[i].GetComponent<ShopItemProperties>().itemIndex.ToString();
            string useButtonAura = totalAuras[i].GetComponent<ShopItemProperties>().useButton;
            string shopItemAura = "Aura-" + index;
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
            PlayerPrefs.SetInt("Aura-" + index.ToString(), 1);

            //Encontramos el boton y lo desactivamos
            GameObject BuyButton = GameObject.Find(index.ToString());
            BuyButton.SetActive(false);
            //Encontramos el boton de usar y lo activamos
            GameObject ActiveButton = GameObject.Find(nombreBoton);
            ActiveButton.GetComponent<Button>().interactable = true;

        }
    }

    public void setIndexChapa(int index)
    {
        //Debug
        Debug.Log(index);
        PlayerPrefs.SetInt("Skin", index);
    }
    public void setIndexAuras(int index)
    {
        //Debug
        Debug.Log(index);
        PlayerPrefs.SetInt("Aura", index);
    }

}
       

    

 


            
