using System;  // Array

public class Model
{
	public ViewModel view = new ViewModel();
	public bool isVerbose = true;
	private string[] text = new string[]{"Canvas", "Text"};
	private string[] digits = new string[]{
		"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
	};
	private string entry = "";
	private string page = "";
	private string footer = "";
	private string problem = "";
	private int lineMax = 9;
	private string state = "";
	private int score = 0;

	public void Start()
	{
		state = "start";
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
		if ("\b" == input) {
			RemoveLastDigit();
		}
		else {
			int digit = Array.IndexOf(digits, input);
			if (0 <= digit) {
				InputDigit(input);
			}
		}
	}

	public void InputDigit(string input)
	{
		if (entry.Length < lineMax) {
			entry += input;
		}
	}

	public void RemoveLastDigit()
	{
		if (1 <= entry.Length) {
			entry = entry.Substring(0, entry.Length - 1);
			
		}
	}

	private string Format()
	{
		string formatted;
		problem = "\n\n\n\n\n\n";
		if ("" == entry) {
			footer = "SCORE\n" + score;
		}
		else {
			footer = "ENTER\n" + entry;
		}
		formatted = problem + footer;
		return formatted;
	}

	public void Update()
	{
		if ("start" == state) {
			page = "ADD1TUP\n\n\n\n\nPRESS\nENTEROR\nSPACEKEY";
		}
		else {
			page = Format();
		}
		SetText(text, page);
	}

	private void Submit()
	{
		entry = "";
		if ("start" == state) {
			state = "play";
		}
	}
}
