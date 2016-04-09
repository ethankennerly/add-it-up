using UnityEngine;  // MonoBehaviour

public class MainView : MonoBehaviour
{
	private Controller controller = new Controller();

	void Start ()
	{
		controller.Start();
	}
	
	void Update ()
	{
		controller.Update();
	}
}
