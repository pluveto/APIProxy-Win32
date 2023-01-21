// See https://aka.ms/new-console-template for more information
internal interface IHandler
{
    string Name { get; }

    public object Handle(string[] args);
}