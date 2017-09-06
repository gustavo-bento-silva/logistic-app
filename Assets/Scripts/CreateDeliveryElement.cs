using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CreateDeliveryElement : MonoBehaviour {

	public GameObject confirmPopUpPrefab;
	public Text remetente;
	public Text destinatario;
	public Text date;
	public Text status;
	public string key;

	public void OnDeliveryClick(){
		ConfirmPopUpController.Instantiate (confirmPopUpPrefab, key, Table.entregasMotorista, Table.entregasHub);
	}

	public static GameObject Instantiate(GameObject prefab, DeliveryModel delivery)
	{
		GameObject go = GameObject.Instantiate (prefab);
		CreateDeliveryElement cd = go.GetComponent<CreateDeliveryElement> ();
		cd.remetente.text = delivery.remetente;
		cd.destinatario.text = delivery.destinatario;
		cd.date.text = delivery.deliveryDate;
		cd.key = delivery.deliveryID;

		DateTime current = System.DateTime.Now;
		string dateStr = delivery.deliveryDate.Replace ("/", "-");
		DateTime date = DateTime.ParseExact (dateStr, "dd-MM-yyyy", null);
		//if today is before the other date
		if (current> date) {
			cd.status.text = DeliveryStatus.Atrasado.ToString ();
		} else {
			cd.status.text = DeliveryStatus.OK.ToString ();
		}
		return go;
	}
}
