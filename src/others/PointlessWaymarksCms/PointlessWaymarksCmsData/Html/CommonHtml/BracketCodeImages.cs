﻿using System;
using System.Collections.Generic;
using System.Linq;
using PointlessWaymarksCmsData.Database;
using PointlessWaymarksCmsData.Database.Models;
using PointlessWaymarksCmsData.Html.ImageHtml;

namespace PointlessWaymarksCmsData.Html.CommonHtml
{
    public static class BracketCodeImages
    {
        public const string BracketCodeToken = "image";

        public static List<ImageContent> DbContentFromBracketCodes(string toProcess, IProgress<string> progress)
        {
            if (string.IsNullOrWhiteSpace(toProcess)) return new List<ImageContent>();

            progress?.Report("Searching for Image Codes...");

            var resultList = BracketCodeCommon.ContentBracketCodeMatches(toProcess, BracketCodeToken)
                .Select(x => x.contentGuid).Distinct().ToList();

            var returnList = new List<ImageContent>();

            if (!resultList.Any()) return returnList;

            foreach (var loopGuid in resultList)
            {
                var context = Db.Context().Result;

                var dbContent = context.ImageContents.FirstOrDefault(x => x.ContentId == loopGuid);
                if (dbContent == null) continue;

                progress?.Report($"Image Code - Adding DbContent For {dbContent.Title}");

                returnList.Add(dbContent);
            }

            return returnList;
        }

        public static string ImageBracketCode(ImageContent content)
        {
            return $@"{{{{{BracketCodeToken} {content.ContentId}; {content.Title}}}}}";
        }

        /// <summary>
        ///     Processes {{image guid;human_identifier}} with a specified function - best use may be for easily building
        ///     library code.
        /// </summary>
        /// <param name="toProcess"></param>
        /// <param name="pageConversion"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        private static string ImageCodeProcess(string toProcess, Func<SingleImagePage, string> pageConversion,
            IProgress<string> progress)
        {
            if (string.IsNullOrWhiteSpace(toProcess)) return string.Empty;

            progress?.Report("Searching for Image Codes");

            var resultList = BracketCodeCommon.ContentBracketCodeMatches(toProcess, BracketCodeToken);

            if (!resultList.Any()) return toProcess;

            var context = Db.Context().Result;

            foreach (var loopMatch in resultList)
            {
                var dbImage = context.ImageContents.FirstOrDefault(x => x.ContentId == loopMatch.contentGuid);
                if (dbImage == null) continue;

                progress?.Report($"Image Code for {dbImage.Title} processed");
                var singleImageInfo = new SingleImagePage(dbImage);

                toProcess = toProcess.Replace(loopMatch.bracketCodeText, pageConversion(singleImageInfo));
            }

            return toProcess;
        }

        /// <summary>
        ///     This method processes a image code for use with the CMS Gui Previews (or for another local working
        ///     program).
        /// </summary>
        /// <param name="toProcess"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static string ImageCodeProcessForDirectLocalAccess(string toProcess, IProgress<string> progress)
        {
            return ImageCodeProcess(toProcess, page => page.PictureInformation.LocalPictureFigureTag().ToString(),
                progress);
        }

        /// <summary>
        ///     This method processes a image code for use in email.
        /// </summary>
        /// <param name="toProcess"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static string ImageCodeProcessForEmail(string toProcess, IProgress<string> progress)
        {
            return ImageCodeProcess(toProcess, page => page.PictureInformation.EmailPictureTableTag().ToString(),
                progress);
        }

        /// <summary>
        ///     Processes {{image guid;human_identifier}} into figure html with a link to the image page.
        /// </summary>
        /// <param name="toProcess"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static string ImageCodeProcessToFigureWithLink(string toProcess, IProgress<string> progress)
        {
            return ImageCodeProcess(toProcess,
                page => page.PictureInformation.PictureFigureWithCaptionAndLinkToPicturePageTag("100vw").ToString(),
                progress);
        }
    }
}