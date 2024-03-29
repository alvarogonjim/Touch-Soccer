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
    public int dinero=50000;
    public static string nombreBoton;
    public static int creditos;
    private float NewPrecioAgrandar;
    private float newValueAgrandar;
    private float NewPrecioTurnoExtra;
    private float newValueTurnoExtra;
    private float NewPrecioBarrera;
    private float newValueBarrera;
	private int precioChapaCredito;
	private int precioChapaCoin;
    //public GameObject banner;

    // Use this for initialization
    void Awake()
    {

      

    }
    void Start()
    {
		
		dinero = PlayerPrefs.GetInt("PlayerMoney");
      		creditos = PlayerPrefs.GetInt("PlayerCredits");
//        GameObject.Find("DisponibleAgrandar").GetComponent<Text>().text = PlayerPrefs.GetInt("Agrandar").ToString();
//        GameObject.Find("DisponibleEliminar").GetComponent<Text>().text = PlayerPrefs.GetInt("Eliminar").ToString();
        GameObject.Find("DisponibleBarrera").GetComponent<Text>().text = PlayerPrefs.GetInt("Barrera").ToString();
        GameObject.Find("Dinero").GetComponent<Text>().text = dinero.ToString();
        GameObject.Find("Creditos").GetComponent<Text>().text = creditos.ToString();
   
        StartCoroutine("ocultarPaneles");

        //	banner.SetActive (true);
    }
    // Update is called once per frame
    void Update()
    {
        creditos = PlayerPrefs.GetInt("PlayerCredits");
        
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
            GameObject.Find("Dinero").GetComponent<Text>().text = dinero.ToString();

            //Guardamos el objeto
            PlayerPrefs.SetInt("Chapa-" + index.ToString(), 1);

            //Encontramos el boton y lo desactivamos
            GameObject BuyButton = GameObject.Find("Chapa"+index);
            BuyButton.SetActive(false);


            if (GameObject.Find("ChapaCredito" + index) == true)
            {
                GameObject BuyButton2 = GameObject.Find("ChapaCredito" + index);
                BuyButton2.SetActive(false);
            }

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
            GameObject BuyButton = GameObject.Find("Chapa" + index);
            BuyButton.SetActive(false);


            if (GameObject.Find("ChapaCredito" + index) == true)
            {
                GameObject BuyButton2 = GameObject.Find("ChapaCredito" + index);
                BuyButton2.SetActive(false);
            }

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
            GameObject.Find("Dinero").GetComponent<Text>().text = dinero.ToString();

            //Guardamos el objeto
            PlayerPrefs.SetInt("Aura-" + index.ToString(), 1);

            //Encontramos el boton y lo desactivamos
            GameObject BuyButton = GameObject.Find("Aura"+index);
            BuyButton.SetActive(false);


            if (GameObject.Find("AuraCredito" + index) == true)
            {
                GameObject BuyButton2 = GameObject.Find("AuraCredito" + index);
                BuyButton2.SetActive(false);
            }

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
            GameObject BuyButton = GameObject.Find("Aura" + index);
            BuyButton.SetActive(false);

            if (GameObject.Find("AuraCredito" + index) == true)
            {
                GameObject BuyButton2 = GameObject.Find("AuraCredito" + index);
                BuyButton2.SetActive(false);
            }

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
            GameObject.Find("Dinero").GetComponent<Text>().text = dinero.ToString();

            //Guardamos el objeto
            PlayerPrefs.SetInt("Formacion-" + index.ToString(), 1);

            //Encontramos el boton y lo desactivamos
            GameObject BuyButton = GameObject.Find("Formacion"+index);
            BuyButton.SetActive(false);

            //Encontramos el boton de comprar creditos y lo desactivamos

            if (GameObject.Find("FormacionCredito" + index) == true)
            {
                GameObject BuyButton2 = GameObject.Find("FormacionCredito" + index);
                BuyButton2.SetActive(false); 
            }

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
            GameObject BuyButton = GameObject.Find("Formacion" + index);
            BuyButton.SetActive(false);

            if (GameObject.Find("FormacionCredito" + index) == true)
            {
                GameObject BuyButton2 = GameObject.Find("FormacionCredito" + index);
                BuyButton2.SetActive(false);
            }

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
            GameObject.Find("Dinero").GetComponent<Text>().text = dinero.ToString();

            //Guardamos el objeto
            PlayerPrefs.SetInt("Campo-" + index.ToString(), 1);

            //Encontramos el boton y lo desactivamos
            GameObject BuyButton = GameObject.Find("Campo"+index);
            BuyButton.SetActive(false);


            if (GameObject.Find("CampoCredito" + index) == true)
            {
                GameObject BuyButton2 = GameObject.Find("CampoCredito" + index);
                BuyButton2.SetActive(false);
            }


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
            GameObject BuyButton = GameObject.Find("Campo" + index);
            BuyButton.SetActive(false);

            if (GameObject.Find("CampoCredito" + index) == true)
            {
                GameObject BuyButton2 = GameObject.Find("CampoCredito" + index);
                BuyButton2.SetActive(false);
            }


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
                
                GameObject BuyButton = GameObject.Find("Chapa" + i);
                //Encontramos el boton y lo 
                BuyButton.SetActive(false);


                if (GameObject.Find("ChapaCredito" + i) == true)
                {
                    GameObject BuyButton2 = GameObject.Find("ChapaCredito" + i);
                    BuyButton2.SetActive(false);
                }

                //Encontramos el boton de usar y lo activamos
                GameObject ActiveButton = GameObject.Find(useButton);
                ActiveButton.GetComponent<Button>().interactable = true;
            }
        }
        GameObject.Find("PanelChapas").SetActive(false);
        for (int j = 0; j < totalAuras.Length; j++)
          {
      
        
              string useButtonAura = totalAuras[j].GetComponent<ShopItemProperties>().useButton;
              string shopItemAura = "Aura-" + j;
             
            if (PlayerPrefs.GetInt(shopItemAura) == 1)
            {
                GameObject BuyButton = GameObject.Find("Aura" + j);
                BuyButton.SetActive(false);

                if (GameObject.Find("AuraCredito" + j) == true)
                {
                    GameObject BuyButton2 = GameObject.Find("AuraCredito" + j);
                    BuyButton2.SetActive(false);
                }

                //Encontramos el boton de usar y lo activamos
                GameObject ActiveButton = GameObject.Find(useButtonAura);
                ActiveButton.GetComponent<Button>().interactable = true;
                Debug.Log(shopItemAura);
            }
          }
        GameObject.Find("PanelAuras").SetActive(false); 

        
        
        for (int k = 0; k < totalFormaciones.Length; k++)
        {
            string indexFormacion = totalFormaciones[k].GetComponent<ShopItemProperties>().itemIndex.ToString();
            string useButtonFormacion = totalFormaciones[k].GetComponent<ShopItemProperties>().useButton;
            string shopItemFormacion = "Formacion-" + indexFormacion;
            Debug.Log(shopItemFormacion);
            if (PlayerPrefs.GetInt(shopItemFormacion) == 1)
            {
                //Encontramos el boton y lo desactivamos
                GameObject BuyButton = GameObject.Find("Formacion" + k);
                BuyButton.SetActive(false);

                if (GameObject.Find("FormacionCredito" + k) == true)
                {
                    GameObject BuyButton2 = GameObject.Find("FormacionCredito" + k);
                    BuyButton2.SetActive(false);
                }



                //Encontramos el boton de usar y lo activamos
                GameObject ActiveButton = GameObject.Find(useButtonFormacion);
                ActiveButton.GetComponent<Button>().interactable = true;
            }
        }
        GameObject.Find("PanelFormaciones").SetActive(false);
        
        
        for (int l = 0; l < totalCampos.Length; l++)
        {
            string useButtonCampo = totalCampos[l].GetComponent<ShopItemProperties>().useButton;
            string shopItemCampo = "Campo-" + l;
            Debug.Log(shopItemCampo);
            if (PlayerPrefs.GetInt(shopItemCampo) == 1)
            {
                //Encontramos el boton y lo desactivamos
                GameObject BuyButton = GameObject.Find("Campo" + l);
                BuyButton.SetActive(false);

                if (GameObject.Find("CampoCredito" + l) == true)
                {
                    GameObject BuyButton2 = GameObject.Find("CampoCredito" + l);
                    BuyButton2.SetActive(false);
                }


                //Encontramos el boton de usar y lo activamos
                GameObject ActiveButton = GameObject.Find(useButtonCampo);
                ActiveButton.GetComponent<Button>().interactable = true;
            }
        }
        
        GameObject.Find("PanelCredits").SetActive(false);
		GameObject.Find("PanelMoney").SetActive(false);
        GameObject.Find("PanelCampos").SetActive(false);
        GameObject.Find("PanelHabilidades").SetActive(false);
		yield return new WaitForSeconds (3);
}

    public void Slider_Changed1(float newValue1)
    {
        
        GameObject TextoPrecio = GameObject.Find("TextoPrecioAgrandar");
        float.TryParse(TextoPrecio.GetComponent<Text>().text, out NewPrecioAgrandar);
        NewPrecioAgrandar = 850 * newValue1;
        TextoPrecio.GetComponent<Text>().text = NewPrecioAgrandar.ToString();
        GameObject ValorSlider = GameObject.Find("ValorSliderAgrandar");
        ValorSlider.GetComponent<Text>().text = newValue1.ToString();
        newValueAgrandar = newValue1;
    }

    public void Slider_Changed2(float newValue1)
    {

        GameObject TextoPrecio = GameObject.Find("TextoPrecioTurnoExtra");
        float.TryParse(TextoPrecio.GetComponent<Text>().text, out NewPrecioTurnoExtra);
        NewPrecioTurnoExtra = 1000 * newValue1;
        TextoPrecio.GetComponent<Text>().text = NewPrecioTurnoExtra.ToString();
        GameObject ValorSlider = GameObject.Find("ValorSliderTurnoExtra");
        ValorSlider.GetComponent<Text>().text = newValue1.ToString();
        newValueTurnoExtra = newValue1;
    }

    public void Slider_Changed3(float newValue1)
    {

        GameObject TextoPrecio = GameObject.Find("TextoPrecioBarrera");
        float.TryParse(TextoPrecio.GetComponent<Text>().text, out NewPrecioBarrera);
        NewPrecioBarrera = 1250 * newValue1;
        TextoPrecio.GetComponent<Text>().text = NewPrecioBarrera.ToString();
        GameObject ValorSlider = GameObject.Find("ValorSliderBarrera");
        ValorSlider.GetComponent<Text>().text = newValue1.ToString();
        newValueBarrera = newValue1;
    }


    public void compra500000Coins()
    {
        creditos = PlayerPrefs.GetInt("PlayerCredits");
        if(creditos >= 4000)
        {
            creditos = creditos - 4000;
            PlayerPrefs.SetInt("PlayerCredits", creditos);
            GameObject.Find("Creditos").GetComponent<Text>().text = creditos.ToString();
            dinero = PlayerPrefs.GetInt("PlayerMoney");
            dinero = dinero + 500000;

            PlayerPrefs.SetInt("PlayerMoney", dinero);
            GameObject.Find("Dinero").GetComponent<Text>().text = dinero.ToString();
        }
    }

    public void compra160000Coins()
    {
        creditos = PlayerPrefs.GetInt("PlayerCredits");
        if (creditos >= 1700)
        {
            creditos = creditos - 1700;
            PlayerPrefs.SetInt("PlayerCredits", creditos);
            GameObject.Find("Creditos").GetComponent<Text>().text = creditos.ToString();
            dinero = PlayerPrefs.GetInt("PlayerMoney");
            dinero = dinero + 160000;

            PlayerPrefs.SetInt("PlayerMoney", dinero);
            GameObject.Find("Dinero").GetComponent<Text>().text = dinero.ToString();
        }
    }
    public void compra70000Coins()
    {
        creditos = PlayerPrefs.GetInt("PlayerCredits");
        if (creditos >= 700)
        {
            creditos = creditos - 700;
            PlayerPrefs.SetInt("PlayerCredits", creditos);
            GameObject.Find("Creditos").GetComponent<Text>().text = creditos.ToString();
            dinero = PlayerPrefs.GetInt("PlayerMoney");
            dinero = dinero + 70000;

            PlayerPrefs.SetInt("PlayerMoney", dinero);
            GameObject.Find("Dinero").GetComponent<Text>().text = dinero.ToString();
        }
    }
    public void compra32500Coins()
    {
        creditos = PlayerPrefs.GetInt("PlayerCredits");
        if (creditos >= 300)
        {
            creditos = creditos - 300;
            PlayerPrefs.SetInt("PlayerCredits", creditos);
            GameObject.Find("Creditos").GetComponent<Text>().text = creditos.ToString();
            dinero = PlayerPrefs.GetInt("PlayerMoney");
            dinero = dinero + 32500;

            PlayerPrefs.SetInt("PlayerMoney", dinero);
            GameObject.Find("Dinero").GetComponent<Text>().text = dinero.ToString();
        }
    }
    public void compra12500Coins()
    {
        creditos = PlayerPrefs.GetInt("PlayerCredits");
        if (creditos >= 100)
        {
            creditos = creditos - 100;
            PlayerPrefs.SetInt("PlayerCredits", creditos);
            GameObject.Find("Creditos").GetComponent<Text>().text = creditos.ToString();
            dinero = PlayerPrefs.GetInt("PlayerMoney");
            dinero = dinero + 12500;

            PlayerPrefs.SetInt("PlayerMoney", dinero);
            GameObject.Find("Dinero").GetComponent<Text>().text = dinero.ToString();
        }
    }
    public void compra6200Coins()
    {
        creditos = PlayerPrefs.GetInt("PlayerCredits");
        if (creditos >= 50)
        {
            creditos = creditos - 50;
            GameObject.Find("Creditos").GetComponent<Text>().text = creditos.ToString();
            PlayerPrefs.SetInt("PlayerCredits", creditos);
            dinero = PlayerPrefs.GetInt("PlayerMoney");
            dinero = dinero + 6200;
            PlayerPrefs.SetInt("PlayerMoney", dinero);
            GameObject.Find("Dinero").GetComponent<Text>().text = dinero.ToString();
        }
    }

    public void comprarHabilidad(string nombreHabilidad)
    {
        Debug.Log("ESTAMOS EN LA HABILIDAD");
        if (nombreHabilidad.Equals("Agrandar"))
        {
            Debug.Log("AGRANDAR");
            Debug.Log(NewPrecioAgrandar);
            
            Debug.Log(dinero);
            if (dinero >= (int)NewPrecioAgrandar)
            {
                Debug.Log("Entra");
                //Decrementamos el dinero 
                dinero = dinero - (int)NewPrecioAgrandar;
                PlayerPrefs.SetInt("PlayerMoney", dinero);
                GameObject.Find("Dinero").GetComponent<Text>().text = dinero.ToString();
                int Agrandar = PlayerPrefs.GetInt("Agrandar");
                Agrandar = Agrandar + (int)newValueAgrandar;
                PlayerPrefs.SetInt("Agrandar", Agrandar);
                GameObject.Find("DisponibleAgrandar").GetComponent<Text>().text = PlayerPrefs.GetInt("Agrandar").ToString();
                Debug.Log("compro");
            }
        }
        if (nombreHabilidad.Equals("TurnoExtra"))
        {
            if (dinero >= NewPrecioTurnoExtra)
            {
                //Decrementamos el dinero 
                dinero = dinero - (int)NewPrecioTurnoExtra;
                PlayerPrefs.SetInt("PlayerMoney", dinero);
                int TurnoExtra = PlayerPrefs.GetInt("TurnoExtra");
                GameObject.Find("Dinero").GetComponent<Text>().text = dinero.ToString();
                TurnoExtra = TurnoExtra + (int)newValueTurnoExtra;
                PlayerPrefs.SetInt("TurnoExtra", TurnoExtra);
                GameObject.Find("DisponibleTurnoExtra").GetComponent<Text>().text = PlayerPrefs.GetInt("TurnoExtra").ToString();


            }
        }
        if (nombreHabilidad.Equals("Barrera"))
        {
            if (dinero >= NewPrecioBarrera)
            {
                //Decrementamos el dinero 
                dinero = dinero - (int)NewPrecioBarrera;
                PlayerPrefs.SetInt("PlayerMoney", dinero);
                GameObject.Find("Dinero").GetComponent<Text>().text = dinero.ToString();
                int Barrera = PlayerPrefs.GetInt("Barrera");
                Barrera = Barrera + (int)newValueBarrera;
                PlayerPrefs.SetInt("Barrera", Barrera);
                GameObject.Find("DisponibleBarrera").GetComponent<Text>().text = PlayerPrefs.GetInt("Barrera").ToString();

            }
        }


    }


}


	





