using UnityEngine;
using System.Collections;

public class TestController : MonoBehaviour {

	public void onClickMotoristas()
	{
		FirebaseManager.MoveAllToMotoristas ();
	}
	public void onClickHub()
	{
		FirebaseManager.MoveAllToHub ();
	}
}
