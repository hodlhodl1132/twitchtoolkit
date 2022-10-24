using System;
using System.IO;
using System.Net;
using System.Text;

namespace TwitchToolkitDev;

public class RequestState
{
	private const int BUFFER_SIZE = 1024;

	public StringBuilder requestData;

	public byte[] bufferRead;

	public WebRequest request;

	public WebResponse response;

	public Stream responseStream;

	public Func<RequestState, bool> Callback;

	public string jsonString;

	public string urlCalled;

	public object CallbackClass { get; internal set; }

	public RequestState()
	{
		bufferRead = new byte[1024];
		requestData = new StringBuilder("");
		request = null;
		responseStream = null;
	}
}
