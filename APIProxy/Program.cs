
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;


internal class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        // Setup global exception handler
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            var ret = AppReturn<Object>.ofAppError(args.ExceptionObject switch
            {
                Exception e => e.Message,
                _ => "Unknown error"
            });
            Exit(ret, 1);
        };

        // Setup handler manager
        var hm = new HandlerManager();
        hm.WithHandler(new ClipboardHandler());

        // Parse args
        if (args.Length < 1) throw new ArgumentException("No handler name specified");
        // Split args, first as handler name, rest as args
        var (handlerName, handlerArgs) = (args[0], args.Skip(1).ToArray());
        Trace.WriteLine($"Handler name: {handlerName}");
        Trace.WriteLine($"Handler args: {string.Join(" ", handlerArgs)}");

        // Handle
        try
        {
            var result = hm.Handle(handlerName, handlerArgs);
            Exit(AppReturn<Object>.ofSuccess(result), 0);
        }
        catch (System.Exception e)
        {
            Exit(AppReturn<Object>.ofAppError(e.Message), 1);
        }
    }

    static void Exit<T>(AppReturn<T> ret, int exitCode = 0) where T : class
    {
        Console.WriteLine("Type: " + ret.Type);
        Console.WriteLine("Msg: " + ret.Msg);
        Console.WriteLine("HMsg: " + ret.HMsg);
        Console.Write("Data: ");
        if (ret.Data != null)
        {
            Console.WriteLine(JsonSerializer(ret.Data));
        } else
        {
            Console.WriteLine("null");
        }
        System.Environment.Exit(exitCode);
    }

    public static string JsonSerializer<T>(T t)
    {
        DataContractJsonSerializer ser = new DataContractJsonSerializer(t.GetType());
        MemoryStream ms = new MemoryStream();
        ser.WriteObject(ms, t);
        string jsonString = Encoding.UTF8.GetString(ms.ToArray());
        ms.Close();
        return jsonString;
    }

}
