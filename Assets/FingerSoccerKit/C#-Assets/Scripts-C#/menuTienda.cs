using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class menuTienda : MonoBehaviour
{



    public  GameObject[] totalObjetos;
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

        for(int i = 0; i<totalObjetos.Length; i++) 
        {
            string index = totalObjetos[i].GetComponent<ShopItemProperties>().itemIndex.ToString();
            string useButton = totalObjetos[i].GetComponent<ShopItemProperties>().useButton;
            string shopItem = "Objeto-" + index;

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

        precioItem=totalObjetos[index].GetComponent<ShopItemProperties>().itemPrice;
        nombreBoton = totalObjetos[index].GetComponent<ShopItemProperties>().useButton;

        if (dinero >= precioItem)
                {
                //Decrementamos el dinero 
                dinero = dinero - precioItem;
                PlayerPrefs.SetInt("PlayerMoney", dinero);

                //Guardamos el objeto
                PlayerPrefs.SetInt("Objeto-" + index.ToString(),1);

                //Encontramos el boton y lo desactivamos
                GameObject BuyButton = GameObject.Find(index.ToString());
                BuyButton.SetActive(false);
                //Encontramos el boton de usar y lo activamos
                GameObject ActiveButton = GameObject.Find(nombreBoton);
                ActiveButton.GetComponent<Button>().interactable = true;

                //Para cada chapa le metemos la nueva textura.
                foreach (GameObject chapa in chapas)
                {
                    Material mat = Resources.Load("Assets/FingerSoccerKit/Textures/Flags/Spain.png" , typeof(Material)) as Material;
                    chapa.GetComponent<Renderer>().material = mat;
                    
                }

            }


        }
       

    }

 


            
