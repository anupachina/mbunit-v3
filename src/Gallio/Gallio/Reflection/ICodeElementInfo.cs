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
using System.Collections.Generic;

namespace Gallio.Reflection
{
    /// <summary>
    /// <para>
    /// Represents a structural element of some code base. 
    /// </para>
    /// <para>
    /// This interface is the base of a hierarchy of abstract reflection objects.
    /// Different implementations of these objects may be used to perform
    /// reflection over different sources.
    /// </para>
    /// </summary>
    /// <seealso cref="Reflector"/>
    public interface ICodeElementInfo : IEquatable<ICodeElementInfo>
    {
        /// <summary>
        /// Gets the name of the code element.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the kind of code element represented by this instance.
        /// </summary>
        CodeElementKind Kind { get; }

        /// <summary>
        /// Gets a <see cref="CodeReference" /> for this code element.
        /// This value is never null.
        /// </summary>
        /// <returns>The code reference</returns>
        CodeReference CodeReference { get; }

        /// <summary>
        /// Gets information about all the code element's attributes.
        /// </summary>
        /// <param name="inherit">If true, includes inherited attributes</param>
        /// <returns>The code element's attributes</returns>
        IEnumerable<IAttributeInfo> GetAttributeInfos(bool inherit);

        /// <summary>
        /// Gets information about the code element's attributes of the specified type.
        /// </summary>
        /// <param name="attributeType">The attribute type</param>
        /// <param name="inherit">If true, includes inherited attributes</param>
        /// <returns>The code element's attributes</returns>
        IEnumerable<IAttributeInfo> GetAttributeInfos(Type attributeType, bool inherit);

        /// <summary>
        /// Returns true if the code element has an attribute of the specified type.
        /// </summary>
        /// <param name="attributeType">The attribute type</param>
        /// <param name="inherit">If true, includes inherited attributes</param>
        /// <returns>True if the code element has at least one attribute of the specified type</returns>
        bool HasAttribute(Type attributeType, bool inherit);

        /// <summary>
        /// Gets all of the code element's attributes.
        /// </summary>
        /// <param name="inherit">If true, includes inherited attributes</param>
        /// <returns>The attributes</returns>
        IEnumerable<object> GetAttributes(bool inherit);

        /// <summary>
        /// Gets the code element's attributes of the specified type.
        /// </summary>
        /// <param name="attributeType">The attribute type</param>
        /// <param name="inherit">If true, includes inherited attributes</param>
        /// <returns>The attributes</returns>
        IEnumerable<object> GetAttributes(Type attributeType, bool inherit);

        /// <summary>
        /// Gets the XML documentation associated with the code element.
        /// </summary>
        /// <returns>The XML documentation or null if none available</returns>
        string GetXmlDocumentation();

        /// <summary>
        /// Gets the location of a resource that contains the declaration of this code element, or
        /// null if not available.  The location may refer to the code element's source code
        /// or to the location of its compiled assembly.
        /// </summary>
        /// <returns>The code location</returns>
        CodeLocation GetCodeLocation();
    }
}
