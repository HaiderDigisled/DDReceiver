using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;

namespace Test_Stream
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.TCP_Listener();
            
        }

        public void TCP_Listener() {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 16384;
                IPAddress localAddr = IPAddress.Any;

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[1024];
                String data = null;

                // Enter the listening loop.
                while (true)
                {

                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    try
                    {

                        data = null;

                        // Get a stream object for reading and writing
                        NetworkStream stream = client.GetStream();

                        int i;

                        //var content = stream.Read(bytes);
                        //Loop to receive all the data sent by the client.
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            // Translate data bytes to a ASCII string.
                            data = System.Text.Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                            Console.WriteLine("Received: {0}", data);

                            // Process the data sent by the client.
                            data = data.ToUpper();

                            //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                            // Send back a response.
                            //stream.Write(msg, 0, msg.Length);
                            //Console.WriteLine("Sent: {0}", msg);
                            var dir = Directory.GetCurrentDirectory();
                            var file = $"Data_{DateTime.Now.ToString("yyyy-MM-dd")}.txt";
                            var fulldir = string.Concat(dir, "\\", file);

                            var fileInfo = new FileInfo(fulldir);

                            using (StreamWriter writer = fileInfo.AppendText())
                            {
                                writer.WriteLine(string.Concat(data, Environment.NewLine));
                            };
                            //SendEmail("File Written", string.Concat(data, ""), "Success Alert");
                            //stream.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception : {ex.Message} :  StackTrace : {ex.StackTrace}");
                        //SendEmail("Device Stream API", string.Concat(ex.StackTrace, "------", ex.Message), "Failure Alert - ");
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

   
        public void SendEmail(string subject, string body,string ALertType)
        {

            MailMessage mm = new MailMessage();

            mm.From = new MailAddress("info.rapidosolution@gmail.com");
            string password = "Rapido123";

            mm.To.Add("haiderzaidi94@hotmail.com");
            mm.To.Add("shanhaider802qw@gmail.com");


            mm.Subject = string.Concat(ALertType, subject);
            mm.Body = body;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential(mm.From.Address, password);
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            smtp.Send(mm);
            Console.WriteLine($"{ALertType} Sent");
        }
    }
}
