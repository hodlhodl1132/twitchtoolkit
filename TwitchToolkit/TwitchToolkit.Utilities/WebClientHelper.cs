using System;
using System.Linq;
using System.Net;

namespace TwitchToolkit.Utilities;

public class WebClientHelper
{
	public static string UploadString(string[] args, string method = "POST", string[] headers = null)
	{
		if (args == null || args.Length == 0)
		{
			throw new ApplicationException("Specify the URI of the resource to retrieve.");
		}
		string urlParams = "";
		for (int j = 1; j < args.Length; j++)
		{
			urlParams += args[j];
		}
		WebClient client = new WebClient();
		client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
		if (headers != null)
		{
			for (int i = 0; i < headers.Count(); i += 2)
			{
				if (headers[i] != null && headers[i + 1] != null)
				{
					client.Headers.Add(headers[i], headers[i + 1]);
				}
			}
		}
		Helper.Log(client.Headers.ToString());
		Helper.Log(args[0] + "?" + urlParams);
		using WebClient wc = new WebClient();
		wc.Headers[HttpRequestHeader.ContentType] = "application/json";
		Uri uri = new Uri(args[0]);
		return wc.UploadString(uri, method, urlParams);
	}

	public static string UploadData(string[] args, string method = "POST", string[] headers = null)
	{
		if (args == null || args.Length == 0)
		{
			throw new ApplicationException("Specify the URI of the resource to retrieve.");
		}
		string urlParams = "";
		for (int j = 1; j < args.Length; j++)
		{
			urlParams += args[j];
		}
		byte[] urlParamBytes = Helper.LanguageEncoding().GetBytes(urlParams);
		WebClient client = new WebClient();
		client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
		if (headers != null)
		{
			for (int i = 0; i < headers.Count(); i += 2)
			{
				if (headers[i] != null && headers[i + 1] != null)
				{
					client.Headers.Add(headers[i], headers[i + 1]);
				}
			}
		}
		Helper.Log(client.Headers.ToString());
		Helper.Log(args[0] + "?" + urlParams);
		using WebClient wc = new WebClient();
		wc.Headers[HttpRequestHeader.ContentType] = "application/json";
		Uri uri = new Uri(args[0]);
		byte[] HtmlResult = wc.UploadData(args[0], method, urlParamBytes);
		return Helper.LanguageEncoding().GetString(HtmlResult);
	}
}
