namespace Sitecore.Support.Form.Web.UI.Controls
{
  using Sitecore.Form.Core.Attributes;
  using Sitecore.Form.Web.UI.Controls;
  using Sitecore.Support.Form.UI.Adapters;
  using System.Web.UI;

  [Adapter(typeof(ListControlAdapter)), ValidationProperty("Value")]
  public class RadioList : Sitecore.Form.Web.UI.Controls.RadioList
  {
  }
}
