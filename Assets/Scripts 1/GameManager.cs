﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public int piedra;
	public int tijera;
	public int papel;

    public enum estadosJuego { ROBA,JUEGACARTA,MAREO,CONSTRUCCION}

    public estadosJuego estadoActual;

    public static GameManager instance; // SINGLETON

    public Dictionary<Card.TIPO, Vector2> recursos;

	public int piedraMareo;
	public int tijeraMareo;
	public int papelMareo;

	public Text textoPiedra;
	public Text textoTijera;
	public Text textoPapel;

	public Text textoPiedraMareo;
	public Text textoTijeraMareo;
	public Text textoPapelMareo;

	private bool botonActivo;

	// Use this for initialization
	void Start () {
        recursos[Card.TIPO.PIEDRA]=new Vector2(piedraMareo,piedra);
        recursos[Card.TIPO.PAPEL] = new Vector2(papelMareo, papel);
        recursos[Card.TIPO.TIJERA] = new Vector2(tijeraMareo, tijera);
        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Se ha detectado más de una instancia");
        }
            
        estadoActual = estadosJuego.JUEGACARTA;
		iniciaJuego ();
	}
	
	// Update is called once per frame
	void Update () {
		cambiaTextos ();
        switch(estadoActual){
            case(estadosJuego.ROBA): break;//ROBACARTA
            case(estadosJuego.JUEGACARTA): break; //JUEGACARTA
            case(estadosJuego.MAREO): break;//MAREO
            case(estadosJuego.CONSTRUCCION): break;//CONSTRUCCION
        }
	}

	private void iniciaJuego () {
		//this.piedra = piedra;
		
		//tijera = 2;

		//piedraMareo = 0;
		//tijeraMareo = 0;
		//papelMareo = 0;

		botonActivo = true;
	}

	private void cambiaTextos () {
        textoPiedra.text = (recursos[Card.TIPO.PIEDRA][1]).ToString();
        textoTijera.text = (recursos[Card.TIPO.TIJERA][1]).ToString();
        textoPapel.text = (recursos[Card.TIPO.PAPEL][1]).ToString();

        textoPiedraMareo.text = (recursos[Card.TIPO.PIEDRA][0]).ToString();
        textoTijeraMareo.text = (recursos[Card.TIPO.TIJERA][0]).ToString();
        textoPapelMareo.text = (recursos[Card.TIPO.PAPEL][0]).ToString();
	}

    public void juegaCarta(Card cartaIA, Card cartaJugador)
    {
        switch (ganador(cartaIA, cartaJugador))
        {
            case -1:
                // jugador pierde
                recursos[cartaIA.tipoCarta] += new Vector2(2, 0);
                recursos[devuelveTipoPerdedor(cartaJugador.tipoCarta)]+=new Vector2(0,-1);

                break;
            case 0:
                recursos[cartaIA.tipoCarta] += new Vector2(1, 0);
                recursos[devuelveTipoPerdedor(cartaJugador.tipoCarta)] += new Vector2(0, -1);
                break;
            case 1:

                switch (cartaJugador.especialidadCarta)
                {
                    case (Card.ESPECIALIDAD.LENTA):
                        recursos[cartaIA.tipoCarta] += new Vector2(1, -2);
                        break; //0,-2 en fase construccion
                    case (Card.ESPECIALIDAD.NORMAL):
                        recursos[cartaIA.tipoCarta] += new Vector2(0, -1);
                        break; // -1,-1 en fase construccion
                    case (Card.ESPECIALIDAD.PRISA): break; //-1,Freeze y roba
                        
                }
                break;
        }
    }

    public void resuelveMareo(Card cartaIA, Card cartaJugador)
    {
        switch (ganador(cartaIA, cartaJugador))
        {
            case -1:
                // jugador pierde
                recursos[cartaIA.tipoCarta] += new Vector2(2, 0);
                //JUGADOR NO QUITA NADA EN FASE MAREO
                

                break;
            case 0:
                recursos[cartaIA.tipoCarta] += new Vector2(1, 0);
                //JUGADOR NO QUITA NADA EN FASE MAREO
                
                break;
            case 1:

                switch (cartaJugador.especialidadCarta)
                {
                    case (Card.ESPECIALIDAD.LENTA):
                        recursos[cartaIA.tipoCarta] += new Vector2(1, 0);
                        break; //0,-2 en fase construccion
                    case (Card.ESPECIALIDAD.NORMAL):
                        //NO SUMA NADA A MAREO
                        break; // -1,-1 en fase construccion
                    case (Card.ESPECIALIDAD.PRISA): break; //-1,Freeze y roba

                }
                break;
                
        }
        estadoActual = estadosJuego.CONSTRUCCION;
    }

    public void resuelveConstruccion(Card cartaIA, Card cartaJugador)
    {
        switch (ganador(cartaIA, cartaJugador))
        {
            case -1:
                // jugador pierde
                recursos[cartaIA.tipoCarta] += new Vector2(-2, 2);
                recursos[devuelveTipoPerdedor(cartaJugador.tipoCarta)] += new Vector2(0, -1);


                break;
            case 0:
                recursos[cartaIA.tipoCarta] += new Vector2(-1, 1);
                recursos[devuelveTipoPerdedor(cartaJugador.tipoCarta)] += new Vector2(0, -1);

                break;
            case 1:

                switch (cartaJugador.especialidadCarta)
                {
                    case (Card.ESPECIALIDAD.LENTA):
                        recursos[cartaIA.tipoCarta] += new Vector2(-1, 1);
                        recursos[devuelveTipoPerdedor(cartaJugador.tipoCarta)] += new Vector2(0, -2);
                        break; //0,-2 en fase construccion
                    case (Card.ESPECIALIDAD.NORMAL):
                        recursos[cartaIA.tipoCarta] += new Vector2(0, -1);
                        //NO SUMA NADA A MAREO
                        break; // -1,-1 en fase construccion
                    case (Card.ESPECIALIDAD.PRISA): break; //-1,Freeze y roba

                }
                break;
                
        }
        estadoActual = estadosJuego.ROBA;
    }

    public int ganador(Card cartaIA, Card cartaJugador)
    {
        int resultado;
        if ((cartaJugador.tipoCarta == Card.TIPO.PAPEL && cartaIA.tipoCarta == Card.TIPO.TIJERA) || (cartaJugador.tipoCarta == Card.TIPO.PIEDRA && cartaIA.tipoCarta == Card.TIPO.PAPEL) ||(cartaJugador.tipoCarta == Card.TIPO.TIJERA && cartaIA.tipoCarta == Card.TIPO.PIEDRA)) {
            resultado = -1;
        } else if ((cartaJugador.tipoCarta == Card.TIPO.PAPEL && cartaIA.tipoCarta == Card.TIPO.PAPEL) || (cartaJugador.tipoCarta == Card.TIPO.PIEDRA && cartaIA.tipoCarta == Card.TIPO.PIEDRA) ||(cartaJugador.tipoCarta == Card.TIPO.TIJERA && cartaIA.tipoCarta == Card.TIPO.TIJERA)) {
            resultado = 0;
        } else {
            resultado = 1;
        }
        return resultado;
    }

    public Card.TIPO devuelveTipoGanador(Card.TIPO tipo)
    {
        Card.TIPO res;
        if (tipo == Card.TIPO.PIEDRA)
        {
            res = Card.TIPO.PAPEL;
        }
        else if (tipo == Card.TIPO.PAPEL)
        {
            res = Card.TIPO.TIJERA;
        }
        else
        {
            res = Card.TIPO.PIEDRA;
        }
        return res;
    }

    public Card.TIPO devuelveTipoPerdedor(Card.TIPO tipo)
    {
        Card.TIPO res;
        if (tipo == Card.TIPO.PIEDRA)
        {
            res = Card.TIPO.TIJERA;
        }
        else if (tipo == Card.TIPO.PAPEL)
        {
            res = Card.TIPO.PIEDRA;
        }
        else
        {
            res = Card.TIPO.PAPEL;
        }
        return res;
    }
}
