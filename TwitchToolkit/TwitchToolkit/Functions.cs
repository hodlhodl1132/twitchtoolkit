namespace TwitchToolkit;

public class Functions
{
	public Viewer GetViewer(string username)
	{
		return Viewers.GetViewer(username);
	}

	public string ReturnString()
	{
		return "Hello World!";
	}
}
