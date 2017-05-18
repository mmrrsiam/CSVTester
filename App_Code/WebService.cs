using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{

    public WebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string GetCSVFile(string base64)
    {
        var RetVal = new List<List<string>>() ;
        var csv = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        var Data = new List<List<string>>();
        foreach (var c in csv.Split('\n'))
            if(c!="") Data.Add(c.Split(',').ToList());
        var cnts = (from r in Data.Skip(1)
                    group r by r[0]
                        into g
                    select new { Name = g.Key, Count = g.Count() }).OrderByDescending(x=>x.Count).ToList();
        var adds = (from r in Data.Skip(1) select new { Address = r[2] }).OrderBy(x=>x.Address.Substring(x.Address.IndexOf(' '))).ToList();

        dynamic Ox = new System.Dynamic.ExpandoObject(); 
        Ox.file1 = cnts;
        Ox.file2 = adds;
        var o=new JavaScriptSerializer().Serialize(Ox);

        return o;
    }

}
