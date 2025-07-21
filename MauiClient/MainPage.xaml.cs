using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Client;
using SharedContract;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace MauiClient
{
    public partial class MainPage : ContentPage 
    {
        private string _lastText = "Not Connected";
        public string LastText
        {
            get => _lastText;
            set
            {
                if (_lastText != value)
                {
                    _lastText = value;
                    OnPropertyChanged(); // reports this property
                }
            }
        }

        private CancellationTokenSource _callbackCancellationToken = new CancellationTokenSource();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;

            // Bypass SSL check on localhost for android
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert != null && cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            var handler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, httpHandler); // gRPC web for Blazor compatibility

            // Switch server address for android emulator
            var server = "https://localhost:5001";
            if (DeviceInfo.Platform.ToString().Contains("Android"))
                server = "https://10.0.2.2:5001";

            var channel = GrpcChannel.ForAddress(server, new GrpcChannelOptions { HttpHandler = handler, HttpVersion = new Version(1, 1), DisposeHttpClient = true, MaxReceiveMessageSize = null });
            var service = channel.CreateGrpcService<IgRPCService>();

            var callbackHandler = Task.Run(async () => await CallbackHandler(service.SubscribeToCallback(new CallContext(new CallOptions(cancellationToken: _callbackCancellationToken.Token)))), _callbackCancellationToken.Token);
        }

        private void OnDisconnectClicked(object? sender, EventArgs e)
        {
            //!! THIS IS THE LINE THAT CRASH WITH A UNBALANCED ENTER/EXIT EXCEPTION ON ANDROID !!
            _callbackCancellationToken.Cancel(); // Cancel the callback subscription. 

            LastText = "Disconnected";
        }

        private async Task CallbackHandler(IAsyncEnumerable<Callback> callbacks)
        {
            LastText = "Connected";
            try
            {
                await foreach (var callback in callbacks)
                {
                    LastText = callback.Text;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
