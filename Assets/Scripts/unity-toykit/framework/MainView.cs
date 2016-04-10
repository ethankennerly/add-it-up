using UnityEngine;  // MonoBehaviour

public class MainView : MonoBehaviour
{
	public Controller controller = new Controller();
	// Set your game's model
	// Extend or copy to make your own MainView class.

	public void Start ()
	{
		controller.Start();
	}
	
	public void Update ()
	{
		controller.Update(Time.deltaTime);
	}
}
