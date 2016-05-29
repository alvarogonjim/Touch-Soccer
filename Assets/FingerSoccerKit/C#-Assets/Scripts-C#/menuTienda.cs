﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class menuTienda : MonoBehaviour
{



    public GameObject[] totalChapas;
    public GameObject[] totalAuras;
    public GameObject[] totalFormaciones;
    public GameObject[] totalCampos;
    private static GameObject[] chapas;
    public int precioItem;
    public int precioCreditos;
    public static int dinero = 500;
    public static string nombreBoton;
    public static int creditos;

    //public GameObject banner;

    // Use this for initialization
    void Awake()
    {
        int dinero = PlayerPrefs.GetInt("PlayerMoney");
        int creditos = PlayerPrefs.GetInt("PlayerCredits");

    }
    void Start()
    {
        StartCoroutine("ocultarPaneles");
        //	banner.SetActive (true);
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
        precioItem = totalChapas[index].GetComponent<ShopItemProperties>().itemPrice;
        nombreBoton = totalChapas[index].GetComponent<ShopItemProperties>().useButton;

        if (dinero >= precioItem)
        {
            //Decrementamos el dinero 
            dinero = dinero - precioItem;
            PlayerPrefs.SetInt("PlayerMoney", dinero);

            //Guardamos el objeto
            PlayerPrefs.SetInt("Chapa-" + index.ToString(), 1);

            //Encontramos el boton y lo desactivamos
            GameObject BuyButton = GameObject.Find(index.ToString());
            BuyButton.SetActive(false);
            //Encontramos el boton de usar y lo activamos
            GameObject ActiveButton = GameObject.Find(nombreBoton);
            ActiveButton.GetComponent<Button>().interactable = true;

        }
    }
    public void comprarChapaCreditos(int index)
    {
        precioCreditos = totalChapas[index].GetComponent<ShopItemProperties>().itemCredit;
        nombreBoton = totalChapas[index].GetComponent<ShopItemProperties>().useButton;

        if (creditos >= precioCreditos)
        {
            //Decrementamos el dinero 
            creditos = creditos - precioCreditos;
            PlayerPrefs.SetInt("PlayerCredits", creditos);

            //Guardamos el objeto
            PlayerPrefs.SetInt("Chapa-" + index.ToString(), 1);

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
    public void comprarAuraCreditos(int index)
    {
        precioCreditos = totalAuras[index].GetComponent<ShopItemProperties>().itemCredit;
        nombreBoton = totalAuras[index].GetComponent<ShopItemProperties>().useButton;

        if (creditos >= precioCreditos)
        {
            //Decrementamos el dinero 
            creditos = creditos - precioCreditos;
            PlayerPrefs.SetInt("PlayerCredits", creditos);

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



    public void comprarFormacion(int index)
    {
        precioItem = totalFormaciones[index].GetComponent<ShopItemProperties>().itemPrice;
        nombreBoton = totalFormaciones[index].GetComponent<ShopItemProperties>().useButton;

        if (dinero >= precioItem)
        {
            //Decrementamos el dinero 
            dinero = dinero - precioItem;
            PlayerPrefs.SetInt("PlayerMoney", dinero);

            //Guardamos el objeto
            PlayerPrefs.SetInt("Formacion-" + index.ToString(), 1);

            //Encontramos el boton y lo desactivamos
            GameObject BuyButton = GameObject.Find(index.ToString());
            BuyButton.SetActive(false);
            //Encontramos el boton de usar y lo activamos
            GameObject ActiveButton = GameObject.Find(nombreBoton);
            ActiveButton.GetComponent<Button>().interactable = true;

        }
    }

    public void comprarFormacionCreditos(int index)
    {
        precioCreditos = totalFormaciones[index].GetComponent<ShopItemProperties>().itemCredit;
        nombreBoton = totalFormaciones[index].GetComponent<ShopItemProperties>().useButton;

        if (creditos >= precioCreditos)
        {
            //Decrementamos el dinero 
            creditos = creditos - precioCreditos;
            PlayerPrefs.SetInt("PlayerCredits", creditos);

            //Guardamos el objeto
            PlayerPrefs.SetInt("Formacion-" + index.ToString(), 1);

            //Encontramos el boton y lo desactivamos
            GameObject BuyButton = GameObject.Find(index.ToString());
            BuyButton.SetActive(false);
            //Encontramos el boton de usar y lo activamos
            GameObject ActiveButton = GameObject.Find(nombreBoton);
            ActiveButton.GetComponent<Button>().interactable = true;

        }
    }


    public void setIndexFormaciones(int index)
    {
        //Debug
        Debug.Log(index);
        PlayerPrefs.SetInt("Formaciones", index);
    }
    public void comprarCampo(int index)
    {
        precioItem = totalCampos[index].GetComponent<ShopItemProperties>().itemPrice;
        nombreBoton = totalCampos[index].GetComponent<ShopItemProperties>().useButton;

        if (dinero >= precioItem)
        {
            //Decrementamos el dinero 
            dinero = dinero - precioItem;
            PlayerPrefs.SetInt("PlayerMoney", dinero);

            //Guardamos el objeto
            PlayerPrefs.SetInt("Campo-" + index.ToString(), 1);

            //Encontramos el boton y lo desactivamos
            GameObject BuyButton = GameObject.Find(index.ToString());
            BuyButton.SetActive(false);
            //Encontramos el boton de usar y lo activamos
            GameObject ActiveButton = GameObject.Find(nombreBoton);
            ActiveButton.GetComponent<Button>().interactable = true;

        }
    }
    public void comprarCampoCreditos(int index)
    {
        precioCreditos = totalCampos[index].GetComponent<ShopItemProperties>().itemCredit;
        nombreBoton = totalCampos[index].GetComponent<ShopItemProperties>().useButton;

        if (creditos >= precioCreditos)
        {
            //Decrementamos el dinero 
            creditos = creditos - precioCreditos;
            PlayerPrefs.SetInt("PlayerCredits", creditos);

            //Guardamos el objeto
            PlayerPrefs.SetInt("Campo-" + index.ToString(), 1);

            //Encontramos el boton y lo desactivamos
            GameObject BuyButton = GameObject.Find(index.ToString());
            BuyButton.SetActive(false);
            //Encontramos el boton de usar y lo activamos
            GameObject ActiveButton = GameObject.Find(nombreBoton);
            ActiveButton.GetComponent<Button>().interactable = true;

        }
    }
    public void setIndexCampos(int index)
    {
        //Debug
        Debug.Log(index);
        PlayerPrefs.SetInt("Campos", index);
    }

	IEnumerator ocultarPaneles(){

        //Cargamos las chapas
        chapas = playerController.chapas;
        //Cargamos el dinero
        dinero = PlayerPrefs.GetInt("PlayerMoney");
        //Cargamos las chapas compradas

        for (int i = 0; i < totalChapas.Length; i++)
        {
            
            string useButton = totalChapas[i].GetComponent<ShopItemProperties>().useButton;
            string shopItem = "Chapa-" + i;
            Debug.Log(shopItem);

            if (PlayerPrefs.GetInt(shopItem) == 1)
            {
                GameObject BuyButton = GameObject.Find(i.ToString());
                //Encontramos el boton y lo 
                BuyButton.SetActive(false);
                //Encontramos el boton de usar y lo activamos
                GameObject ActiveButton = GameObject.Find(useButton);
                ActiveButton.GetComponent<Button>().interactable = true;
            }
        }
        GameObject.Find("PanelChapas").SetActive(false);
       /*  for (int j = 0; j < totalAuras.Length; j++)
          {
            GameObject[] botonesCompraAuras = GameObject.FindGameObjectsWithTag("botonCompraAura");
        
              string useButtonAura = totalAuras[j].GetComponent<ShopItemProperties>().useButton;
              string shopItemAura = "Aura-" + j;
              Debug.Log(shopItemAura);
            if (PlayerPrefs.GetInt(shopItemAura) == 1)
            {
                //Encontramos el boton y lo desactivamos
                if (botonesCompraAuras[j] != null)
                {
                    GameObject BuyButton = botonesCompraAuras[j];
                    BuyButton.SetActive(false);
                    //Encontramos el boton de usar y lo activamos
                    GameObject ActiveButton = GameObject.Find(useButtonAura);
                    ActiveButton.GetComponent<Button>().interactable = true;
                }
            }
          }
         */ 
        for (int k = 0; k < totalFormaciones.Length; k++)
        {
            string indexFormacion = totalFormaciones[k].GetComponent<ShopItemProperties>().itemIndex.ToString();
            string useButtonFormacion = totalFormaciones[k].GetComponent<ShopItemProperties>().useButton;
            string shopItemFormacion = "Formacion-" + indexFormacion;
            Debug.Log(shopItemFormacion);
            if (PlayerPrefs.GetInt(shopItemFormacion) == 1)
            {
                //Encontramos el boton y lo desactivamos
                GameObject BuyButton = GameObject.Find(indexFormacion.ToString());
                BuyButton.SetActive(false);
                //Encontramos el boton de usar y lo activamos
                GameObject ActiveButton = GameObject.Find(useButtonFormacion);
                ActiveButton.GetComponent<Button>().interactable = true;
            }
        }
        GameObject.Find("PanelFormaciones").SetActive(false);


        for (int l = 0; l < totalCampos.Length; l++)
        {
           GameObject[] botones= GameObject.FindGameObjectsWithTag("botonComprarCampo");
            string indexCampos = totalCampos[l].GetComponent<ShopItemProperties>().itemIndex.ToString();
            string useButtonCampo = totalCampos[l].GetComponent<ShopItemProperties>().useButton;
            string shopItemCampo = "Campo-" + indexCampos;
            Debug.Log(shopItemCampo);
            if (PlayerPrefs.GetInt(shopItemCampo) == 1)
            {
                //Encontramos el boton y lo desactivamos
                GameObject BuyButton = botones[l];
                BuyButton.SetActive(false);
                //Encontramos el boton de usar y lo activamos
                GameObject ActiveButton = GameObject.Find(useButtonCampo);
                ActiveButton.GetComponent<Button>().interactable = true;
            }
        }
        GameObject.Find("PanelCampos").SetActive(false);
        GameObject.Find("PanelCredits").SetActive(false);
		GameObject.Find("PanelMoney").SetActive(false);
		yield return new WaitForSeconds (2);
	}
}






