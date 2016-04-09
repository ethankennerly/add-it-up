public class Model
{
	public ViewModel view = new ViewModel();
	private string[] text = new string[]{"Canvas", "Text"};
	public bool isVerbose = false;

	public void Start()
	{
		SetText(text, "HELLO\nWORLD");
	}

	private void SetText(string[] address, string text)
	{
		ControllerUtil.SetNews(view.news, address, text, "text");
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
	}
}
