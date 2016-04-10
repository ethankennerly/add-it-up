using UnityEngine;  // Mathf
using System;  // Array
using System.Collections.Generic;  // List

public class Model
{
	public ViewModel view = new ViewModel();
	public bool isVerbose = false;
	private string[] text = new string[]{"Canvas", "Text"};
	private string[] digits = new string[]{
		"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
	};
	public string entry = "";
	private string page = "";
	private string footer = "";
	private string problem = "";
	private int lineMax = 9;
	private int trialMax = 20;
	private int trialCount = 0;
	private int problemLineMax = 4;
	private int problemLineMin = 2;
	private string state = "";
	private int sessionCount = 0;
	private int score = 10;
	private int scoreMax = 999999999;
	private int sum = 0;
	private int amount = 0;
	private List<int> remains = new List<int>();

	public void Start()
	{
		state = "start";
	}

	public List<int> GetRemains()
	{
		return remains;
	}

	public void SetRemains(List<int> numbers)
	{
		remains = numbers;
		sum = 0;
		for (int index = 0; index < remains.Count; index++) {
			sum += remains[index];
		}
	}

	public int GetSum()
	{
		return sum;
	}

	private void Populate()
	{
		remains.Clear();
		int min = score / 5;
		min = Mathf.Max(2, min);
		int range = (int) (0.5f * score) - min;
		sum = (int) (Deck.Random() * range + min);
		float problemLinePower = // 0.1f;
					// 0.125f;
					0.15f;
					// 0.25f;
		int problemLineCount = (int) Mathf.Floor(Mathf.Pow(score, problemLinePower));
		problemLineCount = Mathf.Max(problemLineMin, Mathf.Min(problemLineCount, problemLineMax));
		int step;
		int remaining = sum;
		for (int index = 0; index < problemLineCount - 1; index++) {
			min = remaining / (index + 2);
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
		int remainsStart = problemLineMax - remains.Count;
		for (int index = 0; index < problemLineMax; index++) {
			int remainsIndex = index - remainsStart;
			if (0 <= remainsIndex) {
				problem += remains[remainsIndex];
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

	private float timeSincePenalty = 0.0f;
	private float penaltyInterval = 1.0f;

	private void UpdatePenalty(float deltaTime, bool isActive)
	{
		if (isActive) {
			timeSincePenalty += deltaTime;
			if (penaltyInterval <= timeSincePenalty) {
				timeSincePenalty -= penaltyInterval;
				score = (int) (score * 0.9f);
			}
		}
		else {
			timeSincePenalty = 0.0f;
		}
	}

	public void Update(float deltaTime)
	{
		UpdatePenalty(deltaTime, "play" == state);
		if ("start" == state) {
			int sessionLoop = 3;
			int sessionIndex = sessionCount % sessionLoop;
			if (0 == sessionIndex) {
				page = "ADD1TUP\nPRESS\nENTEROR\nSPACEKEY"
					+ "\nFOR" + trialMax + "MORE\n"
					+ "\nSCORE\n" + score;
			}
			if (1 == sessionIndex) {
				page = "ADD1TUP\nPRESS\nSPACEKEY\nORENTER"
					+ "\nFOR" + trialMax + "MORE\nGOODWORK"
					+ "\nSCORE\n" + score;
			}
			else {
				page = "ADD1TUP\n\nWOW"
					+ "\nPRESS\nENTER\n"
					+ "\nSCORE\n" + score;
			}
		}
		else {
			page = Format();
		}
		SetText(text, page);
	}

	// Return if any combination of 2 or more digits.
	// If so, then replace those digits with the amount.
	// If no room:  Replace zeroes.
	// If no more zeroes, add to lowest digit.
	public bool IsSolveSomeDigits(int amount)
	{
		if (0 == amount) {
			return false;
		}
		float log10 = Mathf.Log10(amount);
		int digitCount = (int) (log10 + 1);
		int remainsCount = remains.Count;
		int sum = 0;
		for (int digit = 1; digit <= digitCount; digit++) {
			sum = 0;
			int multiple = (int) Mathf.Pow(10, digit);
			for (int row = 0; row < remainsCount; row++) {
				int remain = remains[row];
				int part = remain % multiple;
				sum += part;
				if (sum == amount) {
					for (int rowReplace = 0; rowReplace <= row; rowReplace++) {
						remain = remains[rowReplace];
						part = remain % multiple;
						remains[rowReplace] -= part;
					}
					for (int replaceDigit = 1; replaceDigit <= digitCount; replaceDigit++) {
						multiple = (int) Mathf.Pow(10, replaceDigit);
						part = amount % multiple;
						amount -= part;
						remains[remainsCount - 1] += part;
					}
					return true;
				}
			}
		}
		return false;
	}

	private void Evaluate()
	{
		amount = Toolkit.ParseInt(entry);
		if (sum == amount) {
			score += sum;
			Next();
		}
		else if (!IsSolveSomeDigits(amount)) {
			score = (int) (score * 0.75f);
		}
	}

	private void Next()
	{
		if (trialMax <= trialCount) {
			sessionCount++;
			trialCount = 0;
			state = "start";
		}
		else if (scoreMax <= score) {
			score = scoreMax;
			trialCount = 0;
			state = "start";
		}
		else {
			Populate();
		}
		trialCount++;
	}

	private void Submit()
	{
		if ("start" == state) {
			state = "play";
			Next();
		}
		else if ("" != entry) {
			Evaluate();
		}
		entry = "";
	}
}
