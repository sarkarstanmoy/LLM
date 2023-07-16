using Microsoft.SemanticKernel.SkillDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Newtonsoft.Json;
using System.Collections;
using System.Threading.Channels;
using UglyToad.PdfPig.Geometry;
using Docnet.Core.Models;

namespace FirstSK.Plugins
{
    public class PDFReaderPlugin
    {
        private const string fileName = "brian-markup.pdf";

        [SKFunction("Read pdf")]
        public void PDFReader()
        {
            //var docNet = Docnet.Core.DocLib.Instance;
            //var docReader = docNet.GetDocReader($"./{fileName}",);
            //Console.WriteLine(docReader.GetPageReader(0).GetText());

            var pdfDirectory = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            int count = 1;
            StringBuilder stringBuilder = new StringBuilder();
            using (PdfDocument document = PdfDocument.Open(pdfDirectory))
            {
                var page = document.GetPage(1);

                var content = page.GetMarkedContents();
                //foreach (Page page in document.GetPages())
                //{
                //    if (count > 2)
                //    {
                //        break;
                //    }
                //    var r = page.ExperimentalAccess.GetAnnotations();

                //    //var summerizeSkill = kernel.Skills.GetFunction("SummarizePlugin", "Summarize");
                //    //var sresult = await summerizeSkill.InvokeAsync(pageText);
                //    //Console.WriteLine();
                //    //await Console.Out.WriteLineAsync($"OpenAI Token used : {GetToken(page.GetWords().Count())}");
                //    //await Console.Out.WriteLineAsync(sresult.Result);
                //}
            }
        }

        private int GetToken(int numberOfWords)
        {
            return (numberOfWords * 4) / 3;
        }
    }
}