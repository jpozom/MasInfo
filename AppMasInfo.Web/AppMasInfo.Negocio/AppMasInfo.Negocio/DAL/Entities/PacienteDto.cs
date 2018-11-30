﻿using AppMasInfo.Negocio.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Negocio.DAL.Entities
{
    public class PacienteDto : ObjectDto
    {
        #region Metodos privados
        public PacienteDto() : base()
        {
        }

        public PacienteDto(int PaginaActual, int TamanoPagina) : base(PaginaActual, TamanoPagina)
        {
        }
        #endregion

        public long Id { get; set; }
        public string Rut { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int Edad { get; set; }
        public string Direccion { get; set; }        
        public Nullable<System.DateTime> FchCreate { get; set; }
        public string UsrCreate { get; set; }
        public Nullable<System.DateTime> FchUpdate { get; set; }
        public string UsrUpdate { get; set; }
        public int IdEstado { get; set; }
        public string NumeroTelefono { get; set; }

        public int? FiltroIdEstado { get; set; }
        public long? FiltroId { get; set; }
        public long? FiltroIdTutor { get; set; }
        public string FiltroRut { get; set; }       
        public string FiltroNombre { get; set; }

        public EstadoDto DetalleEstado { get; set; }
        public PacienteUbicacionDto DetallePacienteUbicacion { get; set; }
        public UbicacionDto DetalleUbicacion { get; set; }
        public TutorDto DetalleTutor { get; set; }
        public EquipoPacienteDto DetalleEquipoPaciente { get; set; }

        public string DatosPaciente
        {
            get
            {
                // se crea arreglo que se llena con las priopiedades que se asignaron en la obtencion de la lista completa de trabajador                 
                string datosPaciente = string.Empty;
                datosPaciente = String.Format("{0} - {1} {2} {3}", Rut, Nombre, ApellidoPaterno, ApellidoMaterno);

                return datosPaciente;
            }
        }
    }
}
