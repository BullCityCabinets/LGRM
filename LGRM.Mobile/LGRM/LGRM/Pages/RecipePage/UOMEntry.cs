using System;
using Xamarin.Forms;

namespace LGRM.XamF.Pages
{
        public class UOMEntry : Entry
        {
            int widthReq = 60;
            int minWidthReq = 60;
            public double fontB = Device.GetNamedSize(NamedSize.Body, typeof(Label));

            public UOMEntry()
            {
            Application.Current.Resources.TryGetValue("DefaultTextColor", out var resourceValue);
            var defaultTextColor = (Color)resourceValue;

            Keyboard = Keyboard.Numeric;

                FontSize = fontB;

                MinimumWidthRequest = minWidthReq;
                WidthRequest = widthReq;

                HorizontalOptions = LayoutOptions.End;
                HorizontalTextAlignment = TextAlignment.End;
                Margin = 0; //new Thickness(8, 0, 0, 0);
                HeightRequest = 38;
                BackgroundColor = Color.LightGoldenrodYellow;
            TextColor = defaultTextColor;



            }

        }





    
}

