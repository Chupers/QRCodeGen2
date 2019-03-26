using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing;
using ZXing.Net.Mobile.Forms;
using ZXing.QrCode;

namespace QRCodeGen
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        public int Get_Key()
        {
            return DateTime.Now.Minute;
        }
      public static class Credit
        {
            
            public static string Nubmer = "1020-1941-1485-8851";
            public static string Date = "02.20";
            public static string result;
            public static void Get_res()
            {
                result = Nubmer + " " + Date;
            }
           static byte[] b;
           static public byte[] toBit(string s)
            {

                byte[] b = Encoding.UTF8.GetBytes(s);

                return b;
            }

          static  public string From_bit(byte[] vs)
            {
                 result = Encoding.UTF8.GetString(vs);
                return result;
            }
        }

        Image qrCode;
        private View lblExplenationText;

        public View header { get; private set; }

        private void Gen_Clicked(object sender, EventArgs e)
        {
            int code = Get_Key();
            Credit.Get_res();

         
            string info = Credit.result; char[] chars = new char[50];
            string str;
            for (int i = 0; i < info.Length; i++)
            {
                chars[i] = (char)((int)info[i] + code);
            };
            str = new string(chars);
            info = str;
            if (Device.OS == TargetPlatform.iOS)
            {
                var writer = new BarcodeWriter
                {
                    Format  = BarcodeFormat.QR_CODE,
                    Options = new ZXing.Common.EncodingOptions
                    {
                        Width = 50,
                        Height = 50                    }
                };

                var b = writer.Write(info);

                qrCode = new Image
                {
                    Aspect = Aspect.AspectFill,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Source = ImageSource.FromStream(() =>
                    {
                        System.IO.MemoryStream ms = new System.IO.MemoryStream(b);
                        ms.Position = 0;
                        return ms;
                      
                    })
                };

            }
            else
            {
                qrCode = new ZXingBarcodeImageView
                {
                    BarcodeFormat = BarcodeFormat.QR_CODE,
                    BarcodeOptions = new QrCodeEncodingOptions
                    {
                        Height = 500,
                        Width = 500
                    },
                    BarcodeValue = info,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
            }
         
            New = qrCode;
            iu.Children.Insert(4, qrCode);
            



        }

        private async void Scan_button_Clicked(object sender, EventArgs e)
        {
            int code = Get_Key();
            var scan = new ZXingScannerPage();
            await Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
             {
                 Device.BeginInvokeOnMainThread(async () =>
                 {
                     await Navigation.PopAsync();
                     string str;  char[] chars = new char[50];
                     for (int i = 0; i < result.Text.Length; i++)
                     {
                         chars[i] = (char)((int)result.Text[i] - code);
                     };
                     str = new string(chars);
                    
                     Res.Text = str;
                 });
             };


        }
    }
}
    

