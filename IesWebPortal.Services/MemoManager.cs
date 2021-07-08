using IesWebPortal.Model;
using IesWebPortal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IesWebPortal.Services
{
    public class MemoManager : IMemoManager
    {


        //private static Regex riskregex = new Regex(global::IesWebPortal.Properties.Settings.Default.RiskCapture, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        //private static Regex einiecsregex = new Regex(global::IesWebPortal.Properties.Settings.Default.EiniecsCapture, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        //private static Regex unregex = new Regex(global::IesWebPortal.Properties.Settings.Default.UnCapture, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        //private static Regex pictureregex = new Regex(global::IesWebPortal.Properties.Settings.Default.PictureCapture, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly Regex _riskRegex;
        private readonly Regex _einiecsRegex;
        private readonly Regex _unRegex;
        private readonly Regex _pictureRegex;

        public MemoManager(string riskCapture, string einiecsCapture, string unCapture, string pictureCapture)
        {
            _riskRegex = new Regex(riskCapture, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            _einiecsRegex = new Regex(einiecsCapture, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            _unRegex = new Regex(unCapture, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            _pictureRegex = new Regex(pictureCapture, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        }

        public bool IsRisk(string value)
        {
            return _riskRegex.Match(value).Success;
        }

        public bool IsUN(string value)
        {
            return _unRegex.Match(value).Success;
        }

        public bool IsEniec(string value)
        {
            return _einiecsRegex.Match(value).Success;
        }

        public bool IsPicture(string value)
        {
            return _pictureRegex.Match(value).Success;
        }


        private static MLMemoStruct[] BuildMemoStructs(IEnumerable<string> values, Regex regex, string dicokeyname, string languagekey)
        {
            var q =
                    //(
                    //from i in
                    ((from i in
                          (from i in values.Where(x => !string.IsNullOrEmpty(x))
                           select new { match = regex.Match(i), orginaldescription = i })
                      where i.match.Success
                      select new MLMemoStruct()
                      {
                          ShortName = i.match.Groups[dicokeyname].Value,
                          Language = string.IsNullOrEmpty(languagekey) ? ELanguages.None : i.match.Groups[languagekey].Value.ToELanguages(),
                          Description = i.orginaldescription

                      }
                        ));
            //group i by i.ShortName into a
            //select a).ToDictionary(x => x.Key, y => y.ToArray());
            return q.ToArray();
        }

        public IMLMemoStruct[] GetUN(IEnumerable<string> values)
        {
            return BuildMemoStructs(values, _unRegex, "un", null);
        }
        public IMLMemoStruct[] GetEniecs(IEnumerable<string> values)
        {
            return BuildMemoStructs(values, _einiecsRegex, "einiecs", null).ToArray();
        }

        public IMLMemoStruct[] GetRisks(IEnumerable<string> values)
        {
            return BuildMemoStructs(values, _riskRegex, "risk", "language");
        }

    }
}
