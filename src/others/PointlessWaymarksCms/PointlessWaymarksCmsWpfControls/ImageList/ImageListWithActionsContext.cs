﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using MvvmHelpers.Commands;
using Omu.ValueInjecter;
using Ookii.Dialogs.Wpf;
using PointlessWaymarksCmsData;
using PointlessWaymarksCmsData.Database;
using PointlessWaymarksCmsData.Database.Models;
using PointlessWaymarksCmsData.Html.CommonHtml;
using PointlessWaymarksCmsWpfControls.ContentHistoryView;
using PointlessWaymarksCmsWpfControls.ImageContentEditor;
using PointlessWaymarksCmsWpfControls.Status;
using PointlessWaymarksCmsWpfControls.Utility;
using SingleImagePage = PointlessWaymarksCmsData.Html.ImageHtml.SingleImagePage;

namespace PointlessWaymarksCmsWpfControls.ImageList
{
    public class ImageListWithActionsContext : INotifyPropertyChanged
    {
        private Command _deleteSelectedCommand;
        private Command _editSelectedContentCommand;
        private Command _generateSelectedHtmlCommand;
        private Command _imageBracketCodesToClipboardForSelectedCommand;
        private Command _imageBracketLinkCodesToClipboardForSelectedCommand;
        private ImageListContext _listContext;
        private Command _newContentCommand;
        private Command _openUrlForSelectedCommand;
        private Command _refreshDataCommand;
        private StatusControlContext _statusContext;

        public ImageListWithActionsContext(StatusControlContext statusContext)
        {
            StatusContext = statusContext ?? new StatusControlContext();

            GenerateSelectedHtmlCommand = new Command(() => StatusContext.RunBlockingTask(GenerateSelectedHtml));
            EditSelectedContentCommand = new Command(() => StatusContext.RunBlockingTask(EditSelectedContent));
            ImageBracketCodesToClipboardForSelectedCommand = new Command(() =>
                StatusContext.RunBlockingTask(ImageBracketCodesToClipboardForSelected));
            ImageBracketLinkCodesToClipboardForSelectedCommand = new Command(() =>
                StatusContext.RunBlockingTask(ImageBracketLinkCodesToClipboardForSelected));
            OpenUrlForSelectedCommand = new Command(() => StatusContext.RunNonBlockingTask(OpenUrlForSelected));
            NewContentCommand = new Command(() => StatusContext.RunNonBlockingTask(NewContent));
            NewContentFromFilesCommand = new Command(() => StatusContext.RunBlockingTask(NewContentFromFiles));
            RefreshDataCommand = new Command(() => StatusContext.RunBlockingTask(ListContext.LoadData));
            DeleteSelectedCommand = new Command(() => StatusContext.RunBlockingTask(Delete));
            ExtractNewLinksInSelectedCommand =
                new Command(() => StatusContext.RunBlockingTask(ExtractNewLinksInSelected));
            ViewHistoryCommand = new Command(() => StatusContext.RunNonBlockingTask(ViewHistory));


            StatusContext.RunFireAndForgetBlockingTaskWithUiMessageReturn(LoadData);
        }

        public Command DeleteSelectedCommand
        {
            get => _deleteSelectedCommand;
            set
            {
                if (Equals(value, _deleteSelectedCommand)) return;
                _deleteSelectedCommand = value;
                OnPropertyChanged();
            }
        }

        public Command EditSelectedContentCommand
        {
            get => _editSelectedContentCommand;
            set
            {
                if (Equals(value, _editSelectedContentCommand)) return;
                _editSelectedContentCommand = value;
                OnPropertyChanged();
            }
        }


        public Command ExtractNewLinksInSelectedCommand { get; set; }

        public Command GenerateSelectedHtmlCommand
        {
            get => _generateSelectedHtmlCommand;
            set
            {
                if (Equals(value, _generateSelectedHtmlCommand)) return;
                _generateSelectedHtmlCommand = value;
                OnPropertyChanged();
            }
        }

        public Command ImageBracketCodesToClipboardForSelectedCommand
        {
            get => _imageBracketCodesToClipboardForSelectedCommand;
            set
            {
                if (Equals(value, _imageBracketCodesToClipboardForSelectedCommand)) return;
                _imageBracketCodesToClipboardForSelectedCommand = value;
                OnPropertyChanged();
            }
        }

        public Command ImageBracketLinkCodesToClipboardForSelectedCommand
        {
            get => _imageBracketLinkCodesToClipboardForSelectedCommand;
            set
            {
                if (Equals(value, _imageBracketLinkCodesToClipboardForSelectedCommand)) return;
                _imageBracketLinkCodesToClipboardForSelectedCommand = value;
                OnPropertyChanged();
            }
        }

        public ImageListContext ListContext
        {
            get => _listContext;
            set
            {
                if (Equals(value, _listContext)) return;
                _listContext = value;
                OnPropertyChanged();
            }
        }

        public Command NewContentCommand
        {
            get => _newContentCommand;
            set
            {
                if (Equals(value, _newContentCommand)) return;
                _newContentCommand = value;
                OnPropertyChanged();
            }
        }

        public Command NewContentFromFilesCommand { get; set; }

        public Command OpenUrlForSelectedCommand
        {
            get => _openUrlForSelectedCommand;
            set
            {
                if (Equals(value, _openUrlForSelectedCommand)) return;
                _openUrlForSelectedCommand = value;
                OnPropertyChanged();
            }
        }

        public Command RefreshDataCommand
        {
            get => _refreshDataCommand;
            set
            {
                if (Equals(value, _refreshDataCommand)) return;
                _refreshDataCommand = value;
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

        public Command ViewHistoryCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private async Task Delete()
        {
            await ThreadSwitcher.ResumeBackgroundAsync();

            var selected = ListContext.SelectedItems;

            if (selected == null || !selected.Any())
            {
                StatusContext.ToastError("Nothing Selected?");
                return;
            }

            if (selected.Count > 1)
            {
                StatusContext.ToastError("Sorry - please delete one at a time");
                return;
            }

            var selectedItem = selected.Single();

            if (selectedItem.DbEntry == null || selectedItem.DbEntry.Id < 1)
            {
                StatusContext.ToastError("Entry is not saved?");
                return;
            }

            var settings = UserSettingsSingleton.CurrentSettings();

            var possibleContentDirectory = settings.LocalSiteImageContentDirectory(selectedItem.DbEntry, false);
            if (possibleContentDirectory.Exists) possibleContentDirectory.Delete(true);

            var context = await Db.Context();

            var toHistoric = await context.ImageContents.Where(x => x.ContentId == selectedItem.DbEntry.ContentId)
                .ToListAsync();

            foreach (var loopToHistoric in toHistoric)
            {
                var newHistoric = new HistoricImageContent();
                newHistoric.InjectFrom(loopToHistoric);
                newHistoric.Id = 0;
                newHistoric.LastUpdatedOn = DateTime.Now;
                await context.HistoricImageContents.AddAsync(newHistoric);
                context.ImageContents.Remove(loopToHistoric);
            }

            await context.SaveChangesAsync(true);

            await LoadData();
        }

        private async Task EditSelectedContent()
        {
            await ThreadSwitcher.ResumeBackgroundAsync();

            if (ListContext.SelectedItems == null || !ListContext.SelectedItems.Any())
            {
                StatusContext.ToastError("Nothing Selected?");
                return;
            }


            var context = await Db.Context();
            var frozenList = ListContext.SelectedItems;

            foreach (var loopSelected in frozenList)
            {
                var refreshedData =
                    context.ImageContents.SingleOrDefault(x => x.ContentId == loopSelected.DbEntry.ContentId);

                if (refreshedData == null)
                {
                    StatusContext.ToastError(
                        $"{loopSelected.DbEntry.Title} is no longer active in the database? Can not edit - " +
                        "look for a historic version...");
                    continue;
                }

                await ThreadSwitcher.ResumeForegroundAsync();

                var newContentWindow = new ImageContentEditorWindow(refreshedData);

                newContentWindow.Show();

                await ThreadSwitcher.ResumeBackgroundAsync();
            }
        }

        private async Task ExtractNewLinksInSelected()
        {
            await ThreadSwitcher.ResumeBackgroundAsync();

            if (ListContext.SelectedItems == null || !ListContext.SelectedItems.Any())
            {
                StatusContext.ToastError("Nothing Selected?");
                return;
            }

            var context = await Db.Context();
            var frozenList = ListContext.SelectedItems;

            foreach (var loopSelected in frozenList)
            {
                var refreshedData =
                    context.ImageContents.SingleOrDefault(x => x.ContentId == loopSelected.DbEntry.ContentId);

                if (refreshedData == null) continue;

                await LinkExtraction.ExtractNewAndShowLinkStreamEditors($"{refreshedData.UpdateNotes}",
                    StatusContext.ProgressTracker());
            }
        }

        private async Task GenerateSelectedHtml()
        {
            await ThreadSwitcher.ResumeBackgroundAsync();

            if (ListContext.SelectedItems == null || !ListContext.SelectedItems.Any())
            {
                StatusContext.ToastError("Nothing Selected?");
                return;
            }

            var loopCount = 1;
            var totalCount = ListContext.SelectedItems.Count;

            foreach (var loopSelected in ListContext.SelectedItems)
            {
                StatusContext.Progress(
                    $"Generating Html for {loopSelected.DbEntry.Title}, {loopCount} of {totalCount}");

                var htmlContext = new SingleImagePage(loopSelected.DbEntry);

                htmlContext.WriteLocalHtml();

                StatusContext.ToastSuccess($"Generated {htmlContext.PageUrl}");

                loopCount++;
            }
        }

        private async Task ImageBracketCodesToClipboardForSelected()
        {
            await ThreadSwitcher.ResumeBackgroundAsync();

            if (ListContext.SelectedItems == null || !ListContext.SelectedItems.Any())
            {
                StatusContext.ToastError("Nothing Selected?");
                return;
            }

            var finalString = ListContext.SelectedItems.Aggregate(string.Empty,
                (current, loopSelected) =>
                    current + @$"{BracketCodeImages.ImageBracketCode(loopSelected.DbEntry)}{Environment.NewLine}");

            await ThreadSwitcher.ResumeForegroundAsync();

            Clipboard.SetText(finalString);

            StatusContext.ToastSuccess($"To Clipboard: {finalString}");
        }

        private async Task ImageBracketLinkCodesToClipboardForSelected()
        {
            await ThreadSwitcher.ResumeBackgroundAsync();

            if (ListContext.SelectedItems == null || !ListContext.SelectedItems.Any())
            {
                StatusContext.ToastError("Nothing Selected?");
                return;
            }

            var finalString = ListContext.SelectedItems.Aggregate(string.Empty,
                (current, loopSelected) =>
                    current +
                    @$"{BracketCodeImageLinks.ImageLinkBracketCode(loopSelected.DbEntry)}{Environment.NewLine}");

            await ThreadSwitcher.ResumeForegroundAsync();

            Clipboard.SetText(finalString);

            StatusContext.ToastSuccess($"To Clipboard: {finalString}");
        }

        private async Task LoadData()
        {
            await ThreadSwitcher.ResumeBackgroundAsync();

            ListContext = new ImageListContext(StatusContext);
        }

        private async Task NewContent()
        {
            await ThreadSwitcher.ResumeForegroundAsync();

            new ImageContentEditorWindow().Show();
        }

        private async Task NewContentFromFiles()
        {
            await ThreadSwitcher.ResumeForegroundAsync();

            StatusContext.Progress("Starting Image load.");

            var dialog = new VistaOpenFileDialog {Multiselect = true};

            if (!(dialog.ShowDialog() ?? false)) return;

            var selectedFiles = dialog.FileNames?.ToList() ?? new List<string>();

            if (!selectedFiles.Any()) return;

            await ThreadSwitcher.ResumeBackgroundAsync();

            if (selectedFiles.Count > 20)
            {
                StatusContext.ToastError($"Sorry - max limit is 20 files at once, {selectedFiles.Count} selected...");
                return;
            }

            var selectedFileInfos = selectedFiles.Select(x => new FileInfo(x)).ToList();

            if (!selectedFileInfos.Any(x => x.Exists))
            {
                StatusContext.ToastError("Files don't exist?");
                return;
            }

            selectedFileInfos = selectedFileInfos.Where(x => x.Exists).ToList();

            if (!selectedFileInfos.Any(FileTypeHelpers.ImageFileTypeIsSupported))
            {
                StatusContext.ToastError("None of the files appear to be supported file types...");
                return;
            }

            if (selectedFileInfos.Any(x => !FileTypeHelpers.ImageFileTypeIsSupported(x)))
                StatusContext.ToastWarning(
                    $"Skipping - not supported - {string.Join(", ", selectedFileInfos.Where(x => !FileTypeHelpers.ImageFileTypeIsSupported(x)))}");

            foreach (var loopFile in selectedFileInfos.Where(FileTypeHelpers.ImageFileTypeIsSupported))
            {
                await ThreadSwitcher.ResumeForegroundAsync();

                var editor = new ImageContentEditorWindow(loopFile);
                editor.Show();

                StatusContext.Progress($"New Image Editor - {loopFile.FullName} ");

                await ThreadSwitcher.ResumeBackgroundAsync();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task OpenUrlForSelected()
        {
            await ThreadSwitcher.ResumeBackgroundAsync();

            if (ListContext.SelectedItems == null || !ListContext.SelectedItems.Any())
            {
                StatusContext.ToastError("Nothing Selected?");
                return;
            }

            var settings = UserSettingsSingleton.CurrentSettings();

            foreach (var loopSelected in ListContext.SelectedItems)
            {
                var url = $@"http://{settings.ImagePageUrl(loopSelected.DbEntry)}";

                var ps = new ProcessStartInfo(url) {UseShellExecute = true, Verb = "open"};
                Process.Start(ps);
            }
        }

        private async Task ViewHistory()
        {
            await ThreadSwitcher.ResumeBackgroundAsync();

            var selected = ListContext.SelectedItems;

            if (selected == null || !selected.Any())
            {
                StatusContext.ToastError("Nothing Selected?");
                return;
            }

            if (selected.Count > 1)
            {
                StatusContext.ToastError("Please Select a Single Item");
                return;
            }

            var singleSelected = selected.Single();

            if (singleSelected.DbEntry == null || singleSelected.DbEntry.ContentId == Guid.Empty)
            {
                StatusContext.ToastWarning("No History - New/Unsaved Entry?");
                return;
            }

            var db = await Db.Context();

            StatusContext.Progress($"Looking up Historic Entries for {singleSelected.DbEntry.Title}");

            var historicItems = await db.HistoricImageContents
                .Where(x => x.ContentId == singleSelected.DbEntry.ContentId).ToListAsync();

            StatusContext.Progress($"Found {historicItems.Count} Historic Entries");

            if (historicItems.Count < 1)
            {
                StatusContext.ToastWarning("No History to Show...");
                return;
            }

            var historicView = new ContentViewHistoryPage($"Historic Entries - {singleSelected.DbEntry.Title}",
                UserSettingsSingleton.CurrentSettings().SiteName, $"Historic Entries - {singleSelected.DbEntry.Title}",
                historicItems.OrderByDescending(x => x.LastUpdatedOn.HasValue).ThenByDescending(x => x.LastUpdatedOn)
                    .Select(ObjectDumper.Dump).ToList());

            historicView.WriteHtmlToTempFolderAndShow(StatusContext.ProgressTracker());
        }
    }
}