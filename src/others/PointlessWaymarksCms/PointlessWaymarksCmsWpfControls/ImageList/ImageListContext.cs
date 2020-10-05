﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using MvvmHelpers;
using MvvmHelpers.Commands;
using PointlessWaymarksCmsData;
using PointlessWaymarksCmsData.Database;
using PointlessWaymarksCmsData.Database.Models;
using PointlessWaymarksCmsData.Html.CommonHtml;
using PointlessWaymarksCmsWpfControls.Status;
using PointlessWaymarksCmsWpfControls.Utility;
using TinyIpc.Messaging;

namespace PointlessWaymarksCmsWpfControls.ImageList
{
    public class ImageListContext : INotifyPropertyChanged
    {
        private ObservableRangeCollection<ImageListListItem> _items;
        private string _lastSortColumn;
        private Command<ImageListListItem> _openFileCommand;
        private List<ImageListListItem> _selectedItems;
        private StatusControlContext _statusContext;
        private string _userFilterText;

        public ImageListContext(StatusControlContext statusContext)
        {
            StatusContext = statusContext ?? new StatusControlContext();

            SortListCommand = new Command<string>(x => StatusContext.RunNonBlockingTask(() => SortList(x)));
            ToggleListSortDirectionCommand = new Command(() => StatusContext.RunNonBlockingTask(async () =>
            {
                SortDescending = !SortDescending;
                await SortList(_lastSortColumn);
            }));
            OpenFileCommand = new Command<ImageListListItem>(x => StatusContext.RunNonBlockingTask(() => OpenFile(x)));


            StatusContext.RunFireAndForgetBlockingTaskWithUiMessageReturn(LoadData);

            DataNotifications.DataNotificationChannel().MessageReceived += OnDataNotificationReceived;
        }


        public ObservableRangeCollection<ImageListListItem> Items
        {
            get => _items;
            set
            {
                if (Equals(value, _items)) return;
                _items = value;
                OnPropertyChanged();
            }
        }

        public Command<ImageListListItem> OpenFileCommand
        {
            get => _openFileCommand;
            set
            {
                if (Equals(value, _openFileCommand)) return;
                _openFileCommand = value;
                OnPropertyChanged();
            }
        }

        public List<ImageListListItem> SelectedItems
        {
            get => _selectedItems;
            set
            {
                if (Equals(value, _selectedItems)) return;
                _selectedItems = value;
                OnPropertyChanged();
            }
        }

        public bool SortDescending { get; set; }

        public Command<string> SortListCommand { get; set; }

        public StatusControlContext StatusContext
        {
            get => _statusContext;
            set
            {
                if (Equals(value, _statusContext)) return;
                _statusContext = value;
                OnPropertyChanged();
            }
        }

        public Command ToggleListSortDirectionCommand { get; set; }

        public string UserFilterText
        {
            get => _userFilterText;
            set
            {
                if (value == _userFilterText) return;
                _userFilterText = value;
                OnPropertyChanged();

                StatusContext.RunFireAndForgetTaskWithUiToastErrorReturn(FilterList);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async Task DataNotificationReceived(TinyMessageReceivedEventArgs e)
        {
            var translatedMessage = DataNotifications.TranslateDataNotification(e.Message);

            if (translatedMessage.HasError)
                await EventLogContext.TryWriteDiagnosticMessageToLog(
                    $"Data Notification Failure in PhotoListContext - {translatedMessage.ErrorNote}",
                    StatusContext.StatusControlContextId.ToString());

            if (translatedMessage.ContentType != DataNotificationContentType.Image) return;

            await ThreadSwitcher.ResumeBackgroundAsync();

            if (translatedMessage.UpdateType == DataNotificationUpdateType.Delete)
            {
                var toRemove = Items.Where(x => translatedMessage.ContentIds.Contains(x.DbEntry.ContentId)).ToList();

                await ThreadSwitcher.ResumeForegroundAsync();
            }

            if (translatedMessage.UpdateType == DataNotificationUpdateType.New)
            {
                var context = await Db.Context();

                var toAdd = (await context.ImageContents.Where(x => translatedMessage.ContentIds.Contains(x.ContentId))
                    .ToListAsync()).Select(ListItemFromDbItem).ToList();

                await ThreadSwitcher.ResumeForegroundAsync();

                toAdd.ForEach(x => Items.Add(x));
            }

            if (translatedMessage.UpdateType == DataNotificationUpdateType.Update ||
                translatedMessage.UpdateType == DataNotificationUpdateType.LocalContent)
            {
                var context = await Db.Context();

                var dbItems =
                    (await context.ImageContents.Where(x => translatedMessage.ContentIds.Contains(x.ContentId))
                        .ToListAsync()).Select(ListItemFromDbItem);

                await ThreadSwitcher.ResumeForegroundAsync();

                foreach (var loopUpdates in dbItems)
                {
                    var toUpdate = Items.SingleOrDefault(x => x.DbEntry.ContentId == loopUpdates.DbEntry.ContentId);
                    if (toUpdate == null)
                    {
                        Items.Add(loopUpdates);
                        continue;
                    }

                    if (translatedMessage.UpdateType == DataNotificationUpdateType.Update)
                        toUpdate.DbEntry = loopUpdates.DbEntry;

                    toUpdate.SmallImageUrl = GetSmallImageUrl(loopUpdates.DbEntry);
                }
            }
        }

        private async Task FilterList()
        {
            if (Items == null || !Items.Any()) return;

            await ThreadSwitcher.ResumeForegroundAsync();

            ((CollectionView) CollectionViewSource.GetDefaultView(Items)).Filter = o =>
            {
                if (string.IsNullOrWhiteSpace(UserFilterText)) return true;

                var loweredString = UserFilterText.ToLower();

                if (!(o is ImageListListItem pi)) return false;
                if ((pi.DbEntry.Title ?? string.Empty).ToLower().Contains(loweredString)) return true;
                if ((pi.DbEntry.Tags ?? string.Empty).ToLower().Contains(loweredString)) return true;
                if ((pi.DbEntry.Summary ?? string.Empty).ToLower().Contains(loweredString)) return true;
                if ((pi.DbEntry.CreatedBy ?? string.Empty).ToLower().Contains(loweredString)) return true;
                if ((pi.DbEntry.LastUpdatedBy ?? string.Empty).ToLower().Contains(loweredString)) return true;
                return false;
            };
        }

        public string GetSmallImageUrl(ImageContent content)
        {
            if (content == null) return null;

            string smallImageUrl;

            try
            {
                smallImageUrl = PictureAssetProcessing.ProcessImageDirectory(content).SmallPicture?.File.FullName;
            }
            catch
            {
                smallImageUrl = null;
            }

            return smallImageUrl;
        }


        public ImageListListItem ListItemFromDbItem(ImageContent content)
        {
            return new ImageListListItem {DbEntry = content, SmallImageUrl = GetSmallImageUrl(content)};
        }

        public async Task LoadData()
        {
            await ThreadSwitcher.ResumeBackgroundAsync();

            StatusContext.Progress("Connecting to DB");

            var db = await Db.Context();

            StatusContext.Progress("Getting Image Db Entries");
            var dbItems = db.ImageContents.ToList();
            var listItems = new List<ImageListListItem>();

            var totalCount = dbItems.Count;
            var currentLoop = 1;

            foreach (var loopItems in dbItems)
            {
                if (totalCount == 1 || totalCount % 10 == 0)
                    StatusContext.Progress($"Processing Image Item {currentLoop} of {totalCount}");

                listItems.Add(ListItemFromDbItem(loopItems));

                currentLoop++;
            }

            await ThreadSwitcher.ResumeForegroundAsync();

            StatusContext.Progress("Displaying Images");

            Items = new ObservableRangeCollection<ImageListListItem>(listItems);

            SortDescending = true;
            await SortList("CreatedOn");
        }

        private void OnDataNotificationReceived(object sender, TinyMessageReceivedEventArgs e)
        {
            StatusContext.RunFireAndForgetTaskWithUiToastErrorReturn(async () => await DataNotificationReceived(e));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task OpenFile(ImageListListItem listItem)
        {
            await ThreadSwitcher.ResumeBackgroundAsync();

            if (listItem == null)
            {
                StatusContext.ToastError("Nothing Items to Open?");
                return;
            }

            if (string.IsNullOrWhiteSpace(listItem.DbEntry.OriginalFileName))
            {
                StatusContext.ToastError("No File?");
                return;
            }

            var toOpen = UserSettingsSingleton.CurrentSettings().LocalSiteImageContentFile(listItem.DbEntry);

            if (!toOpen.Exists)
            {
                StatusContext.ToastError("File doesn't exist?");
                return;
            }

            var url = toOpen.FullName;

            var ps = new ProcessStartInfo(url) {UseShellExecute = true, Verb = "open"};
            Process.Start(ps);
        }

        private async Task SortList(string sortColumn)
        {
            await ThreadSwitcher.ResumeForegroundAsync();

            _lastSortColumn = sortColumn;

            var collectionView = ((CollectionView) CollectionViewSource.GetDefaultView(Items));
            collectionView.SortDescriptions.Clear();

            if (string.IsNullOrWhiteSpace(sortColumn)) return;
            collectionView.SortDescriptions.Add(new SortDescription($"DbEntry.{sortColumn}",
                SortDescending ? ListSortDirection.Descending : ListSortDirection.Ascending));
        }
    }
}