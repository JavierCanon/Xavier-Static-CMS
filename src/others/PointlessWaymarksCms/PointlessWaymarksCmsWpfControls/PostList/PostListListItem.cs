﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using PointlessWaymarksCmsData.Database.Models;

namespace PointlessWaymarksCmsWpfControls.PostList
{
    public class PostListListItem : INotifyPropertyChanged
    {
        private PostContent _dbEntry;
        private string _smallImageUrl;

        public PostContent DbEntry
        {
            get => _dbEntry;
            set
            {
                if (Equals(value, _dbEntry)) return;
                _dbEntry = value;
                OnPropertyChanged();
            }
        }

        public string SmallImageUrl
        {
            get => _smallImageUrl;
            set
            {
                if (value == _smallImageUrl) return;
                _smallImageUrl = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}