using System;  // Array

public class Model
{
	public ViewModel view = new ViewModel();
	public bool isVerbose = true;
	private string[] text = new string[]{"Canvas", "Text"};
	private string[] digits = new string[]{
		"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
	};

	public void Start()
	{
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

	public void InputString(string input)
	{
		if (isVerbose) {
			Toolkit.Log("InputString: " + input);
		}
		if (" " == input || "\n" == input) {
			Submit();
		}
		else {
			int digit = Array.IndexOf(digits, input);
			if (0 <= digit) {
				InputDigit(digit);
			}
		}
	}

	public void InputDigit(int digit)
	{
		SetText(text, digit.ToString());
	}

	private void Submit()
	{
		SetText(text, "");
	}
}
