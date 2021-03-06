using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace LightBDD.XUnit2.Implementation.Customization
{
    internal class TestFrameworkExecutor : XunitTestFrameworkExecutor
    {
        public TestFrameworkExecutor(AssemblyName assemblyName, ISourceInformationProvider sourceInformationProvider, IMessageSink diagnosticMessageSink)
            : base(assemblyName, sourceInformationProvider, diagnosticMessageSink)
        {

        }

        protected override void RunTestCases(IEnumerable<IXunitTestCase> testCases, IMessageSink executionMessageSink, ITestFrameworkExecutionOptions executionOptions)
        {
            var bddScopeAttribute = GetLightBddScopeAttribute();
            bddScopeAttribute?.SetUp();
            try
            {
                using (var assemblyRunner = new XunitTestAssemblyRunner(TestAssembly, testCases, DiagnosticMessageSink, executionMessageSink, executionOptions))
                    assemblyRunner.RunAsync().Wait();
            }
            finally
            {
                bddScopeAttribute?.TearDown();
            }
        }

        private LightBddScopeAttribute GetLightBddScopeAttribute()
        {
            var asmName = TestAssembly.Assembly.Name;
#if NET45
            var attribs = Assembly.Load(asmName)
#else
            var attribs = Assembly.Load(new AssemblyName(asmName))
#endif
                .GetCustomAttributes(typeof(LightBddScopeAttribute))
                .Cast<LightBddScopeAttribute>().ToArray();
            if (attribs.Length > 1)
                throw new InvalidOperationException($"Only one attribute of {typeof(LightBddScopeAttribute)} type can be defined in assembly {asmName}");
            return attribs.FirstOrDefault();
        }
    }
}