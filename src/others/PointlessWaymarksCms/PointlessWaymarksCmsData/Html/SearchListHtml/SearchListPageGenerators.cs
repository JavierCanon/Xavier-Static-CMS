﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AngleSharp.Html;
using AngleSharp.Html.Parser;
using PointlessWaymarksCmsData.Database;
using PointlessWaymarksCmsData.Database.Models;
using PointlessWaymarksCmsData.Html.TagListHtml;
using PointlessWaymarksCmsData.Rss;

namespace PointlessWaymarksCmsData.Html.SearchListHtml
{
    public static class SearchListPageGenerators
    {
        public static void WriteAllContentCommonSearchListHtml()
        {
            List<object> ContentList()
            {
                var db = Db.Context().Result;
                var fileContent = db.FileContents.Cast<object>().ToList();
                var photoContent = db.PhotoContents.Cast<object>().ToList();
                var imageContent = db.ImageContents.Where(x => x.ShowInSearch).Cast<object>().ToList();
                var postContent = db.PostContents.Cast<object>().ToList();
                var noteContent = db.NoteContents.Cast<object>().ToList();

                return fileContent.Concat(photoContent).Concat(imageContent).Concat(postContent).Concat(noteContent)
                    .OrderBy(x => ((IContentCommon) x).Title).ToList();
            }

            var fileInfo = UserSettingsSingleton.CurrentSettings().LocalSiteAllContentListFile();

            WriteSearchListHtml(ContentList, fileInfo, "All Content",
                UserSettingsSingleton.CurrentSettings().AllContentRssUrl());
            RssBuilder.WriteContentCommonListRss(ContentList().Cast<IContentCommon>().ToList(),
                UserSettingsSingleton.CurrentSettings().LocalSiteAllContentRssFile(), "All Content");
        }

        public static void WriteFileContentListHtml()
        {
            List<object> ContentList()
            {
                var db = Db.Context().Result;
                return db.FileContents.OrderBy(x => x.Title).Cast<object>().ToList();
            }

            var fileInfo = UserSettingsSingleton.CurrentSettings().LocalSiteFileListFile();

            WriteSearchListHtml(ContentList, fileInfo, "Files", UserSettingsSingleton.CurrentSettings().FileRssUrl());
            RssBuilder.WriteContentCommonListRss(ContentList().Cast<IContentCommon>().ToList(),
                UserSettingsSingleton.CurrentSettings().LocalSiteFileRssFile(), "Files");
        }


        public static void WriteImageContentListHtml()
        {
            List<object> ContentList()
            {
                var db = Db.Context().Result;
                return db.ImageContents.Where(x => x.ShowInSearch).OrderBy(x => x.Title).Cast<object>().ToList();
            }

            var fileInfo = UserSettingsSingleton.CurrentSettings().LocalSiteImageListFile();

            WriteSearchListHtml(ContentList, fileInfo, "Images", UserSettingsSingleton.CurrentSettings().ImageRssUrl());
            RssBuilder.WriteContentCommonListRss(ContentList().Cast<IContentCommon>().ToList(),
                UserSettingsSingleton.CurrentSettings().LocalSiteImageRssFile(), "Images");
        }

        public static void WriteNoteContentListHtml()
        {
            List<object> ContentList()
            {
                var db = Db.Context().Result;
                return db.NoteContents.ToList().OrderByDescending(x => x.Title).Cast<object>().ToList();
            }

            var fileInfo = UserSettingsSingleton.CurrentSettings().LocalSiteNoteListFile();

            WriteSearchListHtml(ContentList, fileInfo, "Notes", UserSettingsSingleton.CurrentSettings().NoteRssUrl());
            RssBuilder.WriteContentCommonListRss(ContentList().Cast<IContentCommon>().ToList(),
                UserSettingsSingleton.CurrentSettings().LocalSiteNoteRssFile(), "Notes");
        }

        public static void WritePhotoContentListHtml()
        {
            List<object> ContentList()
            {
                var db = Db.Context().Result;
                return db.PhotoContents.OrderBy(x => x.Title).Cast<object>().ToList();
            }

            var fileInfo = UserSettingsSingleton.CurrentSettings().LocalSitePhotoListFile();

            WriteSearchListHtml(ContentList, fileInfo, "Photos", UserSettingsSingleton.CurrentSettings().PhotoRssUrl());
            RssBuilder.WriteContentCommonListRss(ContentList().Cast<IContentCommon>().ToList(),
                UserSettingsSingleton.CurrentSettings().LocalSitePhotoRssFile(), "Photos");
        }

        public static void WritePostContentListHtml()
        {
            List<object> ContentList()
            {
                var db = Db.Context().Result;
                return db.PostContents.OrderBy(x => x.Title).Cast<object>().ToList();
            }

            var fileInfo = UserSettingsSingleton.CurrentSettings().LocalSitePostListFile();

            WriteSearchListHtml(ContentList, fileInfo, "Posts", UserSettingsSingleton.CurrentSettings().PostsRssUrl());
            RssBuilder.WriteContentCommonListRss(ContentList().Cast<IContentCommon>().ToList(),
                UserSettingsSingleton.CurrentSettings().LocalSitePostRssFile(), "Posts");
        }

        public static void WriteSearchListHtml(Func<List<object>> dbFunc, FileInfo fileInfo, string titleAdd,
            string rssUrl)
        {
            var htmlModel = new SearchListPage(rssUrl) {ContentFunction = dbFunc, ListTitle = titleAdd};

            var htmlTransform = htmlModel.TransformText();

            var parser = new HtmlParser();
            var htmlDoc = parser.ParseDocument((string) htmlTransform);

            var stringWriter = new StringWriter();
            htmlDoc.ToHtml(stringWriter, new PrettyMarkupFormatter());

            var htmlString = stringWriter.ToString();

            if (fileInfo.Exists)
            {
                fileInfo.Delete();
                fileInfo.Refresh();
            }

            File.WriteAllText(fileInfo.FullName, htmlString);
        }

        public static void WriteTagListAndTagPages(IProgress<string> progress)
        {
            progress?.Report("Tag Pages - Getting Tag Data");
            var tags = Db.TagAndContentList(false, progress).Result;

            var allTags = new TagListPage {ContentFunction = () => tags};

            progress?.Report("Tag Pages - Writing All Tag Data");
            allTags.WriteLocalHtml();

            //Tags is reset - above for tag search we don't include tags from pages that are hidden from search - but to
            //ensure all tags have a page we generate pages from all tags (if an image excluded from search had a unique
            //tag we need a page for the links on that page, excluded from search does not mean 'unreachable'...)
            var pageTags = Db.TagAndContentList(true, progress).Result;

            var loopCount = 0;

            foreach (var loopTags in pageTags)
            {
                loopCount++;

                if (loopCount % 30 == 0)
                    progress?.Report($"Generating Tag Page {loopTags.tag} - {loopCount} of {tags.Count}");

                WriteSearchListHtml(() => loopTags.contentObjects,
                    UserSettingsSingleton.CurrentSettings().LocalSiteTagListFileInfo(loopTags.tag),
                    $"Tag - {loopTags.tag}", string.Empty);
            }
        }
    }
}