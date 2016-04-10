using UnityEngine;  // Mathf
using System;  // Array
using System.Collections.Generic;  // Dictionary, List

public class Model : IModel
{
	private ViewModel view;
	private string[] text;
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
	private int score = 50;
	private int scoreMax = 999999999;
	private int sum = 0;
	private int amount = 0;
	private List<int> remains = new List<int>();

	public void SetViewModel(ViewModel viewModel)
	{
		view = viewModel;
	}

	public void Start()
	{
		state = "start";
		view.graph["Canvas"] = new Dictionary<string, object>(){
			{"Text", null}
		};
		text = new string[]{"Canvas", "Text"};
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
		float ratio = // 0.5f;
				0.75f;
				// 1.0f;
		int range = (int) (ratio * score) - min;
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
			min = remaining / (index + 4);
			min = Mathf.Max(1, min);
			range = (int) (0.5f * remaining - min);
			step = (int) (Deck.Random() * range + min);
			remaining -= step;
			remains.Add(step);
		}
		remains.Add(remaining);
		Deck.ShuffleList(remains);
	}

	public void InputString(string input)
	{
		if ("" == input) {
			return;
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

	private void RemoveLastDigit()
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
				float timePenaltyRate = 0.95f;
				score = (int) (score * timePenaltyRate);
			}
		}
		else {
			timeSincePenalty = 0.0f;
		}
	}

	public void Update(float deltaTime)
	{
		InputString(view.inputString);
		UpdatePenalty(deltaTime, "play" == state);
		if ("start" == state) {
			int sessionLoop = 3;
			int sessionIndex = sessionCount % sessionLoop;
			if (0 == sessionIndex) {
				page = "ADD1TUP\nPRESS\nENTEROR\nSPACEKEY"
					+ "\nFOR" + trialMax + "MORE\n"
					+ "\nSCORE\n" + score;
			}
			else if (1 == sessionIndex) {
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
		view.SetText(text, page);
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
		bool isCheat = scoreMax == amount;
		if (sum == amount || isCheat) {
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
