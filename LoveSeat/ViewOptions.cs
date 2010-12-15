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
        /// <summary>
        /// If you have a complex object as a string set this using a JRaw object()
        /// </summary>
        public KeyOptions Key { get; set; }
        /// <summary>
        /// If you have a complex object as a string set this using a JRaw object()
        /// </summary>
        public KeyOptions StartKey { get; set; }
        public string StartKeyDocId { get; set; }
        /// <summary>
        /// If you have a complex object as a string set this using a JRaw object()
        /// </summary>
        public KeyOptions EndKey { get; set; }
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

        public ViewOptions()
        {
            Key = new KeyOptions();
            StartKey = new KeyOptions();
            EndKey = new KeyOptions();
        }

        public  override string ToString()
        {
            string result = "";
            if ((Key != null) && (Key.Count > 0))
                result += "&key=" + HttpUtility.UrlEncode(Key.ToString());
            if ((StartKey != null) && (StartKey.Count > 0))
                result += "&startkey=" + HttpUtility.UrlEncode(StartKey.ToString());
            if ((EndKey != null) && (EndKey.Count > 0))
                result += "&endkey=" + HttpUtility.UrlEncode(EndKey.ToString());
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
            if (!string.IsNullOrEmpty(StartKeyDocId))
                result += "&startkey_docid=" + StartKeyDocId;
            if (!string.IsNullOrEmpty(EndKeyDocId))
                result += "&endkey_docid=" + EndKeyDocId;
            return result.Length < 1 ? "" :  "?" + result.Substring(1);
        }
    }
   

}