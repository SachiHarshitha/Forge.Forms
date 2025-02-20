﻿namespace Forge.Application.AvaloniaUI.Infrastructure
{
    public interface IFilePicker
    {
        string GetFile(string fileName, string filter);
    }

    public interface IFileSaver
    {
        string GetFile(string fileName, string filter);
    }
}
