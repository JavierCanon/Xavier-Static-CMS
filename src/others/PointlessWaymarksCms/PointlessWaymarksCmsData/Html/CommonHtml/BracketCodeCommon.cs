﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PointlessWaymarksCmsData.Html.CommonHtml
{
    public static class BracketCodeCommon
    {
        /// <summary>
        ///     Returns Bracket Code Information from a string
        /// </summary>
        /// <param name="toProcess"></param>
        /// <param name="bracketCodeToken"></param>
        /// <returns></returns>
        public static List<(string bracketCodeText, Guid contentGuid, string displayText)> ContentBracketCodeMatches(
            string toProcess, string bracketCodeToken)
        {
            var resultList = new List<(string bracketCodeText, Guid contentGuid, string displayText)>();

            if (string.IsNullOrWhiteSpace(toProcess)) return resultList;

            var withTextMatch =
                new Regex(
                    $@"{{{{{bracketCodeToken} (?<siteGuid>[\dA-Za-z-]*);\s*[Tt]ext (?<displayText>[^}};]*);[^}}]*}}}}",
                    RegexOptions.Singleline);
            var noTextMatch = withTextMatch.Match(toProcess);
            while (noTextMatch.Success)
            {
                Guid.TryParse(noTextMatch.Groups["siteGuid"].Value, out var parsedContentId);
                resultList.Add((noTextMatch.Value, parsedContentId, noTextMatch.Groups["displayText"].Value));
                noTextMatch = noTextMatch.NextMatch();
            }

            //Remove the more specific pattern matches before processing the less specific matches, 
            //as currently written there are patterns that can match both.
            foreach (var loopResultList in resultList)
                toProcess = toProcess.Replace(loopResultList.bracketCodeText, string.Empty);

            var regexObj = new Regex($@"{{{{{bracketCodeToken} (?<siteGuid>[\dA-Za-z-]*);[^}}]*}}}}",
                RegexOptions.Singleline);
            var textMatch = regexObj.Match(toProcess);
            while (textMatch.Success)
            {
                Guid.TryParse(textMatch.Groups["siteGuid"].Value, out var parsedContentId);
                resultList.Add((textMatch.Value, parsedContentId, string.Empty));
                textMatch = textMatch.NextMatch();
            }

            return resultList;
        }

        /// <summary>
        ///     Extracts the Guid from the first {{(photo|image) guid;human_identifier}} in the string.
        /// </summary>
        /// <param name="toProcess"></param>
        /// <returns></returns>
        public static Guid? PhotoOrImageCodeFirstIdInContent(string toProcess)
        {
            if (string.IsNullOrWhiteSpace(toProcess)) return null;

            var regexObj = new Regex(@"{{(?:photo|image) (?<siteGuid>[\dA-Za-z-]*);[^}]*}}", RegexOptions.Singleline);
            var matchResult = regexObj.Match(toProcess);
            if (matchResult.Success) return Guid.Parse(matchResult.Groups["siteGuid"].Value);

            return null;
        }

        public static string ProcessCodesForEmail(string input, IProgress<string> progress)
        {
            input = BracketCodeFileDownloads.FileDownloadLinkCodeProcess(input, progress);
            input = BracketCodeFiles.FileLinkCodeProcess(input, progress);
            input = BracketCodeImages.ImageCodeProcessForEmail(input, progress);
            input = BracketCodeImageLinks.ImageLinkCodeProcess(input, progress);
            input = BracketCodeNotes.NoteLinkCodeProcess(input, progress);
            input = BracketCodePhotos.PhotoCodeProcessForEmail(input, progress);
            input = BracketCodePhotoLink.PhotoLinkCodeProcess(input, progress);
            input = BracketCodePosts.PostLinkCodeProcess(input, progress);
            input = BracketCodeSpecialPages.SpecialPagesCodeProcess(input, progress);

            return input;
        }

        public static string ProcessCodesForLocalDisplay(string input, IProgress<string> progress)
        {
            input = BracketCodeFileDownloads.FileDownloadLinkCodeProcess(input, progress);
            input = BracketCodeFiles.FileLinkCodeProcess(input, progress);
            input = BracketCodeImages.ImageCodeProcessForDirectLocalAccess(input, progress);
            input = BracketCodeImageLinks.ImageLinkCodeProcess(input, progress);
            input = BracketCodeNotes.NoteLinkCodeProcess(input, progress);
            input = BracketCodePhotos.PhotoCodeProcessForDirectLocalAccess(input, progress);
            input = BracketCodePhotoLink.PhotoLinkCodeProcess(input, progress);
            input = BracketCodePosts.PostLinkCodeProcess(input, progress);
            input = BracketCodeSpecialPages.SpecialPagesCodeProcess(input, progress);

            return input;
        }

        public static string ProcessCodesForSite(string input, IProgress<string> progress)
        {
            input = BracketCodeFileDownloads.FileDownloadLinkCodeProcess(input, progress);
            input = BracketCodeFiles.FileLinkCodeProcess(input, progress);
            input = BracketCodeImages.ImageCodeProcessToFigureWithLink(input, progress);
            input = BracketCodeImageLinks.ImageLinkCodeProcess(input, progress);
            input = BracketCodeNotes.NoteLinkCodeProcess(input, progress);
            input = BracketCodePhotos.PhotoCodeProcessToFigureWithLink(input, progress);
            input = BracketCodePhotoLink.PhotoLinkCodeProcess(input, progress);
            input = BracketCodePosts.PostLinkCodeProcess(input, progress);
            input = BracketCodeSpecialPages.SpecialPagesCodeProcess(input, progress);

            return input;
        }

        /// <summary>
        ///     Returns Bracket Code Information from a string
        /// </summary>
        /// <param name="toProcess"></param>
        /// <param name="bracketCodeToken"></param>
        /// <returns></returns>
        public static List<(string bracketCodeText, string displayText)> SpecialPageBracketCodeMatches(string toProcess,
            string bracketCodeToken)
        {
            var resultList = new List<(string bracketCodeText, string displayText)>();

            if (string.IsNullOrWhiteSpace(toProcess)) return resultList;

            var withTextMatch = new Regex($@"{{{{{bracketCodeToken};\s*[Tt]ext (?<displayText>[^}};]*);[^}}]*}}}}",
                RegexOptions.Singleline);
            var noTextMatch = withTextMatch.Match(toProcess);
            while (noTextMatch.Success)
            {
                resultList.Add((noTextMatch.Value, noTextMatch.Groups["displayText"].Value));
                noTextMatch = noTextMatch.NextMatch();
            }

            //Remove the more specific pattern matches before processing the less specific matches, 
            //as currently written there are patterns that can match both.
            foreach (var loopResultList in resultList)
                toProcess = toProcess.Replace(loopResultList.bracketCodeText, string.Empty);

            var regexObj = new Regex($@"{{{{{bracketCodeToken};}}}}", RegexOptions.Singleline);
            var textMatch = regexObj.Match(toProcess);
            while (textMatch.Success)
            {
                Guid.TryParse(textMatch.Groups["siteGuid"].Value, out var parsedContentId);
                resultList.Add((textMatch.Value, string.Empty));
                textMatch = textMatch.NextMatch();
            }

            return resultList;
        }
    }
}