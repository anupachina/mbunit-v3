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
using Gallio.Runtime.Logging;
using Gallio.Utilities;

namespace Gallio.TDNetRunner.Core
{
    /// <summary>
    /// An <see cref="ILogger" /> implementation that writes messages to a
    /// <see cref="IProxyTestListener" /> object.
    /// </summary>
    internal class ProxyTestListenerLogger : BaseLogger
    {
        private readonly IProxyTestListener testListener;

        /// <summary>
        /// Initializes a new instance of the TDNetLogger class.
        /// </summary>
        /// <param name="testListener">An ITestListener object where the
        /// messages will be written to.</param>
        public ProxyTestListenerLogger(IProxyTestListener testListener)
        {
            this.testListener = testListener;
        }

        /// <inheritdoc />
        protected override void LogImpl(LogSeverity severity, string message, Exception exception)
        {
            switch (severity)
            {
                case LogSeverity.Error:
                    testListener.WriteLine(message, MessageCategory.Error);
                    break;

                case LogSeverity.Warning:
                    testListener.WriteLine(message, MessageCategory.Warning);
                    break;

                case LogSeverity.Important:
                case LogSeverity.Info:
                    testListener.WriteLine(message, MessageCategory.Info);
                    break;

                case LogSeverity.Debug:
                    testListener.WriteLine(message, MessageCategory.Debug);
                    break;
            }

            if (exception != null)
                testListener.WriteLine(ExceptionUtils.SafeToString(exception), MessageCategory.Error);
        }
    }
}