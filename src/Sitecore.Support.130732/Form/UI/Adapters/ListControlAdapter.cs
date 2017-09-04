namespace Sitecore.Support.Form.UI.Adapters
{
  using Sitecore;
  using Sitecore.Form.Core.Client.Submit;
  using Sitecore.Form.Core.Configuration;
  using Sitecore.Form.Core.Utility;
  using Sitecore.Forms.Core.Data;
  using Sitecore.Globalization;
  using Sitecore.WFFM.Abstractions.Data;
  using System;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Text.RegularExpressions;
  using System.Web;

  public class ListControlAdapter : Adapter
  {
    public override IEnumerable<string> AdaptToFriendlyListValues(IFieldItem field, string value, bool returnTexts)
    {
      NameValueCollection values;
      Language contentLanguage;
      IEnumerable<string> enumerable = ParametersUtil.XmlToStringArray(value);
      if ((field == null) || !returnTexts)
      {
        return enumerable;
      }
      Match match = Regex.Match(field.LocalizedParameters, "<items>([^<]*)</items>", RegexOptions.IgnoreCase);
      if (!match.Success)
      {
        return enumerable;
      }
      string queries = HttpUtility.UrlDecode(match.Result("$1"));
      if (queries.StartsWith(StaticSettings.SourceMarker))
      {
        queries = new QuerySettings("root", queries.Substring(StaticSettings.SourceMarker.Length)).ToString();
      }
      if ((Context.Request != null) && !string.IsNullOrEmpty(Context.Request.QueryString["la"]))
      {
        contentLanguage = Language.Parse(Context.Request.QueryString["la"]);
      }
      else if (((Context.Language == null) && (Context.ContentLanguage != null)) && (Context.ContentLanguage.ToString() != string.Empty))
      {
        contentLanguage = Context.ContentLanguage;
      }
      else
      {
        contentLanguage = Context.Language;
      }
      using (new LanguageSwitcher(contentLanguage))
      {
        values = QueryManager.Select(QuerySettings.ParseRange(queries));
      }
      List<string> list = new List<string>();
      foreach (string str2 in enumerable)
      {
        if (!string.IsNullOrEmpty(values[str2]))
        {
          list.Add(values[str2]);
        }
      }
      return list;
    }

    public override string AdaptToFriendlyValue(IFieldItem field, string value) =>
        string.Join(", ", new List<string>(this.AdaptToFriendlyListValues(field, value, true)).ToArray());
  }
}
