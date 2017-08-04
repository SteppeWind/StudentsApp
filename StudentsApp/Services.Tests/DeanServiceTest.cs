using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StudentsApp.BLL.Contracts;
using StudentsApp.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Tests
{
    [TestClass]
    public class DeanServiceTest
    {
        [TestMethod]
        [ExpectedException(typeof(PersonIsExistException))]
        public void Dean_CreateWithSameName()
        {
            
        }
    }
}