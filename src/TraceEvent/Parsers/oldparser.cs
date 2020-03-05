using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;

namespace Microsoft.Diagnostics.Tracing.Parsers
{
    class LinuxKernelTraceEventParser : TraceEventParser
    {
        public static readonly string ProviderName = "Linux Kernel Provider";
        public static readonly Guid ProviderGuid = new Guid(unchecked((int)0x4da4eb17), unchecked((short)0xcd7d), unchecked((short)0x4b13), 0xb8, 0x07, 0x22, 0x90, 0x8d, 0x29, 0x96, 0xb5);

        public LinuxKernelTraceEventParser(TraceEventSource source) : base(source)
        {

        }
        protected override string GetProviderName()
        {
            return ProviderName;
        }

        protected internal override void EnumerateTemplates(Func<string, string, EventFilterResponse> eventsToObserve, Action<TraceEvent> callback)
        {
            var templates = new TraceEvent[1];
            templates[0] = new ProcessTraceData(null, 0xFFFF, 1, "Process", KernelTraceEventParser.ProcessTaskGuid, 1, "Start", ProviderGuid, ProviderName, null);
            foreach(var template in templates)
            {
                callback(template);
            }
        }

        public enum Keywords
        {
            Process = 0x00000001
        }

        public event Action<ProcessTraceData> ProcessStart
        {
            add
            {
                // action, eventid, taskid, taskName, taskGuid, opcode, opcodeName, providerGuid, providerName
                source.RegisterEventTemplate(new ProcessTraceData(value, 1, 1, "Process", KernelTraceEventParser.ProcessTaskGuid, 1, "Start", ProviderGuid, ProviderName, null));
            }
            remove
            {
                source.UnregisterEventTemplate(value, 1, KernelTraceEventParser.ProcessTaskGuid);
            }
        }

    }
}
