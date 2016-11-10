// NULLcode Studio © 2015
// null-code.ru

using UnityEngine;
using System.Collections;
using mvc;

public class PuzzleView : View<PuzzleGameApplication> {

	public int id; //Id of a puzzle
	
	void OnMouseDown()
	{
	    Notify("mouse.clicked", id);	
	}
}
