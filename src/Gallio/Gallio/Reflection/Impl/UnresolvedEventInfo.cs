// Copyright 2008 MbUnit Project - http://www.mbunit.com/
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
using System.Reflection;
using Gallio.Collections;

namespace Gallio.Reflection.Impl
{
    /// <summary>
    /// Represents a <see cref="EventInfo" /> whose native definition could not be resolved
    /// so we fall back on the <see cref="IEventInfo"/> wrapper.
    /// </summary>
    public partial class UnresolvedEventInfo : EventInfo
    {
        private readonly IEventInfo adapter;

        /// <summary>
        /// Creates a reflection object backed by the specified adapter.
        /// </summary>
        /// <param name="adapter">The adapter</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="adapter"/> is null</exception>
        public UnresolvedEventInfo(IEventInfo adapter)
        {
            if (adapter == null)
                throw new ArgumentNullException("adapter");

            this.adapter = adapter;
        }

        /// <inheritdoc />
        public override EventAttributes Attributes
        {
            get { return EventAttributes.None; }
        }

        /// <inheritdoc />
        public override MemberTypes MemberType
        {
            get { return MemberTypes.Event; }
        }

        /// <inheritdoc />
        public override MethodInfo GetAddMethod(bool nonPublic)
        {
            return UnresolvedMemberInfo.ResolveMethod(adapter.AddMethod, nonPublic);
        }

        /// <inheritdoc />
        public override MethodInfo[] GetOtherMethods(bool nonPublic)
        {
            return EmptyArray<MethodInfo>.Instance;
        }

        /// <inheritdoc />
        public override MethodInfo GetRaiseMethod(bool nonPublic)
        {
            return UnresolvedMemberInfo.ResolveMethod(adapter.RaiseMethod, nonPublic);
        }

        /// <inheritdoc />
        public override MethodInfo GetRemoveMethod(bool nonPublic)
        {
            return UnresolvedMemberInfo.ResolveMethod(adapter.RemoveMethod, nonPublic);
        }
    }
}