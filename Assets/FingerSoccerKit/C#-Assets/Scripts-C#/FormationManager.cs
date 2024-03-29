using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FormationManager : MonoBehaviour {


	///*************************************************************************///
	/// Main Formation manager.
	/// You can define new formations here.
	/// To define new positions and formations, please do the following:
	/*
	1. add +1 to formations counter.
	2. define a new case in "getPositionInFormation" function.
	3. for all 5 units, define an exact position on Screen. (you can copy a case and edit it's values)
	4. Note. You always set the units position, as if they are on the left side of the field. 
	The controllers automatically process the position of the units, if they belong to the right side of the field.
	*/
	///*************************************************************************///

	// Available Formations:
	/*
	1-2-2
	1-3-1
	1-2-1-1
	1-4-0
	1-1-1-1-1
	*/

	public static int formations = 5;		//total number of available formations
	public static float fixedZ = -0.5f;		//fixed Z position for all units on the selected formation
	public static float yFixer = -0.75f;	//if you ever needed to translate all units up or down a little bit, you can do it by
											//tweeking this yFixer variable.
	//*****************************************************************************
	// Every unit reads it's position from this function.
	// Units give out their index and formation and get their exact position.
	//*****************************************************************************
	public static Vector3 getPositionInFormation ( int _formationIndex ,   int _UnitIndex  ){
		Vector3 output = Vector3.zero;
        int index = PlayerPrefs.GetInt("Formaciones");
        switch (index) {
		case 0:
			
				if(_UnitIndex == 0) output = new Vector3(-18.6f, 1.25f + yFixer, fixedZ);
				if(_UnitIndex == 1) output = new Vector3(-14.03f, 4.04f + yFixer, fixedZ);
				if(_UnitIndex == 2) output = new Vector3(-9.34f, 6.28f + yFixer, fixedZ);
				if(_UnitIndex == 3) output = new Vector3(-14.03f, -1.75f + yFixer, fixedZ);
				if(_UnitIndex == 4) output = new Vector3(-9.34f, -4.58f + yFixer, fixedZ);
				break;
			
		case 1:
			
				if(_UnitIndex == 0) output = new Vector3(-19.51f, 1.97f + yFixer, fixedZ);
				if(_UnitIndex == 1) output = new Vector3(-5.19f, 1.97f + yFixer, fixedZ);
				if(_UnitIndex == 2) output = new Vector3(-15.07f, -2.72f + yFixer, fixedZ);
				if(_UnitIndex == 3) output = new Vector3(-15.01f, 6.24f + yFixer, fixedZ);
				if(_UnitIndex == 4) output = new Vector3(-10.29f, 1.97f + yFixer, fixedZ);
				break;
			
		case 2:
			
				if(_UnitIndex == 0) output = new Vector3(-3, 1.4f + yFixer, fixedZ);
				if(_UnitIndex == 1) output = new Vector3(-3, 9 + yFixer, fixedZ);
				if(_UnitIndex == 2) output = new Vector3(-3, 6.08f + yFixer, fixedZ);
				if(_UnitIndex == 3) output = new Vector3(-3, -3.07f + yFixer, fixedZ);
				if(_UnitIndex == 4) output = new Vector3(-3, -8.31f + yFixer, fixedZ);
				break;
			
		case 3:
				if(_UnitIndex == 0) output = new Vector3(-5.46f, -6 + yFixer, fixedZ);
				if(_UnitIndex == 1) output = new Vector3(-5.46f, 9.37f + yFixer, fixedZ);
				if(_UnitIndex == 2) output = new Vector3(-9.62f, 5.39f + yFixer, fixedZ);
				if(_UnitIndex == 3) output = new Vector3(-5.51f, 1.33f + yFixer, fixedZ);
				if(_UnitIndex == 4) output = new Vector3(-9.62f, -2.26f + yFixer, fixedZ);
				break;
			
		case 4:
				if(_UnitIndex == 0) output = new Vector3(-4.76f, 1.18f + yFixer, fixedZ);
				if(_UnitIndex == 1) output = new Vector3(-11.88f, 1.01f + yFixer, fixedZ);
				if(_UnitIndex == 2) output = new Vector3(-20.15f, 1.17f + yFixer, fixedZ);
				if(_UnitIndex == 3) output = new Vector3(-11.88f, -4.04f + yFixer, fixedZ);
				if(_UnitIndex == 4) output = new Vector3(-11.68f, 6.62f + yFixer, fixedZ);
				break;
		}


        //Flag si tiene Formacion 5


		
		return output;
	}

}