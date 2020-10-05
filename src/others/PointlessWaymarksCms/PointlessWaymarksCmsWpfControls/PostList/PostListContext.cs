﻿using System.Collections.Generic;
using System.ComponentModel;
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

namespace PointlessWaymarksCmsWpfControls.PostList
{
    public class PostListContext : INotifyPropertyChanged
    {
        private ObservableRangeCollection<PostListListItem> _items;
        private string _lastSortColumn;
        private List<PostListListItem> _selectedItems;
        private bool _sortDescending;
        private Command<string> _sortListCommand;
        private StatusControlContext _statusContext;
        private Command _toggleListSortDirectionCommand;
        private string _userFilterText;

        public PostListContext(StatusControlContext statusContext)
        {
            StatusContext = statusContext ?? new StatusControlContext();

            SortListCommand = new Command<string>(x => StatusContext.RunNonBlockingTask(() => SortList(x)));
            ToggleListSortDirectionCommand = new Command(() => StatusContext.RunNonBlockingTask(async () =>
            {
                SortDescending = !SortDescending;
                await SortList(_lastSortColumn);
            }));

            StatusContext.RunFireAndForgetBlockingTaskWithUiMessageReturn(LoadData);

            DataNotifications.DataNotificationChannel().MessageReceived += OnDataNotificationReceived;
        }


        public ObservableRangeCollection<PostListListItem> Items
        {
            get => _items;
            set
            {
                if (Equals(value, _items)) return;
                _items = value;
                OnPropertyChanged();
            }
        }

        public List<PostListListItem> SelectedItems
        {
            get => _selectedItems;
            set
            {
                if (Equals(value, _selectedItems)) return;
                _selectedItems = value;
                OnPropertyChanged();
            }
        }

        public bool SortDescending
        {
            get => _sortDescending;
            set
            {
                if (value == _sortDescending) return;
                _sortDescending = value;
                OnPropertyChanged();
            }
        }

        public Command<string> SortListCommand
        {
            get => _sortListCommand;
            set
            {
                if (Equals(value, _sortListCommand)) return;
                _sortListCommand = value;
                OnPropertyChanged();
            }
        }

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

        public Command ToggleListSortDirectionCommand
        {
            get => _toggleListSortDirectionCommand;
            set
            {
                if (Equals(value, _toggleListSortDirectionCommand)) return;
                _toggleListSortDirectionCommand = value;
                OnPropertyChanged();
            }
        }

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
            await ThreadSwitcher.ResumeBackgroundAsync();

            var translatedMessage = DataNotifications.TranslateDataNotification(e.Message);

            if (translatedMessage.HasError)
                await EventLogContext.TryWriteDiagnosticMessageToLog(
                    $"Data Notification Failure in PhotoListContext - {translatedMessage.ErrorNote}",
                    StatusContext.StatusControlContextId.ToString());

            if (translatedMessage.ContentType == DataNotificationContentType.Post)
                StatusContext.RunFireAndForgetTaskWithUiToastErrorReturn(async () =>
                    await PostDataNotificationReceived(translatedMessage));
            if (translatedMessage.ContentType != DataNotificationContentType.Post)
                StatusContext.RunFireAndForgetTaskWithUiToastErrorReturn(async () =>
                    await PossibleMainImageUpdateDataNotificationReceived(translatedMessage));
        }

        private void DataNotificationsOnContentDataNotificationEvent(object sender, DataNotificationEventArgs e)
        {
            StatusContext.RunFireAndForgetTaskWithUiToastErrorReturn(async () =>
                await DataNotificationsOnContentDataNotificationEvent(e));
        }

        private async Task DataNotificationsOnContentDataNotificationEvent(DataNotificationEventArgs e)
        {
            await ThreadSwitcher.ResumeBackgroundAsync();

            if (e.UpdateType == DataNotificationUpdateType.Delete)
            {
                var toRemove = Items.Where(x => e.ContentIds.Contains(x.DbEntry.ContentId)).ToList();

                await ThreadSwitcher.ResumeForegroundAsync();

                Items.RemoveRange(toRemove);
            }

            if (e.UpdateType == DataNotificationUpdateType.New)
            {
                var context = await Db.Context();

                var dbItems = (await context.PostContents.Where(x => e.ContentIds.Contains(x.ContentId)).ToListAsync())
                    .Select(ListItemFromDbItem).ToList();

                await ThreadSwitcher.ResumeForegroundAsync();

                Items.AddRange(dbItems);
            }

            if (e.UpdateType == DataNotificationUpdateType.Update ||
                e.UpdateType == DataNotificationUpdateType.LocalContent)
            {
                var context = await Db.Context();

                var dbItems =
                    (await context.PostContents.Where(x => e.ContentIds.Contains(x.ContentId)).ToListAsync()).Select(
                        ListItemFromDbItem);

                await ThreadSwitcher.ResumeForegroundAsync();

                foreach (var loopUpdates in dbItems)
                {
                    var toUpdate = Items.SingleOrDefault(x => x.DbEntry.ContentId == loopUpdates.DbEntry.ContentId);
                    if (toUpdate == null)
                    {
                        Items.Add(loopUpdates);
                        continue;
                    }

                    if (e.UpdateType == DataNotificationUpdateType.Update) toUpdate.DbEntry = loopUpdates.DbEntry;

                    toUpdate.SmallImageUrl = loopUpdates.SmallImageUrl;
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

                if (!(o is PostListListItem pi)) return false;
                if ((pi.DbEntry.Title ?? string.Empty).ToLower().Contains(loweredString)) return true;
                if ((pi.DbEntry.Tags ?? string.Empty).ToLower().Contains(loweredString)) return true;
                if ((pi.DbEntry.Summary ?? string.Empty).ToLower().Contains(loweredString)) return true;
                if ((pi.DbEntry.CreatedBy ?? string.Empty).ToLower().Contains(loweredString)) return true;
                if ((pi.DbEntry.LastUpdatedBy ?? string.Empty).ToLower().Contains(loweredString)) return true;
                return false;
            };
        }

        public string GetSmallImageUrl(PostContent content)
        {
            if (content?.MainPicture == null) return null;

            string smallImageUrl;

            try
            {
                smallImageUrl = PictureAssetProcessing.ProcessPictureDirectory(content.MainPicture.Value).SmallPicture
                    ?.File.FullName;
            }
            catch
            {
                smallImageUrl = null;
            }

            return smallImageUrl;
        }

        public PostListListItem ListItemFromDbItem(PostContent content)
        {
            return new PostListListItem {DbEntry = content, SmallImageUrl = GetSmallImageUrl(content)};
        }

        public async Task LoadData()
        {
            await ThreadSwitcher.ResumeBackgroundAsync();

            StatusContext.Progress("Connecting to DB");

            var db = await Db.Context();

            StatusContext.Progress("Getting Post Db Entries");
            var dbItems = db.PostContents.ToList();
            var listItems = new List<PostListListItem>();

            var totalCount = dbItems.Count;
            var currentLoop = 1;

            foreach (var loopItems in dbItems)
            {
                if (totalCount == 1 || totalCount % 10 == 0)
                    StatusContext.Progress($"Processing Post Item {currentLoop} of {totalCount}");

                listItems.Add(ListItemFromDbItem(loopItems));

                currentLoop++;
            }

            await ThreadSwitcher.ResumeForegroundAsync();

            StatusContext.Progress("Displaying Posts");

            Items = new ObservableRangeCollection<PostListListItem>(listItems);

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

        private async Task PossibleMainImageUpdateDataNotificationReceived(
            InterProcessDataNotification translatedMessage)
        {
            await ThreadSwitcher.ResumeBackgroundAsync();

            var toUpdate = Items.Where(x =>
                    x.DbEntry.MainPicture != null && translatedMessage.ContentIds.Contains(x.DbEntry.MainPicture.Value))
                .ToList();

            toUpdate.ForEach(x =>
            {
                x.SmallImageUrl = null;
                x.SmallImageUrl = GetSmallImageUrl(x.DbEntry);
            });
        }

        private async Task PostDataNotificationReceived(InterProcessDataNotification translatedMessage)
        {
            await ThreadSwitcher.ResumeBackgroundAsync();

            if (translatedMessage.UpdateType == DataNotificationUpdateType.Delete)
            {
                var toRemove = Items.Where(x => translatedMessage.ContentIds.Contains(x.DbEntry.ContentId)).ToList();

                await ThreadSwitcher.ResumeForegroundAsync();

                Items.RemoveRange(toRemove);
            }

            if (translatedMessage.UpdateType == DataNotificationUpdateType.New)
            {
                var context = await Db.Context();

                var dbItems =
                    (await context.PostContents.Where(x => translatedMessage.ContentIds.Contains(x.ContentId))
                        .ToListAsync()).Select(ListItemFromDbItem).ToList();

                await ThreadSwitcher.ResumeForegroundAsync();

                Items.AddRange(dbItems);
            }

            if (translatedMessage.UpdateType == DataNotificationUpdateType.Update ||
                translatedMessage.UpdateType == DataNotificationUpdateType.LocalContent)
            {
                var context = await Db.Context();

                var dbItems =
                    (await context.PostContents.Where(x => translatedMessage.ContentIds.Contains(x.ContentId))
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