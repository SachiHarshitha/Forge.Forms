﻿namespace Forge.Application.AvaloniaUI.Infrastructure.Internal
{
    internal class DialogFilePicker : IFilePicker
    {
        public string GetFile(string fileName, string filter)
        {
            var openFileDialog = new OpenFileDialog
            {
                FileName = fileName ?? string.Empty,
                Filter = filter
            };

            return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
        }
    }
}
