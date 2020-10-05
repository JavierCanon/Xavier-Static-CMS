﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PointlessWaymarksCmsData.Database;
using PointlessWaymarksCmsData.Html.CommonHtml;

namespace PointlessWaymarksCmsData.Html.PhotoGalleryHtml
{
    public static class DailyPhotoPageGenerators
    {
        public static async Task<List<DailyPhotosPage>> DailyPhotoGalleries(IProgress<string> progress)
        {
            var db = await Db.Context();

            progress?.Report("Starting Daily Photo Pages Generation");

            var allDates = (await db.PhotoContents.Select(x => x.PhotoCreatedOn).ToListAsync()).Select(x => x.Date)
                .Distinct().OrderByDescending(x => x).ToList();

            progress?.Report($"Found {allDates.Count} Dates with Photos");

            var returnList = new List<DailyPhotosPage>();

            var loopGoal = allDates.Count;

            for (var i = 0; i < allDates.Count; i++)
            {
                var loopDate = allDates[i];

                if (i % 10 == 0) progress?.Report($"Daily Photo Page - {loopDate:D} - {i} of {loopGoal}");
                var toAdd = await DailyPhotoGallery(loopDate);

                if (i > 0)
                {
                    toAdd.NextDailyPhotosPage = returnList[i - 1];

                    returnList[i - 1].PreviousDailyPhotosPage = toAdd;
                }

                returnList.Add(toAdd);
            }

            return returnList;
        }

        public static async Task<DailyPhotosPage> DailyPhotoGallery(DateTime dateTimeForPictures)
        {
            var db = await Db.Context();

            var startsAfterOrOn = dateTimeForPictures.Date;
            var endsBefore = dateTimeForPictures.AddDays(1).Date;

            var datePhotos = await db.PhotoContents
                .Where(x => x.PhotoCreatedOn >= startsAfterOrOn && x.PhotoCreatedOn < endsBefore)
                .OrderBy(x => x.PhotoCreatedOn).ToListAsync();

            if (!datePhotos.Any()) return null;

            var photographersList = datePhotos.Where(x => !string.IsNullOrWhiteSpace(x.PhotoCreatedBy))
                .Select(x => x.PhotoCreatedBy).Distinct().ToList();
            var createdByList = datePhotos.Select(x => x.CreatedBy).Distinct().ToList();

            var photographersString = photographersList.ToList().JoinListOfStringsToCommonUsageListWithAnd();
            var photographersAndCreatedByString = photographersList.Concat(createdByList).Distinct().ToList()
                .JoinListOfStringsToCommonUsageListWithAnd();

            var photoPage = new DailyPhotosPage
            {
                MainImage = new PictureSiteInformation(datePhotos.First().ContentId),
                ImageList = datePhotos.Select(x => new PictureSiteInformation(x.ContentId)).ToList(),
                Title = $"Photographs - {dateTimeForPictures:MMMM d, dddd, yyyy}",
                Summary =
                    $"Photographs taken on {dateTimeForPictures:M/d/yyyy}{(photographersList.Any() ? " by " : "")}{photographersString}.",
                CreatedBy = photographersAndCreatedByString,
                PhotoPageDate = startsAfterOrOn,
                SiteName = UserSettingsSingleton.CurrentSettings().SiteName,
                PhotoTags =
                    datePhotos.SelectMany(x => Db.TagListParseToSlugs(x, true)).Select(x => x.Trim()).Distinct()
                        .OrderBy(x => x).ToList(),
                PageUrl = UserSettingsSingleton.CurrentSettings().DailyPhotoGalleryUrl(startsAfterOrOn)
            };

            return photoPage;
        }
    }
}