/**
 * This script must be in an Editor folder.
 * Test case:  2014-01 JimboFBX expects "using NUnit.Framework;"
 * Got "The type or namespace 'NUnit' could not be found."
 * http://answers.unity3d.com/questions/610988/unit-testing-unity-test-tools-v10-namespace-nunit.html
 */
using System;  // Exception
using System.Collections.Generic;  // List
using System.Threading;
using UnityEngine;
using NUnit.Framework;

[TestFixture]
internal class TestModel
{
	[Test]
	public void SetRemainsGetSum()
	{
		Model model = new Model();
		List<int> remains = new List<int>(){3, 2};
		model.SetRemains(remains);
		Assert.AreEqual(5, model.GetSum());
	}

	[Test]
	public void IsSolveSomeDigitsShort()
	{
		Model model = new Model();
		List<int> remains = new List<int>(){
		        15,
		        24
		};
		model.SetRemains(remains);
		Assert.AreEqual(false, model.IsSolveSomeDigits(0));
		Assert.AreEqual(true, model.IsSolveSomeDigits(9));
		List<int> expects = new List<int>(){
		        10,
		        29
		};
		CollectionAssert.AreEqual(expects,
			model.GetRemains());
	}

	[Test]
	public void IsSolveSomeDigitsLong()
	{
		Model model = new Model();
		List<int> remains = new List<int>(){
		      4305,
		     56943,
		    304056,
		   3059624
		};
		model.SetRemains(remains);
		Assert.AreEqual(false, model.IsSolveSomeDigits(0));
		Assert.AreEqual(true, model.IsSolveSomeDigits(18));
		List<int> expects = new List<int>(){
		      4300,
		     56940,
		    304050,
		   3059638
		};
		CollectionAssert.AreEqual(expects,
			model.GetRemains());
	}
}

