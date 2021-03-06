// Copyright 2004-2010 Castle Project - http://www.castleproject.org/
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

namespace Castle.MicroKernel.Tests.Registration
{
	using System.Linq;

	using Castle.MicroKernel.Registration;

	using NUnit.Framework;

	[TestFixture]
	public class ComponentRegistrationByNamespaceTestCase : RegistrationTestCaseBase
	{
		private int ComponentsCount()
		{
			return Components().Length;
		}

		private IHandler[] Components()
		{
			return Kernel.GetAssignableHandlers(typeof(object));
		}

		[Test]
		public void Registreting_by_namespace_no_subnamespaces()
		{
			Kernel.Register(AllTypes.FromThisAssembly().Where(Component.IsInNamespace("RootNamespace")));

			Assert.AreEqual(1, ComponentsCount());
		}

		[Test]
		public void Registreting_by_namespace_with_subnamespaces()
		{
			Kernel.Register(
				AllTypes.FromThisAssembly()
					.Where(Component.IsInNamespace("RootNamespace", includeSubnamespaces: true)));

			Assert.AreEqual(2, ComponentsCount());
		}

		[Test]
		public void Registreting_by_namespace_with_subnamespaces_properly_filters_out_namespaces_that_have_common_prefox()
		{
			Kernel.Register(
				AllTypes.FromThisAssembly()
					.Where(Component.IsInNamespace("RootNamespace", includeSubnamespaces: true)));

			Assert.IsFalse(Components().Any(c => c.Service.Namespace == "RootNamespaceEx"));
		}
	}
}

namespace RootNamespace
{
	public class RootComponent
	{
	}
}

namespace RootNamespaceEx
{
	public class RootComponentEx
	{
	}
}

namespace RootNamespace.Sub
{
	public class SubComponent
	{
	}
}