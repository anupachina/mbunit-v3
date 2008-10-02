// Copyright 2005-2008 Gallio Project - http://www.gallio.org/
// Portions Copyright 2000-2004 Jonathan de Halleux
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Text;
using Gallio.Model.Logging;
using Gallio.Model.Serialization;
using Gallio.Reflection;
using Gallio.Runner.Events;
using Gallio.Runner.Extensions;
using Gallio.TDNetRunner.Properties;
using Gallio.Model;

namespace Gallio.TDNetRunner.Core
{
    /// <summary>
    /// A test runner extension that informs TD.NET (and therefore
    /// the user) in real-time what's going with the tests.
    /// </summary>
    internal class ProxyTestListenerExtension : LogExtension
    {
        private readonly IProxyTestListener testListener;

        public ProxyTestListenerExtension(IProxyTestListener testListener)
        {
            if (testListener == null)
                throw new ArgumentNullException(@"testListener");

            this.testListener = testListener;
        }

        /// <inheritdoc />
        protected override void LogAnnotation(AnnotationData annotation)
        {
            StringBuilder message = new StringBuilder();
            message.AppendFormat("[{0}] ", annotation.Type.ToString().ToLower());
            message.Append(annotation.Message);

            if (annotation.CodeLocation != CodeLocation.Unknown)
            {
                message.Append("\n\tLocation: ");
                message.Append(annotation.CodeLocation);
            }

            if (annotation.CodeLocation.Line == 0 && annotation.CodeReference != CodeReference.Unknown)
            {
                message.Append("\n\tReference: ");
                message.Append(annotation.CodeReference);
            }

            if (!string.IsNullOrEmpty(annotation.Details))
            {
                message.Append("\n\tDetails: ");
                message.Append(annotation.Details);
            }

            string category = GetCategoryForAnnotation(annotation.Type);
            testListener.WriteLine(message.ToString(), category);
        }

        /// <inheritdoc />
        protected override void LogTestStepStarted(TestStepStartedEventArgs e)
        {
        }

        /// <inheritdoc />
        protected override void LogTestStepFinished(TestStepFinishedEventArgs e)
        {
            // A TestResult with State == TestState.Passed won't be displayed in the
            // output window (TD.NET just diplays "[TestName] passed" in the status bar.
            // Since that can be harder to notice, and also is lost when the following
            // test finishes, we print a message in the output window so the user can 
            // progressively see if the tests are passing or failing.
            if (e.TestStepRun.Result.Outcome.Status == TestStatus.Passed)
            {
                if (!e.TestStepRun.Step.IsTestCase)
                    return; // nothing interesting to report

                testListener.WriteLine(String.Format(Resources.TDNetLogMonitor_TestCasePassed, 
                    e.TestStepRun.Step.FullName), MessageCategory.Info);
            }

            // Inform TD.NET what happened 
            ProxyTestResult result = new ProxyTestResult();
            result.Name = e.TestStepRun.Step.FullName;
            result.TimeSpan = TimeSpan.FromSeconds(e.TestStepRun.Result.Duration);
            // result.TestRunner = "Gallio"; // note: can crash in older versions of TD.Net with MissingFieldException

            // It's important to set the stack trace here so the user can double-click in the
            // output window to go the faulting line
            StructuredTestLogStream failureStream = e.TestStepRun.TestLog.GetStream(TestLogStreamNames.Failures);
            if (failureStream != null)
                result.StackTrace = failureStream.ToString();

            StructuredTestLogStream warningStream = e.TestStepRun.TestLog.GetStream(TestLogStreamNames.Warnings);
            if (warningStream != null)
                result.Message = warningStream.ToString();

            if (!e.TestStepRun.Step.IsTestCase && failureStream == null && warningStream == null)
                return; // nothing interesting to report

            // TD.NET will automatically count the number of passed, ignored and failed tests
            // provided we call the TestFinished method with the right State
            switch (e.TestStepRun.Result.Outcome.Status)
            {
                case TestStatus.Passed:
                    result.IsSuccess = true;
                    result.IsFailure = false;
                    result.IsExecuted = true;
                    break;

                case TestStatus.Inconclusive:
                    result.IsSuccess = false;
                    result.IsFailure = false;
                    result.IsExecuted = true;
                    break;

                case TestStatus.Failed:
                    result.IsSuccess = false;
                    result.IsFailure = true;
                    result.IsExecuted = true;
                    break;

                case TestStatus.Skipped:
                    result.IsSuccess = false;
                    result.IsFailure = false;
                    result.IsExecuted = false;
                    break;
            }

            testListener.TestFinished(result);
        }

        private static string GetCategoryForAnnotation(AnnotationType type)
        {
            switch (type)
            {
                case AnnotationType.Error:
                    return MessageCategory.Error;

                case AnnotationType.Warning:
                    return MessageCategory.Warning;

                case AnnotationType.Info:
                    return MessageCategory.Info;

                default:
                    throw new ArgumentException("type");
            }
        }
    }
}