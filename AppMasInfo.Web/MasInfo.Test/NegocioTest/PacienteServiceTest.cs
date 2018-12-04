using System;
using AppMasInfo.Negocio.DAL.Entities;
using AppMasInfo.Negocio.DAL.Services;
using AppMasInfo.Utils.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MasInfo.Test.NegocioTest
{
    [TestClass]
    public class PacienteServiceTest
    {
        #region propieades privadas
        Mock<IPacienteService> mockPacienteService;
        #endregion

        #region Setup
        [TestInitialize]
        public void Setup()
        {
            // Mock Init
            mockPacienteService = new Mock<IPacienteService>(MockBehavior.Default);
            // Metodo Eliminar Paciente  
            DeletePacienteSetup();

        }

        private void DeletePacienteSetup()
        {
            // Correcta Ejecucion
            mockPacienteService.Setup(m => m.Delete(It.Is<PacienteDto>(p => p.Id > 0 &&
                                                                                 p.IdEstado > 0 &&
                                                                                 string.IsNullOrEmpty(p.UsrUpdate) == false &&
                                                                                 p.FchUpdate != null)))
                                                                                .Returns(new BaseDto<bool>(true));
            // Incorrecta ejecución
            mockPacienteService.Setup(m => m.Delete(It.Is<PacienteDto>(p => p.Id == 0 ||
                                                                                 p.IdEstado == 0 ||
                                                                                 string.IsNullOrEmpty(p.UsrUpdate) ||
                                                                                 p.FchUpdate == null)))
                .Returns(new BaseDto<bool>(true, new Exception("Algunos campos requeridos estan vacios o null")));

            // Null update data
            mockPacienteService.Setup(m => m.Delete(It.Is<PacienteDto>(p => p == null)))
                .Returns(new BaseDto<bool>(true, new Exception("El objeto debe ser diferente a nulo")));
        }
        #endregion

        #region DeletePaciente Tests
        [TestMethod]
        public void DeletePaciente_CorrectData_Test()
        {
            var filtroObj = new PacienteDto
            {
                IdEstado = (int)EnumUtils.EstadoEnum.Paciente_Habilitado,
                FchUpdate = DateTime.Now,
                UsrUpdate = "john1204",
                Id = 1
            };
            IPacienteService service = mockPacienteService.Object;
            var serviceResponse = service.Delete(filtroObj);

            // Tiene Datos
            Assert.IsTrue(serviceResponse.HasValue);
            // El valor es diferente a nulo
            Assert.IsNotNull(serviceResponse.Value);
            // No tiene error
            Assert.IsFalse(serviceResponse.HasError);
            // y la excepción debe ser nula
            Assert.IsNull(serviceResponse.Error);
        }

        [TestMethod]
        public void DeletePaciente_IncorrectData_Test()
        {
            var filtroObj = new PacienteDto
            {
                FchCreate = DateTime.Now,
                IdEstado = 0,
                Id = 0,
                UsrUpdate = null
            };
            IPacienteService service = mockPacienteService.Object;
            var serviceResponse = service.Delete(filtroObj);

            // Doesn't has data
            Assert.IsFalse(serviceResponse.HasValue);
            // Value must be false
            Assert.IsFalse(serviceResponse.Value);
            // Does has error
            Assert.IsTrue(serviceResponse.HasError);
            // ...and exception must be different than null
            Assert.IsNotNull(serviceResponse.Error);
        }

        [TestMethod]
        public void DeletePaciente_NullData_Test()
        {
            IPacienteService service = mockPacienteService.Object;
            var serviceResponse = service.Delete(null);

            // Doesn't has data
            Assert.IsFalse(serviceResponse.HasValue);
            // Value must be null
            Assert.IsFalse(serviceResponse.Value);
            // Does has error
            Assert.IsTrue(serviceResponse.HasError);
            // ...and exception must be different than null
            Assert.IsNotNull(serviceResponse.Error);
        }
        #endregion
    }
}
