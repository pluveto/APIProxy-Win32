// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Windows.Forms;

internal class ClipboardHandler : IHandler
{
    public ClipboardHandler()
    {
    }

    public string Name { get => "clipboard"; }

    public object Handle(string[] args)
    {
         if(args[0] == "GetFilePaths") {
            var data = new List<string>();
            foreach (var file in Clipboard.GetFileDropList()) {
                data.Add(file);
            }
            return data.ToArray();
        }
        throw new NotImplementedException();
    }
}