﻿using System;
using System.Collections.Generic;
using System.Linq;
using HtmlTags;
using PointlessWaymarksCmsData.Database;
using PointlessWaymarksCmsData.Database.Models;

namespace PointlessWaymarksCmsData.Html.CommonHtml
{
    public static class BracketCodePosts
    {
        public const string BracketCodeToken = "postlink";

        public static List<PostContent> DbContentFromBracketCodes(string toProcess, IProgress<string> progress)
        {
            if (string.IsNullOrWhiteSpace(toProcess)) return new List<PostContent>();

            progress?.Report("Searching for Post Codes...");

            var guidList = BracketCodeCommon.ContentBracketCodeMatches(toProcess, BracketCodeToken)
                .Select(x => x.contentGuid).Distinct().ToList();

            var returnList = new List<PostContent>();

            if (!guidList.Any()) return returnList;

            var context = Db.Context().Result;

            foreach (var loopMatch in guidList)
            {
                var dbContent = context.PostContents.FirstOrDefault(x => x.ContentId == loopMatch);
                if (dbContent == null) continue;

                progress?.Report($"Post Code - Adding DbContent For {dbContent.Title}");

                returnList.Add(dbContent);
            }

            return returnList;
        }

        public static string PostLinkBracketCode(PostContent content)
        {
            return $@"{{{{{BracketCodeToken} {content.ContentId}; {content.Title}}}}}";
        }

        public static string PostLinkCodeProcess(string toProcess, IProgress<string> progress)
        {
            if (string.IsNullOrWhiteSpace(toProcess)) return string.Empty;

            progress?.Report("Searching for Post Codes...");

            var resultList = BracketCodeCommon.ContentBracketCodeMatches(toProcess, BracketCodeToken);

            if (!resultList.Any()) return toProcess;

            var context = Db.Context().Result;

            foreach (var loopMatch in resultList)
            {
                var dbContent = context.PostContents.FirstOrDefault(x => x.ContentId == loopMatch.contentGuid);
                if (dbContent == null) continue;

                progress?.Report($"Adding post link {dbContent.Title} from Code");

                var linkTag =
                    new LinkTag(
                        string.IsNullOrWhiteSpace(loopMatch.displayText)
                            ? dbContent.Title
                            : loopMatch.displayText.Trim(),
                        UserSettingsSingleton.CurrentSettings().PostPageUrl(dbContent), "post-page-link");

                toProcess = toProcess.Replace(loopMatch.bracketCodeText, linkTag.ToString());
            }

            return toProcess;
        }
    }
}