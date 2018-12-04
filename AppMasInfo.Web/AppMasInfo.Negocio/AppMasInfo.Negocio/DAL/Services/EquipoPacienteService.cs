using AppMasInfo.Negocio.DAL.Database;
using AppMasInfo.Negocio.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Services
{
    public class EquipoPacienteService : IEquipoPacienteService
    {
        #region propiedades privadas
        private static EquipoPacienteService Instance = null;
        private Database.MasInfoWebEntities_02 dbContext = null;
        #endregion

        #region singleton
        private EquipoPacienteService() { }

        public static EquipoPacienteService GetInstance()
        {
            if (EquipoPacienteService.Instance == null)
                EquipoPacienteService.Instance = new EquipoPacienteService();

            return EquipoPacienteService.Instance;
        }
        #endregion

        #region AddEquipoPaciente
        public BaseDto<bool> AddEquipoPaciente(EquipoPacienteDto equipo)
        {
            BaseDto<bool> result = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {
                    var equipoPaciente = this.dbContext.EquipoPaciente.Add(
                        new Database.EquipoPaciente
                        {
                            IdPaciente = equipo.IdPaciente,
                            IdTrabajador = equipo.Idtrabajador
                        });

                    // Guardamos los cambios en base de datos
                    this.dbContext.SaveChanges();

                    result = new BaseDto<bool>(true);
                }
            }
            catch (SqlException sqlEx)
            {
                result = new BaseDto<bool>(true, sqlEx);
            }
            catch (Exception ex)
            {
                result = new BaseDto<bool>(true, ex);
            }
            return result;
        }
        #endregion

        #region GetEquipoPacienteByIdPaciente
        public BaseDto<List<EquipoPacienteDto>> GetEquipoPacienteByIdPaciente(PacienteDto p_Filtro)
        {
            BaseDto<List<EquipoPacienteDto>> objResult = null;

            try
            {
                using (this.dbContext = new Database.MasInfoWebEntities_02())
                {
                    var epDb = (from ep in this.dbContext.EquipoPaciente
                                join t in this.dbContext.Trabajador on ep.IdTrabajador equals t.Id
                                join p in this.dbContext.Paciente on ep.IdPaciente equals p.Id
                                join c in this.dbContext.Cargo on t.IdCargo equals c.Id
                                join cf in this.dbContext.CargoFuncion on t.IdCargoFuncion equals cf.Id
                                where ep.IdPaciente == p_Filtro.FiltroId
                                select new EquipoPacienteDto
                                {
                                    Id = ep.Id,
                                    IdPaciente = ep.IdPaciente,
                                    Idtrabajador = ep.IdTrabajador,
                                    Trabajador = new TrabajadorDto
                                    {
                                        Id = ep.IdTrabajador,
                                        Nombre = t.Nombre,
                                        Rut = t.Rut,
                                        ApellidoPaterno = t.ApellidoPaterno,
                                        ApellidoMaterno = t.ApellidoMaterno,
                                        IdUsuario = t.IdUsuario,
                                        IdCargo = t.IdCargo,
                                        IdCargoFuncion = t.IdCargoFuncion,
                                        DetalleCargo = new CargoDto { Id = c.Id, Descripcion = c.Descripcion, IdCargoFuncion = c.IdCargoFuncion },
                                        DetalleFuncion = new CargoFuncionDto { Id = cf.Id, Descripcion = cf.Descripcion }
                                    },
                                    Paciente = new PacienteDto
                                    {
                                        Id = p.Id,
                                        Nombre = p.Nombre,
                                        ApellidoPaterno = p.ApellidoPaterno,
                                        ApellidoMaterno = p.ApellidoMaterno,
                                        Rut = p.Rut
                                    }
                                }).ToList();

                    objResult = new BaseDto<List<EquipoPacienteDto>>(epDb);
                }
            }
            catch (SqlException sqlEx)
            {
                objResult = new BaseDto<List<EquipoPacienteDto>>(true, sqlEx);
            }
            catch (Exception ex)
            {
                objResult = new BaseDto<List<EquipoPacienteDto>>(true, ex);
            }

            return objResult;
        }
        #endregion

        #region Delete
        public BaseDto<bool> Delete(EquipoPacienteDto p_Obj)
        {
            BaseDto<bool> result = null;

            try
            {
                using (this.dbContext = new MasInfoWebEntities_02())
                {
                    //Obtener el objeto origen desde base de datos
                    //El metodo .FirstOrDefault, retorna el primer objeto encontrado de acuerdo
                    // a un determinado filtro de búsqueda, y en caso contrario, retorna null
                    var objOrigenDb = this.dbContext.EquipoPaciente.FirstOrDefault(u => u.Id == p_Obj.Id);

                    if (objOrigenDb != null)
                    {
                        objOrigenDb.Id = p_Obj.Id;

                        this.dbContext.EquipoPaciente.RemoveRange(this.dbContext.EquipoPaciente.Where(u => u.Id == p_Obj.Id));
                        this.dbContext.SaveChanges();
                       
                        result = new BaseDto<bool>(true);
                    }
                    else
                    {
                        throw new Exception("Id EquipoPaciente no encontrado en base de datos");
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                result = new BaseDto<bool>(true, sqlEx);
            }
            catch (Exception ex)
            {
                result = new BaseDto<bool>(true, ex);
            }
            return result;
        }
        #endregion
    }
}
