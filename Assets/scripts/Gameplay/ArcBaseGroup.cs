﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArcBaseGroup : MonoBehaviour {
	
	public List<ArcElectric> arcs = new List<ArcElectric>();
	public List<BaseElectric> bases = new List<BaseElectric>();
	public int nbCrates = 0;
	
	void OnTriggerEnter(Collider _other)
	{//print (_other.tag);
		if (_other.CompareTag("Crate"))
		{
			foreach(ArcElectric _arc in arcs) _arc.turnOFF();
			foreach(BaseElectric _base in bases) _base.turnOFF();
			nbCrates++;
		}
	}
	void OnTriggerExit(Collider _other)
	{
		if (_other.CompareTag("Crate"))
		{
			nbCrates--;
			if(nbCrates==0) {
				foreach(ArcElectric _arc in arcs) _arc.turnON();
				foreach(BaseElectric _base in bases) _base.turnON();
			}
		}
	}
}
