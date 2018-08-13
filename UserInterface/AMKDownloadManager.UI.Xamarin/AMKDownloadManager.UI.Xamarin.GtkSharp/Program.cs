using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using AMKDownloadManager.Core;
using Mono.Unix;
using Mono.Unix.Native;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;
using Xamarin.Forms.Platform.GTK.Helpers;
using Xamarin.Forms.Xaml;

namespace AMKDownloadManager.UI.Xamarin.GtkSharp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            GtkThemes.Init();

            try
            {
               //if (PlatformHelper.GetGTKPlatform() == GTKPlatform.Windows)
                    GtkThemes.LoadCustomTheme("Themes/gtkrc-dark");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            Gtk.Application.Init();
            Forms.Init();

            App app;
            if ((app = UIInitializer.CreateApp(args, out var platformServices)) != null)
            {
                if (ApplicationContext.IsLinux)
                {
                    HandleSignals(platformServices);
                }

                var window = new FormsWindow();
                window.LoadApplication(app);
                window.SetApplicationTitle("AMKDownloadManager");
                window.SetApplicationIcon("icon.png");
                window.Show();
                Gtk.Application.Run();
            }
        }

        public static void HandleSignals(IPlatformInterface platformServices)
        {
            var signalHandler = new Thread(new ParameterizedThreadStart(ListenToSignal));
            signalHandler.Start(platformServices);
        }
        
        private static void ListenToSignal(object state)
        {
            var platformServices = state as IPlatformInterface;
            if (platformServices == null) return;
            
            var listenToSignals = true;
            while (listenToSignals)
            {
                var signals = new List<UnixSignal>
                {
                    new UnixSignal(Mono.Unix.Native.Signum.SIGHUP),
                    new UnixSignal(Mono.Unix.Native.Signum.SIGINT),
                    new UnixSignal(Mono.Unix.Native.Signum.SIGQUIT),
                    new UnixSignal(Mono.Unix.Native.Signum.SIGTERM)
                };

                int index = UnixSignal.WaitAny(signals.ToArray());
                var signal = signals[index].Signum;

                switch (signal)
                {
                    case Signum.SIGHUP:
                        break;
                    case Signum.SIGINT:
                        break;
                    case Signum.SIGQUIT:
                        listenToSignals = false;
                        platformServices.SignalReceived(ApplicationSignals.Quit);
                        break;
                    case Signum.SIGILL:
                        break;
                    case Signum.SIGTRAP:
                        break;
                    case Signum.SIGABRT:
                        break;
                    case Signum.SIGBUS:
                        break;
                    case Signum.SIGFPE:
                        break;
                    case Signum.SIGKILL:
                        break;
                    case Signum.SIGUSR1:
                        break;
                    case Signum.SIGSEGV:
                        break;
                    case Signum.SIGUSR2:
                        break;
                    case Signum.SIGPIPE:
                        break;
                    case Signum.SIGALRM:
                        break;
                    case Signum.SIGTERM:
                        break;
                    case Signum.SIGSTKFLT:
                        break;
                    case Signum.SIGCLD:
                        break;
                    case Signum.SIGCONT:
                        break;
                    case Signum.SIGSTOP:
                        break;
                    case Signum.SIGTSTP:
                        break;
                    case Signum.SIGTTIN:
                        break;
                    case Signum.SIGTTOU:
                        break;
                    case Signum.SIGURG:
                        break;
                    case Signum.SIGXCPU:
                        break;
                    case Signum.SIGXFSZ:
                        break;
                    case Signum.SIGVTALRM:
                        break;
                    case Signum.SIGPROF:
                        break;
                    case Signum.SIGWINCH:
                        break;
                    case Signum.SIGPOLL:
                        break;
                    case Signum.SIGPWR:
                        break;
                    case Signum.SIGSYS:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}