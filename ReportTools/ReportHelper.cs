using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml;
using Microsoft.Reporting.WebForms;

namespace ReportTools
{
    public static class  ReportHelper
    {

        public class PageInfo
        {
            public double PageWidth;
            public double PageHeight;
            public double MarginTop;
            public double MarginLeft;
            public double MarginRight;
            public double MarginBottom;
        }

        public static PageInfo ParsePageInfoString(string strvalues)
        {
            PageInfo pageinfo = new PageInfo();
            var fields = typeof(PageInfo).GetFields().ToDictionary(x => x.Name);
            if (string.IsNullOrWhiteSpace(strvalues)) return pageinfo;
            var values = strvalues.Split(new char[] { ',' });
            var queuelist = new Queue<string>(new string[] { "PageWidth", "PageHeight", "MarginTop", "MarginLeft", "MarginRight", "MarginBottom" });

            foreach (var value in values)
            {
                double p;
                var fieldname = queuelist.Dequeue();
                p = XmlConvert.ToDouble(value);
                if (p == double.NaN) continue;
                fields[fieldname].SetValue(pageinfo, p);

            }
            return pageinfo;
        }

        private const string DeviceInfoFormat =
        @"  <DeviceInfo>
                <OutputFormat>PDF</OutputFormat> 
                <PageWidth>{0}in</PageWidth> 
                <PageHeight>{1}in</PageHeight> 
                <MarginTop>{2}in</MarginTop> 
                <MarginLeft>{3}in</MarginLeft>
                <MarginRight>{4}in</MarginRight> 
                <MarginBottom>{5}in</MarginBottom>
            </DeviceInfo>";

        private const string DefaultDeviceInfo =
        #region OldSettings
            //"<DeviceInfo>" +
            //"  <OutputFormat>PDF</OutputFormat>" +
            //"  <PageWidth>8.5in</PageWidth>" +
            //"  <PageHeight>11in</PageHeight>" +
            //"  <MarginTop>0.25in</MarginTop>" +
            //"  <MarginLeft>0.25in</MarginLeft>" +
            //"  <MarginRight>0.25in</MarginRight>" +
            //"  <MarginBottom>0.25in</MarginBottom>" +
            //"</DeviceInfo>";

              //"<DeviceInfo>" +
            //"  <OutputFormat>PDF</OutputFormat>" +
            //"  <PageWidth>8in</PageWidth>" +
            //"  <PageHeight>11in</PageHeight>" +
            //"  <MarginTop>0.5in</MarginTop>" +
            //"  <MarginLeft>0.5in</MarginLeft>" +
            //"  <MarginRight>0.5in</MarginRight>" +
            //"  <MarginBottom>0.5in</MarginBottom>" +
            //"</DeviceInfo>"; 

            //<ColumnSpacing>0.13cm</ColumnSpacing>
        #endregion
    @"  <DeviceInfo> 
            <OutputFormat>PDF</OutputFormat> 
            <PageSize>A4</PageSize>
            <MarginTop>0.5in</MarginTop>
            <MarginLeft>0.5in</MarginLeft>
            <MarginRight>0.5in</MarginRight>
            <MarginBottom>0.5in</MarginBottom>
        </DeviceInfo>";

        private static string reportType = "PDF";
        
        private const double Inch = 2.54;
        private const int Decimals = 5;

        public static byte[] RenderReport(string reportname,
            string[] reportsourcename,
            object[] reportsource,
            SubreportProcessingEventHandler subreportprocessingeventhandler,
            double pagewidth,
            double pageheight,
            double margintop,
            double marginleft,
            double marginright,
            double marginbottom,
            Dictionary<string, string> reportparameters = null
            )
        {
            var deviceinfo = string.Empty;
            if (pagewidth != 0 && pageheight != 0) 
                deviceinfo = string.Format(DeviceInfoFormat,
                    XmlConvert.ToString(Math.Round(pagewidth / Inch,Decimals)),
                    XmlConvert.ToString(Math.Round(pageheight / Inch, Decimals)),
                    XmlConvert.ToString(Math.Round(margintop / Inch, Decimals)),
                    XmlConvert.ToString(Math.Round(marginleft / Inch, Decimals)),
                    XmlConvert.ToString(Math.Round(marginright / Inch, Decimals)),
                    XmlConvert.ToString(Math.Round(marginbottom / Inch, Decimals))
                );

            return RenderReport(reportname, reportsourcename, reportsource, subreportprocessingeventhandler, deviceinfo, reportparameters);
        }

        private static ReportParameter[] GetReportParameters(Dictionary<string, string> reportparameters)
        {
            ReportParameter[] result = null;
            if (reportparameters == null)
            {
                return result;
            }
            result = reportparameters.Select(x => new ReportParameter(x.Key, x.Value)).ToArray();
            return result;
        }

        public static byte[] RenderReport(string reportname,
            string[] reportsourcename,
            object[] reportsource,
            SubreportProcessingEventHandler subreportprocessingeventhandler,
            string deviceinfo,
            Dictionary<string,string> reportparameters = null
            )
        {
            var odeviceinfo = deviceinfo;
            LocalReport localReport = new LocalReport() { ShowDetailedSubreportMessages = false };
            
            if (subreportprocessingeventhandler != null)
                localReport.SubreportProcessing += new SubreportProcessingEventHandler(subreportprocessingeventhandler);
            localReport.ReportPath = reportname;

            if (string.IsNullOrEmpty(deviceinfo))
            {
                var defaultpagesettings = localReport.GetDefaultPageSettings();
                var margintop = Convert.ToDouble(defaultpagesettings.Margins.Top) / 100;
                var marginleft = Convert.ToDouble(defaultpagesettings.Margins.Left) / 100;
                var marginright = Convert.ToDouble(defaultpagesettings.Margins.Right) / 100;
                var marginbottom = Convert.ToDouble(defaultpagesettings.Margins.Bottom)/ 100;

                var pagewidth =  Convert.ToDouble(defaultpagesettings.PaperSize.Width)/100;
                var pageheight =  Convert.ToDouble(defaultpagesettings.PaperSize.Height)/100;

                odeviceinfo = string.Format(DeviceInfoFormat,
                    XmlConvert.ToString(pagewidth),
                    XmlConvert.ToString(pageheight),
                    XmlConvert.ToString(margintop),
                    XmlConvert.ToString(marginleft),
                    XmlConvert.ToString(marginright),
                    XmlConvert.ToString(marginbottom)
                );

            }
            if (reportsourcename!=null)
                for (int i = 0; i < reportsourcename.Length; i++)
                {
                    ReportDataSource reportDataSource = new ReportDataSource(reportsourcename[i], reportsource[i]);
                    localReport.DataSources.Add(reportDataSource);
                }
            string mimeType;
            string encoding;
            string fileNameExtension;

            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx


            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            //Render the report
            if ((reportparameters != null) && localReport.GetParameters().Count()>0)
            {
                localReport.SetParameters(GetReportParameters(reportparameters));
            }

            renderedBytes = localReport.Render(
                reportType,
                odeviceinfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return renderedBytes;

        }
        public static byte[] RenderReport(string reportname,
            string[] reportsourcename,
            object[] reportsource,
            SubreportProcessingEventHandler subreportprocessingeventhandler,
            Dictionary<string, string> reportparameters = null
            )
        {
            return RenderReport(reportname, reportsourcename, reportsource, subreportprocessingeventhandler, DefaultDeviceInfo, reportparameters);
        }

        public const string IDAutomationHC39MFontName = "IDAutomationHC39M";
        public const string Code128FontName = "Code 128";

        
        private static Brush BlackSolidBrush = new SolidBrush(Color.Black);

        public static byte[] RenderText(

            string text,
            float width,
            float height,
            float resolution,
            string fontname,
            float fontsize)
        {
            byte[] result = null;

            float dotratio = (float)(resolution / Inch);
            float reswidth =    (float)(dotratio*width);
            float resheight=    (float)(dotratio*height);
            var font = new Font(fontname, fontsize);
            using (
                        var bitmap = new Bitmap(
                            Convert.ToInt32(reswidth),
                            Convert.ToInt32(resheight), PixelFormat.Format24bppRgb)
                        )
            {

                StringFormat strFormat = StringFormat.GenericTypographic;
                strFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
                bitmap.SetResolution(resolution, resolution);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    g.Clear(Color.White);
                    g.Flush();
                    
                    g.DrawString(text,
                        font, BlackSolidBrush, new RectangleF(0, 0, bitmap.Width, bitmap.Height), strFormat);
                    g.Flush();
                }
                var oups = TransformToFormat1bppIndexedBitMap(bitmap);
                using (var memorystream = new MemoryStream())
                {

                    System.Drawing.Imaging.ImageCodecInfo objImageCodecInfo = GetEncoderInfo("image/jpeg");
                    // Create an Encoder object based on the GUID for the Quality parameter category.
                    System.Drawing.Imaging.Encoder myQualityEncoder = System.Drawing.Imaging.Encoder.Quality;
                    System.Drawing.Imaging.Encoder myCompressionEncoder = System.Drawing.Imaging.Encoder.Compression;

                    // Create an EncoderParameters object.
                    // An EncoderParameters object has an array of EncoderParameter objects. In this case, there is only one
                    // EncoderParameter object in the array.
                    EncoderParameters myEncoderParameters = new EncoderParameters(2);

                    EncoderParameter myQualityEncoderParam = new EncoderParameter(myQualityEncoder, (long)100);
                    EncoderParameter myCompressionEncoderParam = new EncoderParameter(myCompressionEncoder, (long)EncoderValue.CompressionNone);

                    myEncoderParameters.Param[0] = myQualityEncoderParam;
                    myEncoderParameters.Param[1] = myCompressionEncoderParam;  


                    //bitmap.Save(memorystream, ImageFormat.Bmp);
                    //oups.Save(memorystream, ImageFormat.Jpeg,);
                    oups.Save(memorystream, objImageCodecInfo, myEncoderParameters);
                    result = memorystream.ToArray();
                }
            }
            
            return result;
        }

        public static Bitmap TransformToFormat1bppIndexedBitMap(Bitmap originalbitmap)
        {
            if (originalbitmap.PixelFormat != PixelFormat.Format24bppRgb) throw new Exception("Biptmap format not supported");
            
            var result = new Bitmap(originalbitmap.Width, originalbitmap.Height, PixelFormat.Format1bppIndexed);
            result.SetResolution(originalbitmap.HorizontalResolution, originalbitmap.VerticalResolution);
            //result.Palette.Entries[0] = Color.White;
            //result.Palette.Entries[1] = Color.Black;
            int widthdiv8 = result.Width / 8;

            byte[] srcline = new byte[3*originalbitmap.Width];
            byte[] destline = new byte[widthdiv8];

            Rectangle rect = new Rectangle(0, 0, originalbitmap.Width, originalbitmap.Height);

            System.Drawing.Imaging.BitmapData originalbmpData = originalbitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, originalbitmap.PixelFormat);
            try
            {
                System.Drawing.Imaging.BitmapData bmpData = result.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, result.PixelFormat);
                try
                {
                    IntPtr ptr;
                    IntPtr ptrdest;
                    for (int y = 0; y < originalbitmap.Height; y++)
                    {
                        
                        ptr = (IntPtr)(originalbmpData.Scan0.ToInt32() + y * originalbmpData.Stride);
                        ptrdest = (IntPtr)(bmpData.Scan0.ToInt32() + y * bmpData.Stride);
                        System.Runtime.InteropServices.Marshal.Copy(ptr, srcline, 0, srcline.Length);
                        
                        int orginalidx = 0;
                        int mask = 0x80;
                        int destidx = 0;
                        byte destvalue = 0;
                        byte value;
                        var bFlush =false;
                        while (orginalidx < srcline.Length)
                        {
                            value = (byte)(srcline[orginalidx] | srcline[orginalidx + 1] | srcline[orginalidx + 2]);
                            if (value != 0)
                            {
                                bFlush = true;
                                destvalue = (byte)(destvalue | mask);
                            }
                            
                            
                            mask = mask >> 1;
                            if (mask==0) {
                                bFlush = false;
                                destline[destidx] = destvalue;
                                destvalue = 0;
                                mask = 0x80;
                                destidx++;
                            }
                            orginalidx += 3;
                        }
                        if (bFlush) if (destidx<destline.Length) destline[destidx] = destvalue;
                        System.Runtime.InteropServices.Marshal.Copy(destline, 0, ptrdest, destline.Length);

                    }
                }
                finally
                {
                    result.UnlockBits(bmpData);
                }
            }
            finally
            {
                originalbitmap.UnlockBits(originalbmpData);
            
            }


            return result;
        }


        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }


    }
}
