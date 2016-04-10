public class MainExample : MainView
{
	public void Start()
	{
		controller.SetModel(new ModelExample());
		base.Start();
	}
}
