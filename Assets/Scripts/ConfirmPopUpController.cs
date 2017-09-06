using UnityEngine;
using System.Collections;

public class ConfirmPopUpController : MonoBehaviour {

	string key;
	Table from;
	Table to;

	public void OnClosePopUp()
	{
		GameObject.Destroy (gameObject);
	}

	public void OnClickOk(){
		FirebaseManager.ChangeDeliveryStatus (key, from, to);
		GameObject.Destroy (gameObject);
	}

	public static GameObject Instantiate(GameObject prefab, string key, Table from, Table to)
	{
		GameObject go = GameObject.Instantiate (prefab);
		ConfirmPopUpController cpc = go.GetComponent<ConfirmPopUpController> ();
		cpc.key = key;
		cpc.from = from;
		cpc.to = to;

		return go;
	}

}
