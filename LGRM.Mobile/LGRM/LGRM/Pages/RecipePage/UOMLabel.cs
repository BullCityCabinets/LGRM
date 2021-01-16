using Xamarin.Forms;

namespace LGRM.XamF.Pages
{

        public class UOMLabel : Label
        {
            public double fontB = Device.GetNamedSize(NamedSize.Body, typeof(Label));

            public UOMLabel()
            {
            FontSize = fontB;
            VerticalTextAlignment = TextAlignment.Center;                        
            HorizontalTextAlignment = TextAlignment.End;
            LineBreakMode = LineBreakMode.TailTruncation;

            Margin = 0;
            Padding = new Thickness(0, 0, 8, 0); // -10);                       
            
            
            }
        }





    
}

