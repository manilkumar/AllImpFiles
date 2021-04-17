using System;
using System.IO;
using System.Threading;

namespace ConsoleApp2
{
    class Program
    {
        static void WorkOnData(object data)
        {
            Console.WriteLine("Working on: {0}", data);
            Thread.Sleep(1000);
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ParameterizedThreadStart ps = new ParameterizedThreadStart(WorkOnData);

            Thread thread = new Thread(ps);
            thread.Start(99);

            Fruit fruit = new Watermelon();
            Console.WriteLine(fruit.GetColor());
            fruit = new Banana();
            Console.WriteLine(fruit.GetColor());
        }
    }


    public class Music
    {
        public virtual void PMusic()
        {
            // From base
        }
    }


    public class PlayMusic : Music
    {
        public override void PMusic()
        {
            // Code for play music
        }
    }


    public class PauseMusic : Music
    {
        public override void PMusic()
        {
            // Code for pause music
        }
    }

    public abstract class Fruit
    {
        public abstract string GetColor();
    }

    public class Banana : Fruit
    {
        public override string GetColor()
        {
            return "Yellow";
        }
    }

    public class Watermelon : Banana
    {
        public override string GetColor()
        {
            return "Green";
        }
    }

    /* ========= Publisher of the Event ============== */
    public class MyClass
    {
        // Define a delegate named LogHandler, which will encapsulate
        // any method that takes a string as the parameter and returns no value
        public delegate void LogHandler(string message);

        // Define an Event based on the above Delegate
        public event LogHandler Log;

        // Instead of having the Process() function take a delegate
        // as a parameter, we've declared a Log event. Call the Event,
        // using the OnXXXX Method, where XXXX is the name of the Event.
        public void Process()
        {
            OnLog("Process() begin");
            OnLog("Process() end");
        }

        // By Default, create an OnXXXX Method, to call the Event
        protected void OnLog(string message)
        {
            if (Log != null)
            {
                Log(message);
            }
        }
    }

    // The FileLogger class merely encapsulates the file I/O
    public class FileLogger
    {
        FileStream fileStream;
        StreamWriter streamWriter;

        // Constructor
        public FileLogger(string filename)
        {
            fileStream = new FileStream(filename, FileMode.Create);
            streamWriter = new StreamWriter(fileStream);
        }

        // Member Function which is used in the Delegate
        public void Logger(string s)
        {
            streamWriter.WriteLine(s);
        }

        public void Close()
        {
            streamWriter.Close();
            fileStream.Close();
        }
    }

    /* ========= Subscriber of the Event ============== */
    // It's now easier and cleaner to merely add instances
    // of the delegate to the event, instead of having to
    // manage things ourselves
    public class TestApplication
    {
        public delegate void TestDelegate();
        static void Logger(string s)
        {
            Console.WriteLine(s);
        }

        static void Main(string[] args)
        {
            FileLogger fl = new FileLogger("process.log");
            MyClass myClass = new MyClass();

            // Subscribe the Functions Logger and fl.Logger
            myClass.Log += new MyClass.LogHandler(Logger);
            myClass.Log += new MyClass.LogHandler(fl.Logger);

            // The Event will now be triggered in the Process() Method
            myClass.Process();

            fl.Close();
        }
    }

}
