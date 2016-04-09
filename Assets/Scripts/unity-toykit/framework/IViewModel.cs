using System.Collections.Generic;

public class IViewModel
{
	public string main = "Main";

	public Dictionary<string, object> graph = new Dictionary<string, object>(){
		{"Camera", null},
		{"Canvas", null}
	};

	public string[] buttons = new string[]{
	};

	public string[] sounds = new string[]{
	};

	public List<string> soundNews = new List<string>();

	public Dictionary<string, object> news = new Dictionary<string, object>(){
	};
}
