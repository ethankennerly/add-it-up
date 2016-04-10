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
	public void IsSolveSomeDigits()
	{
		Model model = new Model();
		List<int> remains = new List<int>(){
		      4305,
		     56943,
		    304056,
		   3059624
		};
		model.SetRemains(remains);
		model.entry = "18";
		Assert.AreEqual(true, model.IsSolveSomeDigits());
		List<int> expects = new List<int>(){
		      4310,
		     56940,
		    304050,
		   3059628
		};
		CollectionAssert.AreEqual(expects,
			model.GetRemains());
	}
}

