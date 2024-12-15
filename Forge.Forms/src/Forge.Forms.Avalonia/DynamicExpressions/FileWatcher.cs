using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Forge.Forms.AvaloniaUI.FormBuilding;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions;

public sealed class FileWatcher : IStringProxy, IProxy, IDisposable, INotifyPropertyChanged
{
    private static readonly Dictionary<string, Watcher> Watchers = new(StringComparer.OrdinalIgnoreCase);

    private readonly string filePath;
    private bool disposed;

    public FileWatcher(string filePath)
    {
        filePath = Path.GetFullPath(filePath ?? throw new ArgumentNullException(nameof(filePath)));
        this.filePath = filePath;
        lock (Watchers)
        {
            if (Watchers.ContainsKey(filePath))
                Watchers[filePath].AddListener(this);
            else
                Watchers[filePath] = new Watcher(this);
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public Action ValueChanged { get; set; }

    object IProxy.Value => Value;

    public string Value
    {
        get
        {
            lock (Watchers)
            {
                if (disposed) throw new ObjectDisposedException(nameof(FileWatcher));

                return Watchers[filePath].Value;
            }
        }
    }

    ~FileWatcher()
    {
        Dispose(false);
    }

    private void NotifyChanged()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        ValueChanged?.Invoke();
    }

    private void Dispose(bool disposing)
    {
        lock (Watchers)
        {
            if (disposed) return;

            disposed = true;
            var watcher = Watchers[filePath];
            watcher.RemoveListener(this);
            if (watcher.Count == 0)
            {
                watcher.Dispose();
                Watchers.Remove(filePath);
            }
        }
    }

    private class Watcher : IDisposable
    {
        private readonly string filePath;
#if !BROWSER
        private readonly FileSystemWatcher fileSystemWatcher;
#endif
        private readonly List<FileWatcher> listeners;

        private bool isLatestValue;
        private string value;

        public Watcher(FileWatcher initialListener)
        {
            filePath = initialListener.filePath;
            listeners = new List<FileWatcher> { initialListener };

#if !BROWSER
            fileSystemWatcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(filePath),
                Filter = Path.GetFileName(filePath),
                EnableRaisingEvents = true
            };
            fileSystemWatcher.Created += (s, e) => Update();
            fileSystemWatcher.Changed += (s, e) => Update();
            fileSystemWatcher.Deleted += (s, e) => Update();
            fileSystemWatcher.Renamed += (s, e) => Update();
            fileSystemWatcher.Error += (s, e) => Update();
#endif
        }

        public string Value
        {
            get
            {
                lock (this)
                {
                    if (!isLatestValue)
                    {
                        value = Utilities.TryReadFile(filePath);
                        isLatestValue = true;
                    }

                    return value;
                }
            }
            private set => this.value = value;
        }

        public int Count => listeners.Count;

        public void Dispose()
        {
#if !BROWSER
            fileSystemWatcher.Dispose();
#endif
        }

        public void AddListener(FileWatcher listener)
        {
            listeners.Add(listener);
        }

        public void RemoveListener(FileWatcher listener)
        {
            listeners.Remove(listener);
        }

        private void Update()
        {
#if !BROWSER
            try
            {
                fileSystemWatcher.EnableRaisingEvents = false;
                isLatestValue = false;
                foreach (var listener in listeners) listener.NotifyChanged();
            }
            finally
            {
                fileSystemWatcher.EnableRaisingEvents = true;
            }
#endif
        }
    }
}