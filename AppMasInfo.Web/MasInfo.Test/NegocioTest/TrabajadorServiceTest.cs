using System;
using AppMasInfo.Negocio.DAL.Entities;
using AppMasInfo.Negocio.DAL.Services;
using AppMasInfo.Utils.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MasInfo.Test.NegocioTest
{
    [TestClass]
    public class TrabajadorServiceTest
    {
        #region propieades privadas
        Mock<ITrabajadorService> mockTrabajadorService;
        #endregion

        #region Setup
        [TestInitialize]
        public void Setup()
        {
            // Mock Init
            mockTrabajadorService = new Mock<ITrabajadorService>(MockBehavior.Default);
            // Metodo Eliminar Paciente  
            DeleteTrabajadorSetup();
            // Metodo para Insertar un nuevo Paciente
            InsertarTrabajadorSetup();
            // metodo para Actualizar un registro
            UpdateTrabajadorSetup();
            //Metodo para obteber un paciente por Id
            GetTrabajadorByIdSetup();
        }

        private void DeleteTrabajadorSetup()
        {
            // Correcta Ejecucion
            mockTrabajadorService.Setup(m => m.Delete(It.Is<TrabajadorDto>(t => t.Id > 0 &&
                                                                                 t.IdEstado > 0 &&
                                                                                 string.IsNullOrEmpty(t.UsrUpdate) == false &&
                                                                                 t.FchUpdate != null)))
                                                                                .Returns(new BaseDto<bool>(true));
            // Incorrecta ejecución
            mockTrabajadorService.Setup(m => m.Delete(It.Is<TrabajadorDto>(t => t.Id == 0 ||
                                                                                 t.IdEstado == 0 ||
                                                                                 string.IsNullOrEmpty(t.UsrUpdate) ||
                                                                                 t.FchUpdate == null)))
                .Returns(new BaseDto<bool>(true, new Exception("Algunos campos requeridos estan vacios o null")));

            // Null update data
            mockTrabajadorService.Setup(m => m.Delete(It.Is<TrabajadorDto>(t => t == null)))
                .Returns(new BaseDto<bool>(true, new Exception("El objeto debe ser diferente a nulo")));
        }

        private void UpdateTrabajadorSetup()
        {
            // Correcta ejecucuion
            mockTrabajadorService.Setup(m => m.UpdateTrabajador(It.Is<TrabajadorDto>(t => t.Id > 0 && t.IdEstado > 0 &&
                                                                                 string.IsNullOrEmpty(t.Nombre) == false &&
                                                                                 string.IsNullOrEmpty(t.UsrUpdate) == false &&
                                                                                 t.FchUpdate != null)))
                .Returns(new BaseDto<bool>(true));
            // Incorrect update data
            mockTrabajadorService.Setup(m => m.UpdateTrabajador(It.Is<TrabajadorDto>(t => t.Id == 0 && string.IsNullOrEmpty(t.Nombre) &&
                                                                                         t.IdEstado > 0 &&
                                                                                         string.IsNullOrEmpty(t.UsrUpdate) ||
                                                                                         t.FchUpdate == null)))
                .Returns(new BaseDto<bool>(true, new Exception("Algunos campos requeridos estan vacios o null")));
            // Null update data
            mockTrabajadorService.Setup(m => m.UpdateTrabajador(It.Is<TrabajadorDto>(t => t == null)))
                .Returns(new BaseDto<bool>(true, new Exception("El objeto debe ser diferente a nulo")));
        }

        private void InsertarTrabajadorSetup()
        {
            // Correct create data
            mockTrabajadorService.Setup(m => m.InsertarTrabajador(It.Is<TrabajadorDto>(p => string.IsNullOrEmpty(p.Nombre) == false &&
                                                                                 p.IdEstado > 0 &&
                                                                                 string.IsNullOrEmpty(p.UsrCreate) == false &&
                                                                                 p.FchCreate != null)))
                .Returns(new BaseDto<bool>(true));
            // Incorrect create data
            mockTrabajadorService.Setup(m => m.InsertarTrabajador(It.Is<TrabajadorDto>(p => string.IsNullOrEmpty(p.Nombre) ||
                                                                                 p.IdEstado == 0 ||
                                                                                 string.IsNullOrEmpty(p.UsrCreate) ||
                                                                                 p.FchCreate == null)))
                .Returns(new BaseDto<bool>(true, new Exception("Algunos campos requeridos estan vacios o null")));
            // Null create data
            mockTrabajadorService.Setup(m => m.InsertarTrabajador(It.Is<TrabajadorDto>(p => p == null)))
                .Returns(new BaseDto<bool>(true, new Exception("El objeto debe ser diferente a nulo")));
        }

        private void GetTrabajadorByIdSetup()
        {
            // Correct filter
            mockTrabajadorService.Setup(m => m.GetTrabajadorById(It.Is<TrabajadorDto>(p => p.FiltroId == 1)))
                .Returns(new BaseDto<TrabajadorDto>(new TrabajadorDto()));
            // Incorrect filter
            mockTrabajadorService.Setup(m => m.GetTrabajadorById(It.Is<TrabajadorDto>(p => p.FiltroId == 2)))
                .Returns(new BaseDto<TrabajadorDto>(null));
            // Empty filter
            mockTrabajadorService.Setup(m => m.GetTrabajadorById(It.Is<TrabajadorDto>(p => p.FiltroId == null)))
                .Returns(new BaseDto<TrabajadorDto>(null));
            //// Null filter
            mockTrabajadorService.Setup(m => m.GetTrabajadorById(It.Is<TrabajadorDto>(p => p == null)))
                .Returns(new BaseDto<TrabajadorDto>(true, new Exception("Error: Filter object must be different than null")));
        }
        #endregion

        #region InsertarTrabajador Tests
        [TestMethod]
        public void InsertarTrabajador_CorrectData_Test()
        {
            var filtroObj = new TrabajadorDto
            {
                Nombre = "john",
                FchCreate = DateTime.Now,
                IdEstado = (int)EnumUtils.EstadoEnum.Trabajador_Habilitado,
                UsrCreate = "john1204",
            };
            ITrabajadorService service = mockTrabajadorService.Object;
            var serviceResponse = service.InsertarTrabajador(filtroObj);

            // Tiene Datos
            Assert.IsTrue(serviceResponse.HasValue);
            // El valor debe diferente a nulo
            Assert.IsNotNull(serviceResponse.Value);
            // No tiene error
            Assert.IsFalse(serviceResponse.HasError);
            // la excepción debe ser nula
            Assert.IsNull(serviceResponse.Error);
        }

        [TestMethod]
        public void InsertarTrabajador_IncorrectData_Test()
        {
            var filtroObj = new TrabajadorDto
            {
                Nombre = null,
                FchCreate = DateTime.Now,
                IdEstado = 0,
                UsrCreate = null,
            };
            ITrabajadorService service = mockTrabajadorService.Object;
            var serviceResponse = service.InsertarTrabajador(filtroObj);

            // No Tiene Datos
            Assert.IsFalse(serviceResponse.HasValue);
            // El valor debe ser nulo
            Assert.IsFalse(serviceResponse.Value);
            // Tiene error
            Assert.IsTrue(serviceResponse.HasError);
            // la excepción debe ser nula
            Assert.IsNotNull(serviceResponse.Error);
        }

        [TestMethod]
        public void InsertarTrabajador_NullData_Test()
        {
            ITrabajadorService service = mockTrabajadorService.Object;
            var serviceResponse = service.InsertarTrabajador(null);

            // No trae datos
            Assert.IsFalse(serviceResponse.HasValue);
            // El valor debe ser nulo
            Assert.IsFalse(serviceResponse.Value);
            // Tiene error
            Assert.IsTrue(serviceResponse.HasError);
            // la excepción debe ser nula
            Assert.IsNotNull(serviceResponse.Error);
        }
        #endregion

        #region  UpdateTrabajador Tests
        [TestMethod]
        public void UpdateTrabajador_CorrectData_Test()
        {
            var filtroObj = new TrabajadorDto
            {
                Nombre = "john",
                IdEstado = (int)EnumUtils.EstadoEnum.Trabajador_Habilitado,
                FchUpdate = DateTime.Now,
                UsrUpdate = "john1204",
                Id = 1
            };
            ITrabajadorService service = mockTrabajadorService.Object;
            var serviceResponse = service.UpdateTrabajador(filtroObj);

            // Does has data
            Assert.IsTrue(serviceResponse.HasValue);
            // Value is different than null
            Assert.IsNotNull(serviceResponse.Value);
            // Doesn't has error
            Assert.IsFalse(serviceResponse.HasError);
            // ...and exception must be null
            Assert.IsNull(serviceResponse.Error);
        }

        [TestMethod]
        public void UpdateTrabajador_IncorrectData_Test()
        {
            var filtroObj = new TrabajadorDto
            {
                FchCreate = DateTime.Now,
                IdEstado = 0,
                Id = 0,
                UsrUpdate = null,
                Nombre = null
            };
            ITrabajadorService service = mockTrabajadorService.Object;
            var serviceResponse = service.UpdateTrabajador(filtroObj);

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
        public void UpdateTrabajador_NullData_Test()
        {
            ITrabajadorService service = mockTrabajadorService.Object;
            var serviceResponse = service.UpdateTrabajador(null);

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

        #region DeleteTrabajador Tests
        [TestMethod]
        public void DeleteTrabajador_CorrectData_Test()
        {
            var filtroObj = new TrabajadorDto
            {
                IdEstado = (int)EnumUtils.EstadoEnum.Trabajador_Habilitado,
                FchUpdate = DateTime.Now,
                UsrUpdate = "john1204",
                Id = 1
            };
            ITrabajadorService service = mockTrabajadorService.Object;
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
        public void DeleteTrabajador_IncorrectData_Test()
        {
            var filtroObj = new TrabajadorDto
            {
                FchCreate = DateTime.Now,
                IdEstado = 0,
                Id = 0,
                UsrUpdate = null
            };
            ITrabajadorService service = mockTrabajadorService.Object;
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
        public void DeleteTrabajador_NullData_Test()
        {
            ITrabajadorService service = mockTrabajadorService.Object;
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

        #region GetTrabajadorById Tests
        [TestMethod]
        public void GetTrabajadorById_NoData_IncorrectFilter_Test()
        {
            var filtroObj = new TrabajadorDto { FiltroId = 2 };
            ITrabajadorService service = mockTrabajadorService.Object;
            var serviceResponse = service.GetTrabajadorById(filtroObj);

            // Doesn't has data
            Assert.IsFalse(serviceResponse.HasValue);
            // Value is null
            Assert.IsNull(serviceResponse.Value);
            // Doesn't has error
            Assert.IsFalse(serviceResponse.HasError);
            // ...and exception must be null
            Assert.IsNull(serviceResponse.Error);
        }

        [TestMethod]
        public void GetTrabajadorById_NoData_EmptyFilter_Test()
        {
            var filtroObj = new TrabajadorDto();
            ITrabajadorService service = mockTrabajadorService.Object;
            var serviceResponse = service.GetTrabajadorById(filtroObj);

            // Doesn't has data
            Assert.IsFalse(serviceResponse.HasValue);
            // Value is null
            Assert.IsNull(serviceResponse.Value);
            // Doesn't has error
            Assert.IsFalse(serviceResponse.HasError);
            // ...and exception must be null
            Assert.IsNull(serviceResponse.Error);
        }

        [TestMethod]
        public void GetTrabajadorById_WithData_CorrectFilter_Test()
        {
            var filtroObj = new TrabajadorDto { FiltroId = 1 };
            ITrabajadorService service = mockTrabajadorService.Object;
            var serviceResponse = service.GetTrabajadorById(filtroObj);

            // Does has data
            Assert.IsTrue(serviceResponse.HasValue);
            // Value is different than null
            Assert.IsNotNull(serviceResponse.Value);
            // Doesn't has error
            Assert.IsFalse(serviceResponse.HasError);
            // ... and exception must be null
            Assert.IsNull(serviceResponse.Error);
        }

        [TestMethod]
        public void GetTrabajadorById_Error_NullFilter_Test()
        {
            ITrabajadorService service = mockTrabajadorService.Object;
            var serviceResponse = service.GetTrabajadorById(null);

            // Doesn't has data
            Assert.IsFalse(serviceResponse.HasValue);
            // Value must be null
            Assert.IsNull(serviceResponse.Value);
            // Does has error
            Assert.IsTrue(serviceResponse.HasError);
            // ... and exception must be different than null
            Assert.IsNotNull(serviceResponse.Error);
        }
        #endregion        
    }
}
