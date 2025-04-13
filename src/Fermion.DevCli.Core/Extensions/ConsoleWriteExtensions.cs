namespace Fermion.DevCli.Core.Extensions;

public static class ConsoleWriteExtensions
{
    public static void PrintMessage(
        string message,
        MessageType type = MessageType.Info,
        bool isHeader = false)
    {
        switch (type)
        {
            case MessageType.Info:
                Console.ForegroundColor = ConsoleColor.Cyan;
                break;
            case MessageType.Success:
                Console.ForegroundColor = ConsoleColor.Green;
                break;
            case MessageType.Warning:
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;
            case MessageType.Error:
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            case MessageType.Directory:
                Console.ForegroundColor = ConsoleColor.Magenta;
                break;
            case MessageType.File:
                Console.ForegroundColor = ConsoleColor.DarkGray;
                break;
        }

        if (isHeader)
        {
            string line = new string('=', message.Length + 4);
            Console.WriteLine(line);
            Console.WriteLine($"| {message} |");
            Console.WriteLine(line);
        }
        else
        {
            string prefix = type switch
            {
                MessageType.Info => "ℹ️ ",
                MessageType.Success => "✅ ",
                MessageType.Warning => "⚠️ ",
                MessageType.Error => "❌ ",
                MessageType.Directory => "📂 ",
                MessageType.File => "📄 ",
                _ => ""
            };

            Console.WriteLine($"{prefix}{message}");
        }

        if (type == MessageType.Error || type == MessageType.Success)
        {
            Console.ResetColor();
        }
    }
}

public enum MessageType
{
    Info,
    Success,
    Warning,
    Error,
    Directory,
    File
}