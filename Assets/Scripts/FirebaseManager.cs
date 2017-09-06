using UnityEngine;
using System.Collections;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public enum Table {
	entregasMotorista,
	entregasHub,
	entregasEntregador,
	entregasTerminadas
}

public class FirebaseManager: MonoBehaviour{

	static public List<DeliveryModel> deliveriesList;

	static public Action dataReaded;
	static DatabaseReference reference;
	static long deliveriesLength = -1;


	private static string projectURL = "https://projetoteste-cbe56.firebaseio.com/";

	// Use this for initialization
	public void Awake () {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl (projectURL);
		reference = FirebaseDatabase.DefaultInstance.RootReference;
		deliveriesList = new List<DeliveryModel> ();
	}


	public static void NewDriverDelivery(string remetente, string destinatario, string date){
		string key = reference.Child (Table.entregasMotorista.ToString()).Push ().Key;
		DeliveryModel delivery = new DeliveryModel(key, remetente, destinatario, date);
		string json = JsonUtility.ToJson (delivery);
		CreateDeliveryFromJson (Table.entregasMotorista, key, json);
//		reference.Child (Table.entregasMotorista.ToString()).Child (key).SetRawJsonValueAsync (json);
	}

	static void CreateDeliveryFromJson(Table table, string key, string json)
	{
		reference.Child (table.ToString()).Child (key).SetRawJsonValueAsync (json);
	}

	public static void ChangeDeliveryStatus(string key, Table from, Table to){
		FirebaseDatabase.DefaultInstance.GetReference (from.ToString()).GetValueAsync ().ContinueWith ((System.Threading.Tasks.Task<DataSnapshot> arg) => {
			if (arg.IsFaulted) {
				Debug.Log("Deu ruim!");
			} else if (arg.IsCompleted) {
				DataSnapshot snapshot = arg.Result;
				deliveriesLength = snapshot.ChildrenCount;

				foreach (var entrega in snapshot.Children){
					if(key == entrega.Key)
					{
						string json = snapshot.Child(key).GetRawJsonValue();
						CreateDeliveryFromJson (to, key, json);
						DeleteFromList(key);
						entrega.Reference.RemoveValueAsync();
						break;
					}
				}
				dataReaded();
			}
		});
	}	

	static void DeleteFromList(string key){
		for (var i = 0; i < deliveriesList.Count; i++) {
			if (key == deliveriesList[i].deliveryID) {
				deliveriesList.RemoveAt (i);
				break;
			}
		}
	}

	public static void MoveAllToMotoristas(){
		FirebaseDatabase.DefaultInstance.GetReference (Table.entregasHub.ToString()).GetValueAsync ().ContinueWith ((System.Threading.Tasks.Task<DataSnapshot> arg) => {
			if (arg.IsFaulted) {
				Debug.Log("Deu ruim!");
			} else if (arg.IsCompleted) {
				DataSnapshot snapshot = arg.Result;
				deliveriesLength = snapshot.ChildrenCount;

				foreach (var entrega in snapshot.Children){
					var key = entrega.Child("deliveryID").Value.ToString();
					ChangeDeliveryStatus(key, Table.entregasHub, Table.entregasMotorista);
				}
			}
		});
	}

	public static void MoveAllToHub(){
		FirebaseDatabase.DefaultInstance.GetReference (Table.entregasMotorista.ToString()).GetValueAsync ().ContinueWith ((System.Threading.Tasks.Task<DataSnapshot> arg) => {
			if (arg.IsFaulted) {
				Debug.Log("Deu ruim!");
			} else if (arg.IsCompleted) {
				DataSnapshot snapshot = arg.Result;
				deliveriesLength = snapshot.ChildrenCount;

				foreach (var entrega in snapshot.Children){
					var key = entrega.Child("deliveryID").Value.ToString();
					ChangeDeliveryStatus(key, Table.entregasMotorista, Table.entregasHub);
				}
			}
		});
	}

	public static void ReadUserData(string table){
		FirebaseDatabase.DefaultInstance.GetReference (table).GetValueAsync ().ContinueWith ((System.Threading.Tasks.Task<DataSnapshot> arg) => {
			if (arg.IsFaulted) {
				Debug.Log("Deu ruim!");
			} else if (arg.IsCompleted) {
				DataSnapshot snapshot = arg.Result;
				deliveriesLength = snapshot.ChildrenCount;

				foreach (var entrega in snapshot.Children){
					var key = entrega.Child("deliveryID").Value.ToString();
					var dest = entrega.Child("destinatario").Value.ToString();
					var rem = entrega.Child("remetente").Value.ToString();
					var date = entrega.Child("deliveryDate").Value.ToString();
					deliveriesList.Add(new DeliveryModel(key, rem, dest, date));
				}
				dataReaded();
			}
		});
	}	

	public void Login()
	{
		
	}
}
	
