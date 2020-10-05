﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using PointlessWaymarksCmsData.Html.NoteHtml;

namespace PointlessWaymarksCmsData.Database.Models
{
    public class HistoricNoteContent : IContentId, ITag, IBodyContent, ICreatedAndLastUpdateOnAndBy, IShowInSiteFeed,
        ITitleSummarySlugFolder
    {
        public string BodyContent { get; set; }
        public string BodyContentFormat { get; set; }
        public int Id { get; set; }
        public Guid ContentId { get; set; }
        public DateTime ContentVersion { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public bool ShowInMainSiteFeed { get; set; }
        public string Tags { get; set; }
        public string Folder { get; set; }
        public string Slug { get; set; }

        public string Summary { get; set; }

        [NotMapped] public string Title => $"Notes - {NoteParts.NoteCreatedByAndUpdatedOnString(this)}";
    }
}