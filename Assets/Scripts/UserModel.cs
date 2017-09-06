using UnityEngine;
using System.Collections;

public class UserModel : MonoBehaviour {

	public string username;
	public string email;

	public UserModel(string username, string email) {
		this.username = username;
		this.email = email;
	}
}
