﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using LoveSeat.Interfaces;
using LoveSeat.Support;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LoveSeat
{
    public class ViewResult<T> : ViewResult
    {
        private readonly IObjectSerializer<T> objectSerializer = null;
        private Dictionary<JToken, T> dict = null;
        public ViewResult(HttpWebResponse response, HttpWebRequest request, IObjectSerializer<T> objectSerializer)
            : base(response, request)
        {
            this.objectSerializer = objectSerializer;
            
        }

        public Dictionary<JToken, T> Dictionary
        {
            get
            {
                if (dict != null) return dict;
                dict = new Dictionary<JToken, T>();
                foreach (var row in this.Rows)
                {
                    dict.Add((JToken)row["key"], objectSerializer.Deserialize(row.Value<string>("value")));
                }
                return dict;
            }
        }

        public IEnumerable<T> Items
        {
            get
            {
                if (objectSerializer == null)
                {
                    throw new InvalidOperationException("ObjectSerializer must be set in order to use the generic view.");
                }
                return this.RawValues.Select(item => objectSerializer.Deserialize(item));
            }
        }
    }

    public class ViewResult : IViewResult
    {
        private readonly HttpWebResponse response;
        private readonly HttpWebRequest request;
        private JObject json = null;
        private readonly string responseString;

        public JObject Json { get { return json ?? (json = JObject.Parse(responseString)); } }
        public ViewResult(HttpWebResponse response, HttpWebRequest request)
        {
            this.response = response;
            this.request = request;
            this.responseString = response.GetResponseString();
        }
        /// <summary>
        /// Typically won't be needed.  Provided for debuging assistance
        /// </summary>
        public HttpWebRequest Request { get { return request; } }
        /// <summary>
        /// Typically won't be needed.  Provided for debugging assistance
        /// </summary>
        public HttpWebResponse Response { get { return response; } }
        public HttpStatusCode StatusCode { get { return response.StatusCode; } }

        public string Etag { get { return response.Headers["ETag"]; } }
        public int TotalRows { get
        {
            if (Json["total_rows"] == null) throw new CouchException(request, response, Json["reason"].Value<string>());
            return Json["total_rows"].Value<int>();
        } }
        public int OffSet { get
        {
            if (Json["offset"] == null) throw new CouchException(request, response, Json["reason"].Value<string>());            
            return Json["offset"].Value<int>();
        } }
        public IEnumerable<JToken> Rows { get
        {
            if (Json["rows"] == null) throw new CouchException(request, response, Json["reason"].Value<string>());
            return (JArray)Json["rows"];
        } }
        /// <summary>
        /// Only populated when IncludeDocs is true
        /// </summary>
        public IEnumerable<JToken> Docs
        {
            get
            {
                return (JArray)Json["doc"];
            }
        }
        /// <summary>
        /// An IEnumerable of strings insteda of the IEnumerable of JTokens
        /// </summary>
        public IEnumerable<string> RawRows
        {
            get
            {
                var arry = (JArray)Json["rows"];
                return arry.Select(item => item.ToString());
            }
        }

        public IEnumerable<string> RawValues
        {
            get
            {
                var arry = (JArray)Json["rows"];
                return arry.Select(item => item["value"].ToString());
            }
        }
        public IEnumerable<string> RawDocs
        {
            get
            {
                var arry = (JArray)Json["rows"];
                return arry.Select(item => item["doc"].ToString());
            }
        }
        public string RawString
        {
            get { return responseString; }
        }

        public bool Equals(IListResult other)
        {
            if (string.IsNullOrEmpty(Etag) || string.IsNullOrEmpty(other.Etag)) return false;
            return Etag == other.Etag;
        }

        public override string ToString()
        {
            return responseString;
        }
        /// <summary>
        /// Provides a formatted version of the json returned from this Result.  (Avoid this method in favor of RawString as it's much more performant)
        /// </summary>
        public string FormattedResponse { get { return Json.ToString(Formatting.Indented); } }
    }
}