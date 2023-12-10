using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using QRCoder;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;
using Bunkering.Core.Utils;
using Microsoft.Extensions.Configuration;
using Bunkering.Core.Data;

namespace Bunkering.Access
{
    public static class Utils
    {
        public static string Stringify(this object any) => JsonConvert.SerializeObject(any);

        public static string GetValue(this Dictionary<string, string> dic, string key)
            => dic.FirstOrDefault(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).Value;

        public static object GetValue(this Dictionary<string, object> dic, string key)
            => dic.FirstOrDefault(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).Value;

        public static TOut Parse<TOut>(this string any) =>
            JsonConvert.DeserializeObject<TOut>(any, new JsonSerializerSettings
            {
                Error = delegate (object sender, ErrorEventArgs args)
                {
                    args.ErrorContext.Handled = true;
                },
                Converters = { new IsoDateTimeConverter() }
            });

        public static async Task<HttpResponseMessage> Send(string url, HttpRequestMessage message)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls 
                                                   | SecurityProtocolType.Tls11 
                                                   | SecurityProtocolType.Tls12;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                return await client.SendAsync(message);
            }
        }

        public static string GenerateSha512(this string inputString)
        {
            SHA512 sha512 = SHA512.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        private static readonly Object lockThis = new object();

        public static string RefrenceCode()
        {
            lock (lockThis)
            {
                Thread.Sleep(1000);
                return $"214{DateTime.Now.ToString("MMddyyHHmmss")}";
            }
        }

        public static void SendMail(Dictionary<string, string> mailsettings, string toEmail, string subject, string body, string bcc = null)
        {
            var credentials = new NetworkCredential(mailsettings.GetValue("UserName"), mailsettings.GetValue("mailPass"));
            var smtp = new SmtpClient(mailsettings.GetValue("mailHost"), int.Parse(mailsettings.GetValue("ServerPort")))
            {
                EnableSsl = bool.Parse(mailsettings.GetValue("UseSsl")),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = credentials
            };


            var mail = new MailMessage {From = new MailAddress(mailsettings.GetValue("mailSender"))};
            mail.To.Add(new MailAddress(toEmail));

            if (!string.IsNullOrEmpty(bcc))
            {
                var copies = bcc.Split(',');
                foreach (var email in copies)
                    mail.Bcc.Add(new MailAddress(email));
            }
            
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            smtp.Send(mail);
        }

        public static string ReadTextFile(string webrootpath, string filename)
        {
            string body;
            using (var sr = new StreamReader($"{webrootpath}\\Templates\\{filename}"))
            {
                body =  sr.ReadToEndAsync().Result;
            }
            return body;
        }
        
        private static Byte[] BitmapToBytes(Bitmap img)
        {
            using MemoryStream stream = new MemoryStream();
            img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return stream.ToArray();
        }

        public static Byte[] GenerateQrCode(string url)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            var imageResult = BitmapToBytes(qrCodeImage);
            return imageResult;
        }

        public static string EncryptString(this string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }

        private static string key = "Bunering*123*Brandonetech#";
        public static string NOA = "NOA"; //notice of arrival

        public static string DecryptString(this string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}