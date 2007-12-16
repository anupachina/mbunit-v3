// Copyright 2007 MbUnit Project - http://www.mbunit.com/
// Portions Copyright 2000-2004 Jonathan De Halleux, Jamie Cansdale
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
using Gallio.Model;
using Gallio.Model.Reflection;

namespace Gallio.Model
{
    /// <summary>
    /// <para>
    /// A test explorer scans a volume of code using reflection to build a
    /// partial test tree.  The tests constructed in this manner may not be
    /// complete or executable but they provide useful insight into the
    /// layout of the test suite that can subsequently be used to drive the
    /// test runner.
    /// </para>
    /// <para>
    /// As a test explorer explores test assemblies and types, it incrementally
    /// populates a <see cref="TestModel" /> with its discoveries.  The <see cref="TestModel" />
    /// is guaranteed to contain all of the tests explicitly explored, but it may
    /// also contain other tests that were discovered by the explorer along the
    /// way.  For example, a valid implementation of <see cref="ITestExplorer" />
    /// could implement <see cref="ExploreType" /> by exploring the entire assembly
    /// as performed by <see cref="ExploreAssembly" />.
    /// </para>
    /// </summary>
    public interface ITestExplorer
    {
        /// <summary>
        /// Gets the test model that is incrementally populated by the test explorer as
        /// it explores tests.
        /// </summary>
        TestModel TestModel { get; }

        /// <summary>
        /// Returns true if the code element represents a test.
        /// </summary>
        /// <param name="element">The element</param>
        /// <returns>True if the element represents a test</returns>
        bool IsTest(ICodeElementInfo element);

        /// <summary>
        /// <para>
        /// Explores the tests defined by an assembly and links them into
        /// the <see cref="TestModel" />.
        /// </para>
        /// </summary>
        /// <param name="assembly">The assembly</param>
        /// <param name="consumer">An action to perform on each assembly-level test
        /// explored, or null if no action is required</param>
        void ExploreAssembly(IAssemblyInfo assembly, Action<ITest> consumer);

        /// <summary>
        /// <para>
        /// Explores the tests defined by a type and links them into the
        /// <see cref="TestModel" />.
        /// </para>
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="consumer">An action to perform on each type-level test
        /// explored, or null if no action is required</param>
        void ExploreType(ITypeInfo type, Action<ITest> consumer);
    }
}
