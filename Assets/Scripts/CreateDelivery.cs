using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CreateDelivery : MonoBehaviour {

	public Text destinatario;
	public Text remetente;
	public Text date;
	public Text key;

	public void OnCreateDelivery() {
		FirebaseManager.NewDriverDelivery (remetente.text, destinatario.text, date.text.ToString());
	}

	public void OnChangeDelivery() {
		FirebaseManager.ChangeDeliveryStatus(key.text, Table.entregasMotorista, Table.entregasHub);
	}
}
