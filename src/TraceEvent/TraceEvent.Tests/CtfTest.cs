using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.LinuxKernel;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.Diagnostics.Tracing.Parsers.Clr;

namespace TraceEventTests
{
    public class CtfTest
    {
        readonly string filename = "C:\\Users\\bache\\Downloads\\mmm.trace.zip";
        readonly string clrFilename = @"E:\perfview-linux-tracing\src\TraceEvent\Ctf\CtfTracing.Tests\inputs\auto-20160204-132425.trace.zip";
        [Fact]
        public void TestCtfSource()
        {
            //string filename = "C:\\Users\\bache\\Downloads\\mmm.trace.zip";
            // var source = new CtfTraceEventSource(filename);
        }

        [Fact]
        public void TestNewParserProcess()
        {
            var startEvents = new List<ProcessStartTraceData>();
            var stopEvents = new List<ProcessStopTraceData>();
            using(CtfTraceEventSource ctfSource = new CtfTraceEventSource(filename))
            {
                var linuxKernelParser = new LinuxKernelEventParser(ctfSource);
                linuxKernelParser.ProcessStart += delegate (ProcessStartTraceData d)
                {
                    startEvents.Add((ProcessStartTraceData)d.Clone());
                };

                linuxKernelParser.ProcessStop += delegate (ProcessStopTraceData d)
                {
                    stopEvents.Add((ProcessStopTraceData)d.Clone());
                };

                ctfSource.Process();
            }
        }

        [Fact]
        public void TestOldParserProcess()
        {
            string processName = "";
            using (CtfTraceEventSource ctfSource = new CtfTraceEventSource(filename))
            {
                var linuxKernelParser = new LinuxKernelTraceEventParser(ctfSource);
                linuxKernelParser.ProcessStart += delegate (ProcessTraceData d)
                {
                    processName = "Process Start " + d.ProcessName;
                };

/*                linuxKernelParser.ProcessStop += delegate (ProcessStopTraceData d)
                {
                    processName = "Process Stop " + d.ProcessName;
                };
*/
                ctfSource.Process();
                System.Diagnostics.Debug.WriteLine("HACK: " + processName);

            }
        }

        [Fact]
        public void TestClrParserProcess()
        {
            using (CtfTraceEventSource ctfSource = new CtfTraceEventSource(clrFilename))
            {
                ctfSource.Clr.GCCreateSegment += delegate (GCCreateSegmentTraceData d)
                {
                       
                };
                ctfSource.Process();
            }
        }
    }
}
