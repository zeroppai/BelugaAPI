using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Codeplex.Data;

namespace BelugaSample
{
	[DataContract]
	public class TimeLine
	{
		[DataMember]
		public string api_id { get; set; }
		[DataMember]
		public string application { get; set; }
		[DataMember]
		public string braches_count { get; set; }
		[DataMember]
		public string created_at { get; set; }
		[DataMember]
		public string created_at_milliseconds { get; set; }
		[DataMember]
		public string date_string { get; set; }
		[DataMember]
		public bool favorited { get; set; }
		[DataMember]
		public string favorites_count { get; set; }
		[DataMember]
		public bool flagged { get; set; }
		[DataMember]
		public string id { get; set; }
		[DataMember]
		public string in_comment_to_status_id { get; set; }
		[DataMember]
		public string in_reply_to_status_id { get; set; }
		[DataMember]
		public bool mobile { get; set; }
		[DataMember]
		public string room { get; set; }
		[DataMember]
		public string room_id { get; set; }
		[DataMember]
		public bool sage { get; set; }
		[DataMember]
		public bool secret { get; set; }
		[DataMember]
		public string text { get; set; }
		[DataMember]
		public string type { get; set; }
		[DataMember]
		public string user { get; set; }
		[DataMember]
		public string user_id { get; set; }
	}

    class Beluga
    {
        private String app_id = "";
        private String app_secret = "";
		public String user_id { get; set; }
		public String user_token { get; set; }
		public String last_id { get; set; }

		public Beluga()
		{
			//トークン等を組み込む場合はここみ直接書く
			user_id = "";
			user_token = "";
		}

		private String getJsonAPI(String url)
		{
			String result = "";
			WebRequest req = WebRequest.Create(url);
			WebResponse rsp = req.GetResponse();
			Stream stm = rsp.GetResponseStream();
			if (stm != null)
			{
				StreamReader reader = new StreamReader(stm, System.Text.Encoding.GetEncoding("Shift_JIS"));
				result += reader.ReadToEnd();
				stm.Close();
			}
			rsp.Close();
			return result;
		}

		public List<TimeLine> JsonToTimeline(String json)
		{
			var items = DynamicJson.Parse(json);
			List<TimeLine> list = new List<TimeLine>();
			foreach (var item in items)
			{
				TimeLine tl = new TimeLine();
				tl.id = item.id;
				tl.text = item.text;
				tl.user = item.user.name;
				tl.user_id = item.user.id;
				list.Add(tl);
			}

			if(list.Count > 0)
				this.last_id = list.Last<TimeLine>().id;

			return list;
		}

		public List<TimeLine> getHome(String since_id = "0")
		{
			String url = "http://api.beluga.fm/1/statuses/home"
				+ "?app_id=" + app_id + "&app_secret=" + app_secret
				+ "&user_id=" + user_id + "&user_token=" + user_token
				+ "&since_id=" + since_id;

			return this.JsonToTimeline(this.getJsonAPI(url));
		}

		public List<TimeLine> getRoom(String room_hash, String since_id = "0")
		{
			String url = "http://api.beluga.fm/1/statuses/room"
				+ "?app_id=" + app_id + "&app_secret=" + app_secret
				+ "&user_id=" + user_id + "&user_token=" + user_token
				+ "&room_hash=" + room_hash + "&since_id=" + since_id;
			return this.JsonToTimeline(this.getJsonAPI(url));
		}

		public List<TimeLine> postText(String text,String room_hash)
		{
			String url = "http://api.beluga.fm/1/statuses/update"
				+ "?app_id=" + app_id + "&app_secret=" + app_secret
				+ "&user_id=" + user_id + "&user_token=" + user_token
				+ "&room_hash=" + room_hash + "&text=" + text;
			return this.JsonToTimeline(this.getJsonAPI(url));
		}
    }
}
