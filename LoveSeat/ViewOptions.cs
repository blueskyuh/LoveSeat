﻿using System;
using System.Web;
using LoveSeat.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace LoveSeat
{
    public class ViewOptions : IViewOptions
    {
        public JToken Key { get; set; }
        public JToken StartKey { get; set; }
        public string StartKeyDocId { get; set; }
        public JToken EndKey { get; set; }
        public string EndKeyDocId { get; set; }
        public int? Limit { get; set; }
        public int? Skip { get; set; }
        public bool? Reduce { get; set; }
        public bool? Group { get; set; }
        public bool? IncludeDocs { get; set; }
        public bool? InclusiveEnd { get; set; }
        public int? GroupLevel { get; set; }
        public bool? Descending { get; set; }
        public bool? Stale { get; set; }
        public string Etag { get; set; }

        public virtual void SetStartKey(string startKey)
        {
            if (!string.IsNullOrEmpty(startKey))
                StartKey = JToken.FromObject(startKey);
        }
        public virtual void SetEndKey(string endKey)
        {
            if (!string.IsNullOrEmpty(endKey))
                EndKey = JToken.FromObject(endKey);
        }
        public virtual void SetKey(string key)
        {
            if (!string.IsNullOrEmpty(key))
                Key = JToken.FromObject(key);
        }

        public  override string ToString()
        {
            string result = "";
            if (Key != null)
                result += "&key=" + HttpUtility.UrlEncode(Key.ToString(Formatting.None));
            if (StartKey != null)
                result += "&startkey=" + HttpUtility.UrlEncode(StartKey.ToString(Formatting.None));
            if (EndKey != null)
                result += "&endkey=" + HttpUtility.UrlEncode(EndKey.ToString(Formatting.None));
            if (Limit.HasValue)
                result += "&limit=" + Limit.Value.ToString();
            if (Skip.HasValue)
                result += "&skip=" + Skip.Value.ToString();
            if (Reduce.HasValue)
                result += "&reduce=" + Reduce.Value.ToString().ToLower();
            if (Group.HasValue)
                result += "&group=" + Group.Value.ToString().ToLower();
            if (IncludeDocs.HasValue)
                result += "&include_docs=" + IncludeDocs.Value.ToString().ToLower();
            if (InclusiveEnd.HasValue)
                result += "&inclusive_end=" + InclusiveEnd.Value.ToString().ToLower();
            if (GroupLevel.HasValue)
                result += "&group_level=" + GroupLevel.Value.ToString();
            if (Descending.HasValue)
                result += "&descending=" + Descending.Value.ToString().ToLower();
            if (Stale.HasValue && Stale.Value)
                result += "&stale=ok";
            if (string.IsNullOrEmpty(StartKeyDocId))
                result += "&startkey_docid=" + StartKeyDocId;
            if (string.IsNullOrEmpty(EndKeyDocId))
                result += "&endkey_docid=" + EndKeyDocId;
            return result.Length < 1 ? "" :  "?" + result.Substring(1);
        }
    }
   

}