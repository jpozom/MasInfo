using System;
using System.Collections.Generic;
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
            // Metodo para obtener una lista completa de pacientes
            GetListaPacienteAllSetup();
            // Metodo para Insertar un nuevo Paciente
            CreatePacienteSetup();
            // metodo para Actualizar un registro
            UpdatePacienteSetup();
            //Metodo para obteber un paciente por Id
            GetPacienteByIdSetup();
            //Metodo para obtener un paciente por filtro estado
            GetListaPacienteByEstadoSetup();
        }
      
        #region DeletePacienteSetup
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

        #region UpdatePacienteSetup
        private void UpdatePacienteSetup()
        {
            // Correcta ejecucuion
            mockPacienteService.Setup(m => m.UpdatePaciente(It.Is<PacienteDto>(p => p.Id > 0 && p.IdEstado > 0 &&
                                                                                 string.IsNullOrEmpty(p.Nombre) == false &&                                                                                
                                                                                 string.IsNullOrEmpty(p.UsrUpdate) == false &&
                                                                                 p.FchUpdate != null)))
                .Returns(new BaseDto<bool>(true));
            // Incorrect update data
            mockPacienteService.Setup(m => m.UpdatePaciente(It.Is<PacienteDto>(p => p.Id == 0 && string.IsNullOrEmpty(p.Nombre) &&
                                                                                         string.IsNullOrEmpty(p.ApellidoPaterno) &&
                                                                                         string.IsNullOrEmpty(p.ApellidoMaterno) &&
                                                                                         string.IsNullOrEmpty(p.Direccion) &&
                                                                                         string.IsNullOrEmpty(p.NumeroTelefono) &&
                                                                                         p.IdEstado > 0 &&
                                                                                         string.IsNullOrEmpty(p.UsrUpdate) ||
                                                                                         p.FchUpdate == null)))
                .Returns(new BaseDto<bool>(true, new Exception("Algunos campos requeridos estan vacios o null")));
            // Null update data
            mockPacienteService.Setup(m => m.UpdatePaciente(It.Is<PacienteDto>(p => p == null)))
                .Returns(new BaseDto<bool>(true, new Exception("El objeto debe ser diferente a nulo")));
        }
        #endregion

        #region CreatePacienteSetup
        private void CreatePacienteSetup()
        {
            // Correct create data
            mockPacienteService.Setup(m => m.CreatePaciente(It.Is<PacienteDto>(p => string.IsNullOrEmpty(p.Nombre) == false &&
                                                                                 p.IdEstado > 0 &&
                                                                                 string.IsNullOrEmpty(p.UsrCreate) == false &&
                                                                                 p.FchCreate != null)))
                .Returns(new BaseDto<bool>(true));
            // Incorrect create data
            mockPacienteService.Setup(m => m.CreatePaciente(It.Is<PacienteDto>(p => string.IsNullOrEmpty(p.Nombre) ||
                                                                                 p.IdEstado == 0 ||
                                                                                 string.IsNullOrEmpty(p.UsrCreate) ||
                                                                                 p.FchCreate == null)))
                .Returns(new BaseDto<bool>(true, new Exception("Algunos campos requeridos estan vacios o null")));
            // Null create data
            mockPacienteService.Setup(m => m.CreatePaciente(It.Is<PacienteDto>(p => p == null)))
                .Returns(new BaseDto<bool>(true, new Exception("El objeto debe ser diferente a nulo")));
        }
        #endregion

        #region GetListaPacienteByEstadoSetup
        private void GetListaPacienteByEstadoSetup()
        {
            // Correct filter
            mockPacienteService.Setup(m => m.GetListaPacienteByEstado(It.Is<PacienteDto>(p => p.FiltroIdEstado == 5)))
                .Returns(new BaseDto<System.Collections.Generic.List<PacienteDto>>(new System.Collections.Generic.List<PacienteDto>()));
            // Incorrect filter
            mockPacienteService.Setup(m => m.GetListaPacienteByEstado(It.Is<PacienteDto>(p => p.FiltroIdEstado == 6)))
                .Returns(new BaseDto<System.Collections.Generic.List<PacienteDto>>(null));
            // Empty filter
            mockPacienteService.Setup(m => m.GetListaPacienteByEstado(It.Is<PacienteDto>(p => p.FiltroIdEstado == null)))
                .Returns(new BaseDto<System.Collections.Generic.List<PacienteDto>>(null));
            // Null filter
            mockPacienteService.Setup(m => m.GetListaPacienteByEstado(It.Is<PacienteDto>(p => p == null)))
                .Returns(new BaseDto<System.Collections.Generic.List<PacienteDto>>(true, new Exception("Error: el objeto de filtro debe ser diferente de nulo")));
        }
        #endregion

        #region GetPacienteByIdSetup
        private void GetPacienteByIdSetup()
        {
            // Correct filter
            mockPacienteService.Setup(m => m.GetPacienteById(It.Is<PacienteDto>(p => p.FiltroId == 1)))
                .Returns(new BaseDto<PacienteDto>(new PacienteDto()));
            // Incorrect filter
            mockPacienteService.Setup(m => m.GetPacienteById(It.Is<PacienteDto>(p => p.FiltroId == 2)))
                .Returns(new BaseDto<PacienteDto>(null));
            // Empty filter
            mockPacienteService.Setup(m => m.GetPacienteById(It.Is<PacienteDto>(p => p.FiltroId == null)))
                .Returns(new BaseDto<PacienteDto>(null));
            //// Null filter
            mockPacienteService.Setup(m => m.GetPacienteById(It.Is<PacienteDto>(p => p == null)))
                .Returns(new BaseDto<PacienteDto>(true, new Exception("Error: el objeto de filtro debe ser diferente de nulo")));
        }
        #endregion

        #region GetListaPacienteAllSetup
        private void GetListaPacienteAllSetup()
        {
            // Correct
            mockPacienteService.Setup(m => m.GetListaPacienteAll())
                .Returns(new BaseDto<List<PacienteDto>>(new List<PacienteDto>()));
        }
        #endregion

        #endregion

        #region CreatePaciente Tests
        [TestMethod]
        public void CreatePaciente_CorrectData_Test()
        {
            //se crea un objeto simulado
            var filtroObj = new PacienteDto
            {
                Nombre = "john",
                FchCreate = DateTime.Now,
                IdEstado = (int)EnumUtils.EstadoEnum.Paciente_Habilitado,
                UsrCreate = "john1204",
            };
            //se crea la interface de implementacion de los metodos
            IPacienteService service = mockPacienteService.Object;
            // se envia el objeto al servicio simulado
            var serviceResponse = service.CreatePaciente(filtroObj);

            //se devuelve la consulta
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
        public void CreatePaciente_IncorrectData_Test()
        {
            var filtroObj = new PacienteDto
            {
                Nombre = null,
                FchCreate = DateTime.Now,
                IdEstado = 0,
                UsrCreate = null,
            };
            IPacienteService service = mockPacienteService.Object;
            var serviceResponse = service.CreatePaciente(filtroObj);

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
        public void CreatePaciente_NullData_Test()
        {
            IPacienteService service = mockPacienteService.Object;
            var serviceResponse = service.CreatePaciente(null);

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

        #region  UpdatePaciente Tests
        [TestMethod]
        public void UpdatePaciente_CorrectData_Test()
        {
            var filtroObj = new PacienteDto
            {
                Nombre = "john",
                IdEstado = (int)EnumUtils.EstadoEnum.Paciente_Habilitado,
                FchUpdate = DateTime.Now,
                UsrUpdate = "john1204",
                Id = 1
            };
            IPacienteService service = mockPacienteService.Object;
            var serviceResponse = service.UpdatePaciente(filtroObj);

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
        public void UpdatePaciente_IncorrectData_Test()
        {
            var filtroObj = new PacienteDto
            {
                FchCreate = DateTime.Now,
                IdEstado = 0,
                Id = 0,
                UsrUpdate = null,
                Nombre = null
            };
            IPacienteService service = mockPacienteService.Object;
            var serviceResponse = service.UpdatePaciente(filtroObj);

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
        public void UpdatePaciente_NullData_Test()
        {
            IPacienteService service = mockPacienteService.Object;
            var serviceResponse = service.UpdatePaciente(null);

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

        #region GetPacienteById Tests
        [TestMethod]
        public void GetPacienteById_NoData_IncorrectFilter_Test()
        {
            var filtroObj = new PacienteDto { FiltroId = 2 };
            IPacienteService service = mockPacienteService.Object;
            var serviceResponse = service.GetPacienteById(filtroObj);

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
        public void GetPacienteById_NoData_EmptyFilter_Test()
        {
            var filtroObj = new PacienteDto();
            IPacienteService service = mockPacienteService.Object;
            var serviceResponse = service.GetPacienteById(filtroObj);

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
        public void GetPacienteById_WithData_CorrectFilter_Test()
        {
            var filtroObj = new PacienteDto { FiltroId = 1 };
            IPacienteService service = mockPacienteService.Object;
            var serviceResponse = service.GetPacienteById(filtroObj);

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
        public void GetPacienteById_Error_NullFilter_Test()
        {
            IPacienteService service = mockPacienteService.Object;
            var serviceResponse = service.GetPacienteById(null);

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

        #region GetListaPacienteAll Tests 
        [TestMethod]
        public void GetListaPacienteAll_WithData__Test()
        {
            IPacienteService service = mockPacienteService.Object;
            var serviceResponse = service.GetListaPacienteAll();

            // Doesn't has data
            Assert.IsTrue(serviceResponse.HasValue);
            // Value is null
            Assert.IsNotNull(serviceResponse.Value);
            // Doesn't has error
            Assert.IsFalse(serviceResponse.HasError);
            // ...and exception must be null
            Assert.IsNull(serviceResponse.Error);
        }
        #endregion        

        #region GetListaPacienteByEstado Tests 
        [TestMethod]
        public void GetListaPacienteByEstado_NoData_IncorrectFilter_Test()
        {
            var filtroObj = new PacienteDto { FiltroIdEstado = 6 };
            IPacienteService service = mockPacienteService.Object;
            var serviceResponse = service.GetListaPacienteByEstado(filtroObj);

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
        public void GetListaPacienteByEstado_NoData_EmptyFilter_Test()
        {
            var filtroObj = new PacienteDto();
            IPacienteService service = mockPacienteService.Object;
            var serviceResponse = service.GetListaPacienteByEstado(filtroObj);

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
        public void GetListaPacienteByEstado_WithData_CorrectFilter_Test()
        {
            var filtroObj = new PacienteDto { FiltroIdEstado = 5 };
            IPacienteService service = mockPacienteService.Object;
            var serviceResponse = service.GetListaPacienteByEstado(filtroObj);

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
        public void GetListaPacienteByEstado_Error_NullFilter_Test()
        {
            IPacienteService service = mockPacienteService.Object;
            var serviceResponse = service.GetListaPacienteByEstado
(null);

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
