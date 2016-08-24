using Ada.Framework.Data.Notifications.DAO;
using Ada.Framework.Data.Notifications.TO;
using System;
using System.Collections.Generic;

namespace Ada.Framework.Data.Notifications.Business
{
    /// <summary>
    /// Contiene las operaciones de negocio que se realizarán sobre la entidad mensaje.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/08/2016 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    public class MensajeBO
    {
        /// <summary>
        /// Obtiene o establece el DAO utilizado por la instancia.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/08/2016 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        private MensajeDAO DAO { get; set; }

        /// <summary>
        /// Constructor que inicializa la instancia.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/08/2016 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public MensajeBO()
        {
            DAO = new MensajeDAO();
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
        public Notificacion<MensajeTO> Agregar(MensajeTO entidad)
        {
            Notificacion<MensajeTO> retorno = new Notificacion<MensajeTO>();
            retorno.Respuesta = entidad;

            try
            {
                Notificacion<bool> respuestaExiste = Existe(entidad);

                if(respuestaExiste.HayError || respuestaExiste.HayExcepcion)
                {
                    retorno.Unir(respuestaExiste);
                    return retorno;
                }

                if(respuestaExiste.Respuesta)
                {
                    retorno.AgregarMensaje("Mensaje", "Error_Ya_Existe");
                }
                else
                {
                    DAO.Agregar(entidad);
                    retorno.AgregarMensaje("Mensaje", "Agregar_OK");
                }
            }
            catch (Exception ex)
            {
                retorno.AgregarMensaje("Mensaje", "Agregar_Error", null, null, null, null, ex);
            }

            return retorno;
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
        public Notificacion<MensajeTO> Modificar(MensajeTO entidad)
        {
            Notificacion<MensajeTO> retorno = new Notificacion<MensajeTO>();
            retorno.Respuesta = entidad;
            try
            {
                Notificacion<bool> respuestaExiste = Existe(entidad);

                if (respuestaExiste.HayError || respuestaExiste.HayExcepcion)
                {
                    retorno.Unir(respuestaExiste);
                    return retorno;
                }

                if (respuestaExiste.Respuesta)
                {
                    DAO.Modificar(entidad);
                    retorno.AgregarMensaje("Mensaje", "Modificar_OK");
                }
                else
                {
                    retorno.AgregarMensaje("Mensaje", "Error_No_Existe");
                }
            }
            catch (Exception ex)
            {
                retorno.AgregarMensaje("Mensaje", "Modificar_Error", null, null, null, null, ex);
            }

            return retorno;
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
        public Notificacion Eliminar(MensajeTO entidad)
        {
            Notificacion retorno = new Notificacion();

            try
            {
                Notificacion<bool> respuestaExiste = Existe(entidad);

                if (respuestaExiste.HayError || respuestaExiste.HayExcepcion)
                {
                    retorno.Unir(respuestaExiste);
                    return retorno;
                }

                if (respuestaExiste.Respuesta)
                {
                    DAO.Eliminar(entidad);
                    retorno.AgregarMensaje("Mensaje", "Eliminar_OK");
                }
                else
                {
                    retorno.AgregarMensaje("Mensaje", "Error_No_Existe");
                }
            }
            catch (Exception ex)
            {
                retorno.AgregarMensaje("Mensaje", "Eliminar_Error", null, null, null, null, ex);
            }

            return retorno;
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
        public Notificacion<bool> Existe(MensajeTO entidad)
        {
            Notificacion<bool> retorno = new Notificacion<bool>();

            try
            {
                retorno.Respuesta = DAO.Existe(entidad);
            }
            catch (Exception ex)
            {
                retorno.AgregarMensaje("Mensaje", "Existe_Error", null, null, null, null, ex);
            }

            return retorno;
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
        public Notificacion<IList<MensajeTO>> ObtenerTodos()
        {
            Notificacion<IList<MensajeTO>> retorno = new Notificacion<IList<MensajeTO>>();

            try
            {
                retorno.Respuesta = DAO.ObtenerTodos();
            }
            catch (Exception ex)
            {
                retorno.AgregarMensaje("Mensaje", "ObtenerTodos_Error", null, null, null, null, ex);
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
        public Notificacion<MensajeTO> ObtenerSegunID(MensajeTO entidad)
        {
            Notificacion<MensajeTO> retorno = new Notificacion<MensajeTO>();

            try
            {
                retorno.Respuesta = DAO.ObtenerSegunID(entidad);
            }
            catch (Exception ex)
            {
                retorno.AgregarMensaje("Mensaje", "ObtenerSegunID_Error", null, null, null, null, ex);
            }

            return retorno;
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
        public Notificacion<IList<MensajeTO>> ObtenerSegunFiltro(MensajeTO filtro)
        {
            Notificacion<IList<MensajeTO>> retorno = new Notificacion<IList<MensajeTO>>();

            try
            {
                retorno.Respuesta = DAO.ObtenerSegunFiltro(filtro);
            }
            catch (Exception ex)
            {
                retorno.AgregarMensaje("Mensaje", "ObtenerSegunFiltro_Error", null, null, null, null, ex);
            }

            return retorno;
        }
    }
}
