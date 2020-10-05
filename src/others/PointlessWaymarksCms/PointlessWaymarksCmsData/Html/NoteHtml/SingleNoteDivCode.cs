﻿using PointlessWaymarksCmsData.Database;
using PointlessWaymarksCmsData.Database.Models;

namespace PointlessWaymarksCmsData.Html.NoteHtml
{
    public partial class SingleNoteDiv
    {
        public SingleNoteDiv(NoteContent dbEntry)
        {
            DbEntry = dbEntry;

            var settings = UserSettingsSingleton.CurrentSettings();
            SiteUrl = settings.SiteUrl;
            SiteName = settings.SiteName;
            PageUrl = settings.NotePageUrl(DbEntry);

            var db = Db.Context().Result;
        }

        public NoteContent DbEntry { get; set; }

        public string PageUrl { get; set; }

        public string SiteName { get; set; }

        public string SiteUrl { get; set; }
    }
}