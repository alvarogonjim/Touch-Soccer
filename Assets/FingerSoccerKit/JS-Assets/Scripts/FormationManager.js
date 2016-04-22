#pragma strict

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

static var formations : int = 5;	//total number of available formations
static var fixedZ : float = -0.5;	//fixed Z position for all units on the selected formation
static var yFixer : float = -0.75;	//if you ever needed to translate all units up or down a little bit, you can do it by
									//tweeking this yFixer variable.


//*****************************************************************************
// Every unit reads it's position from this function.
// Units give out their index and formation and get their exact position.
//*****************************************************************************
static function getPositionInFormation(_formationIndex : int, _UnitIndex : int) : Vector3 {
	var output : Vector3;
	switch(_formationIndex) {
		case 0:
			if(_UnitIndex == 0) output = Vector3(-15, 0 + yFixer, fixedZ);
			if(_UnitIndex == 1) output = Vector3(-10, 5 + yFixer, fixedZ);
			if(_UnitIndex == 2) output = Vector3(-10, -5 + yFixer, fixedZ);
			if(_UnitIndex == 3) output = Vector3(-4.5, 2 + yFixer, fixedZ);
			if(_UnitIndex == 4) output = Vector3(-4.5, -2 + yFixer, fixedZ);
			break;
		
		case 1:
			if(_UnitIndex == 0) output = Vector3(-14, 0 + yFixer, fixedZ);
			if(_UnitIndex == 1) output = Vector3(-9.5, 0 + yFixer, fixedZ);
			if(_UnitIndex == 2) output = Vector3(-7, 3.5 + yFixer, fixedZ);
			if(_UnitIndex == 3) output = Vector3(-7, -3.5 + yFixer, fixedZ);
			if(_UnitIndex == 4) output = Vector3(-3, 0 + yFixer, fixedZ);
			break;
		
		case 2:
			if(_UnitIndex == 0) output = Vector3(-15, 0 + yFixer, fixedZ);
			if(_UnitIndex == 1) output = Vector3(-11, 3.5 + yFixer, fixedZ);
			if(_UnitIndex == 2) output = Vector3(-11, -3.5 + yFixer, fixedZ);
			if(_UnitIndex == 3) output = Vector3(-6, 0 + yFixer, fixedZ);
			if(_UnitIndex == 4) output = Vector3(-3, 0 + yFixer, fixedZ);
			break;
		
		case 3:
			if(_UnitIndex == 0) output = Vector3(-14, 0 + yFixer, fixedZ);
			if(_UnitIndex == 1) output = Vector3(-11, 5.5 + yFixer, fixedZ);
			if(_UnitIndex == 2) output = Vector3(-11, 2 + yFixer, fixedZ);
			if(_UnitIndex == 3) output = Vector3(-11, -2 + yFixer, fixedZ);
			if(_UnitIndex == 4) output = Vector3(-11, -5.5 + yFixer, fixedZ);
			break;
		
		case 4:
			if(_UnitIndex == 0) output = Vector3(-15, 0 + yFixer, fixedZ);
			if(_UnitIndex == 1) output = Vector3(-12.5, 2.5 + yFixer, fixedZ);
			if(_UnitIndex == 2) output = Vector3(-9, 4.5 + yFixer, fixedZ);
			if(_UnitIndex == 3) output = Vector3(-5, 5.5 + yFixer, fixedZ);
			if(_UnitIndex == 4) output = Vector3(-1.5, 5.5 + yFixer, fixedZ);
			break;
	}
	
	return output;
}
