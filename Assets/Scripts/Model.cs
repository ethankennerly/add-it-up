using UnityEngine;  // Mathf
using System;  // Array
using System.Collections.Generic;  // List

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
	private int problemLineMax = 4;
	private int problemLineMin = 2;
	private string state = "";
	private int score = 10;
	private int sum = 0;
	private List<int> remains = new List<int>();

	public void Start()
	{
		state = "start";
	}

	private void Populate()
	{
		remains.Clear();
		int min = score / 5;
		int range = score - min;
		sum = (int) (Deck.Random() * range + min);
		int problemLineCount = (int) Mathf.Floor(Mathf.Pow(score, 0.1f));
		problemLineCount = Mathf.Max(problemLineMin, Mathf.Min(problemLineCount, problemLineMax));
		int step;
		int remaining = sum;
		for (int index = 0; index < problemLineCount - 1; index++) {
			min = remaining / 5;
			min = Mathf.Max(1, min);
			range = (int) (0.5f * remaining - min);
			step = (int) (Deck.Random() * range + min);
			remaining -= step;
			remains.Add(step);
		}
		remains.Add(remaining);
		Deck.ShuffleList(remains);
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

	private string FormatProblem()
	{
		string problem = "";
		for (int index = problemLineMax - 1; 0 <= index; index--) {
			if (index < remains.Count) {
				problem += remains[index];
			}
			problem += "\n";
		}
		return problem;
	}

	private string Format()
	{
		string formatted;
		problem = FormatProblem();
		footer = "ENTER\n" + entry
			+ "\nSCORE\n" + score;
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

	private void Evaluate()
	{
		var amount = Toolkit.ParseInt(entry);
		if (sum == amount) {
			score += sum;
			Populate();
		}
		else {
			score -= 10;
		}
	}

	private void Submit()
	{
		if ("start" == state) {
			state = "play";
			Populate();
		}
		else if ("" != entry) {
			Evaluate();
		}
		entry = "";
	}
}
