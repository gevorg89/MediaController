using SocketIOSharp.Server.Client;
using SocketIOSharp.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketIOSharp.Common;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace WindowsFormsMedia
{
    enum SocketEvent : uint
    {
        VolumeUp =0,
        Previous= 1,
        PlayPause =2,
        Next =3,
        VolumeDown =4,
        MoveLeft = 5,
        MoveTop =6,
        MoveRight =7,
        MoveBottom =8,
        Click = 9,
    }

    public class Server
    {
        private IController _controller;
        private SocketIOServer server;
        private const String EVENT_NAME = "SocketEvent";

        public Server(IController controller)
        {
            _controller = controller;
        }
        
        public void start()
        {
            server = new SocketIOServer(new SocketIOServerOption(Port:8080, AllowEIO3: true));

            server.OnConnection((SocketIOSocket socket) =>
            {
                Console.WriteLine("Client connected!");

                socket.On(SocketIOEvent.DISCONNECT, () =>
                {
                    Console.WriteLine("Client disconnected!");
                });

                socket.On(EVENT_NAME, (Data) => {
                    foreach (JToken Token in Data)
                    {            
                        Console.WriteLine(Token.ToString());
                        var key = Token.Value<int>();
                        switch (key)
                        {
                            case ((int)SocketEvent.VolumeUp):
                                {
                                    _controller.VolumeUp();
                                    break;
                                };
                            case ((int)SocketEvent.VolumeDown):
                                {
                                    _controller.VolumeDown();
                                    break;
                                };

                            case ((int)SocketEvent.Previous):
                                {
                                    _controller.Prevoius();
                                    break;
                                };

                            case ((int)SocketEvent.Next):
                                {
                                    _controller.Next();
                                    break;
                                };

                            case ((int)SocketEvent.PlayPause):
                                {
                                    _controller.PlayPause();
                                    break;
                                };

                            case ((int)SocketEvent.MoveLeft):
                                {
                                    _controller.MoveLeft();
                                    _controller.EmitScreenCapture();
                                    break;
                                };

                            case ((int)SocketEvent.MoveTop):
                                {
                                    _controller.MoveTop();
                                    _controller.EmitScreenCapture();
                                    break;
                                };
                            case ((int)SocketEvent.MoveRight):
                                {
                                    _controller.MoveRight();
                                    _controller.EmitScreenCapture();
                                    break;
                                };

                            case ((int)SocketEvent.MoveBottom):
                                {
                                    _controller.MoveBottom();
                                    _controller.EmitScreenCapture();
                                    break;
                                };

                            case ((int)SocketEvent.Click):
                                {
                                    _controller.Click();
                                    break;
                                };
                        }

                    }
                });

            });

            server.Start();
        }

        public void stop()
        {
            server.Stop();
        }

        internal void SendImage(string sigBase64)
        {
            server.Emit("server", sigBase64);
        }
    }
}
