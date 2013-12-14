using UnityEngine;
using System.Collections;

public class TileSetter : MonoBehaviour {

	public TileImporter.tileList tileType;
	private OTSprite str;

	[ContextMenu ("Parameter")]
	private void Paramater()
	{
		str = GetComponentInChildren<OTSprite>();
		str.frameName = tileType.ToString() + gameObject.name;

	}
}
