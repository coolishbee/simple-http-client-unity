using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoolishHttp
{
	public class HttpResponse
	{
		public string Url { get; set; }
		public bool IsSuccessful { get; set; }
		public bool IsHttpError { get; set; }
		public bool IsNetworkError { get; set; }
		public long StatusCode { get; set; }
		public byte[] Bytes { get; set; }
		public string Text { get; set; }
		public string Error { get; set; }
	}

}