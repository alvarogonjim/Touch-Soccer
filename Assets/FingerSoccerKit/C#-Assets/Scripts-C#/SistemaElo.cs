using UnityEngine;
using System.Collections;

public class SistemaElo : MonoBehaviour {
    private int K;

    /* ******DESCOMENTAR AQUI Y AL FINAL DEL TODO****
	// Use this for initialization
	void Start () {

        //SI EL JUGADOR NO HA JUGADO NINGUNA PARTIDA DEJAREMOS POR DEFECTO QUE EMPIECE EN BRONCE CON UN ELO DE 1000 PUNTOS. NO PUEDE HABER UN ELO INFERIOR A 1000.

        Rb = //ELO ACTUAL DEL JUGADOR B
        Ra = //ELO ACTUAL DEL JUGADOR A
        K = 40; //FACTOR K PUEDE VARIAR ENTRE 25 Y 60
	}
	
	

    //Espectativa del Elo del jugador A contra el B
    public int getEloA()
    { 
        return 1/(1+10^((Rb-Ra)/400));

    }
    //Espectativa del Elo del jugador B contra el A
    public int getEloB()
    {
        return 1 / (1 + 10 ^ ((Ra - Rb) / 400));

    }
    //Conseguir el nuevo ELO del jugador A
    public int getRa()
    {
        //En caso de que el jugador A haya ganado:
        if()
        Ra = Ra + K * (1 - getEloA());

        //En caso de que el jugador A haya perdido:
        else()
        Ra = Ra - K * (0 - getEloA());

        //En caso de que el jugador A haya empatado:
        else()
        Ra = Ra - K * (0.5 - getEloA());

    }
    //Conseguir el nuevo ELO del jugador B
    public int getRb()
    {
        //En caso de que el jugador B haya ganado:
        if ()
            Rb = Rb + K * (1 - getEloB());

        //En caso de que el jugador A haya perdido:
        else()
        Rb = Rb - K * (0 - getEloA());

        //En caso de que el jugador A haya empatado:
        else()
        Rb = Rb - K * (0.5 - getEloA());

    }

    //Determinar la liga en la que esta el jugador segun el ELO:

    public string getLiga(int R)
    {
        if(R >= 1000 && R < 1800)
        {
            //Estas en bronze
            return "Bronze";
        }
        else if (R >= 1800 && R < 2300)
        {
            //Estas en Plata
            return "Plata";
        }
        else if (R >= 2300 && R < 2600)
        {
            //Estas en Oro
            return "Plata";
        }
        else if (R >= 2600)
        {
            //Estas en Diamante
            return "Diamante";
        }
    }
    */
}
