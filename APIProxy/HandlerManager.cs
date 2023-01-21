// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Diagnostics;

internal class HandlerManager
{
    public Dictionary<string, IHandler> Handlers { get; set; } = new Dictionary<string, IHandler>();
    public HandlerManager()
    {
    }

    internal HandlerManager WithHandler(IHandler handler)
    {
        Debug.Assert(!Handlers.ContainsKey(handler.Name), $"Handler {handler.Name} already exists");
        Debug.Assert(!string.IsNullOrEmpty(handler.Name), "Handler name cannot be empty");
        Handlers.Add(handler.Name, handler);
        return this;
    }

    internal IHandler GetHandler(string name)
    {
        if (!Handlers.ContainsKey(name))
        {
            throw new ArgumentException($"Handler {name} not found");
        }
        return Handlers[name];
    }

    public object Handle(string name, string[] args)
    {
        var handler = GetHandler(name);
        return handler.Handle(args);
    }


}