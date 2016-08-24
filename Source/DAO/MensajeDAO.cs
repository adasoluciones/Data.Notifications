using Ada.Framework.Data.DBConnector;
using Ada.Framework.Data.DBConnector.Connections;
using Ada.Framework.Data.DBConnector.Queries;
using Ada.Framework.Data.Notifications.TO;
using System;
using System.Collections.Generic;

namespace Ada.Framework.Data.Notifications.DAO
{
    /// <summary>
    /// Contiene las operaciones CRUD que se realizarán sobre la entidad mensaje.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    internal class MensajeDAO
    {
        /// <summary>
        /// Obtiene la conexión del DAO.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/08/2016 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public ConexionBaseDatos Conexion { get; protected set; }

        /// <summary>
        /// Obtiene el nombre de la conexión de base de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/08/2016 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public string NombreConexion { get { return "Mensajes"; } }

        /// <summary>
        /// Constructor que inicializa la instancia.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/08/2016 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public MensajeDAO()
        {
            Conexion = ConfiguracionBaseDatosFactory.ObtenerConfiguracionDeBaseDatos().ObtenerConexionBaseDatos(NombreConexion);
            Conexion.AutoConectarse = true;
        }

        /// <summary>
        /// Persiste una nueva entidad.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/08/2016 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="entidad">Mensaje a persistir.</param>
        /// <returns>Mensaje persistido.</returns>
        public MensajeTO Agregar(MensajeTO entidad)
        {
            try
            {
                Conexion.Abrir();

                DynamicQuery consulta = Conexion.CrearQueryDinamica();
                AgregarParametros(entidad, consulta);

                IDictionary<string, object> registro = consulta.Obtener();
                entidad = Mapear(registro);

                return entidad;
            }
            finally
            {
                Conexion.Cerrar();
            }
        }

        /// <summary>
        /// Elimina una entidad de base de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/08/2016 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="entidad">Mensaje a eliminar.</param>
        public void Eliminar(MensajeTO entidad)
        {
            try
            {
                Conexion.Abrir();

                DynamicQuery consulta = Conexion.CrearQueryDinamica();
                AgregarParametros(entidad, consulta);

                consulta.Ejecutar();
            }
            finally
            {
                Conexion.Cerrar();
            }
        }

        /// <summary>
        /// Determina la existencia de una entidad en base de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/08/2016 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="entidad">Mensaje a comprobar.</param>
        /// <returns><code>true</code> de existir, o <code>false</code> de lo contrario.</returns>
        public bool Existe(MensajeTO entidad)
        {
            try
            {
                Conexion.Abrir();

                DynamicQuery consulta = Conexion.CrearQueryDinamica();
                AgregarParametros(entidad, consulta);

                int respuesta = Convert.ToInt32(consulta.Obtener()["Respuesta"].ToString());
                return (respuesta == 1);
            }
            finally
            {
                Conexion.Cerrar();
            }
        }

        /// <summary>
        /// Persiste los cambios de una entidad.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/08/2016 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="entidad">Mensaje a persistir.</param>
        /// <returns>Mensaje persistido.</returns>
        public MensajeTO Modificar(MensajeTO entidad)
        {
            try
            {
                Conexion.Abrir();

                DynamicQuery consulta = Conexion.CrearQueryDinamica();
                AgregarParametros(entidad, consulta);

                IDictionary<string, object> registro = consulta.Obtener();
                entidad = Mapear(registro);

                return entidad;
            }
            finally
            {
                Conexion.Cerrar();
            }
        }

        /// <summary>
        /// Obtiene las entidades según los valores de un filtro.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/08/2016 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="filtro">Mensaje de filtro</param>
        /// <returns>Colección de mensajes que cumplen el filtro.</returns>
        public IList<MensajeTO> ObtenerSegunFiltro(MensajeTO filtro)
        {
            IList<MensajeTO> retorno = new List<MensajeTO>();

            try
            {
                Conexion.Abrir();

                DynamicQuery consulta = Conexion.CrearQueryDinamica();
                AgregarParametros(filtro, consulta);

                foreach (IDictionary<string, object> registro in consulta.Consultar())
                {
                    MensajeTO entidad = Mapear(registro);
                    retorno.Add(entidad);
                }
            }
            finally
            {
                Conexion.Cerrar();
            }

            return retorno;
        }

        /// <summary>
        /// Obtiene la entidad según si identificador(es).
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/08/2016 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="entidad">Mensaje a buscar.</param>
        /// <returns>Mensaje persistido.</returns>
        public MensajeTO ObtenerSegunID(MensajeTO entidad)
        {
            try
            {
                Conexion.Abrir();

                DynamicQuery consulta = Conexion.CrearQueryDinamica();
                AgregarParametros(entidad, consulta);

                IDictionary<string, object> registro = consulta.Obtener();
                entidad = Mapear(registro);
            }
            finally
            {
                Conexion.Cerrar();
            }

            return entidad;
        }

        /// <summary>
        /// Obtiene todas las entidades persistidas.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/08/2016 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <returns>Colección de mensajes.</returns>
        public IList<MensajeTO> ObtenerTodos()
        {
            IList<MensajeTO> retorno = new List<MensajeTO>();

            try
            {
                Conexion.Abrir();

                DynamicQuery consulta = Conexion.CrearQueryDinamica();

                foreach (IDictionary<string, object> registro in consulta.Consultar())
                {
                    MensajeTO entidad = Mapear(registro);
                    retorno.Add(entidad);
                }
            }
            finally
            {
                Conexion.Cerrar();
            }

            return retorno;
        }

        /// <summary>
        /// Mapea según un registro de base de datos, los valores en la entidad correspondiente.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/08/2016 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="registro">Representa un registro de base de datos.</param>
        /// <returns>Mensaje cargado.</returns>
        private MensajeTO Mapear(IDictionary<string, object> registro)
        {
            MensajeTO retorno = new MensajeTO();

            retorno.Tabla = registro["Tabla"].ToString();
            retorno.Codigo = registro["Codigo"].ToString();
            retorno.Idioma = registro["Idioma"].ToString();
            retorno.Pais = registro["Pais"].ToString();
            retorno.Tipo = (Severidad)Severidad.ObtenerEnumeracion(registro["Tipo"].ToString());
            retorno.Glosa = registro["Glosa"].ToString();
            retorno.GlosaResumida = registro["GlosaResumida"].ToString();
            retorno.LinkAudio = registro["LinkAudio"].ToString();
            retorno.LinkVideo = registro["LinkVideo"].ToString();

            return retorno;
        }

        /// <summary>
        /// Agrega los valores de una entidad como parámetros de una consulta.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/08/2016 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="entidad">Mensaje con valores de entrada.</param>
        /// <param name="consulta">Consulta a ejecutar.</param>
        private void AgregarParametros(MensajeTO entidad, Query consulta)
        {
            consulta.Parametros.Add("Tabla", entidad.Tabla);
            consulta.Parametros.Add("Codigo", entidad.Codigo);
            consulta.Parametros.Add("Idioma", entidad.Idioma);
            consulta.Parametros.Add("Pais", entidad.Pais);
            consulta.Parametros.Add("Tipo", entidad.Tipo.ToString());
            consulta.Parametros.Add("Glosa", entidad.Glosa);
            consulta.Parametros.Add("GlosaResumida", entidad.GlosaResumida);
            consulta.Parametros.Add("LinkAudio", entidad.LinkAudio);
            consulta.Parametros.Add("LinkVideo", entidad.LinkVideo);
        }
    }
}
