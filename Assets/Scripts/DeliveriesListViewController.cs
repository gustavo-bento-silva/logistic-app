using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeliveriesListViewController : MonoBehaviour {

	public GameObject deliveryElement;
	public Transform deliveryListUI;

	FirebaseManager dataBase;
	List<GameObject> elementsList;

	// Use this for initialization
	void Start () {
		FirebaseManager.dataReaded = FillDeliveryList;
		FirebaseManager.ReadUserData (Table.entregasMotorista.ToString());
	}
	
	public void FillDeliveryList(){
		CleanList ();
		foreach (var delivery in FirebaseManager.deliveriesList) {
			GameObject go = CreateDeliveryElement.Instantiate(deliveryElement, delivery);
			go.name = delivery.remetente;
			go.transform.SetParent (deliveryListUI);
			elementsList.Add (go);
		}
	}

	void CleanList()
	{
		if (elementsList != null) {
			foreach (var element in elementsList) {
				GameObject.Destroy (element);
			}
			elementsList.Clear ();
		} else {
			elementsList = new List<GameObject> ();
		}
	}
}
