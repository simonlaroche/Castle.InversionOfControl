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

namespace Castle.MicroKernel.Tests
{
	using Castle.MicroKernel.Registration;
	using Castle.MicroKernel.SubSystems.Naming;
	using Castle.MicroKernel.Tests.ClassComponents;

	using NUnit.Framework;

	[TestFixture]
	public class NamingPartsSubSystemTestCase
	{
		[SetUp]
		public void SetUpTests()
		{
			kernel = new DefaultKernel();
			kernel.AddSubSystem(SubSystemConstants.NamingKey, new NamingPartsSubSystem());
		}

		private IKernel kernel;

		[Test]
		public void ComponentGraph()
		{
			kernel.Register(Component.For(typeof(ICommon)).ImplementedBy(typeof(CommonImpl1)).Named("common:key1=true"));
			kernel.Register(Component.For(typeof(ICommon)).ImplementedBy(typeof(CommonImpl2)).Named("common:secure=true"));

			var nodes = kernel.GraphNodes;
			Assert.IsNotNull(nodes);
			Assert.AreEqual(2, nodes.Length);
		}

		[Test]
		public void ComponentQuery()
		{
			kernel.Register(Component.For(typeof(ICommon)).ImplementedBy(typeof(CommonImpl1)).Named("common:key1=true"));
			kernel.Register(Component.For(typeof(ICommon)).ImplementedBy(typeof(CommonImpl2)).Named("common:secure=true"));

			var common = kernel.Resolve<ICommon>("common");
			Assert.IsInstanceOf<CommonImpl1>(common);

			common = kernel.Resolve<ICommon>("common:key1=true");
			Assert.IsInstanceOf<CommonImpl1>(common);

			common = kernel.Resolve<ICommon>("common:secure=true");
			Assert.IsInstanceOf<CommonImpl2>(common);
		}

		[Test]
		public void GetAssignableHandlers()
		{
			kernel.Register(Component.For(typeof(ICommon)).ImplementedBy(typeof(CommonImpl1)).Named("common:key1=true"));
			kernel.Register(Component.For(typeof(ICommon)).ImplementedBy(typeof(CommonImpl2)).Named("common:secure=true"));

			var handlers = kernel.GetAssignableHandlers(typeof(ICommon));

			Assert.IsNotNull(handlers);
			Assert.AreEqual(2, handlers.Length);
		}

		[Test]
		public void ServiceLookup()
		{
			kernel.Register(Component.For(typeof(ICommon)).ImplementedBy(typeof(CommonImpl1)).Named("common:key1=true"));
			kernel.Register(Component.For(typeof(ICommon)).ImplementedBy(typeof(CommonImpl2)).Named("common:secure=true"));

			var common = kernel.Resolve<ICommon>();

			Assert.IsNotNull(common);
			Assert.AreEqual(typeof(CommonImpl1), common.GetType());
		}

		[Test]
		public void WorksWithHandlerForwarding()
		{
			kernel.Register(Component.For(typeof(ICommon)).ImplementedBy(typeof(CommonImpl1)).Named("common:key1=true"));
			((IKernelInternal)kernel).RegisterHandlerForwarding(typeof(CommonImpl2), "common:key1=true");
		}
	}
}