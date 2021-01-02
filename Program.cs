using System;
using System.Threading;
using System.Threading.Tasks;

namespace BC_TimerTest
{
    class Program
    {
        private static int _time = 5;
        private static bool _toggle = true;
        private static int _baseTime = 5;
        private static int _stepTime = 1;
        private static int _dynamicTime = 3;
        private static int _maxTime = 12;
        private static DateTime _start = DateTime.Now;
        private static bool _isClosed = false;
        private static bool _progRunning = false;
        private static CancellationTokenSource cts = new CancellationTokenSource();

        private enum ToggleMode
        {
            All,
            Toggle,
            Random
        }

        static void Main(string[] args)
        {
            ConsoleKeyInfo cki;
            Console.TreatControlCAsInput = true;

            Console.WriteLine("Press D to run Dynamic");
            Console.WriteLine("Press F to run Fixed");
            Console.WriteLine("Press R to run Random");
            Console.WriteLine("Press T to run Toggle");
            Console.WriteLine("Press S to Stop");
            Console.WriteLine("Press the Escape (Esc) key to quit: \n");
            do
            {
                cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.T)
                {
                    Console.WriteLine("Ok, Let's GO");
                    Console.WriteLine("Get prepared for 10s");
                    _start = DateTime.Now;

                    Task<bool> DoBeep = Beep(10 - 2);
                    for (int i = 0; i < 10; i++)
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine((10 - i).ToString() + "s");
                    }

                    _start = DateTime.Now;
                    cts = new CancellationTokenSource();
                    
                    ThreadPool.QueueUserWorkItem(new WaitCallback(RunT), cts.Token);
                    
                    _progRunning = true;
                }

                if (cki.Key == ConsoleKey.D)
                {
                    Console.WriteLine("Ok, Let's GO");
                    Console.WriteLine("Get prepared for 10s");
                    _start = DateTime.Now;

                    Task<bool> DoBeep = Beep(10 - 2);
                    for (int i = 0; i < 10; i++)
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine((10 - i).ToString() + "s");
                    }

                    _start = DateTime.Now;
                    cts = new CancellationTokenSource();

                    ThreadPool.QueueUserWorkItem(new WaitCallback(RunD), cts.Token);

                    _progRunning = true;
                }

                if (cki.Key == ConsoleKey.F)
                {
                    Console.WriteLine("Ok, Let's GO");
                    Console.WriteLine("Get prepared for 10s");
                    _start = DateTime.Now;

                    Task<bool> DoBeep = Beep(10 - 2);
                    for (int i = 0; i < 10; i++)
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine((10 - i).ToString() + "s");
                    }

                    _start = DateTime.Now;
                    cts = new CancellationTokenSource();

                    ThreadPool.QueueUserWorkItem(new WaitCallback(RunF), cts.Token);

                    _progRunning = true;
                }

                if (cki.Key == ConsoleKey.R)
                {
                    Console.WriteLine("Ok, Let's GO");
                    Console.WriteLine("Get prepared for 10s");
                    _start = DateTime.Now;

                    Task<bool> DoBeep = Beep(10 - 2);
                    for (int i = 0; i < 10; i++)
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine((10 - i).ToString() + "s");
                    }

                    _start = DateTime.Now;
                    cts = new CancellationTokenSource();

                    ThreadPool.QueueUserWorkItem(new WaitCallback(RunR), cts.Token);

                    _progRunning = true;
                }

                if (cki.Key == ConsoleKey.S)
                {
                    cts.Cancel();
                    cki = new ConsoleKeyInfo();
                    Thread.Sleep(1000);
                    _progRunning = false;
                }
            } while (cki.Key != ConsoleKey.Escape);
        }

        private static void Toggle(ToggleMode mode)
        {
            _toggle = _toggle ? false : true;
            ToggleMode _mode = mode;
            Random rand = new Random();

            if(_mode == ToggleMode.Random)
            {
                int i = rand.Next(1, 100);

                _mode = (i % 2 == 0) ? ToggleMode.All : ToggleMode.Toggle;
            }

            if (_toggle)
            {
                switch(_mode)
                {
                    case ToggleMode.All:
                        Console.WriteLine("{0:h:mm:ss.fff} Âll open after elapsed {1}", DateTime.Now, DateTime.Now - _start);
                        break;
                    case ToggleMode.Toggle:
                        Console.WriteLine("{0:h:mm:ss.fff} 1 open - 2 close after elapsed {1}", DateTime.Now, DateTime.Now - _start);
                        break;
                }
                
                _start = DateTime.Now;
                _isClosed = false;
            }
            else
            {
                switch (_mode)
                {
                    case ToggleMode.All:
                        Console.WriteLine("{0:h:mm:ss.fff} Âll closed after elapsed {1}", DateTime.Now, DateTime.Now - _start);
                        break;
                    case ToggleMode.Toggle:
                        Console.WriteLine("{0:h:mm:ss.fff} 1 closed - 2 open after elapsed {1}", DateTime.Now, DateTime.Now - _start);
                        break;
                }
                
                _start = DateTime.Now;
                _isClosed = true;
            }
        }               
        
        private static void ValesProgAll(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            Toggle(ToggleMode.All);
            autoEvent.Set();
        }

        private static void ValesProgToggle(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            Toggle(ToggleMode.Toggle);
            autoEvent.Set();
        }

        private static void ValesProgRandom(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            Toggle(ToggleMode.Random);
            autoEvent.Set();
        }

        private static async Task<bool> Beep(int _time)
        {
            System.Timers.Timer _timer = new System.Timers.Timer();
            _timer.Interval = _time * 1000;
            _timer.AutoReset = false;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Enabled = true;
            _timer.Start();
            return true;
        }

        private static void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("{0:h:mm:ss.fff} Beep On, elapsed {1}", DateTime.Now, DateTime.Now - _start);
            Thread.Sleep(750);
            Console.WriteLine("{0:h:mm:ss.fff} Beep Off, elapsed {1}", DateTime.Now, DateTime.Now - _start);
        }

        static async void RunT(object obj)
        {
            AutoResetEvent autoEvent;
            Timer stateTimer;
            CancellationToken token = (CancellationToken)obj;
            int _counter = 0;
            Task<bool> DoBeep;

            Console.WriteLine("{0:h:mm:ss.fff} Creating timer.", DateTime.Now);
            _toggle = true;
            Toggle(ToggleMode.Toggle);

            autoEvent = new AutoResetEvent(false);
            _time = _dynamicTime;

            stateTimer = new Timer(ValesProgToggle, autoEvent, (_time * 1000), 10);
            
            _time += _stepTime;

            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    _toggle = false;
                    Toggle(ToggleMode.Toggle);
                    break;
                }
                _counter++;
                if (_counter > 1)
                {
                    if (token.IsCancellationRequested)
                    {
                        _toggle = false;
                        Toggle(ToggleMode.Toggle);
                        break;
                    }

                    Console.WriteLine("Pause");

                    if (_isClosed)
                    {
                        _toggle = false;
                        Toggle(ToggleMode.Toggle);
                    }

                    DoBeep = Beep(_baseTime -2);
                    stateTimer.Change(_baseTime * 1000, 10);
                    autoEvent.WaitOne();

                    Console.WriteLine("Restart");
                   
                    stateTimer.Change((_dynamicTime * 1000), 10);
                }

                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        _toggle = false;
                        Toggle(ToggleMode.Toggle);
                        break;
                    }

                    autoEvent.WaitOne();

                    if (_time <= _maxTime)
                    {
                        if (!_isClosed)
                        {
                            DoBeep = Beep(_baseTime -2);
                            stateTimer.Change((_baseTime * 1000), 10);
                        }
                        else
                        {
                            stateTimer.Change((_time * 1000), 10);
                            _time += _stepTime;
                        }
                    }
                    else
                    {
                        if (_time > _maxTime)
                        {
                            _time = _dynamicTime + _stepTime;
                            break;
                        }
                    }
                }
            }

            stateTimer.Dispose();
            autoEvent.Dispose();
            Console.WriteLine("{0:h:mm:ss.fff} Destroying timer.", DateTime.Now);
        }

        static async void RunD(object obj)
        {
            AutoResetEvent autoEvent;
            Timer stateTimer;
            CancellationToken token = (CancellationToken)obj;
            int _counter = 0;
            Task<bool> DoBeep;

            Console.WriteLine("{0:h:mm:ss.fff} Creating timer.", DateTime.Now);
            _toggle = true;
            Toggle(ToggleMode.All);

            autoEvent = new AutoResetEvent(false);
            _time = _dynamicTime;

            stateTimer = new Timer(ValesProgAll, autoEvent, (_time * 1000), 10);

            _time += _stepTime;

            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    _toggle = false;
                    Toggle(ToggleMode.All);
                    break;
                }
                _counter++;
                if (_counter > 1)
                {
                    if (token.IsCancellationRequested)
                    {
                        _toggle = false;
                        Toggle(ToggleMode.All);
                        break;
                    }

                    Console.WriteLine("Pause");

                    if (_isClosed)
                    {
                        _toggle = false;
                        Toggle(ToggleMode.All);
                    }

                    DoBeep = Beep(_baseTime - 2);
                    stateTimer.Change(_baseTime * 1000, 10);
                    autoEvent.WaitOne();

                    Console.WriteLine("Restart");

                    stateTimer.Change((_dynamicTime * 1000), 10);
                }

                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        _toggle = false;
                        Toggle(ToggleMode.All);
                        break;
                    }

                    autoEvent.WaitOne();

                    if (_time <= _maxTime)
                    {
                        if (!_isClosed)
                        {
                            DoBeep = Beep(_baseTime - 2);
                            stateTimer.Change((_baseTime * 1000), 10);
                        }
                        else
                        {
                            stateTimer.Change((_time * 1000), 10);
                            _time += _stepTime;
                        }
                    }
                    else
                    {
                        if (_time > _maxTime)
                        {
                            _time = _dynamicTime + _stepTime;
                            break;
                        }
                    }
                }
            }

            stateTimer.Dispose();
            autoEvent.Dispose();
            Console.WriteLine("{0:h:mm:ss.fff} Destroying timer.", DateTime.Now);
        }

        static async void RunF(object obj)
        {
            AutoResetEvent autoEvent;
            Timer stateTimer;
            CancellationToken token = (CancellationToken)obj;
            Task<bool> DoBeep;

            Console.WriteLine("{0:h:mm:ss.fff} Creating timer.", DateTime.Now);
            _toggle = true;
            Toggle(ToggleMode.All);

            autoEvent = new AutoResetEvent(false);
            _time = _dynamicTime;

            stateTimer = new Timer(ValesProgAll, autoEvent, (_baseTime * 1000), 10);

            _time += _stepTime;
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    _toggle = false;
                    Toggle(ToggleMode.All);
                    break;
                }
                
                autoEvent.WaitOne();

                if (!_isClosed)
                {
                    DoBeep = Beep(_baseTime - 2);
                    stateTimer.Change((_baseTime * 1000), 10);
                }
                else
                {
                    stateTimer.Change((_baseTime * 1000), 10);
                }
            }

            stateTimer.Dispose();
            autoEvent.Dispose();
            Console.WriteLine("{0:h:mm:ss.fff} Destroying timer.", DateTime.Now);
        }

        static async void RunR(object obj)
        {
            AutoResetEvent autoEvent;
            Timer stateTimer;
            CancellationToken token = (CancellationToken)obj;
            Task<bool> DoBeep;
            Random rand = new Random();

            Console.WriteLine("{0:h:mm:ss.fff} Creating timer.", DateTime.Now);
            _toggle = true;
            Toggle(ToggleMode.Random);

            autoEvent = new AutoResetEvent(false);
            _time = _baseTime * rand.Next(1,_dynamicTime);

            stateTimer = new Timer(ValesProgRandom, autoEvent, (_time * 1000), 10);

            _time += _stepTime;
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    _toggle = false;
                    Toggle(ToggleMode.All);
                    break;
                }

                autoEvent.WaitOne();

                _time = _baseTime * rand.Next(1, _dynamicTime);
                if (!_isClosed)
                {
                    DoBeep = Beep(_time - 2);
                    stateTimer.Change((_time * 1000), 10);
                }
                else
                {
                    stateTimer.Change((_time * 1000), 10);
                }
            }

            stateTimer.Dispose();
            autoEvent.Dispose();
            Console.WriteLine("{0:h:mm:ss.fff} Destroying timer.", DateTime.Now);
        }
    }
}