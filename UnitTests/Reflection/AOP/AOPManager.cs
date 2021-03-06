﻿/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

using System;
using System.Collections.Generic;
using System.IO;
using Utilities.Reflection.AOP.Interfaces;
using Xunit;

namespace UnitTests.Reflection.AOP
{
    public class AOPManager:IDisposable
    {
        protected Utilities.Reflection.AOP.AOPManager Manager { get; set; }

        public AOPManager()
        {
            Manager = new Utilities.Reflection.AOP.AOPManager(AssemblyDirectory: @".\TestingDLL\", RegenerateAssembly: true);
            new DirectoryInfo(@".\TestingDLL").Create();
        }

        [Fact]
        public void Create()
        {
            Assert.NotNull(Manager);
        }

        [Fact]
        public void AddAspect()
        {
            Assert.DoesNotThrow(() => Manager.AddAspect(new TestAspect()));
        }

        [Fact]
        public void Save()
        {
            Assert.DoesNotThrow(() => Manager.AddAspect(new TestAspect()));
            Assert.DoesNotThrow(() => Manager.Setup(typeof(TestClass)));
            Assert.DoesNotThrow(() => Manager.Save());
            Assert.True(new FileInfo(@".\TestingDLL\Aspects.dll").Exists);
        }

        [Fact]
        public void Setup()
        {
            Assert.DoesNotThrow(() => Manager.AddAspect(new TestAspect()));
            Assert.DoesNotThrow(() => Manager.Setup(typeof(TestClass)));
        }

        [Fact]
        public void CreateTest()
        {
            Assert.DoesNotThrow(() => Manager.AddAspect(new TestAspect()));
            TestClass TestObject = Manager.Create<TestClass>();
            Assert.NotNull(TestObject);
            Assert.Equal("A", TestObject.TestMethod());
        }

        public void Dispose()
        {
            new DirectoryInfo(@".\TestingDLL").Delete(true);
            Utilities.Reflection.AOP.AOPManager.Destroy();
        }
    }

    public class TestClass
    {
        public virtual string TestMethod()
        {
            return "B";
        }
    }

    public class TestAspect : IAspect
    {
        public void SetupStartMethod(Utilities.Reflection.Emit.Interfaces.IMethodBuilder Method, Type BaseType)
        {
            
        }

        public void SetupEndMethod(Utilities.Reflection.Emit.Interfaces.IMethodBuilder Method, Type BaseType, Utilities.Reflection.Emit.BaseClasses.VariableBase ReturnValue)
        {
            
        }

        public void SetupExceptionMethod(Utilities.Reflection.Emit.Interfaces.IMethodBuilder Method, Type BaseType)
        {
            
        }

        public void Setup(object Object)
        {
            ((IEvents)Object).Aspectus_Starting += new EventHandler<Utilities.Reflection.AOP.EventArgs.Starting>(Starting);
        }

        public void SetupInterfaces(Utilities.Reflection.Emit.TypeBuilder TypeBuilder)
        {
            
        }

        public ICollection<Type> InterfacesUsing
        {
            get
            {
                return null;
            }
            set
            {
                
            }
        }

        public void Starting(object Object, Utilities.Reflection.AOP.EventArgs.Starting EventArgs)
        {
            EventArgs.ReturnValue = "A";
        }
    }
}
