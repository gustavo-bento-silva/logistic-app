using UnityEngine;
using System.Collections;
using System;

public enum DeliveryStatus{
	Atrasado,
	OK
}

[Serializable]
public class DeliveryModel : MonoBehaviour {

	public string deliveryID;
	public string remetente;
	public string destinatario;
	public string deliveryDate;

	public DeliveryModel(string deliveryID, string remetente, string destinatario, string deliveryDate) {
		this.deliveryID = deliveryID;
		this.remetente = remetente;
		this.destinatario = destinatario;
		this.deliveryDate = deliveryDate;
	}
}
