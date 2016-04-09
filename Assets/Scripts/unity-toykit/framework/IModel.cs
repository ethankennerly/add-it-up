public class IModel
{
	public ViewModel view = new ViewModel();
	private string[] camera = new string[]{"camera"};
	private string[] canvas = new string[]{"canvas"};
	public bool isVerbose = false;

	public void Start()
	{
	}

	private void SetState(string[] address, string state)
	{
		ControllerUtil.SetNews(view.news, address, state);
	}

	public void OnMouseDown(string name)
	{
		if (isVerbose) {
			Toolkit.Log("OnMouseDown: " + name);
		}
		if ("button_0" == name) {
			SetState(camera, "waters");
			SetState(canvas, "close");
		}
	}
}
