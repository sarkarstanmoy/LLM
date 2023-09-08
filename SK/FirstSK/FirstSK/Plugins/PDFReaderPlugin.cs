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
using System.ComponentModel;

namespace FirstSK.Plugins
{
    public class PDFReaderPlugin
    {
        private const string fileName = "resume.pdf";

        [SKFunction, Description("Read pdf")]
        public string PDFReader()
        {
            //var docNet = Docnet.Core.DocLib.Instance;
            //var docReader = docNet.GetDocReader($"./{fileName}",);
            //Console.WriteLine(docReader.GetPageReader(0).GetText());

            var pdfDirectory = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            StringBuilder stringBuilder = new StringBuilder();
            using (PdfDocument document = PdfDocument.Open(pdfDirectory))
            {
                foreach (Page page in document.GetPages())
                {
                    stringBuilder.Append(page.Text);
                    //var summerizeSkill = kernel.Skills.GetFunction("SummarizePlugin", "Summarize");
                    //var sresult = await summerizeSkill.InvokeAsync(pageText);
                    //Console.WriteLine();
                    //await Console.Out.WriteLineAsync($"OpenAI Token used : {GetToken(page.GetWords().Count())}");
                    //await Console.Out.WriteLineAsync(sresult.Result);
                }
            }
            Console.WriteLine(stringBuilder.ToString());
            return stringBuilder.ToString();
        }

        private int GetToken(int numberOfWords)
        {
            return (numberOfWords * 4) / 3;
        }
    }
}